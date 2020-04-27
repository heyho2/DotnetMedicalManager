using Dapper;
using GD.BizBase;
using GD.DataAccess;
using GD.Dtos.Doctor.Doctor;
using GD.Dtos.Doctor.Prescription;
using GD.Dtos.Enum;
using GD.Models.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Doctor
{
    /// <summary>
    /// 
    /// </summary>
    public class PrescriptionInformationBiz : BaseBiz<PrescriptionInformationModel>
    {

        /// <summary>
        /// 获取处方记录分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetPrescriptionRecordPageListResponseDto> GetPrescriptionRecordPageListAsync(GetPrescriptionRecordPageListRequestDto requestDto)
        {
            if (requestDto.IsExport)
            {
                requestDto.PageSize = int.MaxValue;
            }
            requestDto.StartDate = requestDto.StartDate.Date;
            requestDto.EndDate = requestDto.EndDate.Date.AddDays(1).AddSeconds(-1);
            var sqlWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(requestDto.HospitalGuid))
            {
                sqlWhere = $"{sqlWhere} and a.hospital_guid=@HospitalGuid";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.OfficeGuid))
            {
                sqlWhere = $"{sqlWhere} and a.office_guid=@OfficeGuid";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.DoctorGuid))
            {
                sqlWhere = $"{sqlWhere} and a.doctor_guid=@DoctorGuid";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.UserGuid))
            {
                sqlWhere = $"{sqlWhere} and da.user_guid=@UserGuid";
            }
            if (requestDto.PaidStatus != null)
            {
                sqlWhere = $"{sqlWhere} and a.paid_status=@PaidStatus";
            }
            if (requestDto.ReceptionType != null)
            {
                sqlWhere = $"{sqlWhere} and a.reception_type=@ReceptionType";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.Keyword))
            {
                sqlWhere = $@"{sqlWhere} AND (
							a.patient_name LIKE CONCAT(@Keyword,'%') 
							OR a.patient_phone = @Keyword
							OR a.clinical_diagnosis LIKE CONCAT(@Keyword,'%') 
							OR d.item_name LIKE CONCAT(@Keyword,'%') 
							OR c.prescription_no = @Keyword 
                            OR u.user_name LIKE CONCAT(@Keyword,'%') 
						) ";
            }
            var sql = $@"SELECT
							a.information_guid,
                            CASE
		                        a.paid_status 
		                        WHEN 'NotPaid' THEN
		                        '未收款' 
		                        WHEN 'PartiallyUnpaid' THEN
		                        '部分未收款' 
		                        WHEN 'Paided' THEN
		                        '已收款' 
                                ELSE '无有效处方' 
	                        END as `status`,
                            da.appointment_guid,
							da.patient_relationship,
							a.patient_name,
							a.patient_gender,
							a.patient_phone,
							b.office_name,
							a.clinical_diagnosis,
							a.reception_type,
							a.creation_date AS reception_time,
							a.total_cost,
							u.user_name as doctor_name,
							GROUP_CONCAT( distinct c.prescription_no SEPARATOR '/' ) AS prescription_nos
						FROM
							t_doctor_prescription_information a
							LEFT JOIN t_doctor_office b ON a.office_guid = b.office_guid
							LEFT JOIN t_doctor_prescription c ON a.information_guid = c.information_guid 
							AND c.`status` <> 'Cancellation' 
							AND c.`enable` = 1 
							left join t_doctor_prescription_recipe d on d.prescription_guid=c.prescription_guid
							left join t_consumer_doctor_appointment da on da.appointment_guid=a.appointment_guid
							left join t_utility_user u on u.user_guid=a.doctor_guid
						WHERE
							a.`enable` = 1 {sqlWhere}
							and a.creation_date BETWEEN @StartDate and @EndDate
						GROUP BY
							a.information_guid,
                            a.paid_status,
                            da.appointment_guid,
							da.patient_relationship,
							a.patient_name,
							a.patient_gender,
							a.patient_phone,
							b.office_name,
							a.clinical_diagnosis,
							a.reception_type,
							a.creation_date,
							a.total_cost,
							u.user_name
						ORDER BY a.creation_date desc ";
            var result = await MySqlHelper.QueryByPageAsync<GetPrescriptionRecordPageListRequestDto, GetPrescriptionRecordPageListResponseDto, GetPrescriptionRecordPageListItemDto>(sql, requestDto);
            

            return result;
        }

        /// <summary>
        /// 获取历史处方记录分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetHistoryPrescriptionRecordsResponseDto> GetHistoryPrescriptionRecordsAsync(GetHistoryPrescriptionRecordsRequestDto requestDto)
        {
            var sqlWhere = string.Empty;

            requestDto.StartDate = requestDto.StartDate.Date;
            requestDto.EndDate = requestDto.EndDate.Date.AddDays(1).AddSeconds(-1);

            if (!string.IsNullOrWhiteSpace(requestDto.Keyword))
            {
                sqlWhere = $@"{sqlWhere} AND (
        					a.clinical_diagnosis LIKE CONCAT(@Keyword,'%') 
							OR d.item_name LIKE CONCAT(@Keyword,'%') 
							OR u.user_name LIKE CONCAT(@Keyword,'%') 
			                OR c.prescription_no = @Keyword 
							OR a.patient_name LIKE CONCAT(@Keyword,'%') 
						) ";
            }

            var sql = $@"SELECT
							a.information_guid,
                            da.appointment_guid,
							da.patient_relationship,
							a.patient_name,
							a.patient_gender,
							a.patient_phone,
							a.clinical_diagnosis,
							a.reception_type,
							a.creation_date AS reception_time,
							a.total_cost,
                            a.paid_status,
							u.user_name as doctor_name,
							GROUP_CONCAT( distinct c.prescription_no SEPARATOR '/' ) AS prescription_nos 
						FROM
							t_doctor_prescription_information a
							LEFT JOIN t_doctor_prescription c ON a.information_guid = c.information_guid 
							AND c.`status` <> 'Cancellation' 
							AND c.`enable` = 1 
							left join t_doctor_prescription_recipe d on d.prescription_guid  = c.prescription_guid
							left join t_consumer_doctor_appointment da on da.appointment_guid = a.appointment_guid
							left join t_utility_user u on u.user_guid = a.doctor_guid
						WHERE
							a.`enable` = 1 {sqlWhere}
                            and da.user_guid = @UserGuid and a.creation_date BETWEEN @StartDate and @EndDate
						GROUP BY
							a.information_guid,
                            da.appointment_guid,
							da.patient_relationship,
							a.patient_name,
							a.patient_gender,
							a.patient_phone,
							a.clinical_diagnosis,
							a.reception_type,
							a.creation_date,
							a.total_cost,
							u.user_name
						ORDER BY a.creation_date desc ";

            var result = await MySqlHelper.QueryByPageAsync<GetHistoryPrescriptionRecordsRequestDto, GetHistoryPrescriptionRecordsResponseDto, GetHistoryPrescriptionRecordsItemDto>(sql, requestDto);

            return result;
        }

        /// <summary>
        /// 获取统计数据
        /// </summary>
        /// <param name="sDate"></param>
        /// <param name="eDate"></param>
        /// <param name="hospitalGuid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<decimal> GetAPrescriptionMoneyAsync(DateTime sDate, DateTime eDate, string hospitalGuid, PrescriptionStatusEnum? status = null)
        {
            var sqlWhere = string.Empty;
            if (status != null)
            {
                sqlWhere = "AND b.`status`=@Status";
            }
            var sql = $@"SELECT
	                        SUM(b.total_cost)
                        FROM
	                        t_doctor_prescription_information a
	                        INNER JOIN t_doctor_prescription b ON a.information_guid = b.information_guid 
                        WHERE
	                        hospital_guid = @HospitalGuid {sqlWhere}
	                        AND a.`enable` = 1 
	                        AND a.creation_date BETWEEN @StartDate 
	                        AND @EndDate";
            using (var conn = MySqlHelper.GetConnection())
            {
                var count = await conn.QueryFirstOrDefaultAsync<decimal?>(sql, new
                {
                    HospitalGuid = hospitalGuid,
                    StartDate = sDate,
                    EndDate = eDate,
                    Status = status
                });
                return count ?? 0;
            }
        }
        /// <summary>
        /// 数据统计查询
        /// </summary>
        /// <param name="isMonths"></param>
        /// <param name="sDate"></param>
        /// <param name="eDate"></param>
        /// <param name="hospitalGuid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<GetCollectionResponseDto> GetAPrescriptionDataAsync(bool isMonths, DateTime sDate, DateTime eDate, string hospitalGuid, PrescriptionStatusEnum? status = null)
        {
            if (!isMonths)
            {
                eDate = eDate.AddDays(1).AddSeconds(-1);
            }
            else
            {
                eDate = eDate.AddMonths(1).AddSeconds(-1);
            }
            GetCollectionResponseDto result = new GetCollectionResponseDto();
            var sqlWhere = string.Empty;
            if (status != null)
            {
                sqlWhere = "AND b.`status`=@Status";
            }
            var sql = $@"SELECT
	                        b.total_cost,
	                        c.office_guid,
	                        d.office_name,
	                        b.creation_date 
                        FROM
	                        t_doctor_prescription_information a
	                        INNER JOIN t_doctor_prescription b ON a.information_guid = b.information_guid
	                        INNER JOIN t_consumer_doctor_appointment c ON a.appointment_guid = c.appointment_guid
	                        LEFT JOIN t_doctor_office d ON c.office_guid = d.office_guid 
                        WHERE
	                        a.hospital_guid = @HospitalGuid {sqlWhere}
	                        AND a.`enable` = 1 
	                        AND a.creation_date BETWEEN @StartDate 
	                        AND @EndDate";
            List<PrescriptionData> prescriptionDataList = null;
            using (var conn = MySqlHelper.GetConnection())
            {
                prescriptionDataList = (await conn.QueryAsync<PrescriptionData>(sql, new
                {
                    HospitalGuid = hospitalGuid,
                    StartDate = sDate,
                    EndDate = eDate,
                    Status = status
                })).ToList();
            }
            if (prescriptionDataList == null)
            {
                return result;
            }
            List<string> officeNameList = new List<string>();
            officeNameList = prescriptionDataList.Select(s => s.OfficeName).Distinct().ToList();
            result.OfficeNames = officeNameList;
            if (!isMonths)
            {
                var dates = new List<string>();
                var startDate = sDate.Date;
                while (startDate <= eDate.Date)
                {
                    dates.Add(startDate.ToString("yyyy-MM-dd"));
                    startDate = startDate.AddDays(1).Date;
                }
                //对数据进行分组
                var resultList = prescriptionDataList.GroupBy(s => new { s.CreationDate.Date, s.OfficeGuid, s.OfficeName }).Select(r => new GetCollectionTimeto
                {
                    CollectionDate = r.Key.Date.ToString("yyyy-MM-dd"),
                    OfficeName = r.Key.OfficeName,
                    Quantity = r.Select(q => q.TotalCost ?? 0).Sum()
                }).ToList();
                result.CollectionDates = dates;
                var officeNameAndDates = dates.Join(officeNameList, d => 1, gs => 1, (d, gs) => new OfficeNameAndDate
                {
                    Datestr = d,
                    OfficeName = gs
                }).ToList();
                var consecutiveDates = officeNameAndDates.GroupJoin(resultList,
                    d => new { Date = d.Datestr, d.OfficeName },
                    s => new { Date = s.CollectionDate, s.OfficeName },
                    (d, gs) => new GetCollectionTimeto
                    {
                        CollectionDate = d.Datestr,
                        OfficeName = d.OfficeName,
                        Quantity = (gs.FirstOrDefault()?.Quantity) ?? 0
                    }).OrderBy(a => a.CollectionDate).ToList();
                if (consecutiveDates != null)
                {
                    result.Datas = consecutiveDates.GroupBy(s => s.OfficeName).Select(a => new Data
                    {
                        OfficeName = a.Key,
                        Value = a.ToList().Select(r => r.Quantity).ToList()
                    }).ToList();
                }
            }
            else
            {
                //月统计间隔
                var dates = new List<string>();
                var startDate = sDate.Date;
                while (startDate <= eDate.Date)
                {
                    dates.Add(startDate.ToString("yyyy-MM"));
                    startDate = startDate.AddMonths(1).Date;
                }
                //对数据进行分组
                var resultList = prescriptionDataList.GroupBy(s => new { s.CreationDate.Date, s.OfficeGuid, s.OfficeName }).Select(r => new GetCollectionTimeto
                {
                    CollectionDate = r.Key.Date.ToString("yyyy-MM"),
                    OfficeName = r.Key.OfficeName,
                    Quantity = r.Select(q => q.TotalCost ?? 0).Sum()
                }).ToList();
                result.CollectionDates = dates;
                var officeNameAndDates = dates.Join(officeNameList, d => 1, gs => 1, (d, gs) => new OfficeNameAndDate
                {
                    Datestr = d,
                    OfficeName = gs
                }).ToList();
                var consecutiveDates = officeNameAndDates.GroupJoin(resultList,
                    d => new { Date = d.Datestr, d.OfficeName },
                    s => new { Date = s.CollectionDate, s.OfficeName },
                    (d, gs) => new GetCollectionTimeto
                    {
                        CollectionDate = d.Datestr,
                        OfficeName = d.OfficeName,
                        Quantity = (gs.FirstOrDefault()?.Quantity) ?? 0
                    }).OrderBy(a => a.CollectionDate).ToList();
                if (consecutiveDates != null)
                {
                    result.Datas = consecutiveDates.GroupBy(s => s.OfficeName).Select(a => new Data
                    {
                        OfficeName = a.Key,
                        Value = a.ToList().Select(r => r.Quantity).ToList()
                    }).ToList();
                }
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="appointmentGuid"></param>
        /// <returns></returns>
        public async Task<PrescriptionInformationModel> ExistInformation(string appointmentGuid)
        {
            var sql = @"SELECT * FROM t_doctor_prescription_information
                        WHERE appointment_guid = @appointmentGuid";

            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<PrescriptionInformationModel>(sql, new { appointmentGuid });
            }
        }
        public class PrescriptionData
        {
            /// <summary>
            /// 总计
            /// </summary>
            public decimal? TotalCost { get; set; }
            /// <summary>
            /// 科室id
            /// </summary>
            public string OfficeGuid { get; set; }
            /// <summary>
            /// 科室名称
            /// </summary>
            public string OfficeName { get; set; }
            /// <summary>
            /// 时间
            /// </summary>
            public DateTime CreationDate { get; set; }
        }
        public class OfficeNameAndDate
        {
            /// <summary>
            /// 时间
            /// </summary>
            public string Datestr { get; set; }
            /// <summary>
            /// 科室
            /// </summary>
            public string OfficeName { get; set; }
        }
    }
}
