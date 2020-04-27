using Dapper;
using GD.BizBase;
using GD.DataAccess;
using GD.Models.Mall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Consumer
{
    /// <summary>
    /// 问卷删除业务类
    /// </summary>
    public class QuestionnaireHideBiz : BaseBiz<QuestionnaireHideModel>
    {
        /// <summary>
        /// 获取当前用户删除问卷记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="questionnaireGuid"></param>
        /// <returns></returns>
        public async Task<QuestionnaireHideModel> GetQuestionnaireHideModelAsync(string userId, string questionnaireGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<QuestionnaireHideModel>("where user_guid=@userId and questionnaire_guid=@questionnaireGuid and `enable`= 1", new { userId, questionnaireGuid });
                return result?.FirstOrDefault();
            }
        }
    }
}
