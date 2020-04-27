using Dapper;
using GD.BizBase;
using GD.DataAccess;
using GD.Dtos.Doctor.Hospital;
using GD.Models.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Doctor
{
    /// <summary>
    /// 医生排班业务类
    /// </summary>
    public class DoctorScheduleBiz : BaseBiz<DoctorScheduleModel>
    {
        /// <summary>
        /// 获取诊所某天的号源总数量
        /// </summary>
        /// <param name="date"></param>
        /// <param name="hospitalGuid"></param>
        /// <returns></returns>
        public async Task<int> GetAppointmentSourceTotalOneDayAsync(DateTime date, string hospitalGuid)
        {
            var sql = "select sum(appointment_limit) from t_doctor_schedule where hospital_guid=@HospitalGuid and schedule_date=@ScheduleDate and `enable`=1";
            using (var conn = MySqlHelper.GetConnection())
            {
                var count = await conn.QueryFirstOrDefaultAsync<int?>(sql, new
                {
                    HospitalGuid = hospitalGuid,
                    ScheduleDate = date.Date
                });
                return count ?? 0;
            }

        }

        /// <summary>
        /// 获取医院指定日期排班班次模板guid
        /// </summary>
        /// <param name="hospitalGuid"></param>
        /// <param name="scheduleDate"></param>
        /// <returns></returns>
        public async Task<DoctorScheduleModel> GetDoctorScheduleTemplateGuidAsync(string hospitalGuid, DateTime scheduleDate)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = "where hospital_guid=@hospitalGuid and schedule_date=@scheduleDate and `enable`=1 order by schedule_guid limit 1 ";
                var result = await conn.GetListAsync<DoctorScheduleModel>(sql, new { hospitalGuid, scheduleDate = scheduleDate.Date });
                return result.FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取周期下的排班详情列表
        /// </summary>
        /// <param name="cycleGuid"></param>
        /// <returns></returns>
        public async Task<List<GetDoctorScheduleListOfCycleResponseDto>> GetDoctorScheduleListOfCycleAsync(string cycleGuid)
        {
            var sql = @"SELECT
	                        a.template_guid,
	                        a.schedule_date,
	                        b.workshift_type,
	                        TIME_FORMAT(b.start_time,'%H:%i') as start_time,
	                        TIME_FORMAT(b.end_time,'%H:%i') as end_time , 	
	                        a.appointment_limit,
	                        a.schedule_guid,
	                        a.doctor_guid,
							u.user_name as doctor_name,
	                        count( c.appointment_guid ) AS appointment_count 
                        FROM
	                        t_doctor_schedule a
	                        INNER JOIN t_doctor_workshift_detail b ON a.workshift_detail_guid = b.workshift_detail_guid
	                        LEFT JOIN t_consumer_doctor_appointment c ON c.schedule_guid = a.schedule_guid 
	                        AND c.`status` <> 'Cancel' 
							left join t_utility_user u on u.user_guid = a.doctor_guid
                        WHERE
	                        a.cycle_guid = @cycleGuid and a.`enable`=1
                        GROUP BY
	                        a.template_guid,
	                        a.schedule_date,
	                        b.workshift_type,
	                        b.start_time,
	                        b.end_time,
	                        a.appointment_limit,
	                        a.schedule_guid,
	                        a.doctor_guid";
            var data = new List<GetCycleScheduleListItem>();
            using (var conn = MySqlHelper.GetConnection())
            {
                data = (await conn.QueryAsync<GetCycleScheduleListItem>(sql, new { cycleGuid })).ToList();
            }
            var result = data.GroupBy(a => new { a.TemplateGuid, a.ScheduleDate }).Select(a => new GetDoctorScheduleListOfCycleResponseDto
            {
                TemplateGuid = a.Key.TemplateGuid,
                ScheduleDate = a.Key.ScheduleDate,
                Details = GetScheduleDetailGorupByScheduleInterval(a.ToList())
            }).OrderBy(a => a.ScheduleDate).ToList();

            List<GetDoctorScheduleListOfCycleResponseDto.ScheduleDetail> GetScheduleDetailGorupByScheduleInterval(List<GetCycleScheduleListItem> items)
            {
                return items.GroupBy(a => new { a.WorkshiftType, a.StartTime, a.EndTime, a.AppointmentLimit })
                    .Select(a => new GetDoctorScheduleListOfCycleResponseDto.ScheduleDetail
                    {
                        WorkshiftType = a.Key.WorkshiftType,
                        StartTime = a.Key.StartTime,
                        EndTime = a.Key.EndTime,
                        HasAppointment = a.FirstOrDefault(h => h.AppointmentCount > 0) != null,
                        AppointmentLimit = a.Key.AppointmentLimit,
                        Doctors = a.Select(doctor => new GetDoctorScheduleListOfCycleResponseDto.DoctorSchedule
                        {
                            DoctorName = doctor.DoctorName,
                            DoctorGuid = doctor.DoctorGuid
                        }).OrderBy(doctor => doctor.DoctorName).ToList()
                    }).OrderBy(a => a.StartTime).ToList();
            }
            return result;
        }

        /// <summary>
        /// 获取医院某天的排班详情（包括未排班的时间段）
        /// </summary>
        /// <param name="hospitalGuid">医院guid</param>
        /// <param name="scheduleDate">排班日期</param>
        /// <param name="templateGuid">模板guid</param>
        /// <returns></returns>
        public async Task<GetHospitalScheduleListOneDayResponseDto> GetHospitalScheduleListOneDayAsync(string hospitalGuid, DateTime scheduleDate, string templateGuid)
        {
            var sql = @"SELECT
	                        b.template_guid,
	                        a.schedule_date,
                            b.workshift_detail_guid,
	                        b.workshift_type,
	                        TIME_FORMAT(b.start_time,'%H:%i') as start_time,
	                        TIME_FORMAT(b.end_time,'%H:%i') as end_time , 	
	                        case when a.appointment_limit is null then b.appointment_limit else a.appointment_limit end as appointment_limit,
	                        a.schedule_guid,
	                        a.doctor_guid,
							u.user_name as doctor_name,
	                        count( c.appointment_guid ) AS appointment_count 
                        FROM
	                        t_doctor_workshift_detail b
	                        LEFT JOIN t_doctor_schedule a ON a.workshift_detail_guid = b.workshift_detail_guid 
	                            AND a.hospital_guid = @hospitalGuid 
	                            AND a.schedule_date = @scheduleDate 
	                            AND a.`enable` = 1
	                        LEFT JOIN t_consumer_doctor_appointment c ON c.schedule_guid = a.schedule_guid 
	                        AND c.`status` <> 'Cancel' 
													left join t_utility_user u on u.user_guid=a.doctor_guid
                        WHERE
	                        b.template_guid = @templateGuid and b.`enable`=1
                        GROUP BY
	                        b.template_guid,
	                        a.schedule_date,
                            b.workshift_detail_guid,
	                        b.workshift_type,
	                        b.start_time,
	                        b.end_time,
 	                        appointment_limit,
	                        a.schedule_guid,
	                        a.doctor_guid,
							u.user_name";
            var data = new List<GetHospitalScheduleListOneDayItem>();
            using (var conn = MySqlHelper.GetConnection())
            {
                data = (await conn.QueryAsync<GetHospitalScheduleListOneDayItem>(sql, new { hospitalGuid, templateGuid, scheduleDate = scheduleDate.Date })).ToList();
            }
            data.ForEach(a => a.ScheduleDate = scheduleDate.Date);
            var result = data.GroupBy(a => new { a.TemplateGuid, a.ScheduleDate }).Select(a => new GetHospitalScheduleListOneDayResponseDto
            {
                TemplateGuid = a.Key.TemplateGuid,
                ScheduleDate = a.Key.ScheduleDate,
                Details = GetScheduleDetailGorupByScheduleInterval(a.ToList())
            }).FirstOrDefault();

            List<GetHospitalScheduleListOneDayResponseDto.ScheduleDetail> GetScheduleDetailGorupByScheduleInterval(List<GetHospitalScheduleListOneDayItem> items)
            {
                return items.GroupBy(a => new { a.WorkshiftDetailGuid, a.WorkshiftType, a.StartTime, a.EndTime, a.AppointmentLimit })
                    .Select(a => new GetHospitalScheduleListOneDayResponseDto.ScheduleDetail
                    {
                        WorkshiftDetailGuid = a.Key.WorkshiftDetailGuid,
                        WorkshiftType = a.Key.WorkshiftType,
                        StartTime = a.Key.StartTime,
                        EndTime = a.Key.EndTime,
                        HasAppointment = a.FirstOrDefault(h => h.AppointmentCount != null && h.AppointmentCount > 0) != null,
                        AppointmentLimit = a.Key.AppointmentLimit,
                        Doctors = a.Where(doctor => doctor.ScheduleGuid != null).Select(doctor => new GetHospitalScheduleListOneDayResponseDto.DoctorSchedule
                        {
                            DoctorName = doctor.DoctorName,
                            DoctorGuid = doctor.DoctorGuid,
                            ScheduleGuid = doctor.ScheduleGuid
                        }).OrderBy(doctor => doctor.DoctorName).ToList()
                    }).OrderBy(a => a.StartTime).ToList();
            }
            return result;
        }

        /// <summary>
        /// 获取医生指定日期区间内的排班数据
        /// </summary>
        /// <param name="doctorGuid"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<List<DoctorScheduleModel>> GetDoctorScheduleByDateIntervalAsync(string doctorGuid, DateTime startDate, DateTime endDate)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<DoctorScheduleModel>("where doctor_guid=@doctorGuid and schedule_date between @startDate and @endDate and `enable`=1", new
                {
                    doctorGuid,
                    startDate = startDate.Date,
                    endDate = endDate.Date.AddDays(1).AddSeconds(-1)
                });
                return result.ToList();

            }
        }

        /// <summary>
        /// 获取医院指定日期区间内的排班数据
        /// </summary>
        /// <param name="hospitalGuid"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<List<DoctorScheduleModel>> GetHospitalScheduleByDateIntervalAsync(string hospitalGuid, DateTime startDate, DateTime endDate)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<DoctorScheduleModel>("where hospital_guid=@hospitalGuid and schedule_date between @startDate and @endDate and `enable`=1", new
                {
                    hospitalGuid,
                    startDate = startDate.Date,
                    endDate = endDate.Date.AddDays(1).AddSeconds(-1)
                });
                return result.ToList();

            }
        }

        /// <summary>
        /// 编辑某天的排班数据
        /// </summary>
        /// <param name="insertSchedules"></param>
        /// <param name="deleteScheduleIds"></param>
        /// <param name="forUpdates"></param>
        /// <returns></returns>
        public async Task<(bool, string)> EditHospitalScheduleOneDayAsync(List<DoctorScheduleModel> insertSchedules, List<string> deleteScheduleIds, List<ScheduleForUpdate> forUpdates)
        {
            var msg = string.Empty;
            var result = await MySqlHelper.TransactionAsync(async (conn, trans) =>
             {
                 insertSchedules.InsertBatch(conn);
                 if (deleteScheduleIds.Any())
                 {
                     //删除时，再次检测是否有用户预约，避免出现删除时用户预约的极端情况
                     var count = await conn.DeleteListAsync<DoctorScheduleModel>("where schedule_guid in @deleteScheduleIds and appointment_quantity=0", new { deleteScheduleIds = deleteScheduleIds.Distinct() });
                     if (count != deleteScheduleIds.Count)
                     {
                         msg = "操作过程中可能有用户预约导致提交失败";
                         return false;
                     }
                 }
                 foreach (var item in forUpdates)
                 {
                     //修改时，再次检测是否有用户预约，避免出现更新时用户预约的极端情况
                     var count = await conn.ExecuteAsync("update t_doctor_schedule set appointment_limit=@AppointmentLimit where schedule_guid=@ScheduleGuid and appointment_quantity=0", item);
                     if (count != 1)
                     {
                         msg = "操作过程中可能有用户预约导致提交失败";
                         return false;
                     }
                 }
                 return true;
             });
            return (result, msg);
        }

        /// <summary>
        /// 通过排班日期列表获取医生排班记录
        /// </summary>
        /// <param name="scheduleDates"></param>
        /// <param name="hospitalGuid"></param>
        /// <returns></returns>
        public async Task<List<DoctorScheduleModel>> GetModelsByScheduleDatesAsync(List<DateTime> scheduleDates, string hospitalGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<DoctorScheduleModel>("where hospital_guid = @hospitalGuid and schedule_date in @scheduleDates and `enable`=1", new { hospitalGuid, scheduleDates = scheduleDates.Select(a => a.Date) });
                return result.ToList();
            }
        }

        /// <summary>
        /// 批量删除指定日期的排班数据
        /// </summary>
        /// <param name="scheduleDate"></param>
        /// <returns></returns>
        public async Task<bool> DeleteScheduleOneDayAsync(DateTime scheduleDate, string hospitalGuid, int scheduleCount)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                var count = await conn.DeleteListAsync<DoctorScheduleModel>("where hospital_guid=@hospitalGuid and schedule_date=@scheduleDate and appointment_quantity=0", new { hospitalGuid, scheduleDate = scheduleDate.Date });
                return count == scheduleCount;
            });
        }

        /// <summary>
        /// 新增或复制排班数据
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CreateCopyScheduleAsync(List<DoctorScheduleModel> models, DoctorScheduleCycleModel cycleModel, bool hasCycle)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                models.InsertBatch(conn);
                if (!hasCycle)
                {
                    await conn.InsertAsync<string, DoctorScheduleCycleModel>(cycleModel);
                }
                return true;
            });
        }

        /// <summary>
        /// 检测班次模板是否在当月或当月之后有被使用
        /// </summary>
        /// <param name="templateGuid"></param>
        /// <param name="hospitalGuid"></param>
        /// <returns></returns>
        public async Task<bool> WhetherTemplateApplyInCurrentMonthOrLaterAsync(string templateGuid, string hospitalGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = "select * from t_doctor_schedule where hospital_guid=@hospitalGuid and template_guid=@templateGuid and schedule_date>=concat( date_format( NOW(), '%Y-%m-' ), '01' ) order by schedule_date limit 1;";
                return (await conn.QueryFirstOrDefaultAsync<DoctorScheduleModel>(sql, new { templateGuid, hospitalGuid })) != null;
            }
        }

        /// <summary>
        /// 检测班次模板是否被使用
        /// </summary>
        /// <param name="templateGuid"></param>
        /// <param name="hospitalGuid"></param>
        /// <returns></returns>
        public async Task<bool> WhetherTemplateApplyAsync(string templateGuid, string hospitalGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = "select * from t_doctor_schedule where hospital_guid=@hospitalGuid and template_guid=@templateGuid order by schedule_date limit 1;";
                return (await conn.QueryFirstOrDefaultAsync<DoctorScheduleModel>(sql, new { templateGuid, hospitalGuid })) != null;
            }
        }

    }
}
