using Dapper;
using GD.DataAccess;
using GD.Dtos.Health;
using GD.Models.Consumer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Manager.Consumer
{
    public class IndicatorWarningLimitBiz : BaseBiz<IndicatorWarningLimitModel>
    {
        public async Task<List<IndicatorWarningLimitModel>> GetLimits(string userGuid)
        {
            var sql = @"select * from t_consumer_indicator_warning_limit where user_guid = @userGuid";

            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryAsync<IndicatorWarningLimitModel>(sql, new
                {
                    userGuid
                })).ToList();
            }
        }


        public async Task<bool> CreateOrUpdateLimits(
            List<IndicatorWarningLimitModel> createModels,
            List<IndicatorWarningLimitModel> updateModels)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                if (createModels.Count > 0)
                {
                    var result = createModels.InsertBatch(conn);

                    if (result <= 0)
                    {
                        return false;
                    }
                }

                if (updateModels.Count > 0)
                {
                    await BatchUpdateLimits(conn, updateModels);
                }

                return true;
            });
        }

        public async Task<bool> BatchUpdateLimits(MySql.Data.MySqlClient.
           MySqlConnection conn, List<IndicatorWarningLimitModel> accounts)
        {
            var sql = @"update t_consumer_indicator_warning_limit 
                        set turn_on_warning = @TurnOnWarning,
                            min_value = @MinValue,
                            max_value = @MaxValue,
                            last_updated_by = @LastUpdatedBy,
                            last_updated_date = CURRENT_TIMESTAMP
                        where limit_guid = @LimitGuid";

            return await conn.ExecuteAsync(sql, accounts) > 0;
        }

        public async Task<List<GetIndicatorWarningLimit>> GetIndicatorWarningLimits(GetIndicatorWarningLimitRequestDto request)
        {
            var sql = @"select distinct
							i.option_guid as OptionGuid,
							i.option_name as OptionName,
                            i.option_unit as OptionUnit,
							ifnull(l.min_value,0) as `MinValue`,
							ifnull(l.max_value,0) as `MaxValue`,
							ifnull(l.turn_on_warning,false) as TurnOnWarning
					  from  t_health_indicator_option as i
						left join t_consumer_indicator_warning_limit as l on i.option_guid = l.indicator_option_guid and l.user_guid = @ConsumerGuid
					where i.indicator_guid = @IndicatorGuid";

            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryAsync<GetIndicatorWarningLimit>(sql, new
                {
                    request.ConsumerGuid,
                    request.IndicatorGuid
                })).ToList();
            }
        }
        /// <summary>
        /// 获取用户所有预警值数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task<List<IndicatorWarningLimitModel>> GetModelAsyncByUser(string userId, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryAsync<IndicatorWarningLimitModel>("SELECT *  FROM t_consumer_indicator_warning_limit where user_guid=@UserId  and turn_on_warning=1 and `enable`=@enable", new { UserId = userId, enable })).ToList();
            }
        }
    }
}
