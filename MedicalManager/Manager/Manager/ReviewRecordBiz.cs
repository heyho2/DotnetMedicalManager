using GD.DataAccess;
using GD.Dtos.ReviewRecord;
using GD.Models.Manager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace GD.Manager.Manager
{
    public class ReviewRecordBiz : BaseBiz<ReviewRecordModel>
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
    }
}
