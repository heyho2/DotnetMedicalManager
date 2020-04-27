using GD.Common;
using GD.Common.Helper;
using GD.Consumer;
using GD.Dtos.Enum;
using GD.Dtos.Health;
using GD.Dtos.WeChat;
using GD.Health;
using GD.Models.Consumer;
using GD.Models.Health;
using GD.Module.WeChat;
using GD.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.API.Controllers.Health
{
    /// <summary>
    /// 日常健康指标控制器
    /// </summary>
    public class HealthIndicatorController : HealthBaseController
    {
        /// <summary>
        /// 获取用户健康档案第一次打开 false:第一次打开
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> GetHealthStatusAsync()
        {
            var consumerModel = await new ConsumerBiz().GetModelAsync(UserID);
            if (consumerModel == null || !consumerModel.HealthFillStatus)
            {
                return Success(false);
            }
            return Success(true);
        }
        /// <summary>
        /// 更改第一次用户点击创建健康档案
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> UpdateHealthStatusAsync()
        {
            var consumerModel = await new ConsumerBiz().GetModelAsync(UserID);
            if (consumerModel == null)
            {
                Failed(ErrorCode.UserData, "未找到用户数据");
            }
            consumerModel.HealthFillStatus = true;
            var result = await new ConsumerBiz().UpdateAsync(consumerModel);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "更新状态失败");
        }
        /// <summary>
        /// 获取日常指标列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetHealthIndicatorResponseDto>>))]
        public async Task<IActionResult> GeHealthIndicatorListtAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                userId = UserID;
            }
            HealthIndicatorBiz health = new HealthIndicatorBiz();
            var healthIndicatorList = await health.GetHealthIndicatorList();
            if (healthIndicatorList == null)
            {
                return Success(healthIndicatorList);
            }
            healthIndicatorList = healthIndicatorList.GroupBy(s => s.IndicatorGuid).Select(s => new GetHealthIndicatorResponseDto
            {
                IndicatorGuid = s.Key,
                IndicatorName = s.FirstOrDefault().IndicatorName,
                IndicatorType = s.FirstOrDefault().IndicatorType,
                MaxValue = s.FirstOrDefault()?.MaxValue,
                MinValue = s.FirstOrDefault()?.MinValue,
                OptionName = s.FirstOrDefault().OptionName,
                OptionUnit = s.FirstOrDefault().OptionUnit,
                OptionGuid = s.FirstOrDefault().OptionGuid
            }).ToList();
            foreach (var item in healthIndicatorList)
            {
                //单个值
                if (!item.IndicatorType)
                {
                    item.IndicatorName = item.OptionName;
                    //查找用户的最近数据
                    item.ResultVale = await health.GetHealthIndicatorValue(userId, item.IndicatorGuid, item.OptionGuid);
                }
            }
            //给第一条数据添加建议
            if (healthIndicatorList.Count > 0)
            {
                healthIndicatorList.FirstOrDefault().Suggestion = await health.GetHealthIndicatorSuggestion(userId);
            }
            return Success(healthIndicatorList);
        }
        /// <summary>
        /// 获取健康指标项数据
        /// </summary>
        /// <param name="indicatorGuid">健康指标id</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<HealthIndicatorOption>>))]
        public async Task<IActionResult> GetHealthIndicatorOptionAsync(string indicatorGuid)
        {
            if (string.IsNullOrEmpty(indicatorGuid))
            {
                return Failed(ErrorCode.Empty, "指标id不能为空");
            }
            var result = await new HealthIndicatorBiz().GetHealthIndicatorOptionList(indicatorGuid);
            if (result != null)
            {
                foreach (var item in result)
                {
                    item.ResultVale = await new HealthIndicatorBiz().GetHealthIndicatorValue(UserID, indicatorGuid, item.OptionGuid);
                }
            }
            return Success(result);
        }
        /// <summary>
        /// 获取健康指标项详情数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetHealthIndicatorRecordResponseDto>>))]
        public async Task<IActionResult> GetHealthIndicatorDetailAsync(GetHealthIndicatorDetailRequestDto requestDto)
        {
            GetHealthIndicatorRecordResponseDto resultResponse = new GetHealthIndicatorRecordResponseDto();
            string userId = string.Empty;
            if (string.IsNullOrWhiteSpace(requestDto.UserId))
            {
                userId = UserID;
            }
            else
            {
                userId = requestDto.UserId;
            }
            resultResponse.TotalCount = await new HealthIndicatorBiz().GetHealthIndicatorDetailCount(userId, requestDto);
            var result = await new HealthIndicatorBiz().GetHealthIndicatorDetailList(userId, requestDto);
            resultResponse.DateList = result.GroupBy(a => a.CreationDate).Select(s => s.Key).OrderBy(s => s).ToList();
            var resultGroup = result.GroupBy(a => new { a.IndicatorOptionGuid }).Select(a => new GetHealthIndicatorDetailResponseDto
            {
                OptionGuid = a.Key.IndicatorOptionGuid,
                OptionName = a.FirstOrDefault().OptionName,
                OptionUnit = a.FirstOrDefault().OptionUnit,
                MaxValue = a.FirstOrDefault()?.MaxValue,
                MinValue = a.FirstOrDefault()?.MinValue,
                OptionList = a.ToList().Select(s => new KeyValuePair<DateTime, decimal?>(s.CreationDate, s.IndicatorValue)).OrderBy(s => s.Key).ToList()
            }).ToList();
            //查询个人用户的范围值
            var indicatorList = await new IndicatorWarningLimitBiz().GetModelAsyncByUser(userId);
            if (indicatorList != null && indicatorList.Count > 0)
            {
                foreach (var item in resultGroup)
                {
                    var indicatorModel = indicatorList.FirstOrDefault(s => s.IndicatorOptionGuid == item.OptionGuid);
                    if (indicatorModel != null)
                    {
                        item.MaxValue = indicatorModel.MaxValue;
                        item.MinValue = indicatorModel.MinValue;
                    }
                }
            }
            resultResponse.DetailList = resultGroup;
            return Success(resultResponse);
        }
        /// <summary>
        /// 新增健康指标数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> AddAHealthIndicatorAsync([FromBody]AddHealthIndicatorRequestDto requestDto)
        {
            string userId = string.Empty;
            if (string.IsNullOrWhiteSpace(requestDto.UserId))
            {
                userId = UserID;
            }
            else
            {
                userId = requestDto.UserId;
            }
            var healthIndicatorModel = await new HealthIndicatorBiz().GetAsync(requestDto.IndicatorGuid);
            if (healthIndicatorModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "未找到对应的健康指标Id");
            }
            if (!healthIndicatorModel.IndicatorType && requestDto.HealthIndicatorOptionList.All(d => !d.IndicatorValue.HasValue))
            {
                return Failed(ErrorCode.Empty, "单指标必须提供值，请检查！");
            }
            if (requestDto.HealthIndicatorOptionList.Count == 0)
            {
                return Failed(ErrorCode.DataBaseError, "健康指标选项不能为空");
            }
            var groupOption = requestDto.HealthIndicatorOptionList.GroupBy(s => s.OptionGuid);
            foreach (var item in groupOption)
            {
                if (item.Count() > 1)
                {
                    return Failed(ErrorCode.DataBaseError, "健康指标选项存在重复数据");
                }
            }
            List<string> optionRequestList = requestDto.HealthIndicatorOptionList.Select(s => s.OptionGuid).ToList();
            //判断是否存在必填的选项
            var healthIndicatorOptionModelList = (await new HealthIndicatorOptionBiz().GetHealthIndicatorOptionAsync(requestDto.IndicatorGuid));
            if (healthIndicatorOptionModelList == null)
            {
                return Failed(ErrorCode.DataBaseError, "健康指标选项不存在");
            }
            //全局匹配差集
            var infoCheckResult = optionRequestList.Except(healthIndicatorOptionModelList.Select(s => s.OptionGuid).ToList())?.ToList();
            if (infoCheckResult != null && infoCheckResult.Count != 0)
            {
                return Failed(ErrorCode.DataBaseError, "健康指标选项Id不存在");
            }
            //必须有的选项值
            List<string> optionStrList = healthIndicatorOptionModelList.Where(s => s.Required).Select(s => s.OptionGuid).ToList();
            if (optionStrList != null)
            {
                //差集
                var infoResult = optionStrList.Except(optionRequestList)?.ToList();
                if (infoResult != null && infoResult.Count != 0)
                {
                    return Failed(ErrorCode.DataBaseError, "健康指标选项必填项不能为空");
                }
                //校验必填项值是否为空
                foreach (var item in optionStrList)
                {
                    var requestValue = requestDto.HealthIndicatorOptionList.FirstOrDefault(s => s.OptionGuid == item);
                    if (requestValue != null)
                    {
                        if (!requestValue.IndicatorValue.HasValue)
                        {
                            return Failed(ErrorCode.DataBaseError, "健康指标选项必填项值不能为空");
                        }
                    }
                }
            }
            ConsumerIndicatorModel consumerIndicatorModel = new ConsumerIndicatorModel()
            {
                IndicatorRecordGuid = Guid.NewGuid().ToString("N"),
                IndicatorGuid = requestDto.IndicatorGuid,
                UserGuid = userId,
                CreatedBy = userId,
                CreationDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now
            };
            List<ConsumerIndicatorDetailModel> consumerIndicatorDetailModelList = requestDto.HealthIndicatorOptionList.Select(s => new ConsumerIndicatorDetailModel
            {
                RecordDetailGuid = Guid.NewGuid().ToString("N"),
                IndicatorRecordGuid = consumerIndicatorModel.IndicatorRecordGuid,
                IndicatorOptionGuid = s.OptionGuid,
                IndicatorValue = s.IndicatorValue.HasValue ? s.IndicatorValue.Value : (decimal?)null,
                CreatedBy = userId,
                CreationDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now
            }).ToList();
            //保存健康指标数据
            var result = await new HealthIndicatorBiz().CreateHealthIndicatorAsync(consumerIndicatorModel, consumerIndicatorDetailModelList);
            //用户更新指标数据判断是否需要预警操作
            if (result && string.IsNullOrWhiteSpace(requestDto.UserId))
            {
                //查询用户指标预警是否存在
                if (consumerIndicatorDetailModelList != null)
                {
                    var consumerModel = await new ConsumerBiz().GetModelAsync(userId);
                    //该用户绑定过了健康管理师
                    if (consumerModel != null && !string.IsNullOrWhiteSpace(consumerModel.HealthManagerGuid))
                    {
                        foreach (var item in consumerIndicatorDetailModelList)
                        {
                            var consumerIndicatorWarningLimitModel = await new IndicatorWarningLimitBiz().GetModelAsyncByUserAndOption(userId, item.IndicatorOptionGuid);
                            //存在预警开启
                            if (consumerIndicatorWarningLimitModel != null)
                            {
                                if (!item.IndicatorValue.HasValue || !consumerIndicatorWarningLimitModel.MaxValue.HasValue || !consumerIndicatorWarningLimitModel.MinValue.HasValue)
                                {
                                    continue;
                                }
                                //判断更新值是否正常范围
                                if (item.IndicatorValue.Value > consumerIndicatorWarningLimitModel.MaxValue || item.IndicatorValue.Value < consumerIndicatorWarningLimitModel.MinValue)
                                {
                                    //查找指标名称
                                    var healthIndicatorOptionModel = await new HealthIndicatorOptionBiz().GetAsync(item.IndicatorOptionGuid);
                                    if (healthIndicatorOptionModel == null)
                                    {
                                        Logger.Error("发送预警失败用户Id:" + userId + "未找对应指标Id:" + item.IndicatorOptionGuid);
                                        continue;
                                    }
                                    string msgStatus = string.Empty;
                                    if (item.IndicatorValue.Value > consumerIndicatorWarningLimitModel.MaxValue)
                                    {
                                        msgStatus = "高于预警值，";
                                    }
                                    else
                                    {
                                        msgStatus = "低于预警值，";
                                    }
                                    string msg = "该用户" + healthIndicatorOptionModel.OptionName + msgStatus + "当前值为" + item.IndicatorValue.Value + healthIndicatorOptionModel.OptionUnit;
                                    //判断之前预警记录表是否存在当前用户指标预警状态是待处理就改成已失效状态
                                    var consumerIndicatorWarningModel = await new IndicatorWarningBiz().GetModelAsyncByUserAndOption(userId, item.IndicatorOptionGuid);
                                    if (consumerIndicatorWarningModel != null)
                                    {
                                        consumerIndicatorWarningModel.Status = IndicatorWarningStatusEnum.Expired.ToString();
                                        //var resultUpdateWaring = await new IndicatorWarningBiz().UpdateAsync(consumerIndicatorWarningModel);
                                    }
                                    var userModel = new UserBiz().GetUser(userId);
                                    if (userModel == null)
                                    {
                                        Logger.Error("发送预警失败用户Id:" + userId + "未找对应用户:" + userId);
                                        continue;
                                    }
                                    int age = 0;
                                    if (userModel.Birthday.HasValue)
                                    {
                                        age = DateTime.Now.Year - userModel.Birthday.Value.Year;
                                        if (DateTime.Now.Month < userModel.Birthday.Value.Month || (DateTime.Now.Month == userModel.Birthday.Value.Month && DateTime.Now.Day < userModel.Birthday.Value.Day))
                                        {
                                            age--;
                                        }
                                        if (age < 0)
                                        {
                                            age = 0;
                                        }
                                    }
                                    //新增
                                    IndicatorWarningModel indicatorWarningModel = new IndicatorWarningModel
                                    {
                                        WarningGuid = Guid.NewGuid().ToString("N"),
                                        IndicatorOptionGuid = healthIndicatorOptionModel.OptionGuid,
                                        ConsumerGuid = userId,
                                        HealthManagerGuid = consumerModel.HealthManagerGuid,
                                        Name = userModel.UserName,
                                        Status = IndicatorWarningStatusEnum.Pending.ToString(),
                                        Age = age,
                                        Phone = userModel.Phone,
                                        CreatedBy = userId,
                                        LastUpdatedBy = userId,
                                        CreationDate = DateTime.Now,
                                        LastUpdatedDate = DateTime.Now,
                                        Description = msg
                                    };
                                    //数据操作方法
                                    var resultStatus = await new IndicatorWarningBiz().CreateUpdataeWarningAsync(consumerIndicatorWarningModel, indicatorWarningModel);
                                    if (!resultStatus)
                                    {
                                        Logger.Error("发送预警失败用户Id:" + userId + "数据修改或新增发生异常");
                                        continue;
                                    }
                                    var healthManagerModel = await new HealthManagerBiz().GetAsync(consumerModel.HealthManagerGuid);
                                    if (healthManagerModel == null)
                                    {
                                        Logger.Error("发送预警失败用户Id:" + userId + "未找对健康管理师Id:" + consumerModel.HealthManagerGuid);
                                    }
                                    if (string.IsNullOrWhiteSpace(healthManagerModel.EnterpriseUserId))
                                    {
                                        Logger.Error("发送预警失败用户Id:" + userId + "未找对健康管理师企业微信Id");
                                    }
                                    DateTime warningDt = DateTime.Now;
                                    var qyMsg = new QyTextCardMessageRequest
                                    {
                                        ToUser = healthManagerModel.EnterpriseUserId,
                                        TextCard = new QyTextCardMessageRequest.Content
                                        {
                                            Title = "预警通知",
                                            Description = $"<div class=\"normal\">用户姓名：{userModel.UserName}</div>" +
                                        $"<div class=\"normal\">用户年龄：{age}</div>" +
                                        $"<div class=\"normal\">用户手机：{userModel.Phone}</div>" +
                                        $"<div class=\"normal\">预警时间：{warningDt.ToString("yyyy-MM-dd HH:mm:ss")}</div>" +
                                        $"<div class=\"blue\">预警类型：{msg}</div>",
                                            Url = PlatformSettings.WarningUrl + indicatorWarningModel.WarningGuid,
                                            BtnTxt = "详情"
                                        },
                                        AgentId = PlatformSettings.HealthManagerMobileAgentid
                                    };
                                    //WarningTextcard content = new WarningTextcard
                                    //{
                                    //    Title = "预警通知",
                                    //    Description = $"<div class=\"normal\">用户姓名：{userModel.UserName}</div>" +
                                    //    $"<div class=\"normal\">用户年龄：{age}</div>" +
                                    //    $"<div class=\"normal\">用户手机：{userModel.Phone}</div>" +
                                    //    $"<div class=\"normal\">预警时间：{warningDt.ToString("yyyy-MM-dd HH:mm:ss")}</div>" +
                                    //    $"<div class=\"blue\">预警类型：{msg}</div>",
                                    //    Url = PlatformSettings.WarningUrl + indicatorWarningModel.WarningGuid
                                    //};
                                    ////发送预警消息
                                    //QyMessageWarningRequest massage = new QyMessageWarningRequest
                                    //{
                                    //    ToUser = healthManagerModel.EnterpriseUserId,//"20200100276",//
                                    //    Textcard = content,
                                    //    ToParty = "",
                                    //    MsgType = "textcard",
                                    //    AgentId = PlatformSettings.HealthManagerMobileAgentid
                                    //};
                                    var token = await EnterpriseWeChatApi.GetEnterpriseAccessToken(PlatformSettings.EnterpriseWeChatAppid, PlatformSettings.HealthManagerMobileSecret);
                                    var sendResult = await EnterpriseWeChatApi.SendQyMessageAsync(qyMsg, token.AccessToken);
                                    if (sendResult.Errcode != 0)
                                    {
                                        Logger.Error("发送预警失败用户Id:" + userId);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result ? Success() : Failed(ErrorCode.DataBaseError, "更新健康指标失败");
        }
        /// <summary>
        /// 更新医生建议
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> UpdateConsumerHealthIndicatorSuggestion([FromBody]
        UpdateHealthIndicatorSuggestion request)
        {
            var userBiz = new UserBiz();
            var model = await userBiz.GetModelAsync(request.UserGuid, true);
            if (model is null || !model.Enable)
            {
                return Failed(ErrorCode.Empty, "指定会员不存在");
            }

            var suggestionModel = new ConsumerIndicatorSuggestionModel()
            {
                IndicatorSuggestionGHuid = Guid.NewGuid().ToString("N"),
                Suggestion = request.Suggestion,
                UserGuid = request.UserGuid,
                CreatedBy = UserID,
                LastUpdatedBy = UserID
            };

            var result = await new HealthIndicatorBiz().UpdateConsumerHealthIndicatorSuggestion(suggestionModel);

            if (result)
            {
                new HealthRabbitMQNotificationBiz().HealthRabbitMQNotification(new HealthMessageDto()
                {
                    HealthType = HealthMessageDto.HealthTypeEnum.HealthIndicator,
                    Content = suggestionModel.Suggestion,
                    Title = "您的日常健康指标医生建议有更新",

                }, request.UserGuid);
            }

            return result ? Success() : Failed(ErrorCode.DataBaseError, "更新医生建议失败");
        }
    }
}
