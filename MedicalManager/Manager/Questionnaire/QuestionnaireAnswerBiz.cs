using Dapper;
using GD.DataAccess;
using GD.Models.Questionnaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Manager.Questionnaire
{
    /// <summary>
    /// 问卷答案选项业务类
    /// </summary>
    public class QuestionnaireAnswerBiz : BaseBiz<QuestionnaireAnswerModel>
    {
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

        /// <summary>
        /// 通过问题guid获取答案选项
        /// </summary>
        /// <returns></returns>
        public async Task<List<QuestionnaireAnswerModel>> GetModelsByQuestionGuidAsync(string questionGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<QuestionnaireAnswerModel>("where question_guid=@questionGuid and `enable`=1", new { questionGuid });
                return result.ToList();
            }
        }



    }
}
