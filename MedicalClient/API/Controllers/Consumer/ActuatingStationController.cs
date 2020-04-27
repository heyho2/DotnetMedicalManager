using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GD.Common;
using GD.Common.Helper;
using GD.Consumer;
using GD.DataAccess;
using GD.Dtos.Consumer.ActuatingStation;
using GD.Dtos.Consumer.Appointment;
using GD.Dtos.Merchant.Appointment;
using GD.Dtos.Merchant.Merchant;
using GD.Dtos.Merchant.Therapist;
using GD.Dtos.WeChat;
using GD.Mall;
using GD.Merchant;
using GD.Models.CommonEnum;
using GD.Models.Consumer;
using GD.Module.WeChat;
using GD.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GD.API.Controllers.Consumer
{
    /// <summary>
    /// 执行端
    /// </summary>
    public class ActuatingStationController : ConsumerBaseController
    {
        /// <summary>
        /// 执行端-获取我的预约列表(今日/全部)
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetMyAppointmentListByConditionResponseDto>))]
        public async Task<IActionResult> GetMyAppointmentListByCondition([FromBody]GetMyAppointmentListByConditionRequestDto requestDto)
        {
            if (string.IsNullOrWhiteSpace(requestDto.UserGuid))
            {
                requestDto.UserGuid = UserID;
            }
            //var therapistModel = await new TherapistBiz().GetModelAsync(requestDto.UserGuid);
            var result = await new ActuatingStationBiz().GetMyAppointmentListByCondition(requestDto);
            return Success(result);
        }

        /// <summary>
        /// 执行端-预约的开始与结束
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public IActionResult ChangeAppointmentStatus([FromBody]ChangeAppointmentStatusRequestDto requestDto)
        {
            var model = new ConsumptionBiz().GetModel(requestDto.ConsumptionGuid);
            if (model == null)
            {
                return Failed(ErrorCode.Empty, "没找到该预约信息！");
            }

            var isSuccess = false;
            switch (requestDto.ConsumptionStatus)
            {
                case ConsumptionStatusEnum.Canceled:
                    if (!model.ConsumptionStatus.Equals(ConsumptionStatusEnum.Booked.ToString()))
                    {
                        return Failed(ErrorCode.UserData, "该预约记录的状态不支持取消，请检查！");
                    }
                    //取消预约 须在个人产品数量上+1
                    isSuccess = ChangeStatus(model, ConsumptionStatusEnum.Canceled);
                    break;
                case ConsumptionStatusEnum.Arrive:
                    if (!model.ConsumptionStatus.Equals(ConsumptionStatusEnum.Booked.ToString()))
                    {
                        return Failed(ErrorCode.UserData, "该预约记录的状态不支持取消，请检查！");
                    }
                    isSuccess = ChangeStatus(model, ConsumptionStatusEnum.Arrive);
                    break;
                case ConsumptionStatusEnum.Completed:
                    if (!model.ConsumptionStatus.Equals(ConsumptionStatusEnum.Arrive.ToString()))
                    {
                        return Failed(ErrorCode.UserData, "该预约记录的状态不支持取消，请检查！");
                    }
                    isSuccess = ChangeStatus(model, ConsumptionStatusEnum.Arrive);
                    break;
                default:
                    return Failed(ErrorCode.UserData, "该预约记录的状态不支持该操作，请检查！");
            }
            return Success(isSuccess);
        }

        /// <summary>
        /// 美疗师启动预约的消费
        /// </summary>
        /// <param name="consumptionGuid">预约消费guid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> StartConsumption(string consumptionGuid)
        {
            var consumptionBiz = new ConsumptionBiz();
            var model = consumptionBiz.GetModel(consumptionGuid);
            if (model == null)
            {
                return Failed(ErrorCode.Empty, "没找到该预约信息！");
            }
            var checkWorking = await consumptionBiz.CheckHasWorkingConsumptionAsync(model.OperatorGuid);
            if (checkWorking)
            {
                return Failed(ErrorCode.UserData, "尚有服务正在进行中，请先完成进行中的服务");
            }
            if (!string.Equals(model.ConsumptionStatus.ToString(), ConsumptionStatusEnum.Booked.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return Failed(ErrorCode.UserData, "该消费记录不处于[已预约]状态，不能执行开始操作");
            }
            var result = ChangeStatus(model, ConsumptionStatusEnum.Arrive);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "开始操作失败");
        }

        /// <summary>
        /// 美疗师完成预约的消费
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> CompleteConsumption([FromBody]CompleteConsumptionRequestDto requestDto)
        {
            var model = new ConsumptionBiz().GetModel(requestDto.ConsumptionGuid);
            if (model == null)
            {
                return Failed(ErrorCode.Empty, "没找到该预约信息！");
            }
            if (!string.Equals(model.ConsumptionStatus.ToString(), ConsumptionStatusEnum.Arrive.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return Failed(ErrorCode.UserData, "该消费记录不处于[已到店开始]状态，不能执行开始操作");
            }
            model.MerchantRemark = requestDto.MerchantRemark;
            model.ConsumptionStatus = ConsumptionStatusEnum.Completed.ToString();
            model.ConsumptionEndDate = DateTime.Now;
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            var result = await new ConsumptionBiz().UpdateAsync(model);
            NotifyUser(model.ConsumptionGuid);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "完成操作失败");
        }

        /// <summary>
        /// 用户预约的服务完成后发送微信模板消息通知
        /// </summary>
        /// <param name="consumptionGuid"></param>
        private void NotifyUser(string consumptionGuid)
        {
            Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(PlatformSettings.UserAppointmentCompletedNotificationTemplate))
                {
                    return;
                }

                var userOpenId = "";
                string controllerName = ControllerContext.ActionDescriptor.ControllerName;
                string actionName = ControllerContext.ActionDescriptor.ActionName;
                try
                {
                    var model = new ConsumptionBiz().GetModel(consumptionGuid);
                    var userModel = new UserBiz().GetModelAsync(model.UserGuid).Result;
                    Logger.Debug($"NotifyUser-用户预约的服务已完成-用户openid-{userModel?.WechatOpenid}");
                    if (string.IsNullOrWhiteSpace(userModel?.WechatOpenid ?? ""))
                    {
                        return;
                    }
                    var merchantModel = new MerchantBiz().GetAsync(model.MerchantGuid).Result;
                    var projectModel = new ProjectBiz().GetAsync(model.ProjectGuid).Result;
                    var resToken = WeChartApi.GetAccessToken(PlatformSettings.CDClientAppId, PlatformSettings.CDClientAppSecret).Result;
                    Logger.Debug($"NotifyUser-用户预约的服务已完成-获取token-{JsonConvert.SerializeObject(resToken)}");
                    if (string.IsNullOrWhiteSpace(resToken.AccessToken))
                    {
                        Common.Helper.Logger.Error($"GD.API.Controllers.Consumer.{controllerName}.{actionName}  openId:[{userOpenId}] {Environment.NewLine} error:用户预约后发送模板消息通知云医执行端公众号-获取token失败。{resToken.Errmsg}");
                        return;
                    }
                    userOpenId = userModel.WechatOpenid;
                    var clientMsg = new WeChatTemplateMsg
                    {
                        Touser = userModel.WechatOpenid,
                        Template_Id = PlatformSettings.UserAppointmentNotificationTemplate,
                        Data = new
                        {
                            First = new { Value = "【您预约的服务已完成】" },
                            //完成时间
                            Keyword1 = new { Value = $"{model.ConsumptionEndDate.Value.ToString("MM月dd日 HH:mm")}" },
                            //服务项目
                            Keyword2 = new { Value = projectModel?.ProjectName },
                            //服务门店
                            Keyword3 = new { Value = merchantModel.MerchantName },
                            //备注
                            Remark = new { Value = "如对我们服务有什么建议，请及时与我们联系" },
                        }
                    };
                    var clientTempMsgRes = WeChartApi.SendTemplateMsg(clientMsg, resToken.AccessToken);
                    Logger.Debug($"NotifyUser-用户预约的服务已完成-发送模板消息-{JsonConvert.SerializeObject(clientTempMsgRes)}");
                }
                catch (Exception ex)
                {
                    Logger.Error($"GD.API.Controllers.Consumer.{controllerName}.{actionName}  openId:[{userOpenId}] {Environment.NewLine} error:用户预约服务完成后发送模板消息通知云医用户端公众号端失败。{ex.Message}");
                }
            });
        }

        /// <summary>
        /// 美疗师设置预约记录过期
        /// </summary>
        /// <param name="consumptionGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> SetMissStautsForConsumptionAsync(string consumptionGuid)
        {
            var consumptionBiz = new ConsumptionBiz();
            var model = consumptionBiz.GetModel(consumptionGuid);
            if (model == null)
            {
                return Failed(ErrorCode.Empty, "没找到该预约信息！");
            }
            if (!string.Equals(model.ConsumptionStatus.ToString(), ConsumptionStatusEnum.Booked.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return Failed(ErrorCode.UserData, "该预约记录不处于[已预约]状态，不能修改状态为[已错过]");
            }
            if (DateTime.Now < model.AppointmentDate)
            {
                return Failed(ErrorCode.UserData, "当前还未到预约的时间，不能修改预约记录状态为[已错过]");
            }
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            var result = await consumptionBiz.UpdateAsync(model);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "修改预约记录状态为[已错过]失败");

        }


        /// <summary>
        /// 状态更新
        /// </summary>
        /// <param name="model"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        private bool ChangeStatus(ConsumptionModel model, ConsumptionStatusEnum status)
        {
            model.ConsumptionStatus = status.ToString();
            if (string.Equals(model.ConsumptionStatus, ConsumptionStatusEnum.Arrive.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                model.ConsumptionDate = DateTime.Now;
            }
            else if (string.Equals(model.ConsumptionStatus, ConsumptionStatusEnum.Completed.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                model.ConsumptionEndDate = DateTime.Now;
            }
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            return new ConsumptionBiz().UpdateAsync(model).Result;
        }

        /// <summary>
        /// 执行端-获取我的月排班-查看当天预约班次信息等
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<SelectOneDayMySchduleInfoResponseDto>>))]
        public async Task<IActionResult> SelectOneDayMySchduleInfo([FromBody]SelectOneDayMySchduleInfoRequestDto requestDto)
        {
            if (string.IsNullOrEmpty(requestDto.TherapistGuid))
            {
                requestDto.TherapistGuid = UserID;
            }

            var response = await new ActuatingStationBiz().SelectOneDayMySchduleInfo(requestDto);
            return Success(response);
        }

        /// <summary>
        /// 美疗师执行端登录
        /// </summary>
        /// <param name="loginRequestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<TherapistLoginResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> TherapistLoginAsync([FromBody]TherapistLoginRequestDto loginRequestDto)
        {
            var model = await new TherapistBiz().GetModelByPhoneAsync(loginRequestDto.TherapistPhone);

            if (model == null)
            {
                return Failed(ErrorCode.InvalidIdPassword);
            }

            if (!string.Equals(model.TherapistPassword, CryptoHelper.AddSalt(model.TherapistGuid, loginRequestDto.TherapistPassword), StringComparison.OrdinalIgnoreCase))
            {
                return Failed(ErrorCode.InvalidIdPassword);
            }

            var response = new TherapistLoginResponseDto
            {
                TherapistGuid = model.TherapistGuid,
                TherapistName = model.TherapistName,
                Token = CreateToken(model.TherapistGuid, Common.EnumDefine.UserType.Aesthetician, 30),
            };

            return Success(response);
        }


        /// <summary>
        /// 美疗师执行端修改密码 
        /// 重置密码
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<TherapistLoginResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> TherapistResetPasswordAsync([FromBody]TherapistResetPasswordAsyncRequestDto requestDto)
        {
            var therapistBiz = new TherapistBiz();

            var biz = new AccountBiz();

            if (!biz.VerifyCode(requestDto.Phone, requestDto.Code))
            {
                return Failed(ErrorCode.VerificationCode, "手机验证码错误！");
            }

            var model = await therapistBiz.GetModelByPhoneAsync(requestDto.Phone);

            if (model == null)
            {
                return Failed(ErrorCode.Empty, "该手机号未注册");
            }

            model.LastUpdatedBy = string.IsNullOrWhiteSpace(UserID) ? "test" : UserID;
            model.TherapistPassword = CryptoHelper.AddSalt(model.TherapistGuid, requestDto.Password);
            if (string.IsNullOrEmpty(model.TherapistPassword))
            {
                return Failed(ErrorCode.SystemException, "密码加盐失败");
            }

            return therapistBiz.UpdateAsync(model).Result ? Success() : Failed(ErrorCode.DataBaseError, "密码更新失败！");
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public IActionResult TherapistUpdatePassword(string password)
        {
            // 前端传输的密码为MD5加密后的结果
            if (string.IsNullOrEmpty(password) || password.Length != 32)
            {
                return Failed(ErrorCode.FormatError, "密码为空或者无效");
            }
            var therapistBiz = new TherapistBiz();
            var biz = new AccountBiz();
            var userModel = therapistBiz.GetAsync(UserID).Result;
            if (userModel == null)
            {
                return Failed(ErrorCode.Empty, "用户不存在或者已经注销");
            }

            userModel.TherapistPassword = CryptoHelper.AddSalt(UserID, password);
            if (string.IsNullOrEmpty(userModel.TherapistPassword))
            {
                return Failed(ErrorCode.SystemException, "密码加盐失败");
            }

            return therapistBiz.UpdateAsync(userModel).Result ? Success() : Failed(ErrorCode.DataBaseError, "密码更新失败");
        }

        /// <summary>
        /// 获取美疗师资料
        /// </summary>
        /// <param name="therapistId"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetTherapistInfoResponseDto>))]
        public async Task<IActionResult> GetTherapistInfoAsync(string therapistId)
        {
            var model = await new TherapistBiz().GetAsync(therapistId);
            var picture = await new AccessoryBiz().GetAsync(model.PortraitGuid);
            var result = new GetTherapistInfoResponseDto
            {
                TherapistName = model.TherapistName,
                TherapistPhone = model.TherapistPhone,
                MerchantGuid = model.MerchantGuid,
                PortraitUrl = $"{picture?.BasePath}{picture?.RelativePath}"
            };
            return Success(result);
        }


    }
}
