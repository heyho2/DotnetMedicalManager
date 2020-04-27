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
    /// 
    /// </summary>
    public class QuestionnaireResultDetailBiz : BaseBiz<QuestionnaireResultDetailModel>
    {
        /// <summary>
        /// 通过用户答卷结果guid获取结果详情
        /// </summary>
        /// <returns></returns>
        public async Task<List<QuestionnaireResultDetailModel>> GetModelsByResultGuidAsync(string resultGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<QuestionnaireResultDetailModel>("where result_guid=@resultGuid and `enable`=1", new { resultGuid });
                return result.ToList();
            }
        }
    }
}
