using Dapper;
using GD.BizBase;
using GD.DataAccess;
using GD.Dtos.Common;
using GD.Dtos.Health.HealthManager;
using GD.Models.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Health
{
    /// <summary>
    /// 健康管理师随访记录业务类
    /// </summary>
    public class FollowupRecordBiz : BaseBiz<FollowupRecordModel>
    {
        /// <summary>
        /// 获取随访记录分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetFollowupRecordPageListResponseDto> GetFllowupRecordPageListAsync(GetFllowupRecordPageListRequestDto requestDto)
        {
            var sqlWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(requestDto.OperatorId))
            {
                sqlWhere = "and a.created_by=@OperatorId";
            }
            var sql = $@"SELECT
	                a.followup_guid,
	                a.followup_time,
	                b.user_name AS operator,
	                a.content,
	                a.suggestion 
                FROM
	                t_health_manager_followup_record a
	                INNER JOIN t_health_manager b ON a.health_manager_guid = b.manager_guid 
                WHERE
	                a.consumer_guid = @ConsumerGuid 
	                AND a.`enable` = 1 {sqlWhere}
                ORDER BY
	                a.followup_time DESC";
            return await MySqlHelper.QueryByPageAsync<GetFllowupRecordPageListRequestDto, GetFollowupRecordPageListResponseDto, GetFollowupRecordPageListItemDto>(sql, requestDto);
        }

        /// <summary>
        /// 获取指定消费者下随访记录记录人列表
        /// </summary>
        /// <param name="consumerId"></param>
        /// <returns></returns>
        public async Task<List<KeyValueDto<string, string>>> GetFollowupOperatorsOfTheConsumerAsync(string consumerId)
        {
            using (var conn=MySqlHelper.GetConnection())
            {
                var sql = @"SELECT distinct
	                            b.manager_guid AS `Key`,
	                            b.user_name AS `Value` 
                            FROM
	                            t_health_manager_followup_record a
	                            INNER JOIN t_health_manager b ON a.health_manager_guid = b.manager_guid 
                            WHERE
	                            a.consumer_guid = @consumerId and a.`enable`=1
                            ORDER BY
	                            b.user_name";
                var result= await conn.QueryAsync<KeyValueDto<string, string>>(sql, new { consumerId });
                return result.ToList();
            }
        }
    }
}
