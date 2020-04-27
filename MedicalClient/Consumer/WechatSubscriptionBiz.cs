using Dapper;
using GD.BizBase;
using GD.DataAccess;
using GD.Models.Consumer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GD.Consumer
{
    public class WechatSubscriptionBiz : BaseBiz<WechatSubscriptionModel>
    {
        /// <summary>
        /// 获取用户最新的关注记录
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public async Task<WechatSubscriptionModel> GetLatestRecommendRecordAsync(string openid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<WechatSubscriptionModel>("select * from t_consumer_wechat_subscription where open_id=@openid and `enable`=1 order by creation_date desc limit 1", new { openid });
            }
        }
    }
}
