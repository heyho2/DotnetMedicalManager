using GD.Common;
using GD.Common.Helper;
using GD.DataAccess;
using GD.Dtos.Merchant.Category;
using GD.Dtos.Merchant.Merchant;
using GD.Dtos.Merchant.Therapist;
using GD.Merchant;
using GD.Models.Merchant;
using GD.Module.WeChat;
using GD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.API.Controllers.Merchant
{
    /// <summary>
    /// 服务人员控制器
    /// </summary>
    public class TherapistController : MerchantBaseController
    {
        /// <summary>
        /// 获取服务人员列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetTherapistListResponseDto>))]
        public async Task<IActionResult> GetTherapistListAsync([FromQuery]GetTherapistListRequestDto requestDto)
        {
            requestDto.MerchantGuid = string.IsNullOrWhiteSpace(requestDto.MerchantGuid) ? (UserID ?? "") : requestDto.MerchantGuid;

            if (string.IsNullOrWhiteSpace(requestDto.MerchantGuid))
            {
                return Failed(ErrorCode.Empty, "商铺guid必填");
            }

            var response = await new TherapistBiz().GetTherapistListAsync(requestDto);

            return Success(response);
        }

        /// <summary>
        /// 获取指定商户指定分类下项目列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetMerchantClassifyProjectListResponseDto>>))]
        public async Task<IActionResult> GetMerchantClassifyProjects(string classifyId, string name)
        {
            if (string.IsNullOrEmpty(classifyId))
            {
                return Failed(ErrorCode.Empty, "参数不正确");
            }

            var classfieds = classifyId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct();

            if (classfieds.Count() <= 0)
            {
                return Failed(ErrorCode.Empty, "参数不正确");
            }

            var categoryBiz = new MerchantCategoryBiz();

            var projects = await categoryBiz.GetMerchantClassifyProjects(UserID, classfieds, name);

            return Success(projects);
        }

        /// <summary>
        /// 获取指定商户下用户（手机号码）已购大类项目
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetMerchantConsumerClassifyListResponseDto>>))]
        public async Task<IActionResult> GetMerchantConsumerClassifies(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                return Failed(ErrorCode.Empty, "参数不正确");
            }

            var categoryBiz = new MerchantCategoryBiz();

            var userModel = new UserBiz().GetModelByPhoneAsync(phone);

            if (userModel is null)
            {
                return Failed(ErrorCode.Empty, "无此用户");
            }

            var classifies = await categoryBiz.GetMerchantConsumerClassifies(UserID, userModel.UserGuid);

            return Success((classifies == null || classifies.Count <= 0) ? new List<GetMerchantConsumerClassifyListResponseDto>() : classifies);
        }

        /// <summary>
        /// 获取指定商户下用户（手机号码）已购服务项目列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetConsumerProjectsResponseDto>>))]
        public async Task<IActionResult> GetConsumerProjects(string classifyGuid, string phone)
        {
            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(classifyGuid))
            {
                return Failed(ErrorCode.Empty, "参数不正确");
            }

            var userModel = new UserBiz().GetModelByPhoneAsync(phone);

            if (userModel is null)
            {
                return Failed(ErrorCode.Empty, "无此用户");
            }

            var categoryBiz = new MerchantCategoryBiz();

            var projects = await categoryBiz.GetMerchantConsumerProjects(UserID, classifyGuid, userModel.UserGuid);

            return Success((projects == null || projects.Count <= 0) ? new List<GetConsumerProjectsResponseDto>() : projects);
        }

        /// <summary>
        ///  新增服务人员及其关联项目
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public IActionResult AddNewTherapist([FromBody] AddNewTherapistRequestDto requestDto)
        {
            if (requestDto.ClassifyGuids.Count() <= 0)
            {
                return Failed(ErrorCode.Empty, "所属大类未选择");
            }

            if (requestDto.Tag?.Count() <= 0)
            {
                return Failed(ErrorCode.Empty, "擅长需填写");
            }

            if (string.Join("", requestDto.Tag).Length > 300)
            {
                return Failed(ErrorCode.Empty, "擅长超过最大长度限制");
            }

            if (requestDto.MerchantProjectGuidList.Count() <= 0)
            {
                return Failed(ErrorCode.Empty, "服务项目未选择");
            }

            if (!string.IsNullOrEmpty(requestDto.Introduction))
            {
                if (requestDto.Introduction.Length > 500)
                {
                    return Failed(ErrorCode.Empty, "个人简介超过最大长度限制");
                }
            }

            var therapistBiz = new TherapistBiz();

            var IsTherapistPhoneExist = therapistBiz.IsTherapistPhoneExist(requestDto.TherapistPhone);

            if (IsTherapistPhoneExist)
            {
                return Failed(ErrorCode.UserData, "该手机号已注册！");
            }

            var therapistGuid = Guid.NewGuid().ToString("N");

            var tModel = new TherapistModel()
            {
                TherapistGuid = therapistGuid,
                TherapistName = requestDto.TherapistName,
                JobTitle = requestDto.JobTitle,
                MerchantGuid = UserID,
                PortraitGuid = requestDto.PortraitGuid,
                TherapistPhone = requestDto.TherapistPhone,
                TherapistPassword = CryptoHelper.AddSalt(therapistGuid, requestDto.TherapistPassword),
                Introduction = requestDto.Introduction,
                Tag = JsonConvert.SerializeObject(requestDto.Tag),
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                LastUpdatedBy = UserID,
                LastUpdatedDate = DateTime.Now
            };

            var tpModelList = requestDto.MerchantProjectGuidList.Distinct().Select(d => new TherapistProjectModel()
            {
                TherapistProjectGuid = Guid.NewGuid().ToString("N"),
                TherapistGuid = tModel.TherapistGuid,
                ProjectGuid = d,
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                LastUpdatedBy = UserID,
                LastUpdatedDate = DateTime.Now,
                Enable = true
            }).ToList();

            var classifyModels = requestDto.ClassifyGuids.Distinct().Select(d => new MerchantTherapistClassifyModel()
            {
                TherapistClassifyGuid = Guid.NewGuid().ToString("N"),
                TherapistGuid = tModel.TherapistGuid,
                ClassifyGuid = d,
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                LastUpdatedBy = UserID,
                LastUpdatedDate = DateTime.Now,
                Enable = true,
                OrgGuid = ""
            }).ToList();

            var response = therapistBiz.AddNewTherapist(tModel, tpModelList, classifyModels);

            return Success(response);
        }

        /// <summary>
        /// 获取指定服务人员详细信息
        /// </summary>
        /// <param name="therapistGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetTherapistDetailInfoResponseDto>))]
        public async Task<IActionResult> GetTherapistDetailInfo(string therapistGuid)
        {
            if (string.IsNullOrEmpty(therapistGuid))
            {
                return Failed(ErrorCode.Empty, "参数不正确");
            }

            var therapistBiz = new TherapistBiz();

            var therapist = await therapistBiz.GetTherapistDetailInfoAsync(UserID, therapistGuid);

            return Success(therapist);
        }

        /// <summary>
        /// 更新服务人员
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> UpdateTherapist([FromBody] ModifyTherapistRequestDto requestDto)
        {
            if (requestDto.ClassifyGuids.Count() <= 0)
            {
                return Failed(ErrorCode.Empty, "所属大类未选择");
            }

            if (requestDto.Tag?.Count() <= 0)
            {
                return Failed(ErrorCode.Empty, "擅长需填写");
            }

            if (string.Join("", requestDto.Tag).Length > 300)
            {
                return Failed(ErrorCode.Empty, "擅长超过最大长度限制");
            }

            if (requestDto?.MerchantProjectGuidList.Count() <= 0)
            {
                return Failed(ErrorCode.Empty, "服务项目未选择");
            }

            if (!string.IsNullOrEmpty(requestDto.Introduction))
            {
                if (requestDto.Introduction.Length > 500)
                {
                    return Failed(ErrorCode.Empty, "个人简介超过最大长度限制");
                }
            }

            var therapistBiz = new TherapistBiz();

            var model = await therapistBiz.GetModelAsync(requestDto.TherapistGuid);

            if (model is null)
            {
                return Failed(ErrorCode.Empty, "服务人员不存在");
            }

            var IsTherapistPhoneExist = therapistBiz.IsTherapistPhoneExist(requestDto.TherapistPhone, true, requestDto.TherapistGuid);

            if (IsTherapistPhoneExist)
            {
                return Failed(ErrorCode.UserData, "该手机号已注册！");
            }

            model.TherapistName = requestDto.TherapistName;
            model.TherapistPhone = requestDto.TherapistPhone;
            model.PortraitGuid = requestDto.PortraitGuid;
            model.JobTitle = requestDto.JobTitle;
            model.Introduction = requestDto.Introduction;
            model.Tag = JsonConvert.SerializeObject(requestDto.Tag);
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;

            var tpModelList = requestDto.MerchantProjectGuidList.Distinct().Select(d => new TherapistProjectModel()
            {
                TherapistProjectGuid = Guid.NewGuid().ToString("N"),
                TherapistGuid = model.TherapistGuid,
                ProjectGuid = d,
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                LastUpdatedBy = UserID,
                LastUpdatedDate = DateTime.Now,
                Enable = true
            }).ToList();

            var classifyModels = requestDto.ClassifyGuids.Distinct().Select(d => new MerchantTherapistClassifyModel()
            {
                TherapistClassifyGuid = Guid.NewGuid().ToString("N"),
                TherapistGuid = model.TherapistGuid,
                ClassifyGuid = d,
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                LastUpdatedBy = UserID,
                LastUpdatedDate = DateTime.Now,
                Enable = true,
                OrgGuid = ""
            }).ToList();

            var result = therapistBiz.UpdateTherapist(model, tpModelList, classifyModels);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "更新失败，请检查！");
        }

        /// <summary>
        /// 服务人员绑定微信OpenId
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto)), AllowAnonymous]
        public async Task<IActionResult> BindTherapistWeChatOpenIdAsync([FromBody]BindTherapistWeChatOpenIdRequestDto requestDto)
        {
            var biz = new TherapistBiz();

            var model = await biz.GetModelByPhoneAsync(requestDto.TherapistPhone);

            if (model == null)
            {
                return Failed(ErrorCode.InvalidIdPassword);
            }

            if (!string.Equals(model.TherapistPassword, CryptoHelper.AddSalt(model.TherapistGuid, requestDto.TherapistPassword), StringComparison.OrdinalIgnoreCase))
            {
                return Failed(ErrorCode.InvalidIdPassword);
            }

            if (string.Equals(model.WeChatOpenId, requestDto.WeChatOpenId, StringComparison.OrdinalIgnoreCase))
            {
                return Failed(ErrorCode.UserData, "已绑定过，无需重复绑定");
            }

            model.WeChatOpenId = requestDto.WeChatOpenId;

            var result = await biz.UpdateAsync(model);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "服务人员绑定微信失败");
        }

        /// <summary>
        /// 重置服务人员为手机号后六位
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ResetTherapistPwdAsync(string therapistId)
        {
            var therapistModel = await new TherapistBiz().GetModelAsync(therapistId);

            if (therapistModel == null)
            {
                return Failed(ErrorCode.Empty, "无此服务人员数据，请核对");
            }

            //手机号后六位
            var sourcesMd5Pwd = therapistModel.TherapistPhone.Substring
                (therapistModel.TherapistPhone.Length - 6).Md5().ToUpper();

            var pwd = CryptoHelper.AddSalt(therapistModel.TherapistGuid, sourcesMd5Pwd);

            therapistModel.TherapistPassword = pwd;

            var affect = therapistModel.Update();

            return affect > 0 ? Success() : Failed(ErrorCode.DataBaseError, "重置服务人员密码错误");
        }

        /// <summary>
        /// 获取商户端某天某项目的服务人员和排班详情(不分页)
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetTherapistsScheduleByProjectIdOneDayResponseDto>>))]
        public async Task<IActionResult> GetTherapistsScheduleByProjectIdOneDayAsync([FromBody]GetTherapistsScheduleByProjectIdOneDayRequestDto requestDto)
        {
            requestDto.ScheduleDate = DateTime.Now;
            requestDto.MerchantGuid = UserID;

            var result = await new TherapistBiz().GetTherapistsScheduleByProjectIdOneDayAsync(requestDto);

            if (result == null)
            {
                return Success();
            }

            var scheduleTemplateModel = await new ScheduleTemplateBiz().GetModelByDateAsync(requestDto.MerchantGuid, requestDto.ScheduleDate);

            if (scheduleTemplateModel == null)
            {
                return Failed(ErrorCode.Empty, "店铺当天未进行排班！");
            }

            var times = await GetMerchantMaxDurationTimeButtonsAsync(requestDto.MerchantGuid, scheduleTemplateModel.TemplateGuid);

            if (!times.Any())
            {
                return Failed(ErrorCode.Empty, "店铺未配置班次，请先配置班次！");
            }

            var (currentDay, minAppointTime) = CheckCurrentDayGetMinAppointTime(requestDto.ScheduleDate);

            var groupDetails = result.ScheduleDetails.GroupBy(a => a.TherapistGuid);

            var response = new List<GetTherapistsScheduleByProjectIdOneDayResponseDto>();

            foreach (var therapist in result.Therapists)
            {
                var therapistDetails = (groupDetails.FirstOrDefault(a => a.Key == therapist.TherapistGuid))?.ToList();

                var responseItem = new GetTherapistsScheduleByProjectIdOneDayResponseDto
                {
                    TherapistGuid = therapist.TherapistGuid,
                    TherapistName = therapist.TherapistName,
                    PortraitUrl = therapist.PortraitUrl,
                    ScheduleGuid = therapistDetails?.FirstOrDefault()?.ScheduleGuid,
                    ScheduleDetails = new List<ScheduleTimeDetailDto>()
                };

                if (therapistDetails != null)
                {
                    foreach (var time in times)
                    {
                        var res = therapistDetails?.FirstOrDefault(a => a.StartTime != null && a.StartTime.CompareTo(time.StartTime) <= 0 && a.EndTime.CompareTo(time.StartTime) > 0);

                        var scheduleTimeDetail = new ScheduleTimeDetailDto
                        {
                            ScheduleDetailGuid = res?.ScheduleDetailGuid,
                            StartTime = time.StartTime,
                            EndTime = time.EndTime,
                            ConsumptionGuid = res?.ConsumptionGuid,
                            Occupy = res != null
                        };

                        if (currentDay && time.StartTime.CompareTo(minAppointTime) < 0)
                        {
                            scheduleTimeDetail.Occupy = true;
                        }

                        responseItem.ScheduleDetails.Add(scheduleTimeDetail);

                    }
                    response.Add(responseItem);
                }
            }
            return Success(response);
        }

        /// <summary>
        /// 获取商户最大营业时间的时间刻度按钮
        /// </summary>
        /// <param name="merchantId">商户Id</param>
        /// <param name="templateGuid"></param>
        /// <param name="timeStep">时间刻度按钮间隔，默认为15分钟</param>
        /// <returns></returns>
        private async Task<List<TimeDto>> GetMerchantMaxDurationTimeButtonsAsync(string merchantId, string templateGuid, int timeStep = 15)
        {
            var maxDuration = await new MerchantWorkShiftDetailBiz().GetMaxDuration(merchantId, templateGuid);
            if (maxDuration == null)
            {
                return new List<TimeDto>();
            }
            string businessHoursStart = Convert.ToDateTime(maxDuration.StartTime).ToString("HH:mm");
            string businessHourEnd = Convert.ToDateTime(maxDuration.EndTime).ToString("HH:mm");
            DateTime dtStart = Convert.ToDateTime(businessHoursStart);
            DateTime dtEnd = Convert.ToDateTime(businessHourEnd);
            DateTime nextTime = dtStart;
            List<TimeDto> lstTime = new List<TimeDto>();
            while (nextTime < dtEnd)
            {
                lstTime.Add(new TimeDto { StartTime = nextTime.ToString("HH:mm"), EndTime = nextTime.AddMinutes(timeStep).ToString("HH:mm") });
                nextTime = nextTime.AddMinutes(timeStep);
            }
            return lstTime;

        }

        /// <summary>
        /// 检测当前排班是否是当天，并获取当天能够预约的最早时间
        /// </summary>
        /// <param name="scheduleDate"></param>
        /// <returns></returns>
        (bool currentDay, string minAppointTime) CheckCurrentDayGetMinAppointTime(DateTime scheduleDate)
        {
            var currentDate = DateTime.Now;

            var minAppointTime = currentDate.AddMinutes(30).ToString("HH:mm");

            if (currentDate.AddMinutes(30).Date > currentDate.Date)
            {
                minAppointTime = "23:59";
            }

            var isCurrentDay = scheduleDate.Date == currentDate.Date;

            return (isCurrentDay, minAppointTime);
        }

        /// <summary>
        /// 获取指定商户分类下服务人员列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetMerchantClassifyTherapistListResponseDto>>))]
        public async Task<IActionResult> GetMerchantClassifyTherapistsAsync()
        {
            var response = await new TherapistBiz().GetMerchantClassifyTherapists(UserID);

            return Success(response);
        }

        /// <summary>
        /// 获取服务人员端口AppId
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetTherapistClientAppId()
        {
            var appId = PlatformSettings.DoctorClientAppId;
            return Success(appId, null);
        }

        /// <summary>
        /// 更新服务人员OpenId
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateTherapistOpenIdAsync([FromBody]UpdateTherapistOpenIdRequestDto requestDto)
        {
            var result = await WeChartApi.Oauth2AccessTokenAsync(PlatformSettings.DoctorClientAppId, PlatformSettings.DoctorClientAppSecret, requestDto.Code);
            if (result.Errcode != 0)
            {
                return Failed(ErrorCode.SystemException, "获取服务人员openId失败");
            }
            var model = await new TherapistBiz().GetAsync(UserID);
            if (model == null)
            {
                return Failed(ErrorCode.UserData, "无此服务人员数据");
            }
            if (!string.IsNullOrWhiteSpace(result.OpenId) && model.WeChatOpenId != result.OpenId)
            {
                model.WeChatOpenId = result.OpenId;
                model.LastUpdatedDate = DateTime.Now;
                model.LastUpdatedBy = UserID;
                var res = await new TherapistBiz().UpdateAsync(model);
            }
            return Success();
        }
    }
}
