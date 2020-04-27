using GD.API.Code;
using GD.Common;
using GD.Component;
using GD.Dtos.FAQs.FAQsClient;
using GD.FAQs;
using GD.Mall;
using GD.Models.FAQs;
using GD.Module;
using GD.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Senparc.Weixin.TenPay.V3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GD.Models.FAQs.DoctorBalanceModel;
using static GD.Models.FAQs.DoctorWithDrawApplyModel;

namespace GD.API.Controllers.FAQs
{
    /// <summary>
    /// 问答模块 医生端接口
    /// </summary>
    public class FAQsDoctorClientController : FAQsBaseController
    {
        /// <summary>
        /// 获取我的提问分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetOwnedQuestionsPageListResponseDto>))]
        public async Task<IActionResult> GetOwnedQuestionsPageListAsync([FromQuery]GetOwnedQuestionsPageListRequestDto requestDto)
        {
            var response = await new FaqsQuestionBiz().GetOwnedQuestionsPageListAsync(requestDto, UserID);
            return Success(response);
        }
        /// <summary>
        /// 获取问题的回答分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetFAQsAnswerPageListResponseDto>))]
        public async Task<IActionResult> GetFAQsAnswerPageListAsync([FromQuery]GetFAQsAnswerPageListRequestDto requestDto)
        {
            var response = await new FaqsAnswerBiz().GetFAQsAnswerPageListAsync(requestDto);
            foreach (var item in response.CurrentPage)
            {
                if (item.ReceiveType.Equals(FaqsAnswerModel.AnswerReceiveTypeEnum.Money.ToString()))
                {
                    item.Score = item.Score / 100;
                }
            }
            return Success(response);
        }
        /// <summary>
        /// 获取用户收藏的问题列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetQuestionCollectionListResponseDto>))]
        public async Task<IActionResult> GetQuestionCollectionListAsync([FromQuery]GetQuestionCollectionListRequestDto requestDto)
        {
            var response = await new FaqsQuestionBiz().GetQuestionCollectionListAsync(requestDto, UserID);
            return Success(response);
        }
        /// <summary>
        /// 问题列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetFaqsQuestionPageResponseDto>))]
        public async Task<IActionResult> GetFaqsQuestionPageAsync([FromQuery]GetFaqsQuestionPageRequestDto request)
        {
            var response = await new FaqsQuestionBiz().GetFaqsQuestionPageAsync(request);

            var users = await new UserExBiz().GetListAsync(response.CurrentPage.Select(a => a.UserGuid));

            foreach (var item in response.CurrentPage)
            {
                item.UserName = users.FirstOrDefault(a => a.UserGuid == item.UserGuid)?.UserName;

            }
            return Success(response);
        }
        /// <summary>
        /// 获取我的回答列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetFaqsQuestionPageResponseDto>))]
        public async Task<IActionResult> GetMyReplyPageAsync([FromQuery]GetMyReplyPageRequestDto request)
        {
            var response = await new FaqsQuestionBiz().GetMyReplyPageAsync(request, UserID);
            var users = await new UserExBiz().GetListAsync(response.CurrentPage.Select(a => a.UserGuid));
            foreach (var item in response.CurrentPage)
            {
                item.UserName = users.FirstOrDefault(a => a.UserGuid == item.UserGuid)?.UserName;
            }
            return Success(response);
        }
        readonly static object lockObject = new object();
        /// <summary>
        /// 回答问题
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ReplyFaqsQuestionAsync([FromBody]ReplyFaqsQuestionRequestDto request)
        {
            var questionBiz = new FaqsQuestionBiz();
            var question = await new FaqsQuestionBiz().GetModelAsync(request.QuestionGuid);
            if (question == null)
            {
                return Failed(ErrorCode.Empty, "参数错误");
            }
            var isExistAnswer = await questionBiz.GetIsAnswerExist(question.QuestionGuid, UserID);
            if (isExistAnswer)
            {
                return Failed(ErrorCode.DataBaseError, "该问题已回答过，不能二次回答！");
            }
            var model = new FaqsAnswerModel
            {
                CreatedBy = UserID,
                Enable = true,
                Content = request.Content,
                AnswerGuid = Guid.NewGuid().ToString("N"),
                LastUpdatedBy = UserID,
                ReceiveType= question.RewardType,
                MainAnswer = false,
                OrgGuid = string.Empty,
                QuestionGuid = request.QuestionGuid,
                UserGuid = UserID,
                RewardIntergral = 0
            };
            FaqsAnswerBiz faqsAnswerBiz = new FaqsAnswerBiz();
            lock (lockObject)
            {
                question.AnswerNum = (faqsAnswerBiz.GetTotalByQuestionIdAsync(question.QuestionGuid).Result + 1);
                var result = faqsAnswerBiz.InsertAsync(model, question).Result;
                return Success();
            }
        }
        /// <summary>
        /// 获取问题详细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetFaqsQuestionInfoResponseDto>))]
        public async Task<IActionResult> GetFaqsQuestionInfoAsync([FromQuery]GetFaqsQuestionInfoRequestDto request)
        {
            var question = await new FaqsQuestionBiz().GetModelAsync(request.QuestionGuid);
            if (question == null)
            {
                return Failed(ErrorCode.Empty, "参数错误");
            }
            var hot = await new HotBiz().GetModelAsync(request.QuestionGuid);
            var user = await new UserBiz().GetModelAsync(question.UserGuid);
            FaqsAnswerBiz faqsAnswerBiz = new FaqsAnswerBiz();
            var attachmentGuidList = JsonConvert.DeserializeObject<string[]>(question.AttachmentGuidList ?? "[]");
            var accessorys = await new AccessoryExBiz().GetModelsAsync(attachmentGuidList);
            var userPortrait = await new AccessoryBiz().GetAsync(user.PortraitGuid);
            var resopnse = new GetFaqsQuestionInfoResponseDto
            {
                QuestionGuid = question.QuestionGuid,
                AnswerNum = question.AnswerNum,
                Content = question.Content,
                CreationDate = question.CreationDate,
                Enable = question.Enable,
                Status = question.Status,
                VisitCount = hot?.VisitCount ?? 0,
                RewardIntergral = question.RewardType.Equals(FaqsQuestionModel.RewardTypeEnum.Money.ToString()) ? (decimal)question.RewardIntergral / 100 : question.RewardIntergral,
                RewardType = question.RewardType,
                UserGuid = question.UserGuid,
                UserName = user?.UserName,
                AttachedPictures = accessorys.Select(a => new GetFaqsQuestionInfoResponseDto.Attachment
                {
                    Guid = a.AccessoryGuid,
                    Url = $"{a.BasePath}{a.RelativePath}"
                }).ToArray(),
                Portrait = $"{userPortrait?.BasePath}{userPortrait?.RelativePath}"
            };
            return Success(resopnse);
        }

        #region 收益中心
        /// <summary>
        ///  收益中心
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetMyEaringsAsyncResponse>))]
        public async Task<IActionResult> GetMyEaringsAsync()
        {
            var balanceBiz = new DoctorBalanceBiz();
            var balanceModel = await balanceBiz.GetAsync(UserID ?? "");
            if (balanceModel == null)
            {
                balanceModel = new DoctorBalanceModel
                {
                    BalanceGuid = UserID,
                    TotalEarnings = 0,
                    AccBalance = 0,
                    TotalWithdraw = 0,
                    Status = DoctorBalanceStatusEnum.Normal.ToString(),
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = string.Empty
                };
                var isSucc = await balanceBiz.InsertAsync(balanceModel);
                if (!isSucc)
                {
                    return Failed(ErrorCode.Empty, "网络异常，请检查！");
                }
            }
            else if (!balanceModel.Enable || balanceModel.Status.Equals(DoctorBalanceStatusEnum.Frozen.ToString()))
            {
                return Failed(ErrorCode.Empty, "该用户收益数据不可用，请检查！");
            }
            var response = balanceModel.ToDto<GetMyEaringsAsyncResponse>();
            response.TotalEarnings = response.TotalEarnings / 100;
            response.AccBalance = response.AccBalance / 100;
            response.TotalWithdraw = response.TotalWithdraw / 100;
            return Success(response);
        }

        /// <summary>
        /// 获取收益明细
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetEaringsRecordsAsyncResponse>>))]
        public async Task<IActionResult> GetEaringsRecordsAsync(GetEaringsRecordsAsyncRequest request)
        {
            var earingsRecordModelList = await new DoctorEaringsDetailBiz().GetPageModelListByIDAsync(UserID ?? "", request.PageIndex, request.PageSize);
            if (earingsRecordModelList == null)
            {
                return Failed(ErrorCode.Empty, "无收益明细数据！");
            }
            var responseList = new List<GetEaringsRecordsAsyncResponse>();
            foreach (var item in earingsRecordModelList)
            {
                var response = item.ToDto<GetEaringsRecordsAsyncResponse>();
                response.ReceivedFee = response.ReceivedFee / 100;
                responseList.Add(response);
            }
            return Success(responseList);
        }

        /// <summary>
        /// 获取提现明细
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetWithdrawRecordsAsyncResponse>>))]
        public async Task<IActionResult> GetWithdrawRecordsAsync(GetWithdrawRecordsAsyncRequest request)
        {
            var withdrawModelList = await new DoctorWithDrawApplyBiz().GetPageModelListByIDAsync(UserID ?? "", request.PageIndex, request.PageSize);
            if (withdrawModelList == null)
            {
                return Failed(ErrorCode.Empty, "无提现明细数据！");
            }
            var responseList = new List<GetWithdrawRecordsAsyncResponse>();
            foreach (var item in withdrawModelList)
            {
                var response = item.ToDto<GetWithdrawRecordsAsyncResponse>();
                response.Withdraw = response.Withdraw / 100;
                responseList.Add(response);
            }
            return Success(responseList);
        }

        /// <summary>
        /// 提现
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<WithdrawEaringsAsyncResponse>))]
        public async Task<IActionResult> WithdrawEaringsAsync(WithdrawEaringsAsyncRequest request)
        {
            var bannerBiz = new DoctorBalanceBiz();
            var balanceModel = await bannerBiz.GetAsync(UserID ?? "");
            if (balanceModel == null || !balanceModel.Enable || balanceModel.Status.Equals(DoctorBalanceStatusEnum.Frozen.ToString()))
            {
                return Failed(ErrorCode.Empty, "无法获取收益数据，请检查！");
            }
            decimal balanceNum = balanceModel.AccBalance / 100;

            if (balanceNum < 99 || request.WithdrawNum < 99 || request.WithdrawNum > 299)
            {
                return Failed(ErrorCode.UserData, "1.余额小于99，2.提现金额小于99，3.提现金额大于299，均不能提现！");
            }
            var withdrawBiz = new DoctorWithDrawApplyBiz();
            if (await withdrawBiz.GetModelDataInTimeAsync(UserID) > 1)
            {
                return Failed(ErrorCode.UserData, "每天只能提现一次！");
            }
            if (string.IsNullOrWhiteSpace(""))
            {
                return Failed(ErrorCode.SystemException, "功能暂为开放！");
            }
            var userModel = new UserBiz().GetUser(UserID);
            //创建流水
            var transferFlowingModel = new TransferFlowingModel
            {
                FlowingGuid = Guid.NewGuid().ToString("N"),
                TransactionNumber = $"FAQSZN_{GetRandomString(10, false, false, true, false, "")}{DateTime.Now.ToString("yyyyMMddHHmmss")}",
                OutTradeNo = $"FAQSZZ_{GetRandomString(10, false, false, true, false, "")}{DateTime.Now.ToString("yyyyMMddHHmmss")}",
                Channel = "微信转账",
                ChannelNumber = "2",
                PayAccount = userModel?.WechatOpenid ?? "",
                Amount = Convert.ToInt32(request.WithdrawNum * 100),
                TransactionStatus = TransferFlowingModel.TransferStatusEnum.WaitForPayment.ToString(),
                Remarks = string.Empty,
                OrgGuid = string.Empty,
                CreatedBy = UserID,
                LastUpdatedBy = UserID
            };
            string timeStamp = (DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds.ToString();
            var newModel = new DoctorWithDrawApplyModel
            {
                ApplyGuid = Guid.NewGuid().ToString("N"),
                ApplyCode = $"FAQSWD_{GetRandomString(10, false, false, true, false, "")}{DateTime.Now.ToString("yyyyMMddHHmmss")}",//FAQSWD 问答提现申请
                DoctorGuid = UserID,
                Withdraw = Convert.ToInt32(request.WithdrawNum * 100),
                Status = FaqsApplyStatusEnum.Apply.ToString(),
                TransactionFlowingGuid = transferFlowingModel.FlowingGuid,
                ApproverGuid = string.Empty,
                Reason = string.Empty,
                Remark = request.Remark,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty
            };
            //调用微信转账
            #region 当前不支持提现，关闭该功能
            var result = await new WeiXinPayBiz().EnterpasePayAsync(transferFlowingModel.OutTradeNo, userModel.WechatOpenid, userModel.UserName, newModel.Withdraw, "192.168.1.1");
            if (!result.result_code.Equals("SUCCESS"))
            {
                balanceModel.AccBalance = balanceModel.AccBalance - newModel.Withdraw;
                balanceModel.TotalWithdraw = balanceModel.TotalWithdraw + newModel.Withdraw;
                newModel.Status = FaqsApplyStatusEnum.Complete.ToString();
                transferFlowingModel.TransactionStatus = TransferFlowingModel.TransferStatusEnum.Success.ToString();
            }
            //申请+流水
            var isSucc = await new DoctorWithDrawApplyBiz().InsertApplyModelAndFlowingModelAsync(newModel, transferFlowingModel, balanceModel);
            if (!isSucc) { return Failed(ErrorCode.DataBaseError, "申请出错，请检查！"); }
            #endregion
            return Success(isSucc);
        }
        ///<summary>
        ///生成随机字符串 
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<param name="useNum">是否包含数字，1=包含，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        ///<param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        ///<param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        ///<returns>指定长度的随机字符串</returns>
        private string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }

        #endregion
    }
}
