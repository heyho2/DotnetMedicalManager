using GD.BizBase;
using GD.DataAccess;
using GD.Dtos.Doctor.Hospital;
using GD.Models.Consumer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;
using GD.Dtos.Enum.DoctorAppointment;
using GD.Dtos.Consumer.Consumer;
using GD.Dtos.Doctor.Doctor;
using GD.Models.Doctor;
using Newtonsoft.Json;

namespace GD.Consumer
{
    /// <summary>
    /// 挂号预约业务类
    /// </summary>
    public class DoctorAppointmentBiz : BaseBiz<DoctorAppointmentModel>
    {
        /// <summary>
        /// 获取诊所今日挂号列表
        /// </summary>
        /// <returns></returns>
        public async Task<GetAppointmentPageListTodayResponseDto> GetAppointmentPageListTodayAsync(GetAppointmentPageListTodayRequestDto requestDto)
        {
            var sqlWhere = string.Empty;
            if (requestDto.AppointmentStatus == null)
            {
                sqlWhere = $"{sqlWhere} AND a.`status` in('Waiting','Treated','Miss') ";
            }
            else
            {
                sqlWhere = $"{sqlWhere} and a.`status`=@AppointmentStatus";
            }

            if (!string.IsNullOrWhiteSpace(requestDto.Keyword))
            {
                sqlWhere = $"{sqlWhere} and (a.patient_name = @Keyword or a.patient_phone = @Keyword or a.appointment_no = @Keyword )";
            }
            //   var sql = $@"SELECT
            //                a.appointment_guid,
            //                   a.appointment_no ,
            //                a.patient_name,
            //                a.patient_gender,
            //                   a.user_guid,
            //                TIMESTAMPDIFF(
            //                 YEAR,
            //                 a.patient_birthday,
            //                NOW()) AS patient_age,
            //                a.appointment_time,
            //                d.office_name,
            //                b.user_name AS doctor_name,
            //                a.patient_phone,
            //                a.`status` AS appointment_status,
            //                   (
            // SELECT  
            //   CASE WHEN (p.`status` = 1) THEN 0 ELSE 1 END as paidStatus 
            //                      FROM t_doctor_prescription as p
            //INNER JOIN t_doctor_prescription_information AS pi 
            //                                       ON p.information_guid = pi.information_guid
            // WHERE pi.appointment_guid = a.appointment_guid AND p.`status` IN(1,2)
            // ORDER BY p.`status` LIMIT 1
            //) as paid_status
            //               FROM
            //                t_consumer_doctor_appointment a
            //                LEFT JOIN t_utility_user b ON b.user_guid = a.doctor_guid
            //                LEFT JOIN t_doctor c ON a.doctor_guid = c.doctor_guid
            //                LEFT JOIN t_doctor_office d ON d.office_guid = c.office_guid 
            //               WHERE
            //                a.doctor_guid=@DoctorGuid
            //                   AND ( a.appointment_time BETWEEN '{DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss")}' AND '{DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss")}' )
            //                {sqlWhere}
            //               ORDER BY
            //                a.appointment_time";
            var sql = $@"SELECT
	                        a.appointment_guid,
	                        a.appointment_no,
	                        a.patient_name,
	                        a.patient_gender,
	                        a.user_guid,
	                        TIMESTAMPDIFF(
		                        YEAR,
		                        a.patient_birthday,
	                        NOW()) AS patient_age,
	                        a.appointment_time,
	                        d.office_name,
	                        b.user_name AS doctor_name,
	                        a.patient_phone,
	                        a.`status` AS appointment_status,
                            CASE
		                        WHEN count( DISTINCT f.prescription_guid )> 0 THEN
		                        0 ELSE 1 
	                        END AS paid_status,
	                        ifnull( sum( f.total_cost ), 0 ) AS obligation_amount 
                        FROM
	                        t_consumer_doctor_appointment a
	                        LEFT JOIN t_utility_user b ON b.user_guid = a.doctor_guid
	                        LEFT JOIN t_doctor c ON a.doctor_guid = c.doctor_guid
	                        LEFT JOIN t_doctor_office d ON d.office_guid = c.office_guid
	                        LEFT JOIN t_doctor_prescription_information e ON e.appointment_guid = a.appointment_guid
	                        LEFT JOIN t_doctor_prescription f ON f.information_guid = e.information_guid 
	                        AND f.`status` = 1 
	 	                        WHERE
 		                        a.doctor_guid = @DoctorGuid 
 		                        AND ( a.appointment_time BETWEEN '{DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss")}' AND '{DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss")}' ) 
		                        {sqlWhere}	
	                        GROUP BY
		                        a.appointment_guid,
		                        a.appointment_no,
		                        a.patient_name,
		                        a.patient_gender,
		                        a.user_guid,
		                        patient_age,
		                        a.appointment_time,
		                        d.office_name,
		                        b.user_name,
		                        a.patient_phone,
		                        a.`status` 
                        ORDER BY
	                        a.appointment_time,a.appointment_guid";
            return await MySqlHelper.QueryByPageAsync<GetAppointmentPageListTodayRequestDto, GetAppointmentPageListTodayResponseDto, GetAppointmentPageListTodayItemDto>(sql, requestDto);
        }

        /// <summary>
        /// 获取诊所挂号列表
        /// </summary>
        /// <returns></returns>
        public async Task<GetAppointmentPageListResponseDto> GetAppointmentPageListAsync(GetAppointmentPageListRequestDto requestDto)
        {
            var sqlWhere = string.Empty;
            if (requestDto.AppointmentStatus != null)
            {
                sqlWhere = $"{sqlWhere} and a.`status`=@AppointmentStatus";
            }

            if (!string.IsNullOrWhiteSpace(requestDto.OfficeGuid))
            {
                sqlWhere = $"{sqlWhere} and d.office_guid=@OfficeGuid";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.DoctorGuid))
            {
                sqlWhere = $"{sqlWhere} and a.doctor_guid=@DoctorGuid";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.Keyword))
            {
                sqlWhere = $"{sqlWhere} and (a.patient_name = @Keyword or a.patient_phone = @Keyword or a.appointment_no = @Keyword )";
            }

            if (requestDto.StartDate.HasValue && requestDto.EndDate.HasValue)
            {
                requestDto.StartDate = requestDto.StartDate.Value.Date;
                requestDto.EndDate = requestDto.EndDate.Value.Date.AddDays(1).AddSeconds(-1);
                sqlWhere = $"{sqlWhere} AND(a.appointment_time BETWEEN @StartDate and @EndDate)";

            }
            if (requestDto.IsExport)
            {
                requestDto.PageIndex = 1;
                requestDto.PageSize = int.MaxValue;
            }


            var sql = $@"SELECT
	                        a.appointment_guid,
                            a.appointment_no ,
	                        a.patient_name,
	                        a.patient_gender,
	                        TIMESTAMPDIFF(
		                        YEAR,
		                        a.patient_birthday,
	                        NOW()) AS patient_age,
	                        a.appointment_time,
                            a.appointment_deadline,
	                        d.office_name,
	                        b.user_name AS doctor_name,
	                        a.patient_phone,
	                        a.`status` AS appointment_status 
                        FROM
	                        t_consumer_doctor_appointment a
	                        LEFT JOIN t_utility_user b ON b.user_guid = a.doctor_guid
	                        LEFT JOIN t_doctor c ON a.doctor_guid = c.doctor_guid
	                        LEFT JOIN t_doctor_office d ON d.office_guid = c.office_guid 
                        WHERE
	                        a.hospital_guid = @HospitalGuid
	                        {sqlWhere}
                        ORDER BY
	                        a.appointment_time";
            return await MySqlHelper.QueryByPageAsync<GetAppointmentPageListRequestDto, GetAppointmentPageListResponseDto, GetAppointmentPageListItemDto>(sql, requestDto);
        }

        /// <summary>
        /// 获取诊所挂号趋势统计数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetAppointmentPersonTimeStatisticsResponseDto> GetAppointmentPersonTimeStatisticsAsync(GetAppointmentPersonTimeStatisticsRequestDto requestDto)
        {
            requestDto.StartDate = requestDto.StartDate.Date;
            requestDto.EndDate = requestDto.EndDate.Date.AddDays(1).AddSeconds(-1);
            var sql = @"SELECT
	                        DATE_FORMAT( appointment_time, '%Y-%m-%d' ) AS appointment_date,
	                        count( appointment_guid ) AS appointment_quantity 
                        FROM
	                        t_consumer_doctor_appointment 
                        WHERE
	                        hospital_guid = @HospitalGuid 
	                        AND `status` <>'Cancel'
	                        AND appointment_time BETWEEN @StartDate 
	                        AND @EndDate
                        GROUP BY
	                        appointment_date 
                        ORDER BY
	                        appointment_date";
            var dailyStatistics = new List<GetAppointmentPersonTimeStatisticsDto>();
            using (var conn = MySqlHelper.GetConnection())
            {
                dailyStatistics = (await conn.QueryAsync<GetAppointmentPersonTimeStatisticsDto>(sql, requestDto)).ToList();
            }
            var dates = new List<string>();
            var startDate = requestDto.StartDate;
            while (startDate <= requestDto.EndDate.Date)
            {
                dates.Add(startDate.ToString("yyyy-MM-dd"));
                startDate = startDate.AddDays(1).Date;
            }
            //连续日期左连接预约统计数据，得到连续日期的预约统计数据，无数据设置为0
            var consecutiveDates = dates.GroupJoin(dailyStatistics, d => d, s => s.AppointmentDate, (d, gs) => new GetAppointmentPersonTimeStatisticsDto
            {
                AppointmentDate = d,
                AppointmentQuantity = (gs.FirstOrDefault()?.AppointmentQuantity) ?? 0
            }).OrderBy(a => a.AppointmentDate).ToList();
            var result = new GetAppointmentPersonTimeStatisticsResponseDto
            {
                AppointmentDates = dates,
                StatisticsDatas = consecutiveDates
            };
            return result;
        }

        /// <summary>
        /// 单日挂号总数量，不包括已取消的挂号
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="hospitalGuid">医院guid</param>
        /// <returns></returns>
        public async Task<int> GetAppointmentTotalOneDayAsync(DateTime date, string hospitalGuid, AppointmentStatusEnum? status = null)
        {
            var sqlWhere = string.Empty;
            if (status != null)
            {
                sqlWhere = "AND `status`=@Status";
            }
            var sql = $@"SELECT
	                        count(appointment_guid) as count
                        FROM
	                        t_consumer_doctor_appointment 
                        WHERE
	                        hospital_guid = @HospitalGuid
                            AND `status` <>'Cancel' {sqlWhere} 
	                        AND appointment_time BETWEEN @StartDate
	                        AND @EndDate";
            using (var conn = MySqlHelper.GetConnection())
            {
                var count = await conn.QueryFirstOrDefaultAsync<int>(sql, new
                {
                    HospitalGuid = hospitalGuid,
                    StartDate = date.Date,
                    EndDate = date.Date.AddDays(1).AddSeconds(-1),
                    Status = status
                });
                return count;
            }
        }
        /// <summary>
        /// 获取用户挂号分页数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<GetAppointmentMemberPageListResponseDto> GetAppointmentMemberPageListAsync(GetAppointmentMemberRequestDto requestDto, string userID)
        {
            var sqlWhere = string.Empty;
            //筛选当前登录用户挂号记录数据
            sqlWhere = $" and a.user_guid='{userID}' ";
            if (requestDto.AppointmentRequestStatus.HasValue)
            {
                sqlWhere = $"{sqlWhere} and a.`status`='{requestDto.AppointmentRequestStatus.Value.ToString()}'";
            }
            var sql = $@"SELECT
	                        a.appointment_guid,
	                        a.appointment_no,
	                        a.`status`,
	                        a.appointment_time,
                            a.appointment_deadline,
	                        a.patient_name,
	                        b.hos_name,
	                        b.location,
	                        b.contact_number,
	                        b.longitude,
	                        b.latitude,
	                        d.user_name,
	                        c.office_name 
                        FROM
	                        t_consumer_doctor_appointment a
	                        LEFT JOIN t_doctor_hospital b ON a.hospital_guid = b.hospital_guid
	                        LEFT JOIN t_doctor c ON a.doctor_guid = c.doctor_guid
	                        LEFT JOIN t_utility_user d ON c.doctor_guid = d.user_guid
	                        WHERE 1=1 {sqlWhere} and a.`enable`=1 ORDER BY a.`status`,a.appointment_time ";
            return await MySqlHelper.QueryByPageAsync<GetAppointmentMemberRequestDto, GetAppointmentMemberPageListResponseDto, GetAppointmentMemberItemDto>(sql, requestDto);
        }
        /// <summary>
        /// 获取医院下所有医生信息
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetAppointmentDoctorPageListResponseDto> GetAppointmentDoctorPageListAsync(string userId, GetAppointmentDoctorRequestDto requestDto)
        {
            var sqlWhere = string.Empty;
            sqlWhere = $" and c.hospital_guid='{requestDto.HspitalGuid}' ";
            if (!string.IsNullOrWhiteSpace(requestDto.OfficeGuid))
            {
                sqlWhere = $"{sqlWhere} and a.office_guid='{requestDto.OfficeGuid}'";
            }
            var sql = $@"SELECT
	                        c.hos_name,
	                        a.doctor_guid,
	                        b.user_name AS DoctorName,
	                        d.config_name AS title,
	                        office_name,
	                        adept_tags,
                            CONCAT(h.base_path,h.relative_path) as picture,
	                        count(DISTINCT f.appointment_guid) as appointment_count,
	                        Max(g.appointment_time) as latest_appointment_date	
                        FROM
	                        t_doctor a
	                        LEFT JOIN t_utility_user b ON a.doctor_guid = b.user_guid
	                        LEFT JOIN t_doctor_hospital c ON a.hospital_guid = c.hospital_guid
	                        LEFT JOIN t_manager_dictionary d ON a.title_guid = d.dic_guid 
                            LEFT JOIN t_consumer_doctor_appointment f on a.doctor_guid=f.doctor_guid
                            LEFT JOIN t_utility_accessory h on a.portrait_guid=h.accessory_guid and h.`enable`=1
	                        LEFT JOIN t_consumer_doctor_appointment g on a.doctor_guid=g.doctor_guid and g.`enable`=1 and g.`status`='Treated' and g.user_guid='{userId}' 
	                        where a.`enable`=1 and a.`status`='approved' {sqlWhere} 
                            GROUP BY 
                            c.hos_name,
	                        a.doctor_guid,
	                        b.user_name,
	                        d.config_name,
	                        office_name,
	                        adept_tags,
                            picture
                            ORDER BY
	                        latest_appointment_date DESC,appointment_count desc";
            if (requestDto.AppointmentDate.HasValue)
            {
                sqlWhere = $"{sqlWhere} and o.schedule_date='{requestDto.AppointmentDate.Value.ToString("yyyy-MM-dd")}'";
                sql = $@"SELECT
	                        c.hos_name,
	                        a.doctor_guid,
	                        b.user_name AS DoctorName,
	                        d.config_name AS title,
	                        office_name,
	                        adept_tags,
                            CONCAT(h.base_path,h.relative_path) as picture,
	                        count(DISTINCT f.appointment_guid) as appointment_count,
	                        Max(g.appointment_time) as latest_appointment_date	
                        FROM
	                        t_doctor_schedule o INNER JOIN
	                        t_doctor a on o.doctor_guid=a.doctor_guid and o.hospital_guid='{requestDto.HspitalGuid}'
	                        LEFT JOIN t_utility_user b ON a.doctor_guid = b.user_guid
	                        LEFT JOIN t_doctor_hospital c ON a.hospital_guid = c.hospital_guid
	                        LEFT JOIN t_manager_dictionary d ON a.title_guid = d.dic_guid 
                            LEFT JOIN t_consumer_doctor_appointment f on a.doctor_guid=f.doctor_guid
                            LEFT JOIN t_utility_accessory h on a.portrait_guid=h.accessory_guid and h.`enable`=1
	                        LEFT JOIN t_consumer_doctor_appointment g on a.doctor_guid=g.doctor_guid and g.`enable`=1 and g.`status`='Treated' and g.user_guid='{userId}' 
	                        where a.`status`='approved' and a.`enable`=1 {sqlWhere} 
                            GROUP BY 
                            c.hos_name,
	                        a.doctor_guid,
	                        b.user_name,
	                        d.config_name,
	                        office_name,
	                        adept_tags,
                            picture
                            ORDER BY
	                        latest_appointment_date DESC,appointment_count desc";
            }
            return await MySqlHelper.QueryByPageAsync<GetAppointmentDoctorRequestDto, GetAppointmentDoctorPageListResponseDto, GetAppointmentDoctorItemDto>(sql, requestDto);
        }

        /// <summary>
        /// 新增预约挂号
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isHospital">是否是医院端创建</param>
        /// <returns></returns>
        public async Task<bool> CreateAppointmentAsync(DoctorAppointmentModel model, string appointmentNoPrefix, bool isHospital = false)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                //读取当前片段的医生排班记录，并加入行锁，避免其他用户读写此行记录
                var sqlSelect = "select * from t_doctor_schedule where schedule_guid = @ScheduleGuid for update;";
                var scheduleModel = await conn.QueryFirstOrDefaultAsync<DoctorScheduleModel>(sqlSelect, new { model.ScheduleGuid });
                //用户个人预约时，必须还有剩余的号源，医院端预约时，可以忽略号源限制
                if (!isHospital && scheduleModel.AppointmentLimit <= scheduleModel.AppointmentQuantity)
                {
                    return false;
                }

                //更新医生排班表记录已预约数量
                var sql = $@"UPDATE t_doctor_schedule 
                            SET appointment_quantity = appointment_quantity + 1 
                            WHERE
	                            schedule_guid = @ScheduleGuid ";
                var result = await conn.ExecuteAsync(sql, new { model.ScheduleGuid });

                //插入预约记录
                var appointmentId = await conn.InsertAsync<string, DoctorAppointmentModel>(model);
                //获取当前片段下预约记录表中的元素按照创建时间排序
                var sqlSelectAppointment = "select * from t_consumer_doctor_appointment where schedule_guid=@ScheduleGuid order by creation_date,appointment_guid;";
                var appointments = (await conn.QueryAsync<DoctorAppointmentModel>(sqlSelectAppointment, new { model.ScheduleGuid })).ToList();
                //取得刚刚插入数据的序号
                var firstIndex = appointments.FindIndex(a => a.AppointmentGuid == appointmentId);
                //更新刚刚出入记录数据的预约排号
                var number = (firstIndex + 1).ToString().PadLeft(2, '0');
                model.AppointmentNo = $"{appointmentNoPrefix}{number}";
                await conn.UpdateAsync(model);
                return true;//提交事物，释放行锁
            });
        }
        /// <summary>
        /// 获取用户预约过的医生数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetAppointmentDoctorAsync(string userId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<string>("select a.doctor_guid from t_consumer_doctor_appointment a where a.user_guid=@userId and a.`enable`= 1 and a.`status`='Treated' order by a.creation_date desc", new { userId });
                return result?.ToList();
            }
        }
        /// <summary>
        /// 获取医院下的科室
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<HospitalDepartmentsResponse>> GetHospitalDepartmentsAsync(string hospitalGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<HospitalDepartmentsResponse>(@"SELECT DISTINCT
                                                a.office_guid,
                                                a.office_name
                                            FROM
                                                t_doctor a
                                                inner join t_doctor_office o on o.office_guid=a.office_guid and o.`enable`=a.`enable`
                                            WHERE
                                                a.hospital_guid =@hospitalGuid
                                                AND a.`enable` = 1
                                                AND a.`status` = 'approved'", new { hospitalGuid });
                return result?.ToList();
            }
        }
        /// <summary>
        /// 获取用户最近去过的科室
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> GetBeenDepartmentsAsync(string userId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<string>($@"SELECT
	                                            a.office_guid 
                                            FROM
	                                            t_consumer_doctor_appointment a 
                                            WHERE
	                                            a.`status` = 'Treated' 
	                                            AND a.user_guid =@userId 
                                            ORDER BY
	                                            appointment_time DESC 
	                                            LIMIT 1
                                            ", new { userId });
                return result?.FirstOrDefault();
            }
        }
        /// <summary>
        /// 获取最近一次用户预约
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<LastAppointmentResponse> GetLastAppointmentAsync(string userId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<LastAppointmentResponse>($@"SELECT
	                                        b.hos_name,
	                                        b.location,
	                                        b.contact_number,
	                                        b.latitude,
	                                        b.longitude,
	                                        a.doctor_guid,
	                                        c.office_name,
	                                        d.config_name AS title,
	                                        a.appointment_time,
                                            f.user_name as DoctorName
                                        FROM
	                                        t_consumer_doctor_appointment a
	                                        LEFT JOIN t_doctor_hospital b ON a.hospital_guid = b.hospital_guid 
	                                        LEFT JOIN t_doctor c ON a.doctor_guid = c.doctor_guid  
	                                        LEFT JOIN t_manager_dictionary d ON c.title_guid = d.dic_guid 
                                            LEFT JOIN t_utility_user f on f.user_guid=a.doctor_guid
                                        WHERE
	                                        a.`status` = 'Treated' and a.`enable`=1 and b.`enable`=1  AND c.`enable`=1 and a.user_guid =@userId
                                        ORDER BY
	                                        appointment_time DESC 
	                                        LIMIT 1
                                            ", new { userId });
                return result?.FirstOrDefault();
            }
        }
        /// <summary>
        /// 分页获取医院列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<HospitalPageListResponseDto> GetHospitalPageListAsync(HospitalPageListRequestDto requestDto)
        {
            string sqlWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(requestDto.KeyWord))
            {
                sqlWhere = $"{sqlWhere} and a.hos_name like @KeyWord";
                requestDto.KeyWord = $"{requestDto.KeyWord}%";
            }
            var sql = $@"SELECT
	                        a.hospital_guid,
	                        a.hos_name,
	                        a.location,
                            a.contact_number,
                            a.latitude,
                            a.longitude,
	                        a.hos_tag,
                            a.is_hospital,
                            a.external_link,
	                        CONCAT(b.base_path,b.relative_path) as logo_url
                        FROM
	                        t_doctor_hospital a
	                        LEFT JOIN t_utility_accessory b ON a.logo_guid = b.accessory_guid
                        where a.`enable`=1 {sqlWhere} order by a.sort desc";
            var result = await MySqlHelper.QueryByPageAsync<HospitalPageListRequestDto, HospitalPageListResponseDto, HospitalItemDto>(sql, requestDto);
            return result;
        }
        /// <summary>
        /// 获取医生最近的7天预约数据
        /// </summary>
        /// <param name="doctorGuid"></param>
        /// <returns></returns>
        public async Task<List<GetAppointmentDoctorInfoResponse>> GetAppointmentDoctorInfoAsync(string doctorGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<GetAppointmentDoctorInfoResponse>($@"SELECT DISTINCT
	                                b.workshift_type,
	                                a.schedule_date,
	                                sum( CASE WHEN a.appointment_limit - a.appointment_quantity > 0 THEN a.appointment_limit - a.appointment_quantity ELSE 0 END ) as appointmentcount
                                FROM
	                                t_doctor_schedule a
                                    inner join t_doctor c on a.hospital_guid=c.hospital_guid and c.doctor_guid=a.doctor_guid and c.`enable`=1 and c.`status`='approved'
	                                LEFT JOIN t_doctor_workshift_detail b ON a.workshift_detail_guid = b.workshift_detail_guid 
	                                WHERE a.schedule_date>=@start and a.schedule_date<=@end  and a.doctor_guid=@doctorGuid and a.`enable`=1
                                GROUP BY
	                                a.schedule_date,
	                                b.workshift_type", new { start = DateTime.Now.Date, end = DateTime.Now.AddDays(6).Date, doctorGuid });
                return result?.ToList();
            }
        }
        /// <summary>
        /// 获取医生排班详情数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<GetAppointmentDoctorScheduleDetailResponse>> GetAppointmentDoctorDetailAsync(GetAppointmentDoctorScheduleDetailRequest request)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<GetAppointmentDoctorScheduleDetailResponse>($@"SELECT
	                a.schedule_guid,
                    b.workshift_detail_guid,
	                a.schedule_date,
	                a.appointment_limit,
	                a.appointment_quantity,
	                b.workshift_type,
	                DATE_FORMAT(b.start_time, '%H:%i' ) as start_time,
	                DATE_FORMAT(b.end_time, '%H:%i' ) as end_time 
                FROM
	                t_doctor_schedule a
	                LEFT JOIN t_doctor_workshift_detail b ON a.workshift_detail_guid = b.workshift_detail_guid WHERE a.doctor_guid=@DoctorGuid and a.schedule_date=@ScheduleDate and b.workshift_type=@WorkshiftType and a.`enable`=1 order by b.start_time", new { request.DoctorGuid, request.ScheduleDate, request.WorkshiftType });
                return result?.ToList();
            }
        }
        /// <summary>
        /// 获取推荐咨询医生
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="officeGuid"></param>
        /// <returns></returns>
        public async Task<List<DoctorRecommendResponseDto>> GetDoctorRecommendAsync(string userId, string officeGuid, string hospitalGuid)
        {
            var sqlWhere = $" and a.office_guid='{officeGuid}' and a.hospital_guid='{hospitalGuid}' ";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<DoctorRecommendResponseDto>($@"SELECT
	                      
	                        a.doctor_guid,
                            c.hos_name,
	                        b.user_name AS DoctorName,
	                        d.config_name AS title,
	                        office_name,
	                        adept_tags,
                            CONCAT(h.base_path,h.relative_path) as picture,
	                        Max(g.appointment_time) as latest_appointment_date	
                        FROM
	                        t_doctor a
	                        LEFT JOIN t_utility_user b ON a.doctor_guid = b.user_guid
	                        LEFT JOIN t_doctor_hospital c ON a.hospital_guid = c.hospital_guid
	                        LEFT JOIN t_manager_dictionary d ON a.title_guid = d.dic_guid 
                            LEFT JOIN t_utility_accessory h on a.portrait_guid=h.accessory_guid and h.`enable`=1
	                        LEFT JOIN t_consumer_doctor_appointment g on a.doctor_guid=g.doctor_guid and g.`enable`=1 and g.`status`='Treated' and g.user_guid='{userId}' 
	                        where a.`status`='approved' {sqlWhere} 
                            GROUP BY 
                            c.hos_name,
	                        a.doctor_guid,
	                        b.user_name,
	                        d.config_name,
	                        office_name,
	                        adept_tags,
                            picture
                            ORDER BY
	                        latest_appointment_date DESC LIMIT 5", new { officeGuid });
                return result?.ToList();
            }
        }
        /// <summary>
        /// 判断当前是否预约过
        /// </summary>
        /// <param name="doctorGuid"></param>
        /// <param name="scheduleGuid"></param>
        /// <param name="patientGuid"></param>
        /// <returns></returns>
        public async Task<DoctorAppointmentModel> GetDoctorAppointmentAsync(string doctorGuid, DateTime scheduleDate, string patientGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<DoctorAppointmentModel>($@"SELECT
	                                            * 
                                            FROM
	                                            t_consumer_doctor_appointment a 
                                            WHERE
	                                            a.`status` = 'Waiting' AND a.`enable`=1 AND a.doctor_guid=@doctorGuid AND date_format(a.appointment_time,'%Y-%m-%d') =@scheduleDate and a.patient_guid=@patientGuid
                                            ", new { doctorGuid, scheduleDate = scheduleDate.ToString("yyyy-MM-dd"), patientGuid });
                return result?.FirstOrDefault();
            }
        }
        /// <summary>
        /// 获取指定用户当月挂号记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<DoctorAppointmentModel>> GetDoctorAppointmentListAsync(string userId)
        {
            DateTime now = DateTime.Now;
            DateTime start = new DateTime(now.Year, now.Month, 1);
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<DoctorAppointmentModel>($@"SELECT
	                                            * 
                                            FROM
	                                            t_consumer_doctor_appointment a 
                                            WHERE
                                              a.user_guid=@userId AND a.creation_date>=@start and a.creation_date<@end order by a.creation_date", new { userId, start, end = start.AddMonths(1) });
                return result?.ToList();
            }
        }
        /// <summary>
        /// 获取三个月内用户挂号记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<DoctorAppointmentModel>> GetThreeMonthsDoctorAppointmentListAsync(string userId)
        {
            DateTime now = DateTime.Now;
            DateTime start = new DateTime(now.Year, now.Month, 1).AddMonths(-3);
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<DoctorAppointmentModel>($@"SELECT
	                                            * 
                                            FROM
	                                            t_consumer_doctor_appointment a 
                                            WHERE
                                              a.user_guid=@userId AND a.creation_date>=@start  order by a.creation_date", new { userId, start });
                return result?.ToList();
            }
        }
        /// <summary>
        /// 获取指定预约表数据
        /// </summary>
        /// <param name="appointmentGuid"></param>
        /// <returns></returns>
        public async Task<AppointmentResponseDto> GetAppointmentAsync(string appointmentGuid)
        {
            DateTime now = DateTime.Now;
            DateTime start = new DateTime(now.Year, now.Month, 1);
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<AppointmentResponseDto>($@"SELECT
	                                c.hos_name as HospitalName,
	                                a.appointment_no,
	                                b.user_name as DoctorName,
                                  d.office_name,
	                                a.appointment_time,
	                                a.appointment_deadline
                                FROM
	                                t_consumer_doctor_appointment a
	                                LEFT JOIN t_utility_user b ON a.doctor_guid = b.user_guid
	                                LEFT JOIN t_doctor_hospital c ON a.hospital_guid = c.hospital_guid
	                                LEFT JOIN  t_doctor d on d.doctor_guid=a.doctor_guid
                                WHERE
	                                a.appointment_guid =@appointmentGuid 
	                                AND a.`enable` =1
                                ", new { appointmentGuid });
                return result?.FirstOrDefault();
            }
        }
        /// <summary>
        /// 修改取消预约
        /// </summary>
        /// <param name="doctorAppointmentModel"></param>
        /// <param name="consumerModel"></param>
        /// <returns></returns>
        public async Task<bool> CancelAppointmentAsync(DoctorAppointmentModel doctorAppointmentModel, ConsumerModel consumerModel)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                await conn.UpdateAsync(doctorAppointmentModel);
                //更新医生排班表记录已预约数量-1
                var sql = $@"UPDATE t_doctor_schedule 
                            SET appointment_quantity = appointment_quantity - 1 
                            WHERE
	                            schedule_guid = @ScheduleGuid ";
                await conn.ExecuteAsync(sql, new { doctorAppointmentModel.ScheduleGuid });
                //修改截止日期
                if (consumerModel != null)
                {
                    await conn.UpdateAsync(consumerModel);
                }
                return true;
            });
        }
        /// <summary>
        /// 获取当前用户是否存在待就诊数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> GetWaitingCountAsync(string userId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<int>($@"SELECT
	                                                    count(1) 
                                                    FROM
	                                                    t_consumer_doctor_appointment a 
                                                    WHERE
	                                                    a.`enable` = 1 
	                                                    AND a.`status` = 'Waiting' 
	                                                    AND user_guid =@userId
	                                                    AND DATEDIFF(
		                                                    date_format( a.appointment_time, '%Y-%m-%d' ),
	                                                    CURDATE()) >=0
                                            ", new { userId });
                return result.FirstOrDefault();
            }
        }
    }
}
