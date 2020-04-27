using Dapper;
using GD.DataAccess;
using GD.Dtos.Health;
using GD.Models.Mall;
using GD.Models.Questionnaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Health
{
    /// <summary>
    /// 用户问卷业务类
    /// </summary>
    public class QuestionnaireResultBiz : BizBase.BaseBiz<QuestionnaireResultModel>
    {
        /// <summary>
        /// 获取用户问卷列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<GetHealthQuestionnairePageListResponseDto> GetHealthQuestionnaireListAsync(GetHealthQuestionnairePageListRequestDto requestDto, string userId)
        {
            string sqlWhere = string.Empty;
            if (!requestDto.FillStatus)
            {
                sqlWhere = $" and (b.fill_status={requestDto.FillStatus} or  b.fill_status is null) ";
            }
            else
            {
                sqlWhere = $" and (b.fill_status={requestDto.FillStatus} ) ";
            }
            var sql = $@"SELECT
                            a.questionnaire_guid,
	                        b.result_guid,
	                        a.questionnaire_name,
                            a.subhead,
                            a.has_depend,
	                        b.fill_status,
	                        b.commented,
	                        b.`comment`,
                            d.questioncount,
	                        d.useranswercount 
                        FROM
	                        t_questionnaire a
	                        LEFT JOIN t_questionnaire_result b ON a.questionnaire_guid = b.questionnaire_guid AND   b.user_guid ='{userId}' and b.`enable` = 1 
                            LEFT JOIN t_consumer_questionnaire_hide c on c.questionnaire_guid=a.questionnaire_guid    AND  c.user_guid ='{userId}'
                            LEFT JOIN (
	                                  	SELECT
		                                o.questionnaire_guid,
		                                o.questioncount,
		                                m.useranswercount 
	                                FROM
		                                (
		                                SELECT
			                                f.questionnaire_guid,
			                                count( 1 ) questioncount 
		                                FROM
			                                t_questionnaire_question f
			                                INNER JOIN t_questionnaire g ON f.questionnaire_guid = g.questionnaire_guid 
		                                WHERE
			                                g.has_depend = FALSE 
		                                GROUP BY
			                                f.questionnaire_guid 
		                                ) o
		                                LEFT JOIN (
		                                SELECT
			                                questionnaire_guid,
			                                count( 1 ) useranswercount 
		                                FROM
			                                t_questionnaire_result j
			                                LEFT JOIN t_questionnaire_result_detail h ON j.result_guid = h.result_guid 
		                                WHERE
			                                j.user_guid = '{userId}' and h.`enable` = 1
		                                GROUP BY
			                                j.questionnaire_guid 
		                                ) m ON o.questionnaire_guid = m.questionnaire_guid 
	                                    ) d ON d.questionnaire_guid = a.questionnaire_guid 
	                                                            WHERE c.hide_guid is null 
                                                                AND a.`enable` = 1 
                                                                AND a.issuing_status=1
	                                                            AND a.display = 1 {sqlWhere}
                                                                ORDER BY a.creation_date desc ";
            return await MySqlHelper.QueryByPageAsync<GetHealthQuestionnairePageListRequestDto, GetHealthQuestionnairePageListResponseDto, GetHealthQuestionnairePageListItemDto>(sql, requestDto);
        }
        /// <summary>
        /// 获取当前用户问卷状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="questionnaireGuid"></param>
        /// <returns></returns>
        public async Task<QuestionnaireResultModel> GetQuestionnaireResultModelAsync(string userId, string questionnaireGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<QuestionnaireResultModel>("where user_guid=@userId and questionnaire_guid=@questionnaireGuid and `enable`= 1", new { userId, questionnaireGuid });
                return result?.FirstOrDefault();
            }
        }
        public async Task<bool> DeleteQuestionnaireAsync(QuestionnaireHideModel questionnaireHideModel, QuestionnaireResultModel questionnaireResultModel)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                //新增删除标记表
                questionnaireHideModel.Insert(conn);
                if (questionnaireResultModel != null)
                {
                    //删除主表用户数据
                    await conn.DeleteAsync(questionnaireResultModel);
                    //删除明细
                    await conn.DeleteListAsync<QuestionnaireResultDetailModel>("where result_guid=@id", new { id = questionnaireResultModel.ResultGuid });
                }
                return true;
            });
        }
    }
}
