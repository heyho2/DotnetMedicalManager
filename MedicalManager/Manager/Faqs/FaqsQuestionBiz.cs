using Dapper;
using GD.DataAccess;
using GD.Dtos.Faqs;
using GD.Models.FAQs;
using System.Threading.Tasks;

namespace GD.Manager.Faqs
{
    ///<summary>
    ///问答模块-问题
    ///</summary>
    public class FaqsQuestionBiz : BaseBiz<FaqsQuestionModel>
    {
        public async Task<SearchFaqsQuestionResponseDto> SearchFaqsQuestionAsync(SearchFaqsQuestionRequestDto request)
        {
            var sqlWhere = $@"AND a.enable=1";

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                request.Keyword = $"{request.Keyword}%";
                sqlWhere = $"{sqlWhere} AND a.content like @Keyword";
            }
            if (request.RewardType != null)
            {
                sqlWhere = $"{sqlWhere} AND a.reward_type = '{request.RewardType.ToString()}'";
            }
            if (!string.IsNullOrWhiteSpace(request.Phone))
            {
                request.Phone = $"{request.Phone}%";
                sqlWhere = $"{sqlWhere} AND (b.phone like @Phone or b.nick_name like @Phone or b.user_name like @Phone )";
            }
            var sql = $@"
SELECT
	a.question_guid,
	a.user_guid,
	a.STATUS,
	a.answer_num,
	a.content,
	a.attachment_guid_list,
	a.creation_date,
	a.ENABLE,
	a.reward_intergral,
	a.reward_type,
    b.user_name
FROM
	t_faqs_question a
left join t_utility_user b on a.user_guid=b.user_guid
WHERE
	1 = 1 {sqlWhere}
ORDER BY
	creation_date desc";
            return await MySqlHelper.QueryByPageAsync<SearchFaqsQuestionRequestDto, SearchFaqsQuestionResponseDto, SearchFaqsQuestionItemDto>(sql, request);
        }

        public override async Task<bool> DeleteAsync(string id)
        {
            var result = await MySqlHelper.TransactionAsync(async (conn, t) =>
            {
                await conn.DeleteAsync<FaqsQuestionModel>(id);
                await conn.DeleteListAsync<FaqsAnswerModel>("where question_guid=@id", new { id });
                return true;
            });
            return result;
        }
    }
}



