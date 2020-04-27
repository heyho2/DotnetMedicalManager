using Dapper;
using GD.BizBase;
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
    public class AftersaleDetailBiz : BaseBiz<AfterSaleDetailModel>
    {
        /// <summary>
        /// 根据订单详情Id集合获取售后详情models
        /// </summary>
        /// <param name="orderDetailIds">订单详情Ids</param>
        /// <returns></returns>
        public async Task<List<AfterSaleDetailModel>> GetByOrderDetialIdsAsync(IEnumerable<string> orderDetailIds)
        {
            var distinctIds = orderDetailIds.Distinct();
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<AfterSaleDetailModel>("where `enable`=1 and order_detail_guid in @distinctIds ", new { distinctIds });
                return result.ToList();
            }
        }

        /// <summary>
        /// 获取售后详情
        /// </summary>
        /// <param name="serviceGuid"></param>
        /// <returns></returns>
        public async Task<List<GetServiceDetailInfoTmpDto>> GetServiceDetailInfoAsync(string serviceDetailGuid)
        {
            var sql = @"SELECT
	                        b.service_no,
	                        b.`status` as service_status,
	                        b.type as service_type,
	                        a.product_guid,
	                        c.product_name,
                            c.product_price,
	                        a.product_count,
	                        CONCAT( e.base_path, e.relative_path ) as product_picture,
	                        a.refund_fee,
	                        f.title as consultation_title,
	                        f.content as content_content,
	                        f.creation_date as consultation_date,
	                        f.role_type ,
	                        g.last_updated_date as refund_date
                        FROM
	                        t_mall_aftersale_detail a
	                        INNER JOIN t_mall_aftersale_service b ON a.service_guid = b.service_guid 
	                        AND a.`enable` = b.`enable`
	                        INNER JOIN t_mall_order_detail c ON c.detail_guid = a.order_detail_guid 
	                        AND a.`enable` = b.`enable`
	                        INNER JOIN t_mall_product d ON d.product_guid = a.product_guid
	                        LEFT JOIN t_utility_accessory e ON e.accessory_guid = d.picture_guid
	                        LEFT JOIN t_mall_aftersale_consultation f ON f.detail_guid = a.detail_guid 
	                        left join t_mall_aftersale_refund g on g.service_guid=a.service_guid
                        WHERE
	                        a.detail_guid = @serviceDetailGuid
	                        AND a.`enable` = 1";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<GetServiceDetailInfoTmpDto>(sql, new { serviceDetailGuid });
                return result.ToList();
            }
        }
    }
}
