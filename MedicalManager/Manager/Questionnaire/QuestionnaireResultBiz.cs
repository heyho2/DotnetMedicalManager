using Dapper;
using GD.DataAccess;
using GD.Dtos.Questionnaire;
using GD.Models.Questionnaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Manager.Questionnaire
{
    public class QuestionnaireResultBiz : BaseBiz<QuestionnaireResultModel>
    {

        /// <summary>
        /// 获取某一个问卷下消费者答题结果分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetConsumerQuestionnairesPageListResponseDto> GetConsumerQuestionnairesPageListAsync(GetConsumerQuestionnairesPageListRequestDto requestDto)
        {
            var sqlWhere = string.Empty;
            if (requestDto.FillStatus != null)
            {
                sqlWhere = "and a.fill_status=@FillStatus";
            }
            if (requestDto.Commented != null)
            {
                sqlWhere = $"{sqlWhere} and a.commented=@Commented";
            }

            var sql = $@"SELECT
                            a.result_guid,
	                        b.user_name,
	                        b.phone,
	                        CASE
		                        WHEN b.birthday IS NULL THEN
		                        NULL ELSE TIMESTAMPDIFF(
			                        YEAR,
			                        date( b.birthday ),
			                        date(
			                        NOW())) 
	                        END AS age ,
	                        a.fill_status,
	                        a.commented
                        FROM
	                        t_questionnaire_result a
	                        INNER JOIN t_utility_user b ON a.user_guid = b.user_guid 
                        WHERE
	                        a.questionnaire_guid = @QuestionnaireGuid {sqlWhere}
	                        order by a.fill_status desc ,a.commented";
            var result = await MySqlHelper.QueryByPageAsync<GetConsumerQuestionnairesPageListRequestDto, GetConsumerQuestionnairesPageListResponseDto, GetConsumerQuestionnairesPageListItemDto>(sql, requestDto);
            return result;
        }
    }
}
