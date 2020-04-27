using Dapper;
using GD.DataAccess;
using GD.Models.Questionnaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Health
{
    /// <summary>
    /// 问题业务类
    /// </summary>
    public class QestionnaireQestionBiz : BizBase.BaseBiz<QuestionnaireQuestionModel>
    {
        /// <summary>
        /// 获取问卷第一个问题
        /// </summary>
        /// <param name="questionnaireGuid"></param>
        /// <returns></returns>
        public async Task<QuestionnaireQuestionModel> QuestionnaireQuestionModelAsync(string questionnaireGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT
	                            *
                            FROM
	                            t_questionnaire_question a
	                            where  a.`enable` = 1 and  a.questionnaire_guid=@questionnaireGuid
                            ORDER BY
	                            a.sort Limit 1";
                var questionnaireQuestionModelList = await conn.QueryAsync<QuestionnaireQuestionModel>(sql, new { questionnaireGuid });
                return questionnaireQuestionModelList?.FirstOrDefault();
            }
        }
        /// <summary>
        /// 获取问卷指定问题
        /// </summary>
        /// <param name="questionnaireGuid"></param>
        /// <returns></returns>
        public async Task<QuestionnaireQuestionModel> AppointQuestionnaireQuestionModelAsync(string questionnaireGuid, int sort)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT
	                            *
                            FROM
	                            t_questionnaire_question a
	                            where  a.`enable` = 1 and  a.questionnaire_guid=@questionnaireGuid and a.sort=@sort";
                var questionnaireQuestionModelList = await conn.QueryAsync<QuestionnaireQuestionModel>(sql, new { questionnaireGuid, sort });
                return questionnaireQuestionModelList?.FirstOrDefault();
            }
        }
        /// <summary>
        /// 通过问卷guid获取Model集合
        /// </summary>
        /// <returns></returns>
        public async Task<List<QuestionnaireQuestionModel>> GetModelsByQuestionnaireGuidAsync(string questionnaireGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<QuestionnaireQuestionModel>("where questionnaire_guid=@questionnaireGuid and `enable`=1", new { questionnaireGuid });
                return result.ToList();
            }
        }
        /// <summary>
        /// 获取依赖问题答案
        /// </summary>
        /// <param name="questionGuid"></param>
        /// <returns></returns>
        public async Task<List<string>> GetDependQuestionAnswerAsync(string questionGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT
	                            a.depend_answer
                            FROM
	                            t_questionnaire_question a
	                            where  a.`enable` = 1 and  a.depend_question=@questionGuid ";
                var questionnaireAnswerList = await conn.QueryAsync<string>(sql, new { questionGuid });
                return questionnaireAnswerList?.ToList();
            }
        }
    }
}
