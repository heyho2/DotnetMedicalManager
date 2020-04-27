using GD.BizBase;
using GD.DataAccess;
using GD.Models.Health;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Threading.Tasks;
using System.Linq;
using GD.Dtos.Health.HealthManager;
using GD.Dtos.Health;
using GD.Models.Consumer;

namespace GD.Health
{
    public class HealthManagerBiz : BaseBiz<HealthManagerModel>
    {
        /// <summary>
        /// 通过工号查询健康管理师
        /// </summary>
        /// <param name="jobNumber">工号</param>
        /// <returns></returns>
        public async Task<HealthManagerModel> GetByJobNumberAsync(string jobNumber)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<HealthManagerModel>("where job_number=@jobNumber and `enable`=1", new { jobNumber });
                return result.FirstOrDefault();
            }
        }
        /// <summary>
        /// 获取绑定用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetHealthManagerConsumerListResponseDto> GetHealthManagerConsumers(GetHealthManagerConsumerListRequestDto request)
        {
            var sql = @"select 
                            c.consumer_guid,
						    u.user_name,
						    u.gender,u.phone,
                            b.maxdate,
                            c.manager_bind_date,
                            e.reportmaxdate,
							(
                            IF(u.birthday IS NULL, '0', TIMESTAMPDIFF(YEAR, u.birthday, CURDATE())) 
							) AS age,
                        concat(p.base_path, p.relative_path) as portrait_img
                        from t_consumer as c
                        inner join t_utility_user AS u on u.user_guid = c.consumer_guid
	                    LEFT JOIN (SELECT Max(a.last_updated_date) maxdate,a.user_guid FROM t_consumer_indicator a  GROUP BY a.user_guid) b on c.consumer_guid=b.user_guid
                       LEFT JOIN (SELECT Max(a.last_updated_date) reportmaxdate,a.user_guid FROM t_consumer_health_report a  GROUP BY a.user_guid) e on c.consumer_guid=e.user_guid
                        left join t_utility_accessory AS p on u.portrait_guid = p.accessory_guid
                        where c.health_manager_guid = @UserId";
            if (!string.IsNullOrWhiteSpace(request.KeyWord))
            {
                sql = $"{sql} AND (u.user_name like @KeyWord or u.phone like @KeyWord)";
                request.KeyWord = $"{request.KeyWord}%";
            }
            return await MySqlHelper.QueryByPageAsync<GetHealthManagerConsumerListRequestDto, GetHealthManagerConsumerListResponseDto, GetHealthManagerConsumerItem>(sql, request);
        }
        /// <summary>
        /// 获取可以绑定用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<GetBindResponseDto>> GetBindConsumers(GetBindRequestDto request)
        {
            var sql = @"select 
                            c.consumer_guid,
						    u.user_name,
						    u.gender,u.phone,
							(
                            IF(u.birthday IS NULL, '-', TIMESTAMPDIFF(YEAR, u.birthday, CURDATE())) 
							) AS age,
                        concat(p.base_path, p.relative_path) as portrait_img
                        from t_consumer as c
                        inner join t_utility_user AS u on u.user_guid = c.consumer_guid
                        left join t_utility_accessory AS p on u.portrait_guid = p.accessory_guid
                        where c.health_manager_guid is null ";
            using (var conn = MySqlHelper.GetConnection())
            {
                if (!string.IsNullOrWhiteSpace(request.KeyWord))
                {
                    sql = $"{sql} AND (u.user_name like @KeyWord or u.phone like @KeyWordPhone)";
                    request.KeyWord = $"{request.KeyWord}%";
                }

                return (await conn.QueryAsync<GetBindResponseDto>(sql, new { KeyWord = request.KeyWord, KeyWordPhone = request.KeyWord })).ToList();
            }
        }
        /// <summary>
        /// 批量会员绑定健康管理师
        /// </summary>
        /// <param name="consumerGuid"></param>
        /// <returns></returns>
        public async Task<bool> BatchBindHealthManager(
            BatchUpdateConsumerBindMangerRequestDto request)
        {
            var models = request.ConsumerGuids.Select(d => new ConsumerModel()
            {
                ConsumerGuid = d,
                HealthManagerGuid = request.ManagerGuid
            });

            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                var sql = @"update t_consumer
                      set  
                            `health_manager_guid` = @HealthManagerGuid,
                            `manager_bind_date` = CURRENT_TIMESTAMP
                      where consumer_guid = @ConsumerGuid and `health_manager_guid` is null";

                var result = await conn.ExecuteAsync(sql, models);

                return true;
            });
        }
    }
}
