using GD.API.Code;
using GD.Common;
using GD.Doctor;
using GD.Dtos.Common;
using GD.Dtos.Doctor.Hospital;
using GD.Dtos.Enum.HospitalScheduleEnum;
using GD.Models.Doctor;
using GD.Models.Merchant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.API.Controllers.Doctor
{
    /// <summary>
    /// 医院班次相关控制器
    /// </summary>
    public class HospitalScheduleController : DoctorBaseController
    {
        /// <summary>
        /// 获取医院班次模板列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<List<GetWorkShiftTemplateListResponseDto>>))]
        public async Task<IActionResult> GetWorkShiftTemplateListAsync()
        {
            var result = await new DoctorWorkshifTemplateBiz().GetModelsByHospitalGuidAsync(UserID);
            return Success(result.OrderByDescending(a => a.CreationDate).ToList());
        }

        /// <summary>
        /// 获取医院模板下的班次详情
        /// </summary>
        /// <param name="templateGuid"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<List<GetWorkShiftDetailsResponseDto>>))]
        public async Task<IActionResult> GetWorkShiftDetailsAsync(string templateGuid)
        {
            var result = await new DoctorWorkshiftDetailBiz().GetModelsByTemplateGuidAsync(templateGuid);
            return Success(result.OrderBy(a => a.WorkshiftType).ThenBy(a => a.StartTime).Select(a => new GetWorkShiftDetailsResponseDto
            {
                WorkshiftDetailGuid = a.WorkshiftDetailGuid,
                WorkshiftType = Enum.Parse<WorkshiftTypeEnum>(a.WorkshiftType),
                StartTime = a.StartTime.ToString().Substring(0, 5),
                EndTime = a.EndTime.ToString().Substring(0, 5),
                AppointmentLimit = a.AppointmentLimit
            }));
        }

        /// <summary>
        /// 新增或编辑班次模板
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto))]
        public async Task<IActionResult> CreateEditWorkshiftTemplateAsync([FromBody]CreateEditWorkshiftTemplateRequestDto requestDto)
        {
            if (requestDto.Details == null || !requestDto.Details.Any())
            {
                return Failed(ErrorCode.UserData, "班次明细不可为空");
            }
            if (requestDto.Details.Select(a => a.StartTime).Distinct().Count() < requestDto.Details.Count || requestDto.Details.Select(a => a.EndTime).Distinct().Count() < requestDto.Details.Count)
            {
                return Failed(ErrorCode.UserData, "存在重复的开始时间或结束时间，请检查");
            }
            var noTimeCross = requestDto.Details.All(a => requestDto.Details.Where(b => a.StartTime < b.EndTime && a.EndTime > b.StartTime).Count() == 1);
            if (!noTimeCross)
            {
                return Failed(ErrorCode.UserData, "时间片段存在时间交叉，请检查");
            }

            var model = new DoctorWorkshifTemplateModel
            {
                TemplateGuid = Guid.NewGuid().ToString("N"),
                TemplateName = requestDto.TemplateName,
                HospitalGuid = UserID,
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                LastUpdatedBy = UserID,
                LastUpdatedDate = DateTime.Now,
                OrgGuid = string.Empty
            };
            var isCreate = true;
            var templates = await new DoctorWorkshifTemplateBiz().GetModelsByHospitalGuidAsync(UserID);
            //编辑
            if (!string.IsNullOrWhiteSpace(requestDto.TemplateGuid))
            {
                var templateModel = await new DoctorWorkshifTemplateBiz().GetAsync(requestDto.TemplateGuid);
                if (templateModel == null || !templateModel.Enable)
                {
                    return Failed(ErrorCode.UserData, "无此班次模板");
                }

                var checkResult = await new DoctorScheduleBiz().WhetherTemplateApplyAsync(requestDto.TemplateGuid, UserID);
                if (checkResult)
                {
                    return Failed(ErrorCode.UserData, "当前模板已被使用过，无法编辑");
                }

                model.TemplateGuid = templateModel.TemplateGuid;
                model.HospitalGuid = templateModel.HospitalGuid;
                model.CreatedBy = UserID;
                model.CreationDate = DateTime.Now;
                model.OrgGuid = templateModel.OrgGuid;
                isCreate = false;
                if (templates.FirstOrDefault(a => a.TemplateGuid != model.TemplateGuid && a.TemplateName == requestDto.TemplateName) != null)
                {
                    return Failed(ErrorCode.UserData, "模板名称已被占用");
                }

            }
            else
            {
                if (templates.FirstOrDefault(a => a.TemplateName == requestDto.TemplateName) != null)
                {
                    return Failed(ErrorCode.UserData, "模板名称已被占用");
                }
            }
            var detailModels = requestDto.Details.OrderBy(a => a.StartTime).Select((a, i) => new DoctorWorkshiftDetailModel
            {
                WorkshiftDetailGuid = Guid.NewGuid().ToString("N"),
                TemplateGuid = model.TemplateGuid,
                WorkshiftType = a.WorkshiftType.ToString(),
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                AppointmentLimit = a.AppointmentLimit,
                AppointmentNoPrefix = NumberToChar(i + 1),
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                LastUpdatedBy = UserID,
                LastUpdatedDate = DateTime.Now,
                OrgGuid = string.Empty
            }).ToList();
            string NumberToChar(int number)
            {
                int num = number + 64;
                ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                byte[] btNumber = new byte[] { (byte)num };
                return asciiEncoding.GetString(btNumber);
            }
            var result = await new DoctorWorkshifTemplateBiz().CreateEditWorkshiftTemplateAsync(model, detailModels, isCreate);
            return Success();
        }

        /// <summary>
        /// 删除班次模板
        /// </summary>
        /// <param name="templateGuid"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteWorkshiftTemplateAsync(string templateGuid)
        {
            var model = await new DoctorWorkshifTemplateBiz().GetAsync(templateGuid);
            if (model == null)
            {
                return Failed(ErrorCode.DataBaseError, "无此模板数据");
            }
            var checkResult = await new DoctorScheduleBiz().WhetherTemplateApplyInCurrentMonthOrLaterAsync(templateGuid, UserID);
            if (checkResult)
            {
                return Failed(ErrorCode.UserData, "当前模板在本月或本月之后的排班中已被应用，无法删除");
            }

            var result = await new DoctorWorkshifTemplateBiz().DeleteAsync(model, UserID, true);
            return result ? Success() : Failed(ErrorCode.UserData, "删除班次模板失败");
        }

        /// <summary>
        /// 获取排班周期列表：默认获取上月和上月之后的排班周期数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetCycleListResponseDto>))]
        public async Task<IActionResult> GetCycleListMonthAsync()
        {
            var result = await new DoctorScheduleCycleBiz().GetCycleListAfterLastMonthAsync(UserID);
            var response = result.Select(a => new GetCycleListResponseDto
            {
                CycleGuid = a.CycleGuid,
                IsCurrentMonth = a.StartDate.Date.AddDays(1 - a.StartDate.Day) == DateTime.Now.Date.AddDays(1 - DateTime.Now.Day),
                CycleDisplay = $"{a.StartDate.ToString("yyyy年MM月")}{(a.StartDate.Date.AddDays(1 - a.StartDate.Day) == DateTime.Now.Date.AddDays(1 - DateTime.Now.Day) ? "(当月)" : "")}"
            });
            return Success(response);
        }

        /// <summary>
        /// 获取周期下的排班详情列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<List<GetDoctorScheduleListOfCycleResponseDto>>))]
        public async Task<IActionResult> GetDoctorScheduleListOfCycleAsync(string cycleGuid)
        {
            var result = await new DoctorScheduleBiz().GetDoctorScheduleListOfCycleAsync(cycleGuid);
            return Success(result);
        }

        /// <summary>
        /// 获取医院某天的排班详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<GetHospitalScheduleListOneDayResponseDto>))]
        public async Task<IActionResult> GetHospitalScheduleListOneDayAsync(DateTime scheduleDate)
        {
            var biz = new DoctorScheduleBiz();
            var templateGuid = (await biz.GetDoctorScheduleTemplateGuidAsync(UserID, scheduleDate))?.TemplateGuid;
            if (string.IsNullOrWhiteSpace(templateGuid))
            {
                return Failed(ErrorCode.UserData, "未找到指定日期的排班数据");
            }
            var result = await biz.GetHospitalScheduleListOneDayAsync(UserID, scheduleDate, templateGuid);
            return Success(result);
        }

        /// <summary>
        /// 编辑医院某天的排班
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto))]
        public async Task<IActionResult> EditHospitalScheduleOneDayAsync([FromBody]EditHospitalScheduleOneDayRequestDto requestDto)
        {
            var biz = new DoctorScheduleBiz();
            if (requestDto.ScheduleDates == null || requestDto.ScheduleDates.Count != 1)
            {
                return Failed(ErrorCode.UserData, "排班日期必填，且只能填一个");
            }
            var scheduleDate = requestDto.ScheduleDates.FirstOrDefault().Date;
            if (scheduleDate < DateTime.Now.Date)
            {
                return Failed(ErrorCode.UserData, "历史数据不可编辑");
            }
            #region 班次模板基础数据检测
            var newTemplateModel = await new DoctorWorkshifTemplateBiz().GetAsync(requestDto.TemplateGuid);
            if (newTemplateModel == null)
            {
                return Failed(ErrorCode.UserData, "传入的班次模板模板数据未找到");
            }
            var workshiftDetailModels = await new DoctorWorkshiftDetailBiz().GetModelsByTemplateGuidAsync(requestDto.TemplateGuid);
            if (!workshiftDetailModels.Any())
            {
                return Failed(ErrorCode.UserData, "班次模板下未找到详细的班次数据");
            }
            var invalidWorkshiftDetailIds = new List<string>();
            var checkWorkshiftDetailIds = requestDto.Details.All(a =>
            {
                var checkRes = workshiftDetailModels.FirstOrDefault(b => b.WorkshiftDetailGuid == a.WorkshiftDetailGuid);
                if (checkRes == null)
                {
                    invalidWorkshiftDetailIds.Add(a.WorkshiftDetailGuid);
                }
                return checkRes != null;
            });
            if (!checkWorkshiftDetailIds)
            {
                return Failed(ErrorCode.UserData, $"班次guid[{string.Join(",", invalidWorkshiftDetailIds.Distinct())}])非法，未在数据库查找到");
            }
            #endregion
            var doctorScheduleModel = await biz.GetDoctorScheduleTemplateGuidAsync(UserID, scheduleDate);
            if (string.IsNullOrWhiteSpace(doctorScheduleModel?.TemplateGuid))
            {
                return Failed(ErrorCode.UserData, "未找到指定日期的排班数据");
            }
            if (requestDto.Details.All(a => a.Doctors == null || !a.Doctors.Any()))
            {
                return Failed(ErrorCode.UserData, "至少需要有一个时段内存在医生排班");
            }

            var todayScheduleDatas = await biz.GetHospitalScheduleListOneDayAsync(UserID, scheduleDate, doctorScheduleModel.TemplateGuid);
            var todayHasAppointment = todayScheduleDatas.Details.FirstOrDefault(a => a.HasAppointment) != null;//判断今日是否有预约
            List<DoctorScheduleModel> insertSchedules = new List<DoctorScheduleModel>();
            List<string> deleteScheduleIds = new List<string>();
            List<ScheduleForUpdate> forUpdates = new List<ScheduleForUpdate>();
            if (requestDto.TemplateGuid != todayScheduleDatas.TemplateGuid)
            {
                if (todayHasAppointment)
                {
                    return Failed(ErrorCode.UserData, "今日已存在预约，不能更换班次模板");
                }
                //更换模板后，若满足编辑前置条件，则需要删除更换模板之前的排班数据
                var oldSchedules = await biz.GetModelsByScheduleDatesAsync(new List<DateTime> { scheduleDate }, UserID);
                deleteScheduleIds.AddRange(oldSchedules.Select(a => a.ScheduleGuid).ToList());
            }

            //检测时段内是否存在有预约且排班数据被修改的情况:结果为false表示存在此情况，则不允许提交排班
            requestDto.Details.ForEach(a =>
            {
                var oldIntervalInfo = todayScheduleDatas.Details.Where(t => t.WorkshiftDetailGuid == a.WorkshiftDetailGuid).FirstOrDefault();
                var oldIntervalDoctorIds = (oldIntervalInfo?.Doctors.OrderBy(t => t.DoctorGuid).Select(t => t.DoctorGuid)) ?? new List<string>();

                var newIntervalDoctorIds = a.Doctors == null ? new List<string>() : a.Doctors.OrderBy(d => d.DoctorGuid).Select(d => d.DoctorGuid).Distinct();
                var sameDoctor = oldIntervalDoctorIds.All(newIntervalDoctorIds.Contains) && oldIntervalDoctorIds.Count() == newIntervalDoctorIds.Count();//判断时段内，医生数据是否一致
                var sameAppointmentLimit = ((oldIntervalInfo?.AppointmentLimit) ?? -1) == (a.AppointmentLimit ?? 0);//判断时段内，号源数量是否一致
                var sameSchedule = sameDoctor && sameAppointmentLimit;//时段内排班数据是否一致
                //无预约，且医生排班数据有变化
                if (oldIntervalInfo == null || (!oldIntervalInfo.HasAppointment && !sameSchedule))
                {
                    insertSchedules.AddRange(newIntervalDoctorIds.Except(oldIntervalDoctorIds).Select(id => new DoctorScheduleModel
                    {
                        ScheduleGuid = Guid.NewGuid().ToString("N"),
                        HospitalGuid = UserID,
                        TemplateGuid = requestDto.TemplateGuid,
                        ScheduleDate = scheduleDate,
                        CycleGuid = doctorScheduleModel.CycleGuid,
                        WorkshiftDetailGuid = a.WorkshiftDetailGuid,
                        DoctorGuid = id,
                        AppointmentLimit = a.AppointmentLimit.Value,
                        AppointmentQuantity = 0,
                        CreatedBy = UserID,
                        LastUpdatedBy = UserID
                    }));
                    var oldIntervalInfo_Doctors = (oldIntervalInfo?.Doctors) ?? new List<GetHospitalScheduleListOneDayResponseDto.DoctorSchedule>();

                    deleteScheduleIds.AddRange(oldIntervalDoctorIds.Except(newIntervalDoctorIds).Join(oldIntervalInfo_Doctors, l => l, r => r.DoctorGuid, (l, r) => r.ScheduleGuid));
                    //若号源数量有修改
                    if (!sameAppointmentLimit)
                    {
                        //先找到时间片段内，新老数据都存在的医生数据，作为排班更新数据源
                        //再通过医生id交集从老数据的排班数据内拿到医生原来的排班guid
                        //最终组合为 排班guid和修改后的号源数量
                        var updates = oldIntervalDoctorIds.Intersect(newIntervalDoctorIds).Join(a.Doctors, l => l, r => r.DoctorGuid, (l, r) => new
                        {
                            DoctorGuid = l,
                            a.AppointmentLimit
                        }).Join(oldIntervalInfo_Doctors, l => l.DoctorGuid, r => r.DoctorGuid, (l, r) => new ScheduleForUpdate
                        {
                            ScheduleGuid = r.ScheduleGuid,
                            AppointmentLimit = l.AppointmentLimit.Value
                        }).ToList();
                        forUpdates.AddRange(updates);
                    }
                }
            });
            (var result, var msg) = await biz.EditHospitalScheduleOneDayAsync(insertSchedules, deleteScheduleIds, forUpdates);
            return result ? Success() : Failed(ErrorCode.UserData, string.IsNullOrWhiteSpace(msg) ? "编辑排班失败" : msg);
        }

        /// <summary>
        /// 新增或复制排班数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto))]
        public async Task<IActionResult> CreateCopyScheduleAsync([FromBody]CreateCopyScheduleRequestDto requestDto)
        {
            var biz = new DoctorScheduleBiz();
            requestDto.ScheduleDates = requestDto.ScheduleDates.Select(a => a.Date).ToList();
            var checkTheSameMonth = requestDto.ScheduleDates.Select(a => a.Date.AddDays(1 - a.Day)).Distinct().Count() > 1;
            if (checkTheSameMonth)
            {
                return Failed(ErrorCode.UserData, "不能跨月选择应用到的日期");
            }
            if (requestDto.Details.All(a => a.Doctors == null || !a.Doctors.Any()))
            {
                return Failed(ErrorCode.UserData, "至少需要有一个时段内存在医生排班");
            }

            #region 班次模板基础数据检测
            var newTemplateModel = await new DoctorWorkshifTemplateBiz().GetAsync(requestDto.TemplateGuid);
            if (newTemplateModel == null)
            {
                return Failed(ErrorCode.UserData, "传入的班次模板模板数据未找到");
            }
            var workshiftDetailModels = await new DoctorWorkshiftDetailBiz().GetModelsByTemplateGuidAsync(requestDto.TemplateGuid);
            if (!workshiftDetailModels.Any())
            {
                return Failed(ErrorCode.UserData, "班次模板下未找到详细的班次数据");
            }
            var invalidWorkshiftDetailIds = new List<string>();
            var checkWorkshiftDetailIds = requestDto.Details.All(a =>
            {
                var checkRes = workshiftDetailModels.FirstOrDefault(b => b.WorkshiftDetailGuid == a.WorkshiftDetailGuid);
                if (checkRes == null)
                {
                    invalidWorkshiftDetailIds.Add(a.WorkshiftDetailGuid);
                }
                return checkRes != null;
            });
            if (!checkWorkshiftDetailIds)
            {
                return Failed(ErrorCode.UserData, $"班次guid[{string.Join(",", invalidWorkshiftDetailIds.Distinct())}])非法，未在数据库查找到");
            }
            #endregion

            var checkWhetherScheduleModel = await biz.GetModelsByScheduleDatesAsync(requestDto.ScheduleDates, UserID);
            var hasScheudleDates = requestDto.ScheduleDates.Intersect(checkWhetherScheduleModel.Select(a => a.ScheduleDate));
            if (hasScheudleDates.Count() > 0)
            {
                return Failed(ErrorCode.UserData, $"[{string.Join(",", hasScheudleDates.Select(a => a.ToString("yyyy/MM/dd")))}]已有排班");
            }
            var oneDate = requestDto.ScheduleDates.FirstOrDefault();
            var startDate = oneDate.Date.AddDays(1 - oneDate.Day);
            var hasCycle = true;
            var cycle = await new DoctorScheduleCycleBiz().GetHospitalCycleByStartDateAsync(UserID, startDate);
            if (cycle == null)
            {
                hasCycle = false;
                cycle = new DoctorScheduleCycleModel
                {
                    CycleGuid = Guid.NewGuid().ToString("N"),
                    HospitalGuid = UserID,
                    StartDate = startDate,
                    EndDate = startDate.AddMonths(1).AddDays(-1),
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = string.Empty
                };
            }
            List<DoctorScheduleModel> modelsOneDay = new List<DoctorScheduleModel>();
            List<DoctorScheduleModel> models = new List<DoctorScheduleModel>();
            //先创建一天的排班数据
            foreach (var item in requestDto.Details)
            {
                modelsOneDay.AddRange(item.Doctors.Select(a => new DoctorScheduleModel
                {
                    ScheduleGuid = Guid.NewGuid().ToString("N"),
                    HospitalGuid = UserID,
                    TemplateGuid = requestDto.TemplateGuid,
                    ScheduleDate = oneDate,
                    CycleGuid = cycle.CycleGuid,
                    WorkshiftDetailGuid = item.WorkshiftDetailGuid,
                    DoctorGuid = a.DoctorGuid,
                    AppointmentLimit = item.AppointmentLimit.Value,
                    AppointmentQuantity = 0,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = string.Empty
                }));
            }
            //将排班数据复制到其他日期上
            requestDto.ScheduleDates.ForEach(a =>
            {
                var oneDayData = modelsOneDay.Select(m => m.Clone() as DoctorScheduleModel).ToList();
                oneDayData.ForEach(m =>
                {
                    m.ScheduleGuid = Guid.NewGuid().ToString("N");
                    m.ScheduleDate = a.Date;
                });
                models.AddRange(oneDayData);
            });
            var result = await biz.CreateCopyScheduleAsync(models, cycle, hasCycle);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "提交排班数据失败");
        }

        /// <summary>
        /// 删除某天的排班数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DeleteSchduleOneDayAsync(DateTime scheduleDate)
        {
            var biz = new DoctorScheduleBiz();
            var models = await biz.GetModelsByScheduleDatesAsync(new List<DateTime> { scheduleDate.Date }, UserID);
            if (DateTime.Now.Date > scheduleDate.Date)
            {
                return Failed(ErrorCode.UserData, "只可删除当天之后的排班数据");
            }
            if (!models.All(a => a.AppointmentQuantity == 0))
            {
                return Failed(ErrorCode.UserData, "当前日期内存在预约记录，无法批量删除整天的排班数据");
            }
            var result = await biz.DeleteScheduleOneDayAsync(scheduleDate.Date, UserID, models.Count);
            return result ? Success() : Failed(ErrorCode.UserData, "批量删除当前日期内的数据失败");
        }

        /// <summary>
        /// 获取医院指定月份有排班的日期列表
        /// </summary>
        /// <param name="date">指定月份内的某一天日期</param>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<List<DateTime>>))]
        public async Task<IActionResult> GetHospitalScheduleDateListInMonthAsync(DateTime date)
        {
            var firstDay = date.Date.AddDays(1 - date.Day);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);
            var biz = new DoctorScheduleBiz();
            var models = await biz.GetHospitalScheduleByDateIntervalAsync(UserID, firstDay, lastDay);
            return Success(models.Select(a => a.ScheduleDate.Date).Distinct().OrderBy(a => a));
        }

        /// <summary>
        /// 获取医院下的医生选择项列表 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(typeof(ResponseDto<List<GetDoctorSelectListReponseDto>>))]
        public async Task<IActionResult> GetDoctorSelectListAsync()
        {
            var response = await new DoctorBiz().GetDoctorSelectListOfHospitalAsync(UserID);
            return Success(response.Select(a => new GetDoctorSelectListReponseDto
            {
                DoctorGuid = a.Key,
                DoctorName = a.Value
            }));
        }
    }
}
