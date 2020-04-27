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
    /// 用户问卷答题详情
    /// </summary>
    public class QuestionnaireResultDetailBiz : BizBase.BaseBiz<QuestionnaireResultDetailModel>
    {
        /// <summary>
        /// 获取用户问卷最新的答题记录
        /// </summary>
        /// <param name="resultGuid"></param>
        /// <returns></returns>
        public async Task<QuestionnaireResultDetailModel> QuestionnaireResultDetailModelAsync(string resultGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT
	                            *
                            FROM
	                            t_questionnaire_result_detail a
	                            where  a.`enable` = 1 and  a.result_guid=@resultGuid
                            ORDER BY
	                            a.sort DESC Limit 1";
                var questionnaireResultDetailList = await conn.QueryAsync<QuestionnaireResultDetailModel>(sql, new { resultGuid });
                return questionnaireResultDetailList?.FirstOrDefault();
            }
        }
        /// <summary>
        /// 查找用户是否做了指定题目
        /// </summary>
        /// <param name="resultGuid"></param>
        /// <param name="questionGuid">题目Id</param>
        /// <returns></returns>
        public async Task<QuestionnaireResultDetailModel> AppointQuestionnaireResultDetailModelAsync(string resultGuid, string questionGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT
	                            *
                            FROM
	                            t_questionnaire_result_detail a
	                            where  a.`enable` = 1 and  a.result_guid=@resultGuid and a.question_guid=@questionGuid";
                var questionnaireResultDetailList = await conn.QueryAsync<QuestionnaireResultDetailModel>(sql, new { resultGuid, questionGuid });
                return questionnaireResultDetailList?.FirstOrDefault();
            }
        }
        /// <summary>
        /// 查找用户是否做了指定题目
        /// </summary>
        /// <param name="resultGuid"></param>
        /// <param name="questionGuid">题目Id</param>
        /// <returns></returns>
        public async Task<QuestionnaireResultDetailModel> CheckAppointQuestionnaireResultDetailModelAsync(string resultGuid, string questionGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT
	                            *
                            FROM
	                            t_questionnaire_result_detail a
	                            where  a.result_guid=@resultGuid and a.question_guid=@questionGuid";
                var questionnaireResultDetailList = await conn.QueryAsync<QuestionnaireResultDetailModel>(sql, new { resultGuid, questionGuid });
                return questionnaireResultDetailList?.FirstOrDefault();
            }
        }
        /// <summary>
        /// 查找用户指定顺序题目
        /// </summary>
        /// <param name="resultGuid"></param>
        /// <param name="questionGuid">题目Id</param>
        /// <returns></returns>
        public async Task<QuestionnaireResultDetailModel> AppointOrderQuestionnaireResultDetailModelAsync(string resultGuid, int sort)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT
	                            *
                            FROM
	                            t_questionnaire_result_detail a
	                            where  a.`enable` = 1 and  a.result_guid=@resultGuid and a.sort=@sort";
                var questionnaireResultDetailList = await conn.QueryAsync<QuestionnaireResultDetailModel>(sql, new { resultGuid, sort });
                return questionnaireResultDetailList?.FirstOrDefault();
            }
        }
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
        /// <summary>
        /// 获取当前用户答卷题目数
        /// </summary>
        /// <param name="resultGuid"></param>
        /// <returns></returns>
        public async Task<int> GetQuestionnaireResultDetailCount(string resultGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT 
		                           count(1)
	                            FROM
		                           t_questionnaire_result_detail a
	                            where  a.`enable` = 1 and  a.result_guid=@resultGuid ";
                var count = await conn.QueryFirstOrDefaultAsync<int>(sql, new { resultGuid });
                return count;
            }
        }
    }
}
