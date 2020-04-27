using Dapper;
using GD.DataAccess;
using GD.Dtos.Mall.Order;
using GD.Models.Mall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Mall
{
    public class OrderDetailBiz : BizBase.BaseBiz<OrderDetailModel>
    {
        /// <summary>
        /// 通过订单明细Id获取订单明细记录
        /// </summary>
        /// <returns></returns>
        public async Task<OrderDetailModel> GetModelAsync(string detailId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<OrderDetailModel>("select * from t_mall_order_detail where detail_guid=@detailId and `enable`=1", new { detailId });
            }
        }

        /// <summary>
        /// 通过订单Id获取订单详情集合
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<List<OrderDetailModel>> GetModelsByOrderIdAsync(string orderId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<OrderDetailModel>("where order_guid = @orderId", new { orderId });
                return result?.AsList();
            }
        }
        /// <summary>
        /// 根据订单号查orderDetailModelList
        /// </summary>
        /// <param name="orderGuid"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task<List<OrderDetailModel>> GetOrderDetailModelListByOrderGuidAsync(string orderGuid, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.GetListAsync<OrderDetailModel>(" where order_guid=@orderGuid and `enable`=@enable ", new { orderGuid, enable })).ToList();
            }
        }

        /// <summary>
        /// 订单详情售后信息
        /// </summary>
        /// <param name="orderGuid">订单guid</param>
        /// <returns></returns>
        public async Task<List<OrderDetailAfterServiceInfoDto>> GetOrderDetailAfterServiceInfoByOrderGuidAsync(string orderGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            a.detail_guid as order_detail_guid,
	                            c.type as service_type,
	                            c.`status` as service_status 
                            FROM
	                            t_mall_order_detail a
	                            LEFT JOIN t_mall_aftersale_detail b ON a.detail_guid = b.order_detail_guid
	                            LEFT JOIN t_mall_aftersale_service c ON c.service_guid = b.service_guid 
                            WHERE
	                            a.order_guid = @orderGuid";
                var result = await conn.QueryAsync<OrderDetailAfterServiceInfoDto>(sql, new { orderGuid });
                return result.ToList();
            }
        }

    }
}
