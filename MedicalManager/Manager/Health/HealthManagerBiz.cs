using Dapper;
using GD.DataAccess;
using GD.Dtos.Health;
using GD.Models.Consumer;
using GD.Models.Health;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Manager.Health
{
    public class HealthManagerBiz : BaseBiz<HealthManagerModel>
    {
        /// <summary>
        /// 校验手机号是否已存在
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public async Task<bool> CheckPhone(string phone, string managerGuid = null)
        {
            var sql = "SELECT 1 FROM t_health_manager WHERE phone = @phone";

            if (!string.IsNullOrEmpty(managerGuid))
            {
                sql = $" {sql} and manager_guid <> @managerGuid";
            }

            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.ExecuteScalarAsync(sql, new
                {
                    phone,
                    managerGuid
                })) != null;
            }
        }

        /// <summary>
        /// 校验工号是否已存在
        /// </summary>
        /// <param name="jobNumber"></param>
        /// <returns></returns>
        public async Task<bool> CheckJobNumber(string jobNumber, string managerGuid = null)
        {
            var sql = "SELECT 1 FROM t_health_manager WHERE job_number = @jobNumber";

            if (!string.IsNullOrEmpty(managerGuid))
            {
                sql = $" {sql} and manager_guid <> @managerGuid";
            }

            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.ExecuteScalarAsync(sql, new
                {
                    jobNumber,
                    managerGuid
                })) != null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="managerGuid"></param>
        /// <returns></returns>
        public async Task<GetHealthManagerResponseDto> GetManagerInfoAsync(string managerGuid)
        {
            var response = new GetHealthManagerResponseDto();

            var sql = @"select 
                            m.manager_guid,
                            m.user_name,
						    m.gender,
                            m.phone,
                            m.province,
						    m.city,
                            m.district,
                            m.identity_number,
						    m.job_number,m.occupation_grade,
						    m.portrait_guid,
                            concat(p.base_path, p.relative_path) as portrait_img,
							m.qualification_certificate_guid,
                            m.`enable`
                        from t_health_manager as m
                        left join t_utility_accessory AS p on m.portrait_guid = p.accessory_guid	
                        where m.manager_guid = @managerGuid";

            using (var conn = MySqlHelper.GetConnection())
            {
                response = await conn.QueryFirstOrDefaultAsync<GetHealthManagerResponseDto>(sql, new
                {
                    managerGuid
                });
            }

            if (string.IsNullOrEmpty(response.ManagerGuid)) { return response; }

            var certifacates = JsonConvert.DeserializeObject<string[]>(response.QualificationCertificateGuid);

            sql = @"
                    select 
                        accessory_guid as CertificateGuid,
                        concat(base_path, relative_path) as CertificateImg
                    from t_utility_accessory 
                    where accessory_guid in @certifacates";

            using (var conn = MySqlHelper.GetConnection())
            {
                response.Certificates = (await conn.QueryAsync<QualificateCertificate>(sql, new
                {
                    certifacates
                })).ToList();
            }

            return response;
        }

        /// <summary>
        /// 禁用或启用健康管理师
        /// </summary>
        /// <param name="consumerGuid"></param>
        /// <returns></returns>
        public async Task<bool> UpdateStatus(string managerGuid)
        {
            var sql = @"update t_health_manager
                      set  `enable` =  (IF(`enable` = 0, 1, 0))
                      where manager_guid = @managerGuid";

            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.ExecuteAsync(sql, new { managerGuid });

                return result > 0;
            }
        }

        /// <summary>
        /// 查询健康管理师分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetHealthManagerListResponseDto> GetHealthManagers(GetHealthManagerListRequestDto requestDto)
        {
            var sql = @"select 
                            m.manager_guid,
							m.user_name,
							m.gender,m.phone,
                            concat(p.base_path, p.relative_path) as portrait_img,
							m.creation_date as registration_time,
							m.`enable`,
							IFNULL(
                                (
                                SELECT count(t.health_manager_guid) 
                                FROM t_consumer as t 
                                WHERE t.health_manager_guid is NOT NULL AND t.health_manager_guid = m.manager_guid
                                ),0) 
                        as `count`
                        from t_health_manager as m
                        left join t_utility_accessory AS p on m.portrait_guid = p.accessory_guid
                        where 1 = 1";

            if (!string.IsNullOrEmpty(requestDto.KeyWord))
            {
                sql = $"{sql} and (m.user_name like '%{requestDto.KeyWord}%' or m.phone like '%{requestDto.KeyWord}%')";
            }

            if (requestDto.RegistrationTime.HasValue && requestDto.EndTime.HasValue)
            {
                requestDto.EndTime = requestDto.EndTime.Value.AddDays(1);

                sql = $"{sql} and m.creation_date >= @RegistrationTime and m.creation_date < @EndTime";
            }

            sql = $"{sql} order by m.creation_date desc";

            return await MySqlHelper.QueryByPageAsync<GetHealthManagerListRequestDto, GetHealthManagerListResponseDto, GetHealthManagerItem>(sql, requestDto);
        }

        /// <summary>
        /// 获取更换健康管理师分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetMealAccountListResponseDto<GetChangeHealthManagerItem>> GetChangeHealthManagers(GetChangeHealthManagerListRequestDto requestDto)
        {
            var parameters = new DynamicParameters();

            var response = new GetMealAccountListResponseDto<GetChangeHealthManagerItem>();

            var consumerManagerGuid = await GetConsumerHealthManager(requestDto.ConsumerGuid);

            var sql = $@"select 
                            m.manager_guid,
							m.user_name,
							m.gender,m.phone,
                            concat(p.base_path, p.relative_path) as portrait_img,
							m.creation_date as registration_time
                        from t_health_manager as m
                        left join t_utility_accessory AS p on m.portrait_guid = p.accessory_guid
                        where m.`enable` = 1";

            if (!string.IsNullOrEmpty(requestDto.KeyWord))
            {
                sql = $"{sql} and (m.user_name like '%{requestDto.KeyWord}%' or m.phone like '%{requestDto.KeyWord}%')";
            }

            if (requestDto.RegistrationTime.HasValue && requestDto.EndTime.HasValue)
            {
                requestDto.EndTime = requestDto.EndTime.Value.AddDays(1);

                sql = $"{sql} and m.creation_date >= @RegistrationTime and m.creation_date < @EndTime";

                parameters.Add("@RegistrationTime", requestDto.RegistrationTime);
                parameters.Add("@EndTime", requestDto.EndTime);
            }

            sql = $"{sql} order by m.creation_date desc";

            var items = (List<GetChangeHealthManagerItem>)null;

            using (var conn = MySqlHelper.GetConnection())
            {
                items = (await conn.QueryAsync<GetChangeHealthManagerItem>(sql, parameters)).ToList();
            }

            if (items is null || items.Count <= 0) { return response; }

            if (!string.IsNullOrEmpty(consumerManagerGuid))
            {
                #region 若在第一页则将指定会员健康管理师移至首位
                var consumerIndex = items.FindIndex(d => d.ManagerGuid.Equals(consumerManagerGuid));

                if (consumerIndex > -1)
                {
                    var item = items[consumerIndex];

                    items.RemoveAt(consumerIndex);

                    item.Default = true;

                    items.Insert(0, item);
                }
                #endregion
            }

            response.Total = items.Count;

            items = items.Skip((requestDto.PageIndex - 1) * requestDto.PageSize)
                .Take(requestDto.PageSize).ToList();

            response.CurrentPage = items;

            return response;
        }

        /// <summary>
        /// 获取指定会员健康管理师
        /// </summary>
        /// <param name="consumerGuid"></param>
        /// <returns></returns>
        public async Task<string> GetConsumerHealthManager(string consumerGuid)
        {
            var sql = @"select health_manager_guid from t_consumer
                        where  consumer_guid = @consumerGuid";

            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.ExecuteScalarAsync<string>(sql, new { consumerGuid });
            }
        }

        /// <summary>
        /// 查询指定健康管理师已绑定会员分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetHealthManagerConsumerListResponseDto> GetHealthManagerConsumers(GetHealthManagerConsumerListRequestDto requestDto)
        {
            var sql = @"select 
                            c.consumer_guid,
						    u.user_name,
						    u.gender,u.phone,
							(
                            IF(u.birthday IS NULL, '-', TIMESTAMPDIFF(YEAR, u.birthday, CURDATE())) 
							) AS age,
						    c.manager_bind_date as bind_date 	
                        from t_consumer as c
                        inner join t_utility_user AS u on u.user_guid = c.consumer_guid
                        where c.health_manager_guid = @ManagerGuid";

            if (!string.IsNullOrEmpty(requestDto.KeyWord))
            {
                sql = $"{sql} and (u.user_name like '%{requestDto.KeyWord}%' or u.phone like '%{requestDto.KeyWord}%')";
            }

            if (requestDto.BindTime.HasValue)
            {
                if (requestDto.EndTime.HasValue)
                {
                    requestDto.EndTime = requestDto.EndTime.Value.AddDays(1).AddSeconds(-1);
                }
                else
                {
                    requestDto.EndTime = requestDto.BindTime.Value.AddDays(1).AddSeconds(-1);
                }

                sql = $"{sql} and c.manager_bind_date >= @BindTime and c.manager_bind_date <= @EndTime";
            }

            sql = $"{sql} order by c.manager_bind_date desc";

            return await MySqlHelper.QueryByPageAsync<GetHealthManagerConsumerListRequestDto, GetHealthManagerConsumerListResponseDto, GetHealthManagerConsumerItem>(sql, requestDto);
        }

        /// <summary>
        /// 取消/解除会员绑定健康管理师
        /// </summary>
        /// <param name="consumerGuid"></param>
        /// <returns></returns>
        public async Task<bool> CancelBindHealthManager(string consumerGuid)
        {
            var sql = @"update t_consumer
                      set  
                            `health_manager_guid` = NULL,
                            `manager_bind_date` = NULL
                      where consumer_guid = @consumerGuid";

            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.ExecuteAsync(sql, new { consumerGuid });

                return result > 0;
            }
        }

        /// <summary>
        /// 会员绑定健康管理师
        /// </summary>
        /// <param name="consumerGuid"></param>
        /// <returns></returns>
        public async Task<bool> BindHealthManager(UpdateConsumerBindMangerRequestDto request)
        {
            var sql = @"update t_consumer
                      set  
                            `health_manager_guid` = @ManagerGuid,
                            `manager_bind_date` = CURRENT_TIMESTAMP
                      where consumer_guid = @ConsumerGuid";

            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.ExecuteAsync(sql,
                    new { request.ConsumerGuid, request.ManagerGuid });

                return result > 0;
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

        /// <summary>
        /// 获取未被禁用健康管理师简单列表信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<GetSimpleHealthManagerResponseDto>> GetSimpleHealthManagers()
        {
            var sql = @"select manager_guid,user_name from t_health_manager where `enable` = 1";

            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryAsync<GetSimpleHealthManagerResponseDto>(sql)).ToList();
            }
        }

        /// <summary>
        /// 查询指定会员随访记录分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetHealthManagerFllowupRecordsResponseDto> GetHealthManagerFllowupRecords(GetHealthManagerFllowupRecordsRequestDto requestDto)
        {
            var sql = @"SELECT 
	                        m.user_name,m.phone,f.content,f.suggestion,f.followup_time
                        FROM t_health_manager_followup_record as f
	                        INNER JOIN t_health_manager as m ON m.manager_guid = f.health_manager_guid
                        WHERE f.consumer_guid = @ConsumerGuid";

            if (!string.IsNullOrEmpty(requestDto.KeyWord))
            {
                sql = $"{sql} and (m.user_name like '%{requestDto.KeyWord}%' or m.phone like '%{requestDto.KeyWord}%')";
            }

            if (requestDto.FollowUpTime.HasValue && requestDto.EndTime.HasValue)
            {
                requestDto.EndTime = requestDto.EndTime.Value.AddDays(1);

                sql = $"{sql} and f.followup_time >= @FollowUpTime and f.followup_time < @EndTime";
            }

            sql = $"{sql} order by f.followup_time desc";

            return await MySqlHelper.QueryByPageAsync<GetHealthManagerFllowupRecordsRequestDto, GetHealthManagerFllowupRecordsResponseDto, GetHealthManagerFllowupRecordsItem>(sql, requestDto);
        }
    }
}
