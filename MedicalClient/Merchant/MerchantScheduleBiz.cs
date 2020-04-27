using Dapper;
using GD.BizBase;
using GD.DataAccess;
using GD.Dtos.Mall.Mall;
using GD.Dtos.Merchant.Merchant;
using GD.Models.Merchant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Merchant
{
    public class MerchantScheduleBiz : BaseBiz<MerchantScheduleModel>
    {
        /// <summary>
        /// 通过Id获取Model
        /// </summary>
        /// <param name="scheduleId">排班id</param>
        /// <returns></returns>
        public async Task<MerchantScheduleModel> GetModelAsync(string scheduleId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = "select * from t_merchant_schedule where schedule_guid=@scheduleId and `enable`=1";
                return await conn.QueryFirstOrDefaultAsync<MerchantScheduleModel>(sql, new { scheduleId });
            }
        }

        /// <summary>
        /// 获取店铺某天某些美疗师的排班数据
        /// </summary>
        /// <param name="merchantGuid">店铺guid</param>
        /// <param name="scheduleDate">排班日期</param>
        /// <param name="therapistGuids">美疗师guids</param>
        /// <returns></returns>
        public async Task<List<MerchantScheduleModel>> GetMerchantScheduleOfSomeOneAsync(string merchantGuid, DateTime scheduleDate, string therapistGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = "select * from t_merchant_schedule where merchant_guid=@merchantGuid and schedule_date=@scheduleDate and target_guid = @therapistGuid and `enable`=1";
                var result = await conn.QueryAsync<MerchantScheduleModel>(sql, new { merchantGuid, scheduleDate = scheduleDate.Date, therapistGuid });
                return result?.ToList();
            }
        }


        /// <summary>
        /// 店铺排班
        /// </summary>
        /// <param name="merchantGuid">店铺guid</param>
        /// <param name="scheduleDate">排班日期</param>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task<bool> CreateTheScheduleOfTheMerchant(string merchantGuid, DateTime scheduleDate, List<MerchantScheduleModel> models, List<MerchantScheduleDetailModel> detialModels)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                var sqlDelDetial = @"DELETE a 
                                    FROM
	                                    t_merchant_schedule_detail a
	                                    INNER JOIN t_merchant_schedule b ON a.schedule_guid = b.schedule_guid 
                                    WHERE
	                                    b.merchant_guid = @merchantGuid 
	                                    AND b.schedule_date = @scheduleDate";
                //删除店铺指定日期排班明细数据
                await conn.ExecuteAsync(sqlDelDetial, new { merchantGuid, scheduleDate = scheduleDate.Date });
                //删除店铺指定日期排班数据
                await conn.DeleteListAsync<MerchantScheduleModel>("where merchant_guid=@merchantGuid and schedule_date=@scheduleDate", new { merchantGuid, scheduleDate = scheduleDate.Date });
                //新增排班数据
                foreach (var item in models)
                {
                    if (string.IsNullOrEmpty(await conn.InsertAsync<string, MerchantScheduleModel>(item))) return false;
                }
                //新增排班明细数据
                foreach (var item in detialModels)
                {
                    if (string.IsNullOrEmpty(await conn.InsertAsync<string, MerchantScheduleDetailModel>(item))) return false;
                }

                return true;
            });
        }
        
        /// <summary>
        /// 获取店铺某天某服务项目的服务人员排班列表
        /// </summary>
        /// <param name="merchantId">店铺guid</param>
        /// <param name="projectId">项目guid</param>
        /// <param name="scheduleDate">预约日期</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<GetMerchantTherapistScheduleOfProjectOneDayDataTransfer> GetMerchantTherapistScheduleOfProjectOneDayAsync(GetMerchantTherapistScheduleOfProjectOneDayRequestDto requestDto)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"DROP TEMPORARY TABLE
                            IF
	                            EXISTS tmp_therapist;
                            DROP TEMPORARY TABLE
                            IF
	                            EXISTS tmp_therapist_1;	
                            CREATE TEMPORARY TABLE tmp_therapist AS SELECT distinct
                            a.therapist_guid,
                            a.therapist_name,
                            CONCAT( c.base_path, c.relative_path ) AS PortraitUrl 
                            FROM
	                            t_merchant_therapist a
	                            INNER JOIN t_merchant_therapist_project b ON a.therapist_guid = b.therapist_guid
	                            LEFT JOIN t_utility_accessory c ON a.portrait_guid = c.accessory_guid 
	                            AND c.`enable` = 1 
                            WHERE
	                            a.merchant_guid = @merchantGuid 
	                            AND b.project_guid = @projectGuid 
	                            AND a.`enable` = 1 
	                            AND b.`enable` = 1 
                            ORDER BY
	                            a.therapist_name 
	                            LIMIT @pageIndex,
	                            @pageSize;
                            CREATE TEMPORARY TABLE tmp_therapist_1 SELECT
                            * 
                            FROM
	                            tmp_therapist;
	
                            SELECT
	                            therapist_guid AS TherapistGuid,
	                            therapist_name AS TherapistName,
	                            PortraitUrl 
                            FROM
	                            tmp_therapist;
	
                            SELECT
	                            a.therapist_guid AS TherapistGuid,
	                            b.schedule_guid AS ScheduleGuid,
	                            c.schedule_detail_guid AS ScheduleDetailGuid,
	                            c.consumption_guid AS ConsumptionGuid,
	                            c.start_time AS StartTime,
	                            c.end_time AS EndTime 
                            FROM
	                            tmp_therapist_1 a
	                            INNER JOIN t_merchant_schedule b ON a.therapist_guid = b.target_guid
	                            LEFT JOIN t_merchant_schedule_detail c ON b.schedule_guid = c.schedule_guid 
	                            AND c.`enable` = 1 
                            WHERE
	                            b.schedule_date = @scheduleDate 
	                            AND b.`enable` = 1;
                            DROP TEMPORARY TABLE tmp_therapist;
                            DROP TEMPORARY TABLE tmp_therapist_1;";
                var reader = await conn.QueryMultipleAsync(sql, new { merchantGuid = requestDto.MerchantId, projectGuid = requestDto.ProjectId, scheduleDate = requestDto.ScheduleDate.Date, pageIndex = (requestDto.PageIndex - 1) * requestDto.PageSize, pageSize = requestDto.PageSize });
                var merchantScheduleTherapistDto = (await reader.ReadAsync<MerchantScheduleTherapistDto>())?.ToList();
                var merchantTherapistTimeDetailsDto = (await reader.ReadAsync<MerchantTherapistTimeDetailsDto>())?.ToList();

                return new GetMerchantTherapistScheduleOfProjectOneDayDataTransfer() { Therapists = merchantScheduleTherapistDto, ScheduleDetails = merchantTherapistTimeDetailsDto };
            }
        }

        /// <summary>
        /// 获取指定排班周期的排班日历明细数据
        /// </summary>
        /// <param name="scheduleTemplateGuid">排班周期guid</param>
        /// <returns></returns>
        public async Task<List<MerchantSchduleCalendarDetailDto>> GetMerchantSchduleCalendarDetailAsync(string scheduleTemplateGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
                              NOW() as ScheduleDate,
	                            a.schedule_template_guid,
                                a.start_date,
	                            a.end_date,
	                            b.work_shift_guid,
	                            b.work_shift_name,
	                            GROUP_CONCAT(CONCAT(c.start_time, '-', c.end_time) ORDER BY c.start_time) AS WorkShiftTimeDuration
                            FROM
                                t_merchant_schedule_template a
                                INNER JOIN t_merchant_work_shift b ON a.template_guid = b.template_guid
                                inner join t_merchant_work_shift_detail c on b.work_shift_guid = c.work_shift_guid
                            WHERE
                                a.schedule_template_guid = @scheduleTemplateGuid
                                AND a.`enable` = 1
                            GROUP BY    a.schedule_template_guid,
	                            b.work_shift_guid,
	                            b.work_shift_name;

                           SELECT
	                            b.schedule_date,
	                            b.work_shift_guid,
	                            d.therapist_guid,
	                            d.therapist_name
                            FROM
	                            t_merchant_schedule_template a
	                            INNER JOIN t_merchant_schedule b ON a.schedule_template_guid = b.schedule_template_guid 
	                            AND a.`enable` = b.`enable`
	                            INNER JOIN t_merchant_therapist d ON b.target_guid = d.therapist_guid and b.`enable`=d.`enable`
                            WHERE
                                a.schedule_template_guid = @scheduleTemplateGuid
                                AND a.`enable` = 1 ;";
                var reader = await conn.QueryMultipleAsync(sql, new { scheduleTemplateGuid });
                var dateItems = (await reader.ReadAsync<MerchantSchduleCalendarDateDto>())?.ToList();
                var therapistItems = (await reader.ReadAsync<MerchantSchduleCalendarWorkShfitTherapistDto>())?.ToList();

                if (!dateItems.Any() || !therapistItems.Any())
                {
                    return new List<MerchantSchduleCalendarDetailDto>();
                }
                var firstRow = dateItems[0];//用来获取周期的开始和结束日期
                var StarDate = firstRow.StartDate.Value.Date;
                var EndDate = firstRow.EndDate.Value.Date;
                var dateIndex = StarDate;
                var dateList = new List<MerchantSchduleCalendarDateDto>();
                while (dateIndex <= EndDate)
                {
                    foreach (var item in dateItems)
                    {
                        dateList.Add(new MerchantSchduleCalendarDateDto
                        {
                            ScheduleDate = dateIndex,
                            WorkShiftGuid = item.WorkShiftGuid,
                            WorkShiftName = item.WorkShiftName,
                            WorkShiftTimeDuration = item.WorkShiftTimeDuration
                        });
                    }
                    dateIndex = dateIndex.AddDays(1);
                }
                var details = from date in dateList
                              join th in therapistItems
                              on
                              new { date.ScheduleDate, date.WorkShiftGuid } equals
                              new { th.ScheduleDate, th.WorkShiftGuid }
                              into result
                              from r in result.DefaultIfEmpty()
                              select new MerchantSchduleCalendarDetailDto
                              {
                                  ScheduleDate = date.ScheduleDate,
                                  WorkShiftGuid = date.WorkShiftGuid,
                                  WorkShiftName = date.WorkShiftName,
                                  WorkShiftTimeDuration = date.WorkShiftTimeDuration,
                                  TherapistGuid = r?.TherapistGuid,
                                  TherapistName = r?.TherapistName
                              };

                return details.ToList();
            }
        }
    }
}
