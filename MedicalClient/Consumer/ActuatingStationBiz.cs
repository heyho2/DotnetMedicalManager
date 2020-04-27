using Dapper;
using GD.DataAccess;
using GD.Dtos.Consumer.Appointment;
using GD.Dtos.Merchant.Appointment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GD.Consumer
{
    /// <summary>
    /// 执行端BIz
    /// </summary>
    public class ActuatingStationBiz
    {
        /// <summary>
        ///执行端-获取今日预约列表
        /// </summary>
        /// <param name="targetGuid">目标guid</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">单页条数</param>
        /// <returns></returns>
        public async Task<GetMyAppointmentListByConditionResponseDto> GetMyAppointmentListByCondition(GetMyAppointmentListByConditionRequestDto requestDto)
        {

            using (var conn = MySqlHelper.GetConnection())
            {
                var strWhere = " ";
                if (!string.IsNullOrWhiteSpace(requestDto.SelectStr))
                {
                    strWhere += $" AND u.user_name like '%{requestDto.SelectStr}%'  or u.phone like '%{requestDto.SelectStr}%'   ";
                }
                if (requestDto.IsToday)
                {
                    strWhere += $" AND to_days(csp.appointment_date) = to_days(now())  ";
                }
                var sql = $@"SELECT
	                                    csp.consumption_guid,
	                                    csp.appointment_date,
	                                    u.user_name,
	                                    u.phone,
	                                    tp.therapist_guid,
	                                    tp.therapist_name,
	                                    proj.project_guid,
	                                    proj.project_name,
                                        CONCAT( acc.base_path, acc.relative_path ) AS PortraitUrl,
	                                    csp.consumption_status ,
										s.`name` as member_name,
										s.sex as member_sex,
										s.age_year as member_age_year,
										s.age_month as member_age_month
                                    FROM
	                                    t_consumer_consumption AS csp
	                                    LEFT JOIN t_utility_user AS u ON csp.user_guid = u.user_guid
	                                    LEFT JOIN t_merchant_therapist AS tp ON csp.operator_guid = tp.therapist_guid
	                                    LEFT JOIN t_mall_project AS proj ON csp.project_guid = proj.project_guid 
                                        LEFT JOIN t_utility_accessory acc ON acc.accessory_guid = u.portrait_guid 
	                                    AND acc.`enable` = 1 
																			left join t_consumer_service_member s on s.service_member_guid=csp.service_member_guid
                                    WHERE
	                                    csp.operator_guid = @UserGuid
                                        And   csp.`enable`=@Enable
                                        {strWhere}
                                    ORDER BY
	                                    csp.consumption_status ASC,
                                    	csp.appointment_date DESC  ";
                return await MySqlHelper.QueryByPageAsync<GetMyAppointmentListByConditionRequestDto, GetMyAppointmentListByConditionResponseDto, GetMyAppointmentListByConditionItemDto>(sql, requestDto);
            }
        }

        /// <summary>
        /// 查看当天预约班次信息等 请求Dto
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SelectOneDayMySchduleInfoResponseDto>> SelectOneDayMySchduleInfo(
            SelectOneDayMySchduleInfoRequestDto requestDto)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                string whereStr = whereStr = $" And b.schedule_date BETWEEN @BeginDate and  @EndDate ";// " And  b.schedule_date=@BeginDate ";

                if (requestDto.EndDate == null)
                {
                    requestDto.EndDate = requestDto.BeginDate;
                }
                var sql = $@"SELECT
	                                    b.schedule_date,
	                                    b.work_shift_guid,
	                                    c.work_shift_name,
	                                    d.therapist_guid,
	                                    d.therapist_name,
	                                    GROUP_CONCAT( CONCAT( e.start_time, '-', e.end_time ) ORDER BY e.start_time ) AS WorkShiftTimeDuration 
                                    FROM
	                                    t_merchant_schedule_template a
	                                    INNER JOIN t_merchant_schedule b ON a.schedule_template_guid = b.schedule_template_guid 
	                                    AND a.`enable` = b.`enable`
	                                    INNER JOIN t_merchant_work_shift c ON b.work_shift_guid = c.work_shift_guid
	                                    INNER JOIN t_merchant_therapist d ON b.target_guid = d.therapist_guid
	                                    INNER JOIN t_merchant_work_shift_detail e ON c.work_shift_guid = e.work_shift_guid 
                                    WHERE
	                                     d.therapist_guid = @TherapistGuid
	                                    AND a.`enable` = 1 
                                        {whereStr}
                                    GROUP BY
	                                    b.schedule_date,
	                                    b.work_shift_guid,
	                                    c.work_shift_name,
	                                    d.therapist_guid,
	                                    d.therapist_name 
                                    ORDER BY
	                                    b.schedule_date DESC  ";

                var result = await conn.QueryAsync<SelectOneDayMySchduleInfoResponseDto>(sql, new { BeginDate = requestDto.BeginDate.Value.Date, EndDate = requestDto.EndDate.Value.Date, requestDto.TherapistGuid });
                return result;

            }
        }


    }
}
