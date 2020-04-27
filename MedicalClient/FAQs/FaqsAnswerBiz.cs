using Dapper;
using GD.Common.Helper;
using GD.DataAccess;
using GD.Dtos.FAQs.FAQsClient;
using GD.Mall;
using GD.Models.FAQs;
using GD.Models.Mall;
using GD.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.FAQs
{

    /// <summary>
    /// 问答模块-回答业务类
    /// </summary>
    public class FaqsAnswerBiz
    {
        /// <summary>
        /// 通过主键获取model
        /// </summary>
        /// <param name="answerGuid"></param>
        /// <returns></returns>
        public async Task<FaqsAnswerModel> GetModelAsync(string answerGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<FaqsAnswerModel>(answerGuid);
            }
        }

        /// <summary>
        /// 更新model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(FaqsAnswerModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.UpdateAsync(model);
                return result > 0;
            }
        }

        /// <summary>
        /// 查询问答动态列表
        /// </summary>
        /// <param name="limit">获取的条数</param>
        /// <returns></returns>
        public async Task<List<GetFAQsTrendsResponseDto>> GetFAQsTrendsAsync(int limit)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                    b.question_guid,
	                    d.hospital_name,
	                    du.user_name AS doctor_name,
	                    u.nick_name,
	                    a.creation_date 
                    FROM
	                    t_faqs_answer a
	                    LEFT JOIN t_faqs_question b ON a.question_guid = b.question_guid
	                    LEFT JOIN t_utility_user u ON u.user_guid = b.user_guid
	                    LEFT JOIN t_doctor d ON d.doctor_guid = a.user_guid
	                    LEFT JOIN t_utility_user du ON du.user_guid = a.user_guid 
                    WHERE
	                    a.`enable` =1
                    ORDER BY
	                    a.creation_date DESC 
	                    LIMIT @limit;";
                var result = await conn.QueryAsync<GetFAQsTrendsResponseDto>(sql, new { limit });
                return result?.ToList();
            }
        }

        /// <summary>
        /// 获取问题已抢答的回答数量
        /// </summary>
        /// <param name="questionId">问题Guid</param>
        /// <returns></returns>
        public async Task<int> GetTotalByQuestionIdAsync(string questionId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.RecordCountAsync<FaqsAnswerModel>("where question_guid=@questionId and `enable`=1", new { questionId });
            }
        }

        /// <summary>
        /// 获取问题的回答分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetFAQsAnswerPageListResponseDto> GetFAQsAnswerPageListAsync(GetFAQsAnswerPageListRequestDto requestDto)
        {
            var sql = @"SELECT
	                        b.doctor_guid,
	                        a.answer_guid,
	                        a.main_answer,
	                        a.content,
	                        c.user_name AS doctor_name,
	                        CONCAT( acc.base_path, acc.relative_path ) AS portrait,
	                        b.hospital_name,
	                        b.office_name,
	                        d.config_name AS title_name,
	                        a.reward_intergral AS score,
	                        a.receive_type,
	                        hot.like_count,
	                        a.creation_date,
	                        CASE
		                        WHEN l.like_guid IS NULL THEN 0 
		                        ELSE 1 
	                        END AS is_like 
                        FROM
	                        t_faqs_answer a
	                        LEFT JOIN t_doctor b ON a.user_guid = b.doctor_guid
	                        LEFT JOIN t_utility_user c ON c.user_guid = b.doctor_guid
	                        LEFT JOIN t_manager_dictionary d ON d.dic_guid = b.title_guid
	                        LEFT JOIN t_utility_hot hot ON a.answer_guid = hot.owner_guid
	                        LEFT JOIN t_utility_accessory acc ON acc.accessory_guid = b.portrait_guid
	                        LEFT JOIN t_consumer_like l ON l.target_guid = a.answer_guid 
	                        AND l.created_by = @UserId 
	                        AND l.`enable` = 1 
                        WHERE
	                        a.question_guid = @QuestionGuid 
	                        AND a.`enable` = 1 
                        ORDER BY
	                        a.main_answer DESC,
	                        hot.like_count DESC,
                        a.creation_date DESC";
            return await MySqlHelper.QueryByPageAsync<GetFAQsAnswerPageListRequestDto, GetFAQsAnswerPageListResponseDto, GetFAQsAnswerPageListItemDto>(sql, requestDto);
        }

        /// <summary>
        /// 设置回答为最佳答案（积分）
        /// </summary>
        /// <param name="model">回答model</param>
        /// <param name="scoreModel">积分model</param>
        /// <returns></returns>
        public async Task<bool> SetBestAnswerAsync(FaqsAnswerModel model, ScoreModel scoreModel, FaqsQuestionModel questionModel)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                if (await conn.UpdateAsync(model) == 0)
                {
                    return false;
                }

                if (await conn.UpdateAsync(questionModel) == 0)
                {
                    return false;
                }

                if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, ScoreModel>(scoreModel)))
                {
                    return false;
                }


                return true;
            });
        }

        /// <summary>
        /// 添加答案
        /// </summary>
        /// <returns></returns>
        public virtual async Task<bool> InsertAsync(FaqsAnswerModel entity, FaqsQuestionModel faqsQuestionModel)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                await conn.InsertAsync<string, FaqsAnswerModel>(entity);
                await conn.UpdateAsync(faqsQuestionModel);
                return true;
            });
        }

        /// <summary>
        /// 设置回答为最佳答案（金额）
        /// </summary>
        /// <param name="model">回答model</param>
        /// <param name="收益明细">收益明细model</param>
        /// <returns></returns>
        public async Task<bool> SetBestAnswerAllotMoneyAsync(FaqsAnswerModel model, DoctorEaringsDetailModel earingsDetailModel, DoctorBalanceModel balanceModel, FaqsQuestionModel questionModel, bool isNewBalanceModel)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                if (isNewBalanceModel)
                {
                    if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, DoctorBalanceModel>(balanceModel))) { return false; }
                }
                else
                {
                    if ((await conn.UpdateAsync(balanceModel)) < 1) { return false; }
                }
                if (await conn.UpdateAsync(model) == 0) { return false; }
                if (await conn.UpdateAsync(questionModel) == 0) { return false; }
                if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, DoctorEaringsDetailModel>(earingsDetailModel))) { return false; }
                return true;
            });
        }

        /// <summary>
        /// 问答分配悬赏金额
        /// </summary>
        /// <param name="parentGuid"></param>
        /// <returns></returns>
        public async Task<int> FaqsAllotFeeAsync()
        {
            var qusetionModelList = await Get15DaysQuestionModelListAsync();
            int conpleteNum = 0;
            foreach (var item in qusetionModelList)
            {
                item.Status = FaqsQuestionModel.QuestionStatusEnum.End.ToString();
                item.LastUpdatedDate = DateTime.Now;
                var answerList = await GetAnswerModelListByQuestionGuidAsync(item.QuestionGuid);
                if (answerList == null || answerList.Count < 1)
                {
                    //没有回答，金额退给用户，变更问题状态为过期
                    //获取流水，调用退款接口
                    var flowingModel = await new TransactionFlowingBiz().GetModelsById(item.TransferFlowingGuid);
                    var requestDto = new Dtos.MallPay.FangDiInterface.DoRefundRequestDto
                    {
                        Reason = "FAQS=>问题无回答，退款.",
                        Refund_Fee = Convert.ToInt32(flowingModel.Amount).ToString(),
                        Refund_No = flowingModel.TransactionNumber,
                        Trade_No = flowingModel?.OutTradeNo
                    };
                    var response = await new FangDiPayBiz().RefundAsync(requestDto);
                    if (response.ResultCode.Equals("0") && response.ResultMsg.ToLower().Equals("success"))
                    {
                        flowingModel.TransactionStatus = TransactionStatusEnum.RefundSuccess.ToString();
                        flowingModel.OutRefundNo = response.Refund_No;
                        flowingModel.Update();
                        item.Update();
                    }
                    conpleteNum++;
                    continue;
                }
                int answerCount = answerList.Count;
                if (answerCount > 10) { answerList = answerList.Take(10).ToList(); }//.OrderByDescending(a => a.LikeCount).OrderByDescending(a => a.CreationDate)
                decimal allotNum = 0;
                GetManthNumber(item.RewardIntergral, answerList.Count, ref allotNum);
                var balanceBiz = new DoctorBalanceBiz();
                if ( answerList.Count < 11)
                {
                    var answerModelList = new List<FaqsAnswerModel>();
                    var earingsDetailModelList = new List<DoctorEaringsDetailModel>();
                    var doctorBalanceModelList = new List<Dictionary<bool, DoctorBalanceModel>>();
                    foreach (var answerItem in answerList)
                    {
                        #region 更新Model
                        var newAnswerModel = new FaqsAnswerModel
                        {
                            AnswerGuid = answerItem.AnswerGuid,
                            MainAnswer = answerItem.MainAnswer,
                            Content = answerItem.Content,
                            QuestionGuid = answerItem.QuestionGuid,
                            UserGuid = answerItem.UserGuid,
                            ReceiveType = FaqsAnswerModel.AnswerReceiveTypeEnum.Money.ToString(),
                            RewardIntergral = Convert.ToInt32(allotNum * 100),
                            CreatedBy = answerItem.CreatedBy,
                            LastUpdatedBy = answerItem.LastUpdatedBy,
                            CreationDate = answerItem.CreationDate.Value,
                            LastUpdatedDate = DateTime.Now,
                            OrgGuid = answerItem.OrgGuid,
                            Enable = answerItem.Enable
                        };
                        answerModelList.Add(newAnswerModel);
                        var newEaringsDetailModel = new DoctorEaringsDetailModel
                        {
                            DetailGuid = Guid.NewGuid().ToString("N"),
                            AnswerGuid = answerItem.AnswerGuid,
                            DoctorGuid = answerItem.UserGuid,
                            FeeFrom = DoctorEaringsDetailModel.FeeFromTypeEnum.Answer.ToString(),
                            ReceivedFee = Convert.ToInt32(allotNum * 100),
                            OrgGuid = string.Empty,
                            CreatedBy = answerItem.UserGuid,
                            LastUpdatedBy = answerItem.UserGuid,
                            Remark = string.Empty
                        };
                        earingsDetailModelList.Add(newEaringsDetailModel);
                        var balanceModel = await balanceBiz.GetAsync(answerItem.UserGuid ?? "");
                        var isNewBalanceModel = false;
                        if (balanceModel == null)
                        {
                            isNewBalanceModel = true;
                            balanceModel = new DoctorBalanceModel
                            {
                                BalanceGuid = answerItem.UserGuid,
                                TotalEarnings = Convert.ToInt32(allotNum * 100),
                                AccBalance = Convert.ToInt32(allotNum * 100),
                                TotalWithdraw = 0,
                                Status = DoctorBalanceModel.DoctorBalanceStatusEnum.Normal.ToString(),
                                CreatedBy = answerItem.UserGuid,
                                LastUpdatedBy = answerItem.UserGuid,
                                OrgGuid = string.Empty
                            };
                        }
                        else
                        {
                            balanceModel.TotalEarnings += Convert.ToInt32(allotNum * 100);
                            balanceModel.AccBalance += Convert.ToInt32(allotNum * 100);
                        }
                        doctorBalanceModelList.Add(new Dictionary<bool, DoctorBalanceModel> { { isNewBalanceModel, balanceModel } });
                        #endregion
                    }
                    var isSucc = await FaqsAllotQuestionFee(item, answerModelList, earingsDetailModelList, doctorBalanceModelList);
                    if (!isSucc)
                    {
                        Logger.Error($"FaqsAllotFeeAsync=>问答分配悬赏金失败,QuestionGuid: {item.QuestionGuid}");
                        continue;
                    }
                    conpleteNum++;
                }
            }

            return conpleteNum;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moneyTotal"></param>
        /// <param name="answerTotal"></param>
        /// <param name="allotNum">每人所分金额</param>
        private void GetManthNumber(int moneyTotal, int answerTotal, ref decimal allotNum)
        {
            if (moneyTotal > 0 && answerTotal > 0)
            {
                decimal temp = (decimal)moneyTotal / 100;
                allotNum = Math.Round(temp / answerTotal, 1);
            }
        }

        /// <summary>
        /// 获取满15天内未解答的问题
        /// </summary>
        /// <param name="parentGuid"></param>
        /// <returns></returns>
        public async Task<List<FaqsQuestionModel>> Get15DaysQuestionModelListAsync()
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sqlStr = @"SELECT
	                                        * 
                                        FROM
	                                        t_faqs_question 
                                        WHERE
	                                        creation_date<=  DATE_SUB(NOW(),INTERVAL 15 day) 
	                                        AND ENABLE = 1 
	                                        AND reward_type = 'Money' 
	                                        AND STATUS = 'Solving' ";
                return (await conn.QueryAsync<FaqsQuestionModel>(sqlStr)).ToList();
            }
        }

        /// <summary>
        /// 获取满15天内未解答的问题
        /// </summary>
        /// <param name="parentGuid"></param>
        /// <returns></returns>
        public async Task<List<GetAnswerModelListByQuestionGuidAsyncResponse>> GetAnswerModelListByQuestionGuidAsync(string questionGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sqlStr = @"SELECT
	                                        	a.`answer_guid`,
	                                            a.`question_guid`,
	                                            a.`user_guid`,
	                                            a.`main_answer`,
	                                            a.`reward_intergral`,
	                                            a.`receive_type`,
	                                            a.`content`,
	                                            a.`created_by`,
	                                            a.`creation_date`,
	                                            a.`last_updated_by`,
	                                            a.`last_updated_date`,
	                                            a.`org_guid`,
	                                            a.`enable` ,
	                                            hot.like_count
                                        FROM
	                                        t_faqs_answer a
	                                        LEFT JOIN t_utility_hot hot ON a.answer_guid = hot.owner_guid 
                                        WHERE
	                                        a.question_guid = @questionGuid
	                                        AND a.`enable` = 1 
                                        ORDER BY
	                                        hot.like_count DESC,
	                                        a.creation_date DESC ";
                return (await conn.QueryAsync<GetAnswerModelListByQuestionGuidAsyncResponse>(sqlStr, new { questionGuid })).ToList();
            }
        }
        /// <summary>
        /// 发布问题
        /// </summary>
        /// <param name="scoreModel">发布问题消费积分model</param>
        /// <param name="questionModel">问题model</param>
        /// <returns></returns>
        public async Task<bool> FaqsAutoAllotFeeUpdateAsync(FaqsQuestionModel questionModel, List<DoctorEaringsDetailModel> earingsDetailModelList, DoctorBalanceModel balanceModel)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, DoctorBalanceModel>(balanceModel)))
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

        /// <summary>
        /// 悬赏金额分配
        /// </summary>
        /// <returns></returns>
        public async Task<bool> FaqsAllotQuestionFee(FaqsQuestionModel item, List<FaqsAnswerModel> answerModelList, List<DoctorEaringsDetailModel> earingsDetailModelList, List<Dictionary<bool, DoctorBalanceModel>> doctorBalanceModelList)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                if (await conn.UpdateAsync(item) < 1) { return false; }
                foreach (var answerItem in answerModelList)
                {
                    if (await conn.UpdateAsync(answerItem) < 1) { return false; }
                }
                foreach (var earingsDetailItem in earingsDetailModelList)
                {
                    if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, DoctorEaringsDetailModel>(earingsDetailItem))) { return false; }
                }

                foreach (var dic in doctorBalanceModelList)
                {
                    foreach (var dicItem in dic)
                    {
                        if (dicItem.Key)
                        {
                            if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, DoctorBalanceModel>(dicItem.Value))) { return false; }
                        }
                        else
                        {
                            if (await conn.UpdateAsync(dicItem.Value) < 1) { return false; }
                        }
                    }
                }
                return true;
            });
        }


    }
}
