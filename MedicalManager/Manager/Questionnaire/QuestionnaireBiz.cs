using GD.DataAccess;
using GD.Dtos.Questionnaire;
using GD.Models.Questionnaire;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace GD.Manager.Questionnaire
{
    /// <summary>
    /// 问卷业务类
    /// </summary>
    public class QuestionnaireBiz : BaseBiz<QuestionnaireModel>
    {
        /// <summary>
        /// 获取问卷分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetQuestionnairePageListResponseDto> GetQuestionnairePageListAsync(GetQuestionnairePageListRequestDto requestDto)
        {
            var sqlWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(requestDto.Name))
            {
                sqlWhere = "and a.questionnaire_name like @Name";
                requestDto.Name = $"{requestDto.Name}%";
            }
            var sql = $@"SELECT
	                        a.questionnaire_guid,
	                        a.questionnaire_name,
	                        b.user_name as creator,
	                        a.creation_date,
	                        a.issuing_date,
	                        a.display,
	                        a.issuing_status 
                        FROM
	                        t_questionnaire a
	                        INNER JOIN t_manager_account b ON a.created_by = b.user_guid 
                        WHERE
	                        a.`enable` = 1 {sqlWhere}
                        ORDER BY
	                        a.issuing_status,
	                        a.issuing_date DESC,
	                        a.creation_date DESC";
            var result = await MySqlHelper.QueryByPageAsync<GetQuestionnairePageListRequestDto, GetQuestionnairePageListResponseDto, GetQuestionnairePageListItemDto>(sql, requestDto);
            return result;
        }

        /// <summary>
        /// 删除问卷
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> DeleteQuestionnaireAsync(QuestionnaireModel model)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                await conn.DeleteAsync(model);
                await conn.DeleteListAsync<QuestionnaireQuestionModel>("where questionnaire_guid=@QuestionnaireGuid", new { model.QuestionnaireGuid });
                await conn.DeleteListAsync<QuestionnaireAnswerModel>("where questionnaire_guid=@QuestionnaireGuid", new { model.QuestionnaireGuid });
                return true;
            });
        }
    }
}
