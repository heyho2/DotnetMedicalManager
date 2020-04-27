using Dapper;
using GD.DataAccess;
using GD.Dtos.Order;
using GD.Models.CommonEnum;
using GD.Models.Mall;
using System.Collections.Generic;
using System.Threading.Tasks;
using static GD.Dtos.Order.GetMerchantOrderPageListRequestDto;

namespace GD.Manager.Mall
{
    /// <summary>
    /// 订单相关操作(存储过程操作)
    /// </summary>
    public class OrderBiz : BaseBiz<OrderModel>
    {
        /// <summary>
        /// 查询商户订单分页列表
        /// </summary>
        public async Task<GetMerchantOrderPageListResponseDto> GetMerchantOrderPageListAsync(GetMerchantOrderPageListRequestDto requestDto)
        {
            var sqlWhere = "a.`enable` = 1 ";

            if (requestDto.BeginDate != null)
            {
                requestDto.BeginDate = requestDto.BeginDate?.Date;
                sqlWhere = $"{sqlWhere} and a.creation_date >= @BeginDate";
            }
            if (requestDto.EndDate != null)
            {
                requestDto.EndDate = requestDto.EndDate?.AddDays(1).Date;
                sqlWhere = $"{sqlWhere} and a.creation_date < @EndDate";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.Keyword))
            {
                requestDto.Keyword = $"%{requestDto.Keyword}%";
                sqlWhere = $"{sqlWhere} and (b.phone like @Keyword or b.nick_name like @Keyword)";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.OrderNo))
            {
                sqlWhere = $"{sqlWhere} and a.order_no=@OrderNo";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.MerchantName))
            {
                sqlWhere = $"{sqlWhere} and c.merchant_name=@MerchantName";
            }

            switch (requestDto.OrderStatus)
            {
                case OrderStatusConditionEnum.Obligation:
                    sqlWhere = $"{sqlWhere} and a.order_status='{OrderStatusConditionEnum.Obligation.ToString()}' and a.pay_type<>'{PayTypeEnum.OffLinePay.ToString()}'";
                    break;
                case OrderStatusConditionEnum.OffLinePay:
                    sqlWhere = $"{sqlWhere} and a.order_status='{OrderStatusConditionEnum.Obligation.ToString()}' and a.pay_type='{PayTypeEnum.OffLinePay.ToString()}'";
                    break;
                case OrderStatusConditionEnum.Shipped:
                    sqlWhere = $"{sqlWhere} and a.order_status='{OrderStatusConditionEnum.Received.ToString()}' and a.express_no is null";
                    break;
                case OrderStatusConditionEnum.Received:
                    sqlWhere = $"{sqlWhere} and a.order_status='{OrderStatusConditionEnum.Received.ToString()}' and a.express_no is not null";
                    break;
            }
            if (requestDto.OrderStatus != null)
            {
                sqlWhere = $"{sqlWhere} and a.order_status='{requestDto.OrderStatus.ToString()}'";
            }
            var sql = $@"SELECT
	                        a.order_guid,
	                        a.order_no,
	                        b.nick_name,
	                        b.phone,
	                        a.paid_amount,
	                        a.creation_date,
	                        a.order_status ,
	                        a.payment_date,
	                        a.pay_type,
	                        a.payables_amount,
                            a.freight,
	                        a.discount_amout,
	                        a.order_receiver,
	                        a.order_phone,
	                        a.express_company,
	                        a.express_no,
	                        a.order_address,
                            c.merchant_name
                        FROM
	                        t_mall_order a
	                        INNER JOIN t_utility_user b ON a.user_guid = b.user_guid
                            LEFT JOIN t_merchant c on a.merchant_guid=c.merchant_guid
                        WHERE
                            {sqlWhere}
                        ORDER BY
	                        a.creation_date DESC";
            return await MySqlHelper.QueryByPageAsync<GetMerchantOrderPageListRequestDto, GetMerchantOrderPageListResponseDto, GetMerchantOrderPageListItemDto>(sql, requestDto);
        }
        /// <summary>
        /// 获取订单下的商品列表V2
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        public async Task<List<OrderProductDto>> GetOrderProductListV2Async(List<string> orderIds)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            a.order_guid,
	                            a.product_guid,
	                            a.product_count,
	                            a.product_price,
	                            a.product_name,
                                c.config_name
                            FROM
	                            t_mall_order_detail a
                                left join t_mall_product b on a.product_guid=b.product_guid
                                left join t_manager_dictionary c on c.dic_guid=b.category_guid
                            WHERE
	                            order_guid IN @orderIds;";
                var result = await conn.QueryAsync<OrderProductDto>(sql, new { orderIds });
                return result?.AsList();
            }
        }
        /// <summary>
        /// 获取订单下的商品列表
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        public async Task<List<OrderProductDto>> GetOrderProductListAsync(List<string> orderIds)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            order_guid,
	                            product_guid,
	                            product_count,
	                            product_price,
	                            product_name
                            FROM
	                            t_mall_order_detail
                            WHERE
	                            order_guid IN @orderIds;";
                var result = await conn.QueryAsync<OrderProductDto>(sql, new { orderIds });
                return result?.AsList();
            }
        }
        /// <summary>
        /// 获取商品下包含的项目列表
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public async Task<List<OrderProductProjectsDto>> GetOrderProductProjectsAsync(List<string> productIds)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            a.product_guid,
	                            b.project_name,
	                            a.project_times
                            FROM
	                            t_mall_product_project a
	                            INNER JOIN t_mall_project b ON a.project_guid = b.project_guid
                            WHERE
	                            a.product_guid IN @productIds;";
                var result = await conn.QueryAsync<OrderProductProjectsDto>(sql, new { productIds });
                return result?.AsList();
            }
        }
    }
}
