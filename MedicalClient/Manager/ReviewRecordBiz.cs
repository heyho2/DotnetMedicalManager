using GD.DataAccess;
using GD.Dtos.Admin.ReviewRecord;
using GD.Models.Manager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace GD.Manager
{
    public class ReviewRecordBiz
    {
        /// <summary>
        /// 审核记录
        /// </summary>
        /// <param name="ownerGuid"></param>
        /// <returns></returns>
        public async Task<GetReviewRecordPageResponseDto> GetReviewRecordPageAsync(GetReviewRecordPageRequestDto request)
        {
            var sqlWhere = $@"AND ENABLE = 1 and owner_guid=@OwnerGuid and type=@Type";
            var sqlOrderBy = "creation_date desc";
            var sql = $@"
SELECT * FROM
    t_manager_review_record
 WHERE
	1 = 1 {sqlWhere}
ORDER BY
	{sqlOrderBy}";

            return await MySqlHelper.QueryByPageAsync<GetReviewRecordPageRequestDto, GetReviewRecordPageResponseDto, GetReviewRecordPageItemDto>(sql, request);
        }

        /// <summary>
        /// 获取目标最新审批记录
        /// </summary>
        /// <param name="targetGuid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<ReviewRecordModel> GetLatestReviewRecordByTargetGuidAsync(string targetGuid, string type)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            *
                            FROM
	                            t_manager_review_record
	                            where owner_guid=@targetGuid and type=@type and `enable`=1
	                            order by last_updated_date desc
                            limit 1";
                return await conn.QueryFirstOrDefaultAsync<ReviewRecordModel>(sql, new { targetGuid, type });
            }
        }
    }
}
