using Dapper;
using GD.DataAccess;
using GD.Models.Consumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Health
{
    /// <summary>
    /// 用户健康指标
    /// </summary>
    public class ConsumerIndicatorBiz : BizBase.BaseBiz<ConsumerIndicatorModel>
    {
        public async Task<List<string>> GetConsumerOptions(string consumerGuid)
        {
            var sql = @"SELECT DISTINCT o.option_guid FROM t_consumer_indicator as i
		                INNER JOIN t_health_indicator_option as o ON o.indicator_guid = i.indicator_guid
                        WHERE i.user_guid = @consumerGuid";

            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryAsync<string>(sql, consumerGuid)).ToList();
            }
        }
    }
}
