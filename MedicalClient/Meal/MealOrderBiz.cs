using GD.DataAccess;
using GD.Models.Meal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GD.Dtos.Meal.MealClient;
using System.Linq;

namespace GD.Meal
{
    /// <summary>
    /// 点餐订单业务类
    /// </summary>
    public class MealOrderBiz
    {

        /// <summary>
        /// 获取唯一model
        /// </summary>
        /// <param name="orderGuid"></param>
        /// <returns></returns>
        public async Task<MealOrderModel> GetModelAsync(string orderGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<MealOrderModel>(orderGuid);
            }
        }

        /// <summary>
        /// 获取用户点餐订单分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<GetUserMealOrderPageListResponseDto> GetUserMealOrderPageListAsync(GetUserMealOrderPageListRequestDto requestDto)
        {
            var sql = $@"DROP TEMPORARY TABLE
                        IF
	                        EXISTS t_meal_order_tmp;
                        DROP TEMPORARY TABLE
                        IF
	                        EXISTS t_meal_order_tmp_1;
                        CREATE TEMPORARY TABLE t_meal_order_tmp AS SELECT
                            a.order_guid,
                            a.order_no,
                            a.meal_date,
                            a.meal_start_time,
                            a.meal_end_time,
                            a.category_name,
                            a.total_price,
                            case when a.meal_end_time<NOW() and a.order_status='Paided' then 'Expired' else a.order_status end as real_order_status,
                            a.creation_date ,
                            b.category_advance_day,
                            b.category_schedule_time
                            FROM
	                            t_meal_order a left join t_meal_category b on a.category_guid=b.category_guid
                            WHERE
	                            a.hospital_guid = @HospitalGuid 
	                            AND a.`enable` = 1 
	                            AND a.user_guid = @userId 
                            ORDER BY
	                            case real_order_status when 'Paided' then 1 when 'Completed' then 2 when 'Canceled' then 3 when 'Expired' then 4 else 5 end asc,
	                            a.meal_start_time 
	                            LIMIT @PageIndex,
	                            @PageSize;
                        CREATE TEMPORARY TABLE t_meal_order_tmp_1 AS SELECT
                        * 
                        FROM
	                        t_meal_order_tmp;
                        SELECT
	                        * 
                        FROM
	                        t_meal_order_tmp;
                            
                        SELECT
                            COUNT(order_guid)
                        FROM
	                        t_meal_order 
                        WHERE
	                        hospital_guid = @HospitalGuid 
	                        AND `enable` = 1 
	                        AND user_guid = @userId ;
                        SELECT
	                        a.order_guid,
	                        b.dishes_guid,
	                        b.dishes_name,
	                        b.quantity 
                        FROM
	                        t_meal_order_tmp_1 a
	                        INNER JOIN t_meal_order_detail b ON a.order_guid = b.order_guid
	                        INNER JOIN t_meal_dishes c ON c.dishes_guid = b.dishes_guid;";
            var order = new List<GetUserMealOrderPageListQueryDto>();
            var orderDetail = new List<GetUserMealOrderDetailQueryDto>();
            var count = 0;
            using (var conn = MySqlHelper.GetConnection())
            {
                var reader = await conn.QueryMultipleAsync(sql, new
                {
                    userId = requestDto.UserGuid,
                    requestDto.HospitalGuid,
                    PageIndex = (requestDto.PageIndex - 1) * requestDto.PageSize,
                    requestDto.PageSize
                });
                order = reader.Read<GetUserMealOrderPageListQueryDto>().AsList();
                count = reader.ReadFirstOrDefault<int?>() ?? 0;
                orderDetail = reader.Read<GetUserMealOrderDetailQueryDto>().AsList();
            }
            return new GetUserMealOrderPageListResponseDto
            {
                CurrentPage = order.Select(a => new GetUserMealOrderPageListItemDto
                {
                    OrderGuid = a.OrderGuid,
                    OrderNo = a.OrderNo,
                    MealDate = a.MealDate,
                    MealStartTime = a.MealStartTime,
                    MealEndTime = a.MealEndTime,
                    CanCancel = a.RealOrderStatus == MealOrderStatusEnum.Paided.ToString() && Convert.ToDateTime($"{a.MealDate.AddDays(a.CategoryAdvanceDay * (-1)).ToString("yyyy-MM-dd")} {a.CategoryScheduleTime}") > DateTime.Now,
                    CategoryName = a.CategoryName,
                    TotalPrice = a.TotalPrice,
                    OrderStatus = a.RealOrderStatus,
                    CreationDate = a.CreationDate,
                    OrderDetails = orderDetail.Where(d => d.OrderGuid == a.OrderGuid).Select(d => new GetUserMealOrderPageListItemDto.GetUserMealOrderDetailResponseDto
                    {
                        DishesName = d.DishesName,
                        Quantity = d.Quantity
                    }).AsList()
                }),
                Total = count
            };
        }

        /// <summary>
        /// 获取点餐用户订单数量
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public async Task<int> GetUserOrderTotalAsync(string userGuid, string hospitalGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var total = await conn.QueryFirstOrDefaultAsync<int>("select count(order_guid) from t_meal_order where user_guid=@userGuid and hospital_guid=@hospitalGuid and `enable`=1", new { userGuid, hospitalGuid });
                return total;
            }
        }

        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="orders">订单类表</param>
        /// <param name="orderDetails">订单明细列表</param>
        /// <param name="accountDetails">账户明细列表</param>
        /// <param name="tradeModels">交易类表</param>
        /// <param name="tradeDetailsModels">交易明细列表</param>
        /// <returns></returns>
        public async Task<bool> SubmitMealOrderAsync(
            List<MealOrderModel> orders,
            List<MealOrderDetailModel> orderDetails,
            List<MealAccountDetailModel> accountDetails,
             List<MealAccountTradeModel> tradeModels,
        List<MealAccountTradeDetailModel> tradeDetailsModels)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                orders.InsertBatch(conn);
                orderDetails.InsertBatch(conn);
                accountDetails.InsertBatch(conn);
                tradeModels.InsertBatch(conn);
                tradeDetailsModels.InsertBatch(conn);
                //扣减账户余额
                foreach (var item in accountDetails)
                {
                    var ieffec = await conn.ExecuteAsync($"update t_meal_account set account_balance=account_balance-@AccountDetailFee where account_guid=@AccountGuid and account_balance>=@AccountDetailFee", new { item.AccountDetailFee, item.AccountGuid });
                    if (ieffec == 0)
                    {
                        return false;
                    }
                }
                return true;
            });
        }

        /// <summary>
        /// 取消点餐订单
        /// </summary>
        /// <param name="order">订单model</param>
        /// <param name="tradeDetails">下单时的</param>
        /// <returns></returns>
        public async Task<bool> CancelMealOrderAsync(MealOrderModel order, MealAccountTradeModel refundMealAccountTradeModel, List<MealAccountTradeDetailModel> tradeDetails, List<MealAccountDetailModel> accountDetailModels)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                await conn.UpdateAsync(order);
                await conn.InsertAsync<string, MealAccountTradeModel>(refundMealAccountTradeModel);
                accountDetailModels.InsertBatch(conn);
                foreach (var item in tradeDetails)
                {
                    var ieffec = await conn.ExecuteAsync("update t_meal_account set account_balance = account_balance + @AccountTradeFee where account_guid = @AccountGuid", new { item.AccountTradeFee, item.AccountGuid });
                }
                return true;
            });
        }

        /// <summary>
        /// 订单转让
        /// </summary>
        /// <param name="insertOrder"></param>
        /// <param name="updateOrder"></param>
        /// <param name="orderDetails"></param>
        /// <param name="accountDetails"></param>
        /// <param name="tradeModels"></param>
        /// <param name="tradeDetailsModels"></param>
        /// <returns></returns>
        public async Task<bool> AcceptTransferedMealOrderAsync(
            MealOrderModel insertOrder,
            MealOrderModel updateOrder,
            List<MealOrderDetailModel> orderDetails,
            List<MealAccountDetailModel> accountDetails,
            List<MealAccountTradeModel> tradeModels,
            List<MealAccountTradeDetailModel> tradeDetailsModels)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                await conn.InsertAsync<string, MealOrderModel>(insertOrder);
                await conn.UpdateAsync(updateOrder);
                if (orderDetails.InsertBatch(conn) == 0) return false;
                if (accountDetails.InsertBatch(conn) == 0) return false;
                if (tradeModels.InsertBatch(conn) == 0) return false;
                if (tradeDetailsModels.InsertBatch(conn) == 0) return false;
                //账户余额：退款 / 消费
                foreach (var item in tradeDetailsModels)
                {
                    var opFee = 1;
                    var tradeType = tradeModels.FirstOrDefault(a => a.AccountTradeGuid == item.AccountTradeGuid).AccountTradeType;
                    if (tradeType == 1)
                    {
                        opFee = -1;
                    }
                    var ieffec = await conn.ExecuteAsync($"update t_meal_account set account_balance=account_balance-@Fee where account_guid=@AccountGuid ", new { Fee = opFee * item.AccountTradeFee, item.AccountGuid });
                    if (ieffec == 0)
                    {
                        return false;
                    }
                }
                return true;
            });
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(MealOrderModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                model.LastUpdatedDate = DateTime.Now;
                var result = await conn.UpdateAsync(model);
                return result > 0;
            }
        }

    }
}
