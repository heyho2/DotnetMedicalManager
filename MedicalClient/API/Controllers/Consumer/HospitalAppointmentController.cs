using GD.Common;
using GD.Consumer;
using GD.Doctor;
using GD.Dtos.Consumer.Consumer;
using GD.Dtos.Doctor.Doctor;
using GD.Dtos.Doctor.Hospital;
using GD.Dtos.Enum.DoctorAppointment;
using GD.Models.Consumer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.API.Controllers.Consumer
{
    /// <summary>
    /// 医院预约相关控制器
    /// </summary>
    public class HospitalAppointmentController : ConsumerBaseController
    {
        /// <summary>
        /// 添加就诊人
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> AddPatientAsync([FromBody]GetAddPatientRequestDto requestDto)
        {
            PatientMemberModel patientMemberModel = new PatientMemberModel()
            {
                PatientGuid = Guid.NewGuid().ToString("N"),
                UserGuid = UserID,
                Name = requestDto.Name,
                Phone = requestDto.Phone,
                Gender = requestDto.Gender.ToString(),
                CardNo = requestDto.CardNo,
                CreatedBy = UserID,
                IsDefault = requestDto.IsDefault,
                Relationship = requestDto.Relationship.HasValue ? requestDto.Relationship.Value.ToString() : null,
                CreationDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now,
                LastUpdatedBy = UserID
            };
            if (requestDto.Birthday.HasValue)
            {
                patientMemberModel.Birthday = requestDto.Birthday.Value;
            }
            //判断是否存在本人
            if (requestDto.Relationship.HasValue)
            {
                if (requestDto.Relationship.ToString() == InquiryRelationshipEnum.Own.ToString())
                {
                    var biz = new PatientMemberBiz();
                    var resultOwn = await biz.GetOwnPatientMemberModelAsync(UserID);
                    if (resultOwn != null)
                    {
                        return Failed(ErrorCode.UserData, "已存在'自己'关系的就诊人档案");
                    }
                }
            }
            var result = await new PatientMemberBiz().AddPatientMemberAsync(UserID, patientMemberModel);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "新增就诊人失败");
        }
        /// <summary>
        /// 查询就诊人列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<List<GetPatientResponDto>>))]
        public async Task<IActionResult> QueryPatientMemberAsync()
        {
            var biz = new PatientMemberBiz();
            var result = await biz.GetPatientMemberModelAsync(UserID);
            if (result != null)
            {
                foreach (var item in result)
                {
                    //if (!string.IsNullOrWhiteSpace(item.CardNo) && item.CardNo.Length > 16)
                    //{
                    //    item.CardNo = item.CardNo.Substring(0, 3) + "***********" + item.CardNo.Substring(item.CardNo.Length - 4);
                    //}
                    //else
                    //{
                    //    item.CardNo = string.Empty;
                    //}
                    if (item.Birthday.HasValue)
                    {
                        item.Age = DateTime.Now.Year - item.Birthday.Value.Year;
                    }
                }
            }
            return Success(result);
        }
        /// <summary>
        /// 删除就诊人
        /// </summary>
        /// <param name="patientGuid"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DeletePatientMemberAsync(string patientGuid)
        {
            var biz = new PatientMemberBiz();
            var model = await biz.GetAsync(patientGuid);
            if (model == null || model.UserGuid != UserID)
            {
                return Failed(ErrorCode.UserData, "未找到就诊人");
            }
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            model.Enable = false;
            var result = await biz.UpdateAsync(model);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "删除就诊人失败");

        }
        /// <summary>
        /// 修改就诊人
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto))]
        public async Task<IActionResult> UpdatePatientMemberAsync([FromBody]UpdatePatientRequestDto requestDto)
        {
            var biz = new PatientMemberBiz();
            var model = await biz.GetAsync(requestDto.PatientGuid);
            if (model == null || model.UserGuid != UserID)
            {
                return Failed(ErrorCode.UserData, "未找到就诊人");
            }
            model.CardNo = requestDto.CardNo;
            model.Name = requestDto.Name;
            model.Phone = requestDto.Phone;
            model.Gender = requestDto.Gender.ToString();
            model.Birthday = requestDto.Birthday.HasValue ? (DateTime?)requestDto.Birthday.Value : null;
            model.CreatedBy = UserID;
            model.IsDefault = requestDto.IsDefault;
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            model.Relationship = requestDto.Relationship.HasValue ? requestDto.Relationship.Value.ToString() : null;
            var result = await new PatientMemberBiz().UpdatePatientMemberAsync(UserID, model);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "修改就诊人失败");
        }
        /// <summary>
        /// 查询挂号列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetAppointmentMemberPageListResponseDto>))]
        public async Task<IActionResult> GetAppointmentMemberAsync([FromQuery]GetAppointmentMemberRequestDto requestDto)
        {
            var response = await new DoctorAppointmentBiz().GetAppointmentMemberPageListAsync(requestDto, UserID);
            return Success(response);
        }
        /// <summary>
        /// 删除挂号记录
        /// </summary>
        /// <param name="appointGuid"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DeleteAppointmentAsync(string appointGuid)
        {
            if (string.IsNullOrWhiteSpace(appointGuid))
            {
                return Failed(ErrorCode.DataBaseError, "参数不能为空");
            }
            var biz = new DoctorAppointmentBiz();
            var model = await biz.GetAsync(appointGuid);
            if (model == null || model.UserGuid != UserID)
            {
                return Failed(ErrorCode.UserData, "未找到记录");
            }
            if (model.Status == AppointmentStatusEnum.Waiting.ToString())
            {
                return Failed(ErrorCode.UserData, "待就诊状态无法删除");
            }
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            model.Enable = false;
            var result = await biz.UpdateAsync(model);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "删除挂号记录失败");
        }
        /// <summary>
        /// 取消预约
        /// </summary>
        /// <param name="appointGuid"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto))]
        public async Task<IActionResult> CancelAppointmentAsync(string appointGuid)
        {
            if (string.IsNullOrWhiteSpace(appointGuid))
            {
                return Failed(ErrorCode.DataBaseError, "参数不能为空");
            }
            var biz = new DoctorAppointmentBiz();
            var model = await biz.GetAsync(appointGuid);
            if (model == null || model.UserGuid != UserID)
            {
                return Failed(ErrorCode.UserData, "未找到记录");
            }
            if (model.Status != AppointmentStatusEnum.Waiting.ToString())
            {
                return Failed(ErrorCode.UserData, "只有待诊状态才可以取消");
            }
            //检查累计是否超过二次连续取消或者爽约
            var checkList = await new DoctorAppointmentBiz().GetDoctorAppointmentListAsync(UserID);
            ConsumerModel consumerModel = null;
            if (checkList != null)
            {
                int missCount = 0;
                foreach (var item in checkList)
                {
                    if (item.Status == AppointmentStatusEnum.Cancel.ToString() || item.Status == AppointmentStatusEnum.Miss.ToString() || (item.Status == AppointmentStatusEnum.Waiting.ToString() && DateTime.Now.Date > item.AppointmentTime.Date))
                    {
                        missCount += 1;
                    }
                    else
                    {
                        missCount = 0;
                    }
                    if (missCount >= 2)
                    {
                        consumerModel = await new ConsumerBiz().GetModelAsync(UserID);
                        if (consumerModel != null)
                        {
                            consumerModel.LastUpdatedBy = UserID;
                            consumerModel.LastUpdatedDate = DateTime.Now;
                            //三个月之后才可以预约
                            consumerModel.NoAppointmentDate = DateTime.Now.Date.AddMonths(3);
                        }
                        break;
                    }
                }
            }
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            model.Status = AppointmentStatusEnum.Cancel.ToString();
            var result = await biz.CancelAppointmentAsync(model, consumerModel);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "取消挂号失败");
        }
        /// <summary>
        /// 获取诊所和医院下所有医生
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetAppointmentDoctorPageListResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetAppointmentDoctorAsync([FromQuery]GetAppointmentDoctorRequestDto requestDto)
        {
            DoctorAppointmentBiz doctorAppointmentBiz = new DoctorAppointmentBiz();
            var response = await doctorAppointmentBiz.GetAppointmentDoctorPageListAsync(UserID, requestDto);
            return Success(response);
        }
        /// <summary>
        /// 获取医院下的科室
        /// </summary>
        /// <param name="hospitalGuid"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<List<HospitalDepartmentsResponse>>)), AllowAnonymous]
        public async Task<IActionResult> GetHospitalDepartmentsAsync(string hospitalGuid)
        {
            DoctorAppointmentBiz doctorAppointmentBiz = new DoctorAppointmentBiz();
            if (string.IsNullOrWhiteSpace(hospitalGuid))
            {
                return Failed(ErrorCode.UserData, "医院Id不能为空");
            }
            var response = await doctorAppointmentBiz.GetHospitalDepartmentsAsync(hospitalGuid);
            if (response != null)
            {
                //查找最近用户去过的科室
                string been = await doctorAppointmentBiz.GetBeenDepartmentsAsync(UserID);
                if (!string.IsNullOrWhiteSpace(been))
                {
                    foreach (var item in response)
                    {
                        if (item.OfficeGuid == been)
                        {
                            item.IsBeen = true;
                        }
                    }
                }
            }
            return Success(response);
        }
        /// <summary>
        /// 获取用户最近一次预约
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<LastAppointmentResponse>))]
        public async Task<IActionResult> GetLastAppointmentAsync()
        {
            DoctorAppointmentBiz doctorAppointmentBiz = new DoctorAppointmentBiz();
            var response = await doctorAppointmentBiz.GetLastAppointmentAsync(UserID);
            return Success(response);
        }
        /// <summary>
        /// 分页获取医院列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<HospitalPageListResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> QueryHospitalPageListAsync([FromQuery]HospitalPageListRequestDto requestDto)
        {
            var response = await new DoctorAppointmentBiz().GetHospitalPageListAsync(requestDto);
            return Success(response);
        }
        /// <summary>
        /// 获取医生最近7天可挂号数据
        /// </summary>
        /// <param name="doctorGuid"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<List<GetAppointmentDoctorInfoListResponse>>)), AllowAnonymous]
        public async Task<IActionResult> GetAppointmentDoctorInfoAsync(string doctorGuid)
        {
            DoctorAppointmentBiz doctorAppointmentBiz = new DoctorAppointmentBiz();
            if (string.IsNullOrWhiteSpace(doctorGuid))
            {
                return Failed(ErrorCode.UserData, "医生Id不能为空");
            }
            List<DateTime> dateList = new List<DateTime>();
            for (int i = 0; i < 7; i++)
            {
                dateList.Add(DateTime.Now.Date.AddDays(i));
            }
            var response = await doctorAppointmentBiz.GetAppointmentDoctorInfoAsync(doctorGuid);
            List<GetAppointmentDoctorInfoListResponse> result = new List<GetAppointmentDoctorInfoListResponse>();
            if (response != null)
            {
                result = response.GroupBy(s => s.ScheduleDate).Select(s => new GetAppointmentDoctorInfoListResponse
                {
                    ScheduleDate = s.Key,
                    List = s.ToList().Select(sl => new GetAppointmentDoctorInfo
                    {
                        WorkshiftType = sl.WorkshiftType,
                        AppointmentCount = sl.AppointmentCount
                    }).ToList()
                }).ToList();
                if (result != null)
                {
                    foreach (var item in result)
                    {
                        if (item.List != null)
                        {
                            if (item.List.Where(s => s.WorkshiftType == "AM").FirstOrDefault() == null)
                            {
                                item.List.Add(new GetAppointmentDoctorInfo()
                                {
                                    WorkshiftType = "AM"
                                });
                            }
                            else
                            {
                                if (item.List.Where(s => s.WorkshiftType == "PM").FirstOrDefault() == null)
                                {
                                    item.List.Add(new GetAppointmentDoctorInfo()
                                    {
                                        WorkshiftType = "PM"
                                    });
                                }
                            }
                            item.List = item.List.OrderBy(s => s.WorkshiftType).ToList();
                        }
                    }

                }
            }
            var emptySchedule = new List<GetAppointmentDoctorInfo> {
                new GetAppointmentDoctorInfo{ WorkshiftType = "AM"},
                new GetAppointmentDoctorInfo{ WorkshiftType = "PM"}
            };
            result = dateList.GroupJoin(result, l => l, r => r.ScheduleDate, (l, gs) => new GetAppointmentDoctorInfoListResponse
            {
                ScheduleDate = l,
                List = (gs.FirstOrDefault()) == null ? emptySchedule : gs.FirstOrDefault().List
            }).OrderBy(a => a.ScheduleDate).ToList();
            return Success(result);
        }
        /// <summary>
        ///  获取医生上,下午排班详情数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<List<GetAppointmentDoctorScheduleDetailResponse>>)), AllowAnonymous]
        public async Task<IActionResult> GetAppointmentDoctorDetailAsync([FromQuery]GetAppointmentDoctorScheduleDetailRequest requestDto)
        {
            if (requestDto.ScheduleDate.Date < DateTime.Now.Date || requestDto.ScheduleDate.Date > DateTime.Now.Date.AddDays(6))
            {
                return Failed(ErrorCode.UserData, "排班时间必须在7天内时间查询");
            }
            DoctorAppointmentBiz doctorAppointmentBiz = new DoctorAppointmentBiz();
            var response = await doctorAppointmentBiz.GetAppointmentDoctorDetailAsync(requestDto);
            if (response != null)
            {
                foreach (var item in response)
                {
                    var appointmentEndTime = Convert.ToDateTime($"{item.ScheduleDate.ToString("yyyy-MM-dd")} {item.EndTime.ToString()}");
                    if (DateTime.Now >= appointmentEndTime)
                    {
                        item.IsOvertime = true;
                    }
                }
            }
            return Success(response);
        }
        /// <summary>
        /// 推荐咨询医生
        /// </summary>
        /// <param name="doctorGuid"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<List<DoctorRecommendResponseDto>>))]
        public async Task<IActionResult> GetDoctorRecommendAsync(string doctorGuid)
        {
            if (string.IsNullOrWhiteSpace(doctorGuid))
            {
                return Failed(ErrorCode.UserData, "医生Id不能为空");
            }
            var doctorBiz = new DoctorBiz();
            var model = doctorBiz.GetDoctor(doctorGuid);
            if (model == null)
            {
                return Failed(ErrorCode.UserData, "医生不存在");
            }
            DoctorAppointmentBiz doctorAppointmentBiz = new DoctorAppointmentBiz();
            var response = await doctorAppointmentBiz.GetDoctorRecommendAsync(UserID, model.OfficeGuid, model.HospitalGuid);
            return Success(response);
        }
        /// <summary>
        /// 新增挂号预约
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto<AppointmentResponseDto>))]
        public async Task<IActionResult> AddAppointmentAsync([FromBody]AddAppointmentRequestDto requestDto)
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
            if (scheduleModel.ScheduleDate.Date < DateTime.Now.Date || scheduleModel.ScheduleDate.Date > DateTime.Now.AddDays(6))
            {
                return Failed(ErrorCode.UserData, "此时间段排班目前无法预约");
            }
            var patientMemberModel = await new PatientMemberBiz().GetAsync(requestDto.PatientGuid);
            if (patientMemberModel == null || !patientMemberModel.Enable || patientMemberModel.UserGuid != UserID)
            {
                return Failed(ErrorCode.UserData, "未查询到就诊人信息");
            }
            //判断当前就诊人是否已经预约过
            var checkPatient = await new DoctorAppointmentBiz().GetDoctorAppointmentAsync(requestDto.DoctorGuid, scheduleModel.ScheduleDate, patientMemberModel.PatientGuid);
            if (checkPatient != null)
            {
                return Failed(ErrorCode.UserData, "就诊人当天已经预约过该医生");
            }
            //查询当前用户是否允许当天挂号
            ConsumerModel consumerModel = await new ConsumerBiz().GetModelAsync(UserID);
            if (consumerModel != null)
            {
                if (consumerModel.NoAppointmentDate.HasValue && consumerModel.NoAppointmentDate > DateTime.Now.Date)
                {
                    return Failed(ErrorCode.UserData, "您在一个月内有连续爽约或取消超过3次的记录" + consumerModel.NoAppointmentDate.Value.ToString("yyyy-MM-dd") + "才可再次预约");
                }
            }
            //检查累计是否超过二次连续取消或者爽约
            var checkList = await new DoctorAppointmentBiz().GetThreeMonthsDoctorAppointmentListAsync(UserID);
            if (checkList != null)
            {
                List<DateTime> resultList = new List<DateTime>();
                var group = checkList.GroupBy(s => s.CreationDate.ToString("yyyy-MM"));
                foreach (var items in group)
                {
                    foreach (var item in items)
                    {
                        if (item.Status == AppointmentStatusEnum.Cancel.ToString() || item.Status == AppointmentStatusEnum.Miss.ToString() || (item.Status == AppointmentStatusEnum.Waiting.ToString() && DateTime.Now.Date > item.AppointmentTime.Date))
                        {
                            resultList.Add(item.AppointmentTime.Date);
                        }
                        else
                        {
                            resultList.Clear();
                        }
                        if (resultList.Count >= 3 && DateTime.Now.Date < resultList.LastOrDefault().Date.AddMonths(3))
                        {

                            return Failed(ErrorCode.UserData, "您在一个月内有连续爽约或取消超过3次的记录" + resultList.LastOrDefault().Date.AddMonths(3).ToString("yyyy-MM-dd") + "才可再次预约");
                        }
                    }
                }
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
                UserGuid = UserID,
                AppointmentNo = "",
                ScheduleGuid = requestDto.ScheduleGuid,
                DoctorGuid = requestDto.DoctorGuid,
                OfficeGuid = doctorModel.OfficeGuid,
                AppointmentTime = appointmentTime,
                AppointmentDeadline = appointmentEndTime,
                PatientGuid = patientMemberModel.PatientGuid,
                PatientName = patientMemberModel.Name,
                PatientPhone = patientMemberModel.Phone,
                PatientGender = patientMemberModel.Gender,
                PatientBirthday = patientMemberModel.Birthday,
                PatientCardno = patientMemberModel.CardNo,
                PatientRelationship = patientMemberModel.Relationship,
                Status = AppointmentStatusEnum.Waiting.ToString(),
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty
            };
            if (!string.IsNullOrWhiteSpace(requestDto.Phone))
            {
                model.PatientPhone = requestDto.Phone;
            }
            var result = await new DoctorAppointmentBiz().CreateAppointmentAsync(model, doctorWorkshiftDetailModel.AppointmentNoPrefix, false);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "挂号失败");
            }
            var doctorAppointmentModel = await new DoctorAppointmentBiz().GetAppointmentAsync(model.AppointmentGuid);
            if (doctorAppointmentModel == null)
            {
                return Failed(ErrorCode.UserData, "挂号失败");
            }
            return Success(doctorAppointmentModel);
        }
        /// <summary>
        /// 获取用户是否存在待就诊数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto))]
        public async Task<IActionResult> GetWaitingAsync()
        {
            var result = await new DoctorAppointmentBiz().GetWaitingCountAsync(UserID);
            return Success(result > 0);
        }
    }
}
