using Dapper;
using GD.DataAccess;
using GD.Dtos.Health;
using GD.Models.Questionnaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Health
{
    /// <summary>
    /// 问题答案业务类
    /// </summary>
    public class QuestionnaireAnswerBiz : BizBase.BaseBiz<QuestionnaireAnswerModel>
    {
        /// <summary>
        /// 查找对应问卷题目的答案
        /// </summary>
        /// <param name="questionnaireGuid"></param>
        /// <param name="questionGuid"></param>
        /// <returns></returns>
        public async Task<List<QuestionnaireAnswerDto>> GetQuestionnaireAnswerModelAsync(string questionnaireGuid, string questionGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT
	                            *
                            FROM
	                            t_questionnaire_answer a
	                            where  a.`enable` = 1 and  a.questionnaire_guid=@questionnaireGuid and a.question_guid=@questionGuid
                            ORDER BY
	                            a.sort desc";
                var questionnaireQuestionModelList = await conn.QueryAsync<QuestionnaireAnswerDto>(sql, new { questionnaireGuid, questionGuid });
                return questionnaireQuestionModelList?.ToList();
            }
        }
        /// <summary>
        /// 通过问卷guid获取答案选项
        /// </summary>
        /// <returns></returns>
        public async Task<List<QuestionnaireAnswerModel>> GetModelsByQuestionnaireGuidAsync(string questionnaireGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<QuestionnaireAnswerModel>("where questionnaire_guid=@questionnaireGuid and `enable`=1", new { questionnaireGuid });
                return result.ToList();
            }
        }
    }
}
