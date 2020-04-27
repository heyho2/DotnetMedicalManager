using GD.Common;
using GD.Models.Utility;
using GD.Dtos.FAQs.FAQsClient;
using GD.FAQs;
using GD.Models.FAQs;
using GD.Module;
using GD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GD.Mall;
using GD.Dtos.MallPay.FangDiInterface;
using GD.Models.Mall;

namespace GD.API.Controllers.FAQs
{
    /// <summary>
    /// 问答模块 用户端接口
    /// </summary>
    public class FAQsClientController : FAQsBaseController
    {
        /// <summary>
        /// 获取问答动态
        /// </summary>
        /// <param name="limit">获取的记录条数</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetFAQsTrendsResponseDto>>)), AllowAnonymous]
        public async Task<IActionResult> GetFAQsTrendsAsync(int limit)
        {
            var response = await new FaqsAnswerBiz().GetFAQsTrendsAsync(limit);
            return Success(response);
        }

        /// <summary>
        /// 获取问答广场热门问题
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetHotFAQsResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetHotFAQsPageListAsync([FromQuery]GetHotFAQsRequestDto requestDto)
        {
            var response = await new FaqsQuestionBiz().GetHotFAQsPageListAsync(requestDto);
            return Success(response);
        }

        /// <summary>
        /// 获取问答广场最新问题
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetLatestFAQsPageListResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetLatestFAQsPageListAsync([FromQuery]GetLatestFAQsPageListRequestDto requestDto)
        {
            var response = await new FaqsQuestionBiz().GetLatestFAQsPageListAsync(requestDto);
            return Success(response);
        }

        /// <summary>
        /// 积分-发布问题
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> PostQuestionsAsync([FromBody]PostQuestionsRequestDto requestDto)
        {
            var scoreTotal = await new ScoreExBiz().GetTotalScore(UserID, Common.EnumDefine.UserType.Consumer);
            if (scoreTotal < requestDto.Score)
            {
                return Failed(ErrorCode.UserData, "用户当前积分不足");
            }
            var settingValue = await new FaqsQuestionBiz().GetSettingValueAsync(false);
            if (requestDto.Score < settingValue)
            {
                return Failed(ErrorCode.FormatError, $"悬赏积分不能少于 {settingValue} 元!");
            }
            var scoreModel = new ScoreModel
            {
                ScoreGuid = Guid.NewGuid().ToString("N"),
                UserGuid = UserID,
                UserTypeGuid = GD.Common.EnumDefine.UserType.Consumer.ToString(),
                Variation = requestDto.Score * -1,
                Reason = "提问消费积分",
                PlatformType = GD.Common.EnumDefine.PlatformType.CloudDoctor.ToString(),
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty
            };
            var questionModel = new FaqsQuestionModel
            {
                QuestionGuid = Guid.NewGuid().ToString("N"),
                UserGuid = UserID,
                RewardIntergral = requestDto.Score,
                Status = FaqsQuestionModel.QuestionStatusEnum.Solving.ToString(),
                AnswerNum = 0,
                Content = requestDto.Content,
                AttachmentGuidList = JsonConvert.SerializeObject(requestDto.AttachedPictures),
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty
            };
            var result = await new FaqsQuestionBiz().PostQuestionsAsync(scoreModel, questionModel);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "网络连接异常，发布问题出错");
        }

        #region V1.7新增接口
        /// <summary>
        /// 获取提问支付类型
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetAskPayTypeAsyncResponse>)), AllowAnonymous]
        public async Task<IActionResult> GetAskPayTypeAsync()
        {
            var response = new List<GetAskPayTypeAsyncResponse>();
            foreach (FaqsQuestionModel.RewardTypeEnum item in Enum.GetValues(typeof(FaqsQuestionModel.RewardTypeEnum)))
            {
                response.Add(new GetAskPayTypeAsyncResponse
                {
                    Name = item.GetDescription(),
                    Code = item.ToString().ToLower(),
                    Minimum = await new FaqsQuestionBiz().GetSettingValueAsync(item.Equals(FaqsQuestionModel.RewardTypeEnum.Money) ? true : false)
                });
            }
            return Success(response);
        }
        /// <summary>
        /// 问答微信支付
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<FAQsWechatPayResponse>))]
        public async Task<IActionResult> FAQsWechatPayAsync([FromBody]FAQsWechatPayRequest request)
        {
            Common.Helper.Logger.Debug($"FAQsWechatPayRequest-传入参数-UserID-{UserID}-{JsonConvert.SerializeObject(request)}");
            var fdPayBiz = new FangDiPayBiz();
            var userBiz = new UserBiz();
            var userModel = userBiz.GetUser(UserID);
            if (userModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "无法获取用户信息!");
            }
            //if (string.IsNullOrWhiteSpace(request.Code))
            //{
            //    if (string.IsNullOrWhiteSpace(userModel.WechatOpenid))
            //    {
            //        return Failed(ErrorCode.DataBaseError, "用户OpenId为空，请传Code参数!");
            //    }
            //}
            //else
            //{
            //    var fdResponse = await fdPayBiz.GetOpenID(new GetOpenIDRequestDto { Code = request.Code });
            //    if (string.IsNullOrWhiteSpace(fdResponse.Open_Id))
            //    {
            //        return Failed(ErrorCode.DataBaseError, "无法获取用户OpenID!");
            //    }
            //    Common.Helper.Logger.Debug($"FAQsWechatPayRequest-获取openid-UserID-{UserID}-获取的openid({fdResponse.Open_Id})-用户表的openid({userModel.WechatOpenid})");
            //    if (string.IsNullOrWhiteSpace(fdResponse.Open_Id) || !userModel.WechatOpenid.Equals(fdResponse.Open_Id))
            //    {
            //        userModel.WechatOpenid = fdResponse.Open_Id;
            //        await userBiz.UpdateAsync(userModel);
            //    }
            //}
            if (string.IsNullOrWhiteSpace(userModel.WechatOpenid))
            {
                return Failed(ErrorCode.DataBaseError, "用户无OpenID!");
            }

            var questionBiz = new FaqsQuestionBiz();
            var questionModel = await questionBiz.GetModelAsync(request.QuestionGuid);
            if (questionModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "QuestionGuid无法找到数据，请检查!");
            }
            //交易流水数据
            var newTFModel = new TransactionFlowingModel
            {
                TransactionFlowingGuid = Guid.NewGuid().ToString("N"),
                //流水号
                TransactionNumber = $"FAQSSN_{GetRandomString(10, false, false, true, false, "")}{DateTime.Now.ToString("yyyyMMddHHmmss")}",
                //微信标识订单流水号
                OutTradeNo = $"FAQSPQ_{GetRandomString(10, false, false, true, false, "")}{DateTime.Now.ToString("yyyyMMddHHmmss")}",
                Channel = "微信支付",
                ChannelNumber = "2",
                PayAccount = userModel.WechatOpenid,
                Amount = questionModel.RewardIntergral,
                OutRefundNo = string.Empty,
                TransactionStatus = TransactionStatusEnum.WaitForPayment.ToString(),
                Remarks = string.Empty,
                OrgGuid = string.Empty,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
            };
            var isSuccess = await new TransactionFlowingBiz().UpdateFlowingAndQuestionAsync(newTFModel, questionModel);
            if (!isSuccess)
            {
                return Failed(ErrorCode.DataBaseError, "流水生成失败!");
            }
            //await PayCallBackTestAsync(newTFModel.OutTradeNo);
            var fdXDResponsed = await fdPayBiz.OrdersPay(new OrdersPayRequestDto { Trade_No = newTFModel.OutTradeNo, Amount = (Convert.ToInt32(newTFModel.Amount)).ToString(), Open_Id = userModel.WechatOpenid });
            return Success(fdXDResponsed);
        }

        /// <summary>
        /// 支付回调测试
        /// </summary>
        private async Task PayCallBackTestAsync(string OutTranNo)
        {
            var response = new Dtos.MallPay.ControllerApi.PaymentPushRequest
            {
                // 第三方平台ID
                appid = string.Empty,
                // 商户号
                mch_id = string.Empty,
                //  子商户
                sub_mch_id = string.Empty,
                //子商户平台ID
                sub_appid = string.Empty,
                // 第三方支付支付生成的医疗订单号
                med_trans_id = string.Empty,
                // 医院订单号,若为自费支付(非移动医疗平台下单),则本字段传商户订单号
                hosp_out_trade_no = OutTranNo,// string.Empty,
                // 退款订单号
                refund_no = string.Empty,
                //业务状态| pay_ok: 支付成功,paying: 支付中,refunded: 已退款,closed: 订单已经关闭,canceled: 订单已经撤销
                trade_status = "pay_ok",//string.Empty,
                // 错误代码
                err_code = string.Empty,
                // 错误描述
                err_msg = string.Empty,
                //  订单支付时间,格式为yyyyMMddHHmmss,如20091225091010
                time_end = string.Empty,
                // 支付类型 1:现金 2:医保 3:现金+医保
                pay_type = string.Empty,
                // 总共需要支付的金额,以分为单位
                total_fee = string.Empty,
                // 现金支付金额,以分为单位
                cash_fee = string.Empty,
                // 退款金额|以分为单位
                refund_fee = string.Empty,
                // 医保支付金额,以分为单位
                insurance_fee = string.Empty,
                // 诊疗证编号
                medical_card_id = string.Empty,
                // 医保单据号
                bill_no = string.Empty,
                // 医保流水号
                serial_no = string.Empty,
                // 医保子单号
                insurance_order_id = string.Empty,
                // 支付子单号
                cash_order_id = string.Empty,
            };
            var mallController = new Mall.MallController();
            await mallController.PaymentCallAsync(response);
        }

        /// <summary>
        /// 悬赏金-提交问题
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> PostQuestionsByWeChatPayAsync([FromBody]PostQuestionsByWeChatPayAsyncRequest request)
        {
            //FAQSWD 问答提现   FAQSWP 问答微信支付
            //var transferFlowingModel = await new TransactionFlowingBiz().GetModelByOutTradeNo(requestDto.FaqsTransferFlowing);
            //if (transferFlowingModel == null || !transferFlowingModel.TransactionStatus.Trim().ToLower().Equals(TransactionStatus.Success.ToString().Trim().ToLower()))
            //{
            //    return Failed(ErrorCode.UserData, "对应的流水不是已完成状态，无法提交！");
            //}

            //支付金额 是否 符合要求，最少悬赏金额
            var settingValue = await new FaqsQuestionBiz().GetSettingValueAsync(true);
            if (request.FeeNum < settingValue)
            {
                return Failed(ErrorCode.FormatError, $"悬赏金额不能少于 {settingValue} 元!");
            }
            var questionModel = new FaqsQuestionModel
            {
                QuestionGuid = Guid.NewGuid().ToString("N"),
                UserGuid = UserID,
                RewardIntergral = Convert.ToInt32(request.FeeNum * 100),
                RewardType = FaqsQuestionModel.RewardTypeEnum.Money.ToString(),
                Status = FaqsQuestionModel.QuestionStatusEnum.Solving.ToString(),
                TransferFlowingGuid = string.Empty,//transferFlowingModel.TransactionFlowingGuid,
                AnswerNum = 0,
                Content = request.Content,
                AttachmentGuidList = JsonConvert.SerializeObject(request.AttachedPictures),
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty,
                Enable = false
            };
            var result = await new FaqsQuestionBiz().AddModelAsync(questionModel);
            return string.IsNullOrWhiteSpace(result) ? Failed(ErrorCode.DataBaseError, "网络连接异常，发布问题出错") : Success(result);
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
        /// <summary>
        /// 删除问题
        /// </summary>
        /// <param name="questionGuid">问题guid</param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DeleteQuestionAsync(string questionGuid)
        {
            var questionBiz = new FaqsQuestionBiz();
            var model = await questionBiz.GetModelAsync(questionGuid);
            if (!(model?.Enable ?? false))
            {
                return Failed(ErrorCode.Empty, "当前问题不存在");
            }
            if (string.Equals(model.Status, FaqsQuestionModel.QuestionStatusEnum.Solving.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return Failed(ErrorCode.UserData, "积分尚未分配，请选择最佳答案后删除");
            }
            if (model.UserGuid != UserID)
            {
                return Failed(ErrorCode.UserData, "这不是你的提问，不能删除");
            }
            model.Enable = false;
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            var result = await questionBiz.UpdateAsync(model);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "网络错误，删除问题失败");
        }

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
        /// 获取问题详情
        /// </summary>
        /// <param name="questionGuid">问题guid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetFAQsDetailResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetFAQsDetailAsync(string questionGuid)
        {
            var userId = UserID ?? "";

            var model = await new FaqsQuestionBiz().GetModelAsync(questionGuid);
            if (model == null)
            {
                return Failed(ErrorCode.Empty, "该问题不存在");
            }
            var userModel = await new UserBiz().GetModelAsync(model.UserGuid);
            var userPortrait = await new AccessoryBiz().GetAsync(userModel.PortraitGuid);
            var result = new GetFAQsDetailResponseDto
            {
                NickName = userModel?.NickName,
                Score = model.RewardType.Equals(FaqsQuestionModel.RewardTypeEnum.Money.ToString()) ? (decimal)model.RewardIntergral / 100 : model.RewardIntergral,
                RewardType = model.RewardType,
                Content = model.Content,
                CreationDate = model.CreationDate,
                Status = model.Status,
                AnswerNum = await new FaqsAnswerBiz().GetTotalByQuestionIdAsync(questionGuid),
                IsSelf = userId == model.UserGuid,
                Portrait = $"{userPortrait?.BasePath}{userPortrait?.RelativePath}"
            };
            if (!string.IsNullOrWhiteSpace(model.AttachmentGuidList) && model.AttachmentGuidList != "[]")
            {
                var accessoryIds = JsonConvert.DeserializeObject<List<string>>(model.AttachmentGuidList);
                var picModels = await new AccessoryExBiz().GetModelsAsync(accessoryIds);
                result.AttachedPictures = picModels.Select(a => $"{a.BasePath}{a.RelativePath}").ToList();
            }
            return Success(result);
        }

        /// <summary>
        /// 获取问题的回答分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetFAQsAnswerPageListResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetFAQsAnswerPageListAsync([FromQuery]GetFAQsAnswerPageListRequestDto requestDto)
        {
            if (!string.IsNullOrWhiteSpace(UserID))
            {
                requestDto.UserId = UserID;
            }
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
        /// 设置最佳答案
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> SetBestAnswerAsync(string answerGuid)
        {
            var biz = new FaqsAnswerBiz();
            var model = await biz.GetModelAsync(answerGuid);
            if (!(model?.Enable ?? false))
            {
                return Failed(ErrorCode.Empty, "该回答不存在");
            }
            var questionModel = await new FaqsQuestionBiz().GetModelAsync(model.QuestionGuid);

            if (!string.Equals(questionModel.Status, FaqsQuestionModel.QuestionStatusEnum.Solving.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return Failed(ErrorCode.UserData, "当前问题状态不是[未解决]，不能设置最佳回答");
            }
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            model.MainAnswer = true;
            model.RewardIntergral = questionModel.RewardIntergral;

            questionModel.Status = FaqsQuestionModel.QuestionStatusEnum.Solved.ToString();
            questionModel.LastUpdatedBy = UserID;
            questionModel.LastUpdatedDate = DateTime.Now;
            var result = false;
            if (questionModel.RewardType.Equals(FaqsQuestionModel.RewardTypeEnum.Intergral.ToString()))
            {
                model.ReceiveType = FaqsQuestionModel.RewardTypeEnum.Intergral.ToString();
                var scoreModel = new ScoreModel
                {
                    ScoreGuid = Guid.NewGuid().ToString("N"),
                    UserGuid = model.UserGuid,
                    UserTypeGuid = GD.Common.EnumDefine.UserType.Doctor.ToString(),
                    Variation = questionModel.RewardIntergral,
                    Reason = "回答被设为最佳",
                    PlatformType = GD.Common.EnumDefine.PlatformType.CloudDoctor.ToString(),
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = string.Empty
                };
                result = await biz.SetBestAnswerAsync(model, scoreModel, questionModel);
            }
            else if (questionModel.RewardType.Equals(FaqsQuestionModel.RewardTypeEnum.Money.ToString()))
            {
                model.ReceiveType = FaqsQuestionModel.RewardTypeEnum.Money.ToString();
                var balanceModel = await new DoctorBalanceBiz().GetModelsById(model.UserGuid);
                var isNewBalanceModel = false;
                if (balanceModel == null)
                {
                    isNewBalanceModel = true;
                    balanceModel = new DoctorBalanceModel
                    {
                        BalanceGuid = model.UserGuid,
                        TotalEarnings = 0,
                        AccBalance = 0,
                        TotalWithdraw = 0,
                        Status = DoctorBalanceModel.DoctorBalanceStatusEnum.Normal.ToString(),
                        CreatedBy = UserID,
                        LastUpdatedBy = UserID,
                        OrgGuid = string.Empty
                    };
                }
                var newEaringsDetailModel = new DoctorEaringsDetailModel
                {
                    DetailGuid = Guid.NewGuid().ToString("N"),
                    AnswerGuid = model.AnswerGuid,
                    DoctorGuid = model.UserGuid,
                    FeeFrom = DoctorEaringsDetailModel.FeeFromTypeEnum.Answer.ToString(),
                    ReceivedFee = questionModel.RewardIntergral,
                    OrgGuid = string.Empty,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    Remark = string.Empty
                };
                balanceModel.TotalEarnings += newEaringsDetailModel.ReceivedFee;
                balanceModel.AccBalance += newEaringsDetailModel.ReceivedFee;
                result = await biz.SetBestAnswerAllotMoneyAsync(model, newEaringsDetailModel, balanceModel, questionModel, isNewBalanceModel);
            }
            return result ? Success() : Failed(ErrorCode.DataBaseError, "网络错误，设置最佳回答失败");
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
        /// 浏览问题增加浏览量
        /// </summary>
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> PostVisitQuestionAsync(string questionGuid)
        {
            var res = await new HotExBiz().UpdateVisitTotalAsync(questionGuid);
            return Success(res);
        }
    }
}
