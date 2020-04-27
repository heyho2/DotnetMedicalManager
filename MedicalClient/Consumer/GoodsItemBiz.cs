using Dapper;
using GD.BizBase;
using GD.DataAccess;
using GD.Models.Consumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Consumer
{
    /// <summary>
    /// 个人卡项实体业务类
    /// </summary>
    public class GoodsItemBiz : BaseBiz<GoodsItemModel>
    {
        /// <summary>
        /// 异步获取model
        /// </summary>
        /// <param name="goodGuid">主键id</param>
        /// <returns></returns>
        public async Task<GoodsItemModel> GetModelAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<GoodsItemModel>("select * from t_consumer_goods_item where goods_item_guid=@id and `enable`=1", new { id });
            }
        }

        /// <summary>
        /// 查询指定卡对应卡项服务项目是否可用
        /// </summary>
        /// <param name="goodsGuid"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<GoodsItemModel> GetModelAsync(string goodsGuid, string projectId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<GoodsItemModel>(@"select * from t_consumer_goods_item where goods_guid = @goodsGuid 
                    and project_guid = @projectId and available = 1 and remain > 0 and `enable` = 1", new { goodsGuid, projectId });
            }
        }

        /// <summary>
        /// 通过订单明细Id集合获取商品卡集合
        /// </summary>
        /// <param name="orderDetailIds">订单明细Id集合</param>
        /// <returns></returns>
        public async Task<List<GoodsItemModel>> GetByOrderDetailIdsAsync(IEnumerable<string> orderDetailIds)
        {
            var distinctIds = orderDetailIds.Distinct();
            var sql = @"SELECT
	                        b.* 
                        FROM
	                        t_consumer_goods a
	                        INNER JOIN t_consumer_goods_item b ON a.goods_guid = b.goods_guid 
	                        AND a.`enable` = b.`enable` 
                        WHERE
	                        a.detail_guid in @distinctIds AND a.`enable` =1";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<GoodsItemModel>(sql, new { distinctIds });
                return result.ToList();
            }
        }
    }
}
