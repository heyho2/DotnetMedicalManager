using GD.API.Code;
using GD.Common;
using GD.Common.Helper;
using GD.Consumer;
using GD.DataAccess;
using GD.Doctor;
using GD.Dtos.Consumer.Consumer;
using GD.Dtos.Utility.Message;
using GD.Dtos.Utility.Utility;
using GD.Dtos.WeChat;
using GD.Models.Utility;
using GD.Module.WeChat;
using GD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.API.Controllers.Utility
{
    /// <summary>
    /// 消息相关控制器
    /// </summary>
    public class MessageController : UtilityBaseController
    {
        /// <summary>
        /// 通过接收者用户guid获取发送者用户列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<MessageUserDto>>))]
        public async Task<IActionResult> GetFromUserListByToUserGuidAsync([FromBody]GetFromUserListByToUserGuidRequestDto requestDto)
        {
            var response = await new ConsumerBiz().GetFromUserListByToUserGuidAsync(requestDto.toUserGuid, requestDto.PageIndex, requestDto.PageSize, requestDto.TopicAbountType.ToString());
            return Success(response);
        }

        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetMessageListByFromAndToResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetMessageListByFromAndToAsync([FromBody]GetMessageListByFromAndToRequestDto requestDto)
        {
            var response = await new ConsumerBiz().GetMessageListByFromAndToAsync(requestDto);
            return Success(response);
        }

        /// <summary>
        /// 我的Topic消息列表
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetMyTopicListItemDto>>))]
        public IActionResult GetMyTopicList([FromBody]PageRequestDto pageRequest)
        {
            var topicBiz = new TopicBiz();
            var topisList = topicBiz.GetListByUserId(UserID, pageRequest);
            if (topisList.Count < 1)
            {
                return Failed(ErrorCode.Empty, "暂无数据！");
            }
            var response = topisList.Select(a => a.ToDto<GetMyTopicListItemDto>()).ToList();
            return Success(response);
        }

        /// <summary>
        /// 新增Topic记录
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public IActionResult AddMessageTopic([FromBody]AddMessageInfoListRequestDto topicDto)
        {
            var model = new TopicModel
            {
                TopicGuid = Guid.NewGuid().ToString("N"),
                SponsorGuid = topicDto.SponsorGuid,
                ReceiverGuid = topicDto.ReceiverGuid,
                AboutGuid = topicDto.AboutGuid,
                AboutType = topicDto.EnumTb,
                BeginDate = TimeZoneInfo.ConvertTime(topicDto.BeginDate, TimeZoneInfo.Local),
                EndDate = TimeZoneInfo.ConvertTime(topicDto.EndDate, TimeZoneInfo.Local),
                Enable = true,
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                OrgGuid = "OrgGuid",
                LastUpdatedBy = UserID
            };
            var topicBiz = new TopicBiz();
            var isAddGuid = topicBiz.Add(model);
            return string.IsNullOrWhiteSpace(isAddGuid) ? Failed(ErrorCode.Empty, "数据新增失败！") : Success(isAddGuid);
        }

        /// <summary>
        /// 新增Topic记录(支持批量)
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public IActionResult AddMessageTopicList([FromBody]List<AddMessageInfoListRequestDto> topicDtoList)
        {
            if (topicDtoList == null || topicDtoList.Count < 1)
            {
                return Failed(ErrorCode.Empty, "传入数据为空！");
            }
            var modelList = new List<TopicModel>();
            foreach (var dto in topicDtoList)
            {
                var model = new TopicModel
                {
                    TopicGuid = Guid.NewGuid().ToString("N"),
                    SponsorGuid = dto.SponsorGuid,
                    ReceiverGuid = dto.ReceiverGuid,
                    AboutGuid = dto.AboutGuid,
                    AboutType = dto.EnumTb,
                    BeginDate = TimeZoneInfo.ConvertTime(dto.BeginDate, TimeZoneInfo.Local),
                    EndDate = TimeZoneInfo.ConvertTime(dto.EndDate, TimeZoneInfo.Local),
                    Enable = true,
                    CreatedBy = UserID,
                    CreationDate = DateTime.Now,
                    OrgGuid = "OrgGuid",
                    LastUpdatedBy = UserID
                };
                modelList.Add(model);
            }
            var topicBiz = new TopicBiz();
            var isAddSuccess = topicBiz.AddModelList(modelList);
            return isAddSuccess ? Success("数据新增成功！") : Failed(ErrorCode.Empty, "数据新增失败！");
        }

        /// <summary>
        /// 持久化聊天记录
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public IActionResult AddMessageInfo([FromBody]List<AddMessageInfoRequestDto> requestDtoList)
        {
            var messageBiz = new MessageBiz();
            var messageList = new List<MessageModel>();

            foreach (var dto in requestDtoList)
            {
                DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                long lTime = long.Parse(dto.CreationDate + "0000");
                TimeSpan toNow = new TimeSpan(lTime);
                var msgDate = dtStart.Add(toNow);

                var model = new MessageModel
                {
                    MsgGuid = Guid.NewGuid().ToString("N"),
                    TopicGuid = dto.TopicGuid,
                    FromGuid = dto.FromGuid,
                    ToGuid = dto.ToGuid,
                    Context = dto.Context,
                    IsHtml = dto.IsHtml,
                    CreatedBy = UserID,
                    CreationDate = msgDate,
                    LastUpdatedBy = UserID,
                    LastUpdatedDate = DateTime.Now,
                    OrgGuid = "OrgGuid",
                    Enable = true
                };
                if (!string.IsNullOrWhiteSpace(dto.MessageGuid))
                {
                    model.MsgGuid = dto.MessageGuid;
                }
                messageList.Add(model);
            }
            if (messageList.Count > 0)
            {
                var isAddSuccess = messageBiz.Push2Redis(messageList);
                if (!isAddSuccess)
                {
                    return Failed(ErrorCode.DataBaseError, "批量添加失败！");
                }
                DoctorOfflineMessageNotification(messageList.FirstOrDefault());
            }
            return Success();
        }

        /// <summary>
        /// 医生离线消息通知
        /// </summary>
        /// <param name="messageModel"></param>
        /// <returns></returns>
        private void DoctorOfflineMessageNotification(MessageModel messageModel)
        {
            #region 通知医生离线消息


            if (string.IsNullOrWhiteSpace(PlatformSettings.DoctorOfflineMsgTemplate))
            {
                return;
            }
            Task.Run(() =>
            {
                string controllerName = nameof(MessageController);
                string actionName = nameof(DoctorOfflineMessageNotification);
                try
                {
                    var topiclModel = new TopicBiz().GetModelById(messageModel.TopicGuid);
                    Logger.Debug($"{actionName}-请求参数-{JsonConvert.SerializeObject(messageModel)}{Environment.NewLine}MessageToGuid({messageModel.ToGuid})-TopicReceiverGuid({topiclModel.ReceiverGuid})");
                    if (messageModel.ToGuid == topiclModel.ReceiverGuid)
                    {
                        var doctorStatus = new UserPresenceBiz().GetPresenceStatus(messageModel.ToGuid);
                        Logger.Debug($"{actionName}-当前医生({messageModel.ToGuid})在线状态-{JsonConvert.SerializeObject(doctorStatus)}");
                        //医生为在线状态，不通知
                        if (doctorStatus.IsOnline)
                        {
                            return;
                        }
                        var doctorModel = new DoctorBiz().GetDoctor(messageModel.ToGuid);
                        if (doctorModel == null)
                        {
                            Logger.Debug($"{actionName}-当前接收者不是医生({messageModel.ToGuid})");
                            return;
                        }
                        #region 离线消息十分钟发一次逻辑去掉，改为，离线消息，全部发送给医生
                        //var timeStep = 10;//每十分钟通知一次医生
                        //var msgNotificationKey = $"CloudDoctor:MsgNotificationKey:{messageModel.ToGuid}";
                        //DateTime? latestNotificationTime = null;
                        //if (RedisHelper.Exist(msgNotificationKey))
                        //{
                        //    DateTime.TryParse(RedisHelper.Get<string>(msgNotificationKey), out DateTime result);
                        //    latestNotificationTime = result;
                        //}
                        //else
                        //{
                        //    latestNotificationTime = DateTime.Now.AddHours(-1);
                        //}
                        //Logger.Debug($"{actionName}-当前医生离线消息Redis记录时间({msgNotificationKey}-{latestNotificationTime.Value.ToString("yyyy-MM-dd HH:mm:ss")})");
                        ////通知间隔不足10分钟
                        //if ((DateTime.Now - latestNotificationTime.Value).TotalMinutes < timeStep)
                        //{
                        //    return;
                        //}
                        //RedisHelper.Set(msgNotificationKey, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); 
                        #endregion
                        //var tokenRes = WeChartApi.GetAccessToken(PlatformSettings.DoctorClientAppId, PlatformSettings.DoctorClientAppSecret).Result;

                        //用户无openId，不通知
                        if (string.IsNullOrWhiteSpace(doctorModel.WechatOpenid))
                        {
                            Logger.Debug($"{actionName}-当前医生无企业微信userid数据-{JsonConvert.SerializeObject(doctorModel)}");
                            return;
                        }
                        //if (string.IsNullOrWhiteSpace(tokenRes.AccessToken))
                        //{

                        //    Logger.Error($"GD.API.Controllers.Utility.{controllerName}.{actionName} 获取token失败  appId:[{PlatformSettings.DoctorClientAppId}] {Environment.NewLine} error:{tokenRes?.Errmsg}");
                        //    return;
                        //}
                        var userModel = new UserBiz().GetModelAsync(messageModel.FromGuid).Result;
                        var content = messageModel.Context.Substring(0, messageModel.Context.Length > 25 ? 25 : messageModel.Context.Length);
                        //var tmplateMsg = new WeChatTemplateMsg
                        //{
                        //    Touser = doctorModel.WechatOpenid,
                        //    Template_Id = PlatformSettings.DoctorOfflineMsgTemplate,
                        //    Data = new
                        //    {
                        //        First = new { Value = "【待处理提醒】" },
                        //        //用户昵称
                        //        Keyword1 = new { Value = userModel?.NickName },
                        //        //时间
                        //        Keyword2 = new { Value = messageModel.CreationDate.ToString("MM月dd日 HH:mm") },
                        //        //咨询内容
                        //        Keyword3 = new { Value = $"{content}..." },

                        //        Remark = new { Value = "您有待办信息，请尽快处理" },
                        //    }
                        //};
                        //var response = WeChartApi.SendTemplateMsg(tmplateMsg, tokenRes.AccessToken);
                        //Logger.Debug($"{actionName}-发送模板消息结果-{JsonConvert.SerializeObject(response)}{Environment.NewLine}消息内容-{JsonConvert.SerializeObject(tmplateMsg)}");

                        var qyMsg = new QyTextCardMessageRequest
                        {
                            ToUser = doctorModel.WechatOpenid,
                            AgentId = PlatformSettings.EnterpriseWeChatMobileAgentid,
                            TextCard = new QyTextCardMessageRequest.Content
                            {
                                Title = "问医离线消息通知",
                                Description = $"<div class=\"normal\">用户昵称：{userModel?.NickName}</div>" +
                                $"<div class=\"normal\">咨询时间：{messageModel.CreationDate.ToString("MM月dd日 HH:mm")}</div>" +
                                $"<div class=\"normal\">咨询内容：{content}...</div>" +
                                $"<div class=\"blue\">您有待办信息，请尽快处理</div>"
                            }
                        };
                        var token = EnterpriseWeChatApi.GetEnterpriseAccessToken(PlatformSettings.EnterpriseWeChatAppid, PlatformSettings.EnterpriseWeChatMobileSecret).Result;
                        if (token.Errcode != 0)
                        {
                            Logger.Error($"发送问医离线消息通知获取企业微信token失败：{token.Errmsg}");
                            return;
                        }
                        var sendResult = EnterpriseWeChatApi.SendQyMessageAsync(qyMsg, token.AccessToken).Result;
                        Logger.Debug($"{actionName}-发送问医离线消息通知结果-{JsonConvert.SerializeObject(sendResult)}{Environment.NewLine}消息内容-{JsonConvert.SerializeObject(qyMsg)}");

                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"GD.API.Controllers.Utility.{controllerName}.{actionName}-传入参数:({JsonConvert.SerializeObject(messageModel)}) {Environment.NewLine} error:{ex.Message}");
                }
            });
            #endregion
        }

        /// <summary>
        /// 通过顶部消息Id获取历史消息记录
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetHistoryMessageListStartByTopMsgResponseDto>>))]
        public async Task<IActionResult> GetHistoryMessageListStartByTopMsgAsync([FromQuery]GetHistoryMessageListStartByTopMsgRequestDto requestDto)
        {
            var biz = new MessageBiz();
            var model = await biz.GetAsync(requestDto.TopMsgId);
            if (model != null)
            {
                requestDto.TopMsgDate = model?.CreationDate;
            }
            var result = await biz.GetHistoryMessageListStartByTopMsgAsync(requestDto);
            return Success(result);
        }

        //[HttpGet, AllowAnonymous]
        //public IActionResult TestSendQyAppMessage()
        //{

        //    var qyMsg = new QyMarkdownMessageRequest
        //    {
        //        ToUser = "20200100727",
        //        AgentId = PlatformSettings.EnterpriseWeChatMobileAgentid,
        //        Markdown = new QyMarkdownMessageRequest.MarkdownContent
        //        {
        //            Content = @"您的会议室已经预定，稍后会同步到`邮箱` 
        //            >**事项详情 **
        //            >事项：<font color ='info'>开会</font> 
        //            >组织者：@miglioguan
        //            >参与者：@miglioguan、@kunliu、@jamdeezhou、@kanexiong、@kisonwang
        //            >
        //            >会议室：<font color ='info'>广州TIT 1楼 301</font> 
        //            >日期：<font color ='warning'>2018年5月18日</font> 
        //            >时间：<font color ='comment'>上午9:00-11:00</font> 
        //            >
        //            >请准时参加会议。 
        //            >
        //            >如需修改会议信息，请点击：[修改会议信息](https://work.weixin.qq.com)"
        //        }

        //    };

        //    var qyMsg1 = new QyMarkdownMessageRequest
        //    {
        //        ToUser = "20200100727",
        //        AgentId = PlatformSettings.EnterpriseWeChatMobileAgentid,
        //        Markdown = new QyMarkdownMessageRequest.MarkdownContent
        //        {
        //            Content = @"问医离线消息通知
        //            >用户昵称：王小明
        //            >咨询时间：2020-05-01 09:00:00
        //            >咨询内容：你好啊！！！！！
        //            >咨询时间：2018年5月18日 10:00:00
        //            >
        //            ><font color =""warning"" > 您有待办信息，请尽快处理</font> "
        //        }

        //    };
        //    var token = EnterpriseWeChatApi.GetEnterpriseAccessToken(PlatformSettings.EnterpriseWeChatAppid, PlatformSettings.EnterpriseWeChatMobileSecret).Result;
        //    if (token.Errcode != 0)
        //    {
        //        Logger.Error($"发送问医离线消息通知获取企业微信token失败：{token.Errmsg}");

        //    }
        //    var sendResult = EnterpriseWeChatApi.SendQyMessageAsync(qyMsg1, token.AccessToken).Result;
        //    return Success(sendResult);
        //}
    }
}