using Dapper;
using GD.Models.Utility;
using GD.DataAccess;
using GD.Dtos.FAQs.FAQsClient;
using GD.Models.FAQs;
using System.Threading.Tasks;
using GD.Models.Mall;
using System.Collections.Generic;
using System.Linq;

namespace GD.FAQs
{
    /// <summary>
    /// 问答模块-问题业务类
    /// </summary>
    public class FaqsQuestionBiz
    {
        /// <summary>
        /// 获取唯一实例
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public async Task<FaqsQuestionModel> GetModelAsync(string questionId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<FaqsQuestionModel>(questionId);
            }
        }

        /// <summary>
        /// 更新model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(FaqsQuestionModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.UpdateAsync(model);
                return result > 0;
            }
        }

        /// <summary>
        /// 更新model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateDoubleModelAsync(FaqsQuestionModel model, TransactionFlowingModel transactionFlowingModel)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                if ((await conn.UpdateAsync(model)) < 1) { return false; }
                if ((await conn.UpdateAsync(transactionFlowingModel)) < 1) { return false; }
                return true;
            });
        }
        /// <summary>
        /// 更新model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> AddModelAsync(FaqsQuestionModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.InsertAsync<string, FaqsQuestionModel>(model);
            }
        }

        /// <summary>
        /// 获取未支付状态问题
        /// </summary>
        /// <param name="questionId">问题Guid</param>
        /// <returns></returns>
        public async Task<FaqsQuestionModel> GetModelByFlowingAsync(string transferFlowingGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sqlStr = "select * from t_faqs_question where transfer_flowing_guid=@transferFlowingGuid and enable=0 ";
                return await conn.QueryFirstOrDefaultAsync<FaqsQuestionModel>(sqlStr, new { transferFlowingGuid });
            }
        }
        /// <summary>
        /// 获取问答热门问题分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetHotFAQsResponseDto> GetHotFAQsPageListAsync(GetHotFAQsRequestDto requestDto)
        {
            var wheresql = "";
            if (requestDto.LatestDay != 0)
            {
                wheresql = "and a.creation_date>DATE_SUB(NOW(),INTERVAL @LatestDay DAY) ";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.Keyword))
            {
                wheresql = "and a.content like @Keyword ";
                requestDto.Keyword = $"%{requestDto.Keyword}%";
            }
            var sql = $@"SELECT
	                        a.question_guid,
	                        a.content,
	                        a.creation_date,
	                        a.answer_num,
	                        a.`status`,
	                        ifnull( b.visit_count, 0 ) AS visit_count 
                        FROM
	                        t_faqs_question a
	                        LEFT JOIN t_utility_hot b ON a.question_guid = b.owner_guid 
	                    WHERE a.`enable`=1 {wheresql}
                        ORDER BY
	                        b.visit_count DESC";
            var result = await MySqlHelper.QueryByPageAsync<GetHotFAQsRequestDto, GetHotFAQsResponseDto, GetHotFAQsItemDto>(sql, requestDto);
            return result;
        }

        /// <summary>
        /// 获取问答最新问题分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetLatestFAQsPageListResponseDto> GetLatestFAQsPageListAsync(GetLatestFAQsPageListRequestDto requestDto)
        {
            var wheresql = string.Empty;
            if (!string.IsNullOrWhiteSpace(requestDto.Keyword))
            {
                wheresql = "and a.content like @Keyword ";
                requestDto.Keyword = $"%{requestDto.Keyword}%";
            }
            var sql = $@"SELECT
	                        a.question_guid,
	                        a.content,
	                        a.creation_date,
	                        a.answer_num,
	                        a.`status`,
	                        ifnull( b.visit_count, 0 ) AS visit_count 
                        FROM
	                        t_faqs_question a
	                        LEFT JOIN t_utility_hot b ON a.question_guid = b.owner_guid 
                        WHERE a.`enable`=1 {wheresql}
                        ORDER BY
	                        a.creation_date DESC";
            var result = await MySqlHelper.QueryByPageAsync<GetLatestFAQsPageListRequestDto, GetLatestFAQsPageListResponseDto, GetLatestFAQsPageListItemDto>(sql, requestDto);
            return result;
        }

        /// <summary>
        /// 获取我的提问分页列表
        /// </summary>
        /// <param name="requestDto">分页参数</param>
        /// <param name="userId">用户guid</param>
        /// <returns></returns>
        public async Task<GetOwnedQuestionsPageListResponseDto> GetOwnedQuestionsPageListAsync(GetOwnedQuestionsPageListRequestDto requestDto, string userId)
        {
            var sql = $@"SELECT
	                        a.question_guid,
	                        a.content,
	                        a.creation_date,
	                        a.answer_num,
	                        a.`status`,
	                        ifnull( b.visit_count, 0 ) AS visit_count 
                        FROM
	                        t_faqs_question a
	                        LEFT JOIN t_utility_hot b ON a.question_guid = b.owner_guid 
                        WHERE a.user_guid='{userId}' and a.`enable`=1
                        ORDER BY
	                        a.creation_date DESC";
            var result = await MySqlHelper.QueryByPageAsync<GetOwnedQuestionsPageListRequestDto, GetOwnedQuestionsPageListResponseDto, GetOwnedQuestionsPageListItemDto>(sql, requestDto);
            return result;
        }

        /// <summary>
        /// 发布问题
        /// </summary>
        /// <param name="scoreModel">发布问题消费积分model</param>
        /// <param name="questionModel">问题model</param>
        /// <returns></returns>
        public async Task<bool> PostQuestionsAsync(ScoreModel scoreModel, FaqsQuestionModel questionModel)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, ScoreModel>(scoreModel)))
                {
                    return false;
                }

                if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, FaqsQuestionModel>(questionModel)))
                {
                    return false;
                }

                return true;
            });
        }

        public async Task<GetFaqsQuestionPageResponseDto> GetFaqsQuestionPageAsync(GetFaqsQuestionPageRequestDto request)
        {
            var sqlWhere = $@"AND a.enable=1";
            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                request.Keyword = $"{request.Keyword}%";
                sqlWhere = $"{sqlWhere} AND a.content like @Keyword";
            }
            var orderBySql = $"a.creation_date {(request.IsAscending ? "Asc" : "Desc")}";
            switch (request.SortBy)
            {
                case GetFaqsQuestionPageRequestDto.SortByEnum.Newest:
                    orderBySql = $"a.creation_date {(request.IsAscending ? "Asc" : "Desc")}";
                    break;
                case GetFaqsQuestionPageRequestDto.SortByEnum.Popular:
                    orderBySql = $"b.visit_count {(request.IsAscending ? "Asc" : "Desc")},a.creation_date {(request.IsAscending ? "Asc" : "Desc")}";
                    break;
            }
            if (request.BeginDate != null)
            {
                request.BeginDate = request.BeginDate?.Date;
                sqlWhere = $"{sqlWhere} AND a.creation_date > @BeginDate";
            }
            if (request.EndDate != null)
            {
                request.EndDate = request.EndDate?.AddDays(1).Date;
                sqlWhere = $"{sqlWhere} AND a.creation_date < @EndDate";
            }
            if (request.LatestDay != 0)
            {
                sqlWhere = $"{sqlWhere}  AND a.creation_date>DATE_SUB(NOW(),INTERVAL @LatestDay DAY)";
            }
            var sql = $@"
SELECT
	a.question_guid,
	a.user_guid,
	a.reward_intergral,
	a.`status`,
	a.answer_num,
	a.content,
	a.attachment_guid_list,
	a.creation_date,
	b.visit_count,
	a.ENABLE 
FROM
	t_faqs_question a
	LEFT JOIN t_utility_hot b ON a.question_guid = b.owner_guid 
WHERE
	1 = 1 {sqlWhere}
ORDER BY
	{orderBySql}";
            return await MySqlHelper.QueryByPageAsync<GetFaqsQuestionPageRequestDto, GetFaqsQuestionPageResponseDto, GetFaqsQuestionPageItemDto>(sql, request);
        }

        public async Task<GetFaqsQuestionPageResponseDto> GetMyReplyPageAsync(GetMyReplyPageRequestDto request, string userid)
        {
            var sqlWhere = $@"AND a.enable=1";
            if (request.BeginDate != null)
            {
                request.BeginDate = request.BeginDate?.Date;
                sqlWhere = $"{sqlWhere} AND a.creation_date > @BeginDate";
            }
            if (request.EndDate != null)
            {
                request.EndDate = request.EndDate?.AddDays(1).Date;
                sqlWhere = $"{sqlWhere} AND a.creation_date < @EndDate";
            }
            var sql = $@"
SELECT DISTINCT
	a.question_guid,
	a.user_guid,
	a.reward_intergral,
	a.`status`,
	a.answer_num,
	a.content,
	a.attachment_guid_list,
	a.creation_date,
	a.ENABLE,
    c.visit_count,
    c.like_count
FROM
	t_faqs_question a
	LEFT JOIN t_faqs_answer b ON a.question_guid = b.question_guid 
	LEFT JOIN t_utility_hot c ON a.question_guid = c.owner_guid 
WHERE
	1 = 1 {sqlWhere} and b.user_guid = '{userid}'
ORDER BY
	a.creation_date desc";
            return await MySqlHelper.QueryByPageAsync<GetMyReplyPageRequestDto, GetFaqsQuestionPageResponseDto, GetFaqsQuestionPageItemDto>(sql, request);
        }
        /// <summary>
        /// 获取用户收藏的问题列表
        /// </summary>
        /// <param name="requestDto">分页参数</param>
        /// <param name="userId">用户guid</param>
        /// <returns></returns>
        public async Task<GetQuestionCollectionListResponseDto> GetQuestionCollectionListAsync(GetQuestionCollectionListRequestDto requestDto, string userId)
        {
            var sql = $@"SELECT
	                        a.question_guid,
	                        a.content,
	                        a.creation_date,
	                        a.answer_num,
	                        a.`status`,
	                        ifnull( b.visit_count, 0 ) AS visit_count 
                        FROM
	                        t_faqs_question a
	                        INNER JOIN t_consumer_collection c ON a.question_guid = c.target_guid
	                        LEFT JOIN t_utility_hot b ON a.question_guid = b.owner_guid 
                        WHERE
	                        c.user_guid = '{userId}' 
	                        AND a.`enable` = 1 and c.`enable`=1
                        ORDER BY
	                        c.creation_date DESC";
            var result = await MySqlHelper.QueryByPageAsync<GetQuestionCollectionListRequestDto, GetQuestionCollectionListResponseDto, GetQuestionCollectionListItemDto>(sql, requestDto);
            return result;
        }

        /// <summary>
        /// 根据父级guid获取列表
        /// </summary>
        /// <param name="parentGuid"></param>
        /// <returns></returns>
        public async Task<decimal> GetSettingValueAsync(bool isMoney, bool enable = true)
        {
            var id = isMoney ? "t_manager_dictionary_00000000015" : "t_manager_dictionary_00000000016";
            using (var conn = MySqlHelper.GetConnection())
            {
                var sqlStr = " select extension_field from t_manager_dictionary where enable=@enable and dic_guid = @id ";
                return (await conn.QueryFirstOrDefaultAsync<decimal>(sqlStr, new { enable, id }));
            }
        }

        /// <summary>
        /// 获取该问题是否已回答
        /// </summary>
        /// <param name="parentGuid"></param>
        /// <returns></returns>
        public async Task<bool> GetIsAnswerExist(string questionGuid, string userID,bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sqlStr = " select count(*) from t_faqs_answer where question_guid=@questionGuid and user_guid=@userID and enable=@enable ";
                return (await conn.QueryFirstOrDefaultAsync<int>(sqlStr, new { enable, questionGuid, userID }))>0;
            }
        }


    }
}
