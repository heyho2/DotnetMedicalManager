using Dapper;
using GD.DataAccess;
using GD.Dtos.Health;
using GD.Models.Consumer;
using GD.Models.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Health
{
    /// <summary>
    /// 基础信息业务类
    /// </summary>
    public class HealthInformationBiz : BizBase.BaseBiz<HealthInformationModel>
    {
        /// <summary>
        /// 获取用户健康基础信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<GetHealthInformationResponseDto>> GetHealthInformationList(string userId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT
	                            a.information_guid,
	                            a.information_type,
	                            a.subject_name,
	                            a.subject_unit,
	                            a.subject_prompt_text,
                                a.is_single_line,
	                            b.option_guids,
	                            b.result_value 
                            FROM
	                            t_health_information a
	                            LEFT JOIN t_consumer_health_info b ON a.information_guid = b.information_guid 
	                            AND a.`enable` = 1 and  b.user_guid=@userId
                            ORDER BY
	                            a.sort";
                var healthInformationList = await conn.QueryAsync<GetHealthInformationResponseDto>(sql, new { userId });
                return healthInformationList?.ToList();
            }
        }
        /// <summary>
        /// 删除用户基础信息数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUserHealthInformation(string userId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"DELETE
	                            b
                            FROM
	                            t_health_information a 
	                            INNER JOIN t_consumer_health_info b on a.information_guid=b.information_guid 
                            WHERE
	                            a.`enable` = 1  and a.information_type<>b.information_type and b.user_guid=@userId";
                var healthInformationList = await conn.ExecuteAsync(sql, new { userId });
                return healthInformationList > 0;
            }
        }
        /// <summary>
        /// 获取基础健康信息问题选项
        /// </summary>
        /// <param name="informationGuid"></param>
        /// <returns></returns>
        public async Task<List<HealthInformationOptionModel>> GetHealthInformationOptionAsync(string informationGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.GetListAsync<HealthInformationOptionModel>("where information_guid=@informationGuid and `enable`= 1", new { informationGuid })).ToList();
            }
        }
        /// <summary>
        /// 查找用户信息问题
        /// </summary>
        /// <param name="informationGuid"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ConsumerHealthInfoModel> GetConsumerHealthInfoAsync(string informationGuid, string userId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<ConsumerHealthInfoModel>(@"
                   select * from t_consumer_health_info where information_guid = @informationGuid and user_guid = @userId and `enable`=1", new { informationGuid, userId });
            }
        }
    }
}
