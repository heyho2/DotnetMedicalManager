using Dapper;
using GD.DataAccess;
using GD.Dtos.WechatSubscription;
using GD.Models.Consumer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GD.Manager.Consumer
{
    /// <summary>
    /// 消费者表模型
    /// </summary>
    public class WechatSubscriptionBiz : BaseBiz<WechatSubscriptionModel>
    {
        /// <summary>
        /// 获取医生推广量
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WechatSubscriptionRecommendItemsDto>> GetWechatSubscriptionRecommendCountsAsync(WechatSubscriptionModel.EntranceEnum entrance, string[] userGuids)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"
SELECT
    recommend_user_guid as UserGuid,
	count( subscription_guid ) as Count
FROM
	t_consumer_wechat_subscription 
WHERE
	recommend_user_guid in @userGuids 
	AND entrance = '{entrance.ToString()}' 
	AND `enable` =1
GROUP BY
	recommend_user_guid
";
                var result = await conn.QueryAsync<WechatSubscriptionRecommendItemsDto>(sql, new { userGuids });
                return result;
            }
        }
    }
}
