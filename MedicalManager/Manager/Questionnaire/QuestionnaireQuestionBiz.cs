using Dapper;
using GD.DataAccess;
using GD.Dtos.Questionnaire;
using GD.Manager.Common;
using GD.Models.Questionnaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Manager.Questionnaire
{
    /// <summary>
    /// 问卷问题业务类
    /// </summary>
    public class QuestionnaireQuestionBiz : BaseBiz<QuestionnaireQuestionModel>
    {
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
        /// 创建问题
        /// </summary>
        /// <param name="question"></param>
        /// <param name="answers"></param>
        /// <param name="isCreate"></param>
        /// <param name="cancelDependQuestionModels">解除依赖关系的问题列表</param>
        /// <returns></returns>
        public async Task<bool> CreateQuestionnaireQuestionAsync(QuestionnaireQuestionModel question, List<QuestionnaireAnswerModel> answers, bool isCreate, List<QuestionnaireQuestionModel> cancelDependQuestionModels)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {

                if (isCreate)
                {
                    await conn.InsertAsync<string, QuestionnaireQuestionModel>(question);
                    await conn.ExecuteAsync("update t_questionnaire_question set sort=sort+1 where questionnaire_guid=@questionnaireGuid and question_guid<>@questionGuid and sort>=@sort", new
                    {
                        questionnaireGuid = question.QuestionnaireGuid,
                        questionGuid = question.QuestionGuid,
                        sort = question.Sort
                    });
                }
                else
                {
                    await conn.UpdateAsync(question);
                    foreach (var item in cancelDependQuestionModels)
                    {
                        await conn.UpdateAsync(item);
                    }
                }

                await conn.DeleteListAsync<QuestionnaireAnswerModel>("where Question_Guid=@questionGuid", new { questionGuid = question.QuestionGuid });
                await conn.InsertListAsync(answers);
                return true;
            });
        }

        /// <summary>
        /// 通过被依赖问题guid获取问题集合
        /// </summary>
        /// <param name="dependQuestionGuid"></param>
        /// <returns></returns>
        public async Task<List<QuestionnaireQuestionModel>> GetModelsByDependQuestionGuidAsync(string dependQuestionGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<QuestionnaireQuestionModel>("where depend_question=@dependQuestionGuid and `enable`=1", new { dependQuestionGuid });
                return result.ToList();
            }
        }

        /// <summary>
        /// 移除问卷问题
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> RemoveQuestionAsync(QuestionnaireQuestionModel model)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                await conn.ExecuteAsync("update t_questionnaire_question set sort=sort-1 where questionnaire_guid=@questionnaireGuid and question_guid<>@questionGuid and sort>@sort", new
                {
                    questionnaireGuid = model.QuestionnaireGuid,
                    questionGuid = model.QuestionGuid,
                    sort = model.Sort
                });
                await conn.DeleteAsync(model);
                await conn.DeleteListAsync<QuestionnaireAnswerModel>("where question_guid=@questionGuid", new { questionGuid = model.QuestionGuid });
                return true;
            });
        }

        /// <summary>
        /// 获取当前问题可依赖的问题列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<List<GetQuestionListCanDependItemDto>> GetQuestionListCanDependAsync(GetQuestionListCanDependRequestDto requestDto)
        {
            var sql = @"SELECT
	                        a.question_guid,
	                        a.question_name,
	                        a.sort AS question_sort,
	                        b.answer_guid,
	                        b.answer_label,
	                        b.sort AS answer_sort 
                        FROM
	                        t_questionnaire_question a
	                        INNER JOIN t_questionnaire_answer b ON a.question_guid = b.question_guid and a.`enable`=b.`enable`
                        WHERE
	                        a.questionnaire_guid = @QuestionnaireGuid and a.`enable`=1
	                        AND a.sort < @Sort 
                            AND a.question_type in ('Enum','Bool')";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<GetQuestionListCanDependItemDto>(sql, requestDto);
                return result.ToList();
            }
        }
        /// <summary>
        /// 改变问题序号
        /// </summary>
        /// <param name="model">待变化的问题</param>
        /// <param name="newSort">顺序号（前端传入的索引加一后的结果）</param>
        /// <returns></returns>
        public async Task<bool> ChangeQuestionSortAsync(QuestionnaireQuestionModel model, int newSort, string operatorId)
        {
            var sqlWhere = " questionnaire_guid=@questionnaireGuid and question_guid<>@questionGuid ";
            var updateValue = "+1";
            var oldSort = model.Sort;
            if (oldSort > newSort)//往前移
            {
                sqlWhere = $"{sqlWhere} and sort >=@newSort and sort<=@oldSort";
                updateValue = "+1";
            }
            else//往后移
            {
                sqlWhere = $"{sqlWhere} and sort >=@oldSort and sort<=@newSort";
                updateValue = "-1";
            }
            var sql = $"update t_questionnaire_question set sort=sort{updateValue} where {sqlWhere}";
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                await conn.ExecuteAsync(sql, new
                {
                    questionnaireGuid = model.QuestionnaireGuid,
                    questionGuid = model.QuestionGuid,
                    oldSort,
                    newSort
                });
                model.Sort = newSort;//序号变为新序号
                model.LastUpdatedBy = operatorId;
                model.LastUpdatedDate = DateTime.Now;
                await conn.UpdateAsync(model);
                return true;
            });
        }

        /// <summary>
        /// 查看问卷下存在依赖的问题列表
        /// </summary>
        /// <param name=""></param>
        /// <param name="questionnaireGuid"></param>
        /// <returns></returns>
        public async Task<List<QuestionnaireQuestionModel>> GetQuestionWithDependAsync(string questionnaireGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<QuestionnaireQuestionModel>("where questionnaire_guid=@questionnaireGuid  and is_depend=1 and `enable`=1", new { questionnaireGuid });
                return result.ToList();
            }
        }
    }
}
