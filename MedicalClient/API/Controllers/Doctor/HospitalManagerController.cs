using GD.API.Code;
using GD.AppSettings;
using GD.Common;
using GD.Common.Helper;
using GD.Consumer;
using GD.Doctor;
using GD.Dtos.Common;
using GD.Dtos.Doctor.Doctor;
using GD.Dtos.Doctor.Hospital;
using GD.Dtos.Enum.DoctorAppointment;
using GD.Models.Consumer;
using GD.Module.WeChat;
using GD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GD.API.Controllers.Doctor
{

    /// <summary>
    /// 医院管理端
    /// </summary>
    public class HospitalManagerController : DoctorBaseController
    {
        #region 账号管理

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginRequestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<HospitalLoginResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody]HosipitalLoginRequestDto loginRequestDto)
        {
            var hospitalBiz = new HospitalBiz();

            var model = await hospitalBiz.GetModelByAccountAsync(loginRequestDto.Account);

            if (model is null)
            {
                return Failed(ErrorCode.Empty, "账号不存在或已禁用");
            }

            if (!model.Password.Equals(CryptoHelper.AddSalt(model.HospitalGuid, loginRequestDto.Password), StringComparison.OrdinalIgnoreCase))
            {
                return Failed(ErrorCode.Empty, "账号或密码错误");
            }

            var response = new HospitalLoginResponseDto
            {
                HospitalGuid = model.HospitalGuid,
                HospitalName = model.HosName,
                Token = CreateToken(model.HospitalGuid, Common.EnumDefine.UserType.Doctor, 30),
            };

            return Success(response);
        }
        /// <summary>
        /// 获取企业微信配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous, Produces(typeof(ResponseDto<GetEnterpriseWeChatResponse>))]
        public IActionResult GetEnterpriseWeChatConfig()
        {
            GetEnterpriseWeChatResponse response = new GetEnterpriseWeChatResponse();
            response.Appid = PlatformSettings.EnterpriseWeChatAppid;
            response.Agentid = PlatformSettings.EnterpriseWeChatAgentid;
            return Success(response);
        }
        /// <summary>
        /// 企业微信授权登录
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous, Produces(typeof(HospitalLoginResponseDto))]
        public async Task<IActionResult> EnterpriseWeChatLogin(string code)
        {
            HospitalLoginResponseDto response = null;
            var result = await GetEnterpriseWeChatUserInfo(code);
            if (result == null)
            {
                return Failed(ErrorCode.Empty, "未找到对应用户信息");
            }
            //根据用户部门Id查找对应医院数据
            var enterprise = new EnterpriseWeChat();
            PlatformSettings.Mappings.Bind(enterprise);
            List<Mapping> mapList = enterprise?.Mapping.ToList();

            if (mapList == null)
            {
                return Failed(ErrorCode.Empty, "配置信息错误");
            }
            int[] departmentList = result.department;
            if (departmentList == null || departmentList.Count() == 0)
            {
                return Failed(ErrorCode.Empty, "所属部门为空");
            }
            var hospitalBiz = new HospitalBiz();
            foreach (var item in departmentList)
            {
                var mappingModel = mapList.FirstOrDefault(s => s.DepartmentId == item);
                if (mappingModel != null)
                {
                    var model = await hospitalBiz.GetAsync(mappingModel.HosId);
                    if (model == null)
                    {
                        return Failed(ErrorCode.Empty, "未找到对应医院");
                    }
                    //找到对应医院账号进行登录
                    response = new HospitalLoginResponseDto
                    {
                        HospitalGuid = model.HospitalGuid,
                        HospitalName = model.HosName,
                        Token = CreateToken(model.HospitalGuid, Common.EnumDefine.UserType.Doctor, 30),
                    };
                    return Success(response);
                }
            }
            return Failed(ErrorCode.Empty, "未找到对应用户所在医院");
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private async Task<UserDetail> GetEnterpriseWeChatUserInfo(string code)
        {
            UserDetail enterpriseWeChatUserInfo = null;
            //1.获取（access_token）缓存两小时
            var aToken = await EnterpriseWeChatApi.GetEnterpriseAccessToken(PlatformSettings.EnterpriseWeChatAppid, PlatformSettings.EnterpriseWeChatSecret);
            if (aToken == null)
            {
                return enterpriseWeChatUserInfo;
            }
            //2.根据code查找用户信息数据
            EnterpriseWeChatApi bill = new EnterpriseWeChatApi();
            string getUserurl = $"https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={aToken.AccessToken}&code={code}";
            var userModel = await bill.Send<UserModel>(getUserurl);
            if (userModel == null || string.IsNullOrEmpty(userModel.UserId))
            {
                return enterpriseWeChatUserInfo;
            }
            //3.根据用户Id查询用户信息
            string getUserDetailurl = $"https://qyapi.weixin.qq.com/cgi-bin/user/get?access_token={aToken.AccessToken}&userid={userModel.UserId}";
            var userDetail = await bill.Send<UserDetail>(getUserDetailurl);
            if (userModel == null)
            {
                return enterpriseWeChatUserInfo;
            }
            enterpriseWeChatUserInfo = userDetail;
            return enterpriseWeChatUserInfo;
        }
        /// <summary>
        /// 修改密码 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> ModifyPassword([FromBody]HospitalModifyPasswordRequestDto requestDto)
        {
            if (string.IsNullOrEmpty(UserID))
            {
                return Failed(ErrorCode.Empty, "账号不存在或已禁用");
            }

            var hospitalBiz = new HospitalBiz();

            var model = await hospitalBiz.GetAsync(UserID);

            if (model is null)
            {
                return Failed(ErrorCode.Empty, "账号不存在或已禁用");
            }

            var addSaltPwd = CryptoHelper.AddSalt(model.HospitalGuid, requestDto.Password);

            if (!model.Password.Equals(addSaltPwd, StringComparison.OrdinalIgnoreCase))
            {
                return Failed(ErrorCode.Empty, "账号或密码错误");
            }

            model.LastUpdatedBy = model.HospitalGuid;
            model.LastUpdatedDate = DateTime.Now;
            model.Password = CryptoHelper.AddSalt(model.HospitalGuid, requestDto.NewPassword);

            var result = await hospitalBiz.UpdateAsync(model);

            return result ? Success() : Failed(ErrorCode.Empty, "密码更新失败！");
        }
        #endregion

        /// <summary>
        /// 获取医院当天医生在线统计
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetHospitaCurrentlOnlineDoctorResponseDto>))]
        public async Task<IActionResult> GetHospitalCurrentDoctorOnlineStatistic()
        {
            var hospitalBiz = new HospitalManagerBiz();

            var numbers = await hospitalBiz.GetHospitalDoctorOnlineNumber(UserID);

            var response = new GetHospitaCurrentlOnlineDoctorResponseDto
            {
                Online = numbers[0],
                Consult = numbers[2]
            };

            if (numbers[1] > 0)
            {
                response.OnlineRatio = Math.Round((decimal)response.Online / numbers[1], 4) * 100;
            }

            return Success(response);
        }

        /// <summary>
        /// 获取医院上线医生历史统计
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetHospitaHistoricalOnlineDoctorResponseDto>))]
        public async Task<IActionResult> GetHospitaHistoricalDoctorOnlineStatistic([FromQuery]
        GetHospitaHistoricalDoctorRequestDto requestDto)
        {
            if (requestDto.Type < 0 || requestDto.Type > 2)
            {
                return Failed(ErrorCode.Empty, "请求参数不正确");
            }

            if (requestDto.Type == 2)
            {
                if (!requestDto.BeginDate.HasValue || !requestDto.EndDate.HasValue)
                {
                    return Failed(ErrorCode.Empty, "开始日期或结束日期为空");
                }
            }

            requestDto.HospitalGuid = UserID;

            var hospitalBiz = new HospitalManagerBiz();

            var response = await hospitalBiz.GetHospitaHistoricalOnlineDoctorNumber(requestDto);

            return Success(response);
        }

        /// <summary>
        /// 获取医院医生在线时长排行榜
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetHospitalDoctorOnlineRankResponseDto>>))]
        public async Task<IActionResult> GetHospitalDoctorOnlineRank([FromQuery]
        GetHospitaHistoricalDoctorRequestDto requestDto)
        {
            if (requestDto.Type < 0 || requestDto.Type > 2)
            {
                return Failed(ErrorCode.Empty, "请求参数不正确");
            }

            if (requestDto.Type == 2)
            {
                if (!requestDto.BeginDate.HasValue || !requestDto.EndDate.HasValue)
                {
                    return Failed(ErrorCode.Empty, "开始日期或结束日期为空");
                }
            }

            requestDto.HospitalGuid = UserID;

            var hospitalBiz = new HospitalManagerBiz();

            var response = await hospitalBiz.GetHospitalDoctorOnlineRank(requestDto);

            return Success(response);
        }

        /// <summary>
        /// 获取医院医生解答问题排行榜
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetHospitalDoctorAnswerQuestionRankResponseDto>>))]
        public async Task<IActionResult> GetHospitalDoctorAnswerQuestionRank([FromQuery]
        GetHospitaHistoricalDoctorRequestDto requestDto)
        {
            if (requestDto.Type < 0 || requestDto.Type > 2)
            {
                return Failed(ErrorCode.Empty, "请求参数不正确");
            }

            if (requestDto.Type == 2)
            {
                if (!requestDto.BeginDate.HasValue || !requestDto.EndDate.HasValue)
                {
                    return Failed(ErrorCode.Empty, "开始日期或结束日期为空");
                }
            }

            requestDto.HospitalGuid = UserID;

            var hospitalBiz = new HospitalManagerBiz();

            var response = await hospitalBiz.GetHospitalDoctorAnswerQuestionRank(requestDto);

            return Success(response);
        }

        /// <summary>
        /// 获取医院医生用户咨询排行榜
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetHospitalDoctorConsultRankResponseDto>>))]
        public async Task<IActionResult> GetHospitalDoctorConsultRank([FromQuery]
        GetHospitaHistoricalDoctorRequestDto requestDto)
        {
            if (requestDto.Type < 0 || requestDto.Type > 2)
            {
                return Failed(ErrorCode.Empty, "请求参数不正确");
            }

            if (requestDto.Type == 2)
            {
                if (!requestDto.BeginDate.HasValue || !requestDto.EndDate.HasValue)
                {
                    return Success(ErrorCode.Empty, "开始日期或结束日期为空");
                }
            }

            requestDto.HospitalGuid = UserID;

            var hospitalBiz = new HospitalManagerBiz();

            var response = await hospitalBiz.GetHospitalDoctorConsultRank(requestDto);

            return Success(response);
        }

        /// <summary>
        /// 获取医生列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetHospitalDoctorAnswerAndConsultPageListResponseDto>))]
        public async Task<IActionResult> GetDoctorAnswerAndConsultListAsync(GetHospitalDoctorAnswerAndConsultPageListRequestDto requestDto)
        {
            requestDto.HospitalGuid = UserID;

            var hospitalBiz = new HospitalManagerBiz();

            var response = await hospitalBiz.GetDoctorAnswerAndConsultListAsync(requestDto);

            return Success(response);
        }

        /// <summary>
        /// 获取当前登录是否为医院
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> GetWhetherIsHospitalAsync()
        {
            var hospitalModel = await new HospitalBiz().GetAsync(UserID);
            return Success((hospitalModel?.IsHospital) ?? false);
        }

        #region 工作台
        /// <summary>
        /// 获取诊所今日挂号列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetAppointmentPageListTodayResponseDto>))]
        public async Task<IActionResult> GetAppointmentPageListTodayAsync([FromQuery]GetAppointmentPageListTodayRequestDto requestDto)
        {
            if (string.IsNullOrWhiteSpace(requestDto.DoctorGuid))
            {
                requestDto.DoctorGuid = UserID;
            }
            var result = await new DoctorAppointmentBiz().GetAppointmentPageListTodayAsync(requestDto);
            return Success(result);
        }

        /// <summary>
        /// 接诊
        /// </summary>
        /// <param name="appointmentGuid">挂号预约guid</param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ReceptionPatientAsync(string appointmentGuid)
        {
            var biz = new DoctorAppointmentBiz();
            var model = await biz.GetAsync(appointmentGuid);
            if (model == null)
            {
                return Failed(ErrorCode.UserData, "当前预约记录不存在");
            }
            if (model.Status != AppointmentStatusEnum.Waiting.ToString())
            {
                return Failed(ErrorCode.UserData, "当前预约记录不是待就诊状态，请核对");
            }
            model.Status = AppointmentStatusEnum.Treated.ToString();
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            var result = await biz.UpdateAsync(model);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "接诊操作失败");
        }

        /// <summary>
        /// 过号
        /// </summary>
        /// <param name="appointmentGuid">挂号预约guid</param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto))]
        public async Task<IActionResult> SetAppointmentMissedAsync(string appointmentGuid)
        {
            var biz = new DoctorAppointmentBiz();
            var model = await biz.GetAsync(appointmentGuid);
            if (model == null)
            {
                return Failed(ErrorCode.UserData, "当前预约记录不存在");
            }
            if (model.Status != AppointmentStatusEnum.Waiting.ToString())
            {
                return Failed(ErrorCode.UserData, "当前预约记录不是待就诊状态，请核对");
            }
            model.Status = AppointmentStatusEnum.Miss.ToString();
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            var result = await biz.UpdateAsync(model);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "过号操作失败");
        }

        /// <summary>
        /// 过号转就诊
        /// </summary>
        /// <param name="appointmentGuid">挂号预约guid</param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ChangeMissedAppointmentToWaiting(string appointmentGuid)
        {
            var biz = new DoctorAppointmentBiz();
            var model = await biz.GetAsync(appointmentGuid);
            if (model == null)
            {
                return Failed(ErrorCode.UserData, "当前预约记录不存在");
            }
            if (model.Status != AppointmentStatusEnum.Miss.ToString())
            {
                return Failed(ErrorCode.UserData, "当前预约记录不是过号状态，请核对");
            }
            model.Status = AppointmentStatusEnum.Treated.ToString();
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            var result = await biz.UpdateAsync(model);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "过号操作失败");
        }
        #endregion

        #region 挂号记录
        /// <summary>
        /// 今日挂号人次统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetAppointmentPersonTimeTodayStatisticsResponseDto>))]
        public async Task<IActionResult> GetAppointmentPersonTimeTodayStatisticsAsync()
        {
            var biz = new DoctorAppointmentBiz();
            var todayAppointmentTotal = await biz.GetAppointmentTotalOneDayAsync(DateTime.Now, UserID);
            var yesterdayAppointmentTotal = await biz.GetAppointmentTotalOneDayAsync(DateTime.Now.AddDays(-1), UserID);
            var sourceToal = await new DoctorScheduleBiz().GetAppointmentSourceTotalOneDayAsync(DateTime.Now, UserID);
            var increase = yesterdayAppointmentTotal > 0 ? ((todayAppointmentTotal - yesterdayAppointmentTotal) / (decimal)yesterdayAppointmentTotal) : 1M;
            if (todayAppointmentTotal == 0 && yesterdayAppointmentTotal == 0)
            {
                increase = 0M;
            }
            var result = new GetAppointmentPersonTimeTodayStatisticsResponseDto
            {
                AppointmentTotal = todayAppointmentTotal,
                SourceTotal = sourceToal,
                Increase = Math.Round(increase, 2) * 100
            };
            return Success(result);
        }

        /// <summary>
        /// 今日接诊人次统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetReceptionPersonTimeTodayStatisticsResponseDto>))]
        public async Task<IActionResult> GetReceptionPersonTimeTodayStatisticsAsync()
        {
            var biz = new DoctorAppointmentBiz();
            var todayAppointmentTotal = await biz.GetAppointmentTotalOneDayAsync(DateTime.Now, UserID, AppointmentStatusEnum.Treated);
            var yesterdayAppointmentTotal = await biz.GetAppointmentTotalOneDayAsync(DateTime.Now.AddDays(-1), UserID, AppointmentStatusEnum.Treated);
            var increase = yesterdayAppointmentTotal > 0 ? ((todayAppointmentTotal - yesterdayAppointmentTotal) / (decimal)yesterdayAppointmentTotal) : 1M;
            if (todayAppointmentTotal == 0 && yesterdayAppointmentTotal == 0)
            {
                increase = 0M;
            }
            var result = new GetReceptionPersonTimeTodayStatisticsResponseDto
            {
                ReceptionTotal = todayAppointmentTotal,
                Increase = Math.Round(increase, 2) * 100
            };
            return Success(result);
        }

        /// <summary>
        /// 诊所挂号趋势统计
        /// 查询区间不可超过一年(366)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetAppointmentPersonTimeStatisticsResponseDto>))]
        public async Task<IActionResult> GetAppointmentPersonTimeStatisticsAsync([FromQuery]GetAppointmentPersonTimeStatisticsRequestDto requestDto)
        {
            requestDto.HospitalGuid = UserID;
            var result = await new DoctorAppointmentBiz().GetAppointmentPersonTimeStatisticsAsync(requestDto);
            return Success(result);
        }

        /// <summary>
        /// 获取诊所挂号列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetAppointmentPageListResponseDto>))]
        public async Task<IActionResult> GetAppointmentPageListAsync([FromQuery]GetAppointmentPageListRequestDto requestDto)
        {
            requestDto.HospitalGuid = UserID;
            var result = await new DoctorAppointmentBiz().GetAppointmentPageListAsync(requestDto);
            return Success(result);
        }

        /// <summary>
        /// 新增挂号预约
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto))]
        public async Task<IActionResult> CreateAppointmentAsync([FromBody]CreateAppointmentRequestDto requestDto)
        {
            var doctorModel = await new DoctorBiz().GetAsync(requestDto.DoctorGuid);
            if (doctorModel == null)
            {
                return Failed(ErrorCode.UserData, "未查询到此医生");
            }
            if (!doctorModel.Enable || string.Equals(doctorModel.Status, StatusEnum.Draft.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return Failed(ErrorCode.UserData, "未查询到此医生");
            }
            var scheduleModel = await new DoctorScheduleBiz().GetAsync(requestDto.ScheduleGuid);
            if (scheduleModel == null || !scheduleModel.Enable)
            {
                return Failed(ErrorCode.UserData, "未查询到此排班信息");
            }
            var userModel = new UserBiz().GetUserByPhone(requestDto.Phone);
            if (userModel == null)
            {
                return Failed(ErrorCode.UserData, "通过手机号未查询到用户，请注册");
            }
            var doctorWorkshiftDetailModel = await new DoctorWorkshiftDetailBiz().GetAsync(scheduleModel.WorkshiftDetailGuid);
            var appointmentTime = Convert.ToDateTime($"{scheduleModel.ScheduleDate.ToString("yyyy-MM-dd")} {doctorWorkshiftDetailModel.StartTime.ToString()}");
            var appointmentEndTime = Convert.ToDateTime($"{scheduleModel.ScheduleDate.ToString("yyyy-MM-dd")} {doctorWorkshiftDetailModel.EndTime.ToString()}");
            if (DateTime.Now >= appointmentEndTime)
            {
                return Failed(ErrorCode.UserData, "当前时间不可大于就诊截止时间，请重新挂号");
            }
            var model = new DoctorAppointmentModel
            {
                AppointmentGuid = Guid.NewGuid().ToString("N"),
                HospitalGuid = doctorModel.HospitalGuid,
                UserGuid = userModel.UserGuid,
                AppointmentNo = "",
                ScheduleGuid = requestDto.ScheduleGuid,
                DoctorGuid = requestDto.DoctorGuid,
                OfficeGuid = doctorModel.OfficeGuid,
                AppointmentTime = appointmentTime,
                AppointmentDeadline = appointmentEndTime,
                PatientGuid = null,
                PatientName = requestDto.PatientName,
                PatientPhone = requestDto.Phone,
                PatientGender = requestDto.PatientGender.ToString(),
                PatientBirthday = requestDto.PatientBirthday,
                PatientCardno = requestDto.PatientCardNo,
                Status = AppointmentStatusEnum.Waiting.ToString(),
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty
            };
            var result = await new DoctorAppointmentBiz().CreateAppointmentAsync(model, doctorWorkshiftDetailModel.AppointmentNoPrefix, true);
            return result ? Success() : Failed(ErrorCode.UserData, "挂号失败");
        }
        #endregion

        #region 医院端预约相关下拉框数据
        /// <summary>
        /// 预约状态列表 例如：key = 1 , value = 待就诊
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<List<KeyValuePair<int, string>>>))]
        public IActionResult GetAppointmentStatusList()
        {
            var results = (from v in Enum.GetNames(typeof(AppointmentStatusEnum))
                           let item = Enum.Parse<AppointmentStatusEnum>(v)
                           select new KeyValuePair<int, string>((int)item, item.GetDescription())).ToList();
            return Success(results);
        }

        /// <summary>
        /// 获取科室下存在医生的科室列表 例如：key = "科室Id" , value = "内科"
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<List<KeyValuePair<string, string>>>))]
        public async Task<IActionResult> GetOfficeSelectListWithDoctorOfHospitalAsync(string hospitalGuid)
        {
            if (string.IsNullOrWhiteSpace(hospitalGuid))
            {
                hospitalGuid = UserID;
            }
            var officeModels = await new OfficeBiz().GetOfficeListWithDoctorOfHospitalAsync(hospitalGuid);
            var result = officeModels.Select(a => new KeyValuePair<string, string>(a.OfficeGuid, a.OfficeName));
            return Success(result);
        }

        /// <summary>
        /// 获取科室下的医生列表 例如 key= "医生Id" , value = "张医生"
        /// </summary>
        /// <param name="officeGuid">科室Id</param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<List<KeyValueDto<string, string>>>))]
        public async Task<IActionResult> GetDoctorSelectListAsync(string officeGuid)
        {
            var doctors = await new DoctorBiz().GetDoctorSelectListOfOfficeAsync(officeGuid);
            return Success(doctors);
        }

        /// <summary>
        /// 获取时日期区间内医生排班信息和预约挂号余号信息：排班日期和排班guid
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<List<GetDoctorScheduleByDateIntervalResponseDto>>))]
        public async Task<IActionResult> GetDoctorScheduleByDateIntervalAsync([FromQuery]GetDoctorScheduleByDateIntervalRequestDto requestDto)
        {
            var dateList = new List<DateTime>();
            var sDate = requestDto.StartDate.Date;
            while (sDate <= requestDto.EndDate.Date)
            {
                dateList.Add(sDate.Date);
                sDate = sDate.AddDays(1);
            }
            var result = await new DoctorScheduleBiz().GetDoctorScheduleByDateIntervalAsync(requestDto.DoctorGuid, requestDto.StartDate, requestDto.EndDate);
            var workshifts = await new DoctorWorkshiftDetailBiz().GetModelsByPrimaryKeyIdsAsync(result.Select(a => a.WorkshiftDetailGuid).Distinct());
            var schedules = result.GroupBy(a => a.ScheduleDate).Select(a => new GetDoctorScheduleByDateIntervalResponseDto
            {
                ScheudleDate = a.Key,
                ScheduleDetails = a.Select(d => new GetDoctorScheduleByDateIntervalResponseDto.ScheduleDetail
                {
                    ScheduleGuid = d.ScheduleGuid,
                    AppointmentLimit = (d.AppointmentLimit - d.AppointmentQuantity) >= 0 ? (d.AppointmentLimit - d.AppointmentQuantity) : 0,
                    StartTime = workshifts.FirstOrDefault(p => p.WorkshiftDetailGuid == d.WorkshiftDetailGuid).StartTime.ToString().Substring(0, 5),
                    EndTime = workshifts.FirstOrDefault(p => p.WorkshiftDetailGuid == d.WorkshiftDetailGuid).EndTime.ToString().Substring(0, 5),
                }).OrderBy(d => d.StartTime).ToList()
            });

            var response = dateList.GroupJoin(schedules, l => l, r => r.ScheudleDate, (l, gs) => new GetDoctorScheduleByDateIntervalResponseDto
            {
                ScheudleDate = l,
                ScheduleDetails = ((gs.FirstOrDefault()?.ScheduleDetails) ?? new List<GetDoctorScheduleByDateIntervalResponseDto.ScheduleDetail>()).OrderBy(a => a.StartTime).ToList()
            }).OrderBy(a => a.ScheudleDate).ToList();
            return Success(response);
        }

        #endregion
    }
}
