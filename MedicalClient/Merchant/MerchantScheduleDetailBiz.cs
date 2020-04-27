using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GD.BizBase;
using GD.DataAccess;
using GD.Dtos.Merchant.Merchant;
using GD.Models.Merchant;

namespace GD.Merchant
{
    /// <summary>
    /// 门店排班明细实体业务类
    /// </summary>
    public class MerchantScheduleDetailBiz : BaseBiz<MerchantScheduleDetailModel>
    {
        /// <summary>
        /// 检查排班明细时间段是否和其他预约时段有交叉（检查占用情况）
        /// </summary>
        /// <param name="scheduleId">排班guid</param>
        /// <param name="startTime">开始时间 例如 09:00</param>
        /// <param name="endTime">结束时间 例如 10:00</param>
        /// <returns></returns>
        public async Task<bool> CheckScheduleDetailOccupied(string scheduleId, string startTime, string endTime)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            * 
                            FROM
	                            t_merchant_schedule_detail 
                            WHERE
	                            schedule_guid = @scheduleId 
	                            AND `enable` = 1 
	                            AND ( ( @startTime>=start_time and @startTime<end_time ) OR ( @endTime>start_time and @endTime<=end_time ) ) 
	                            LIMIT 0,
	                            1;";
                var model = await conn.QueryFirstOrDefaultAsync<MerchantScheduleDetailModel>(sql, new { scheduleId, startTime, endTime });
                if (model == null)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 根据 消费guid 获取model
        /// </summary>
        /// <param name="scheduleDetailIds"></param>
        /// <returns></returns>
        public async Task<MerchantScheduleDetailModel> GetModelAsyncByConsumptionGuid(string consumptionGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = "select * from t_merchant_schedule_detail where consumption_guid=@consumptionGuid and `enable`=1 ";
                var model = await conn.QueryFirstOrDefaultAsync<MerchantScheduleDetailModel>(sql, new { consumptionGuid });
                return model;
            }
        }

        /// <summary>
        /// 查询店铺服务人员某天的预约排班明细
        /// </summary>
        /// <param name="scheduleDate"></param>
        /// <param name="targetGuid"></param>
        /// <returns></returns>
        public async Task<List<MerchantScheduleDetailModel>> GetScheduleDetailByTargetGuid(DateTime scheduleDate, string targetGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            detail.* 
                            FROM
	                            t_merchant_schedule sche
	                            INNER JOIN t_merchant_schedule_detail detail ON sche.schedule_guid = detail.schedule_guid 
                            WHERE
	                            sche.schedule_date = @scheduleDate 
	                            AND sche.target_guid = @targetGuid 
	                            AND sche.`enable` = 1 
	                            AND detail.`enable` = 1;";
                var result = await conn.QueryAsync<MerchantScheduleDetailModel>(sql, new { scheduleDate = scheduleDate.ToString("yyyy-MM-dd"), targetGuid });
                return result?.ToList();
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="scheduleDetailIds"></param>
        /// <param name="scheduleId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string[] scheduleDetailIds, string scheduleId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.DeleteListAsync<MerchantScheduleDetailModel>("where schedule_guid=@scheduleId and `enable`=1 and schedule_detail_guid in @scheduleDetailIds", new { scheduleId, scheduleDetailIds });
                return result > 0;
            }
        }

        /// <summary>
        /// 批量获取model
        /// </summary>
        /// <param name="scheduleDetailIds"></param>
        /// <returns></returns>
        public async Task<List<MerchantScheduleDetailModel>> GetModelsAsync(string[] scheduleDetailIds, string scheduleId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<MerchantScheduleDetailModel>("select * from t_merchant_schedule_detail where schedule_guid=@scheduleId and `enable`=1 and schedule_detail_guid in @scheduleDetailIds", new { scheduleId, scheduleDetailIds });
                return result?.ToList();
            }
        }

        /// <summary>
        /// 服务人员锁定排班时间
        /// </summary>
        /// <param name="scheduleDetails"></param>
        /// <param name="scheduleGuid"></param>
        /// <returns></returns>
        public async Task<bool> LockScheduleDetailTimesAsync(List<MerchantScheduleDetailModel> scheduleDetails, string scheduleGuid)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {

                var sql = @"UPDATE t_merchant_schedule_detail a
                            LEFT JOIN t_merchant_schedule_detail b ON a.schedule_guid = b.schedule_guid 
                            AND b.`enable` = 1 
                            AND a.schedule_detail_guid <> b.schedule_detail_guid 
                            AND b.last_updated_date < a.last_updated_date 
                            AND (
                            ( a.start_time >= b.start_time AND a.start_time<b.end_time ) 
                            OR ( a.end_time > b.start_time AND a.end_time<=b.end_time ) 
                            ) 
                            SET a.consumption_guid = 'Lock'
                            WHERE
                                a.schedule_guid = @scheduleGuid
	                            AND a.schedule_detail_guid in @scheduleDetailGuids
                                AND a.`enable`=1
	                            AND b.schedule_detail_guid IS NULL;";
                foreach (var item in scheduleDetails)
                {
                    if (string.IsNullOrEmpty(await conn.InsertAsync<string, MerchantScheduleDetailModel>(item))) return false;
                }
                //检查锁定时间是否可行(有无时间交叉？)
                if ((await conn.ExecuteAsync(sql, new { scheduleDetailGuids = scheduleDetails.Select(a => a.ScheduleDetailGuid).ToArray(), scheduleGuid })) == 0) return false;
                return true;
            });
        }

        /// <summary>
        /// 检查商户在指定的排班周期中是否存在消费者预约
        /// </summary>
        /// <param name="merchantId">商户guid</param>
        /// <param name="scheduleTemplateGuid">排班周期guid</param>
        /// <returns></returns>
        public async Task<bool> CheckConsumerScheduledOfSchedulingCycle(string merchantId, string scheduleTemplateGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            a.schedule_detail_guid 
                            FROM
	                            t_merchant_schedule_detail a
	                            INNER JOIN t_merchant_schedule b ON a.schedule_guid = b.schedule_guid 
                            WHERE
	                            b.merchant_guid = @merchantId 
	                            AND b.schedule_template_guid = @scheduleTemplateGuid 
	                            AND a.consumption_guid <> 'Lock' 
	                            LIMIT 1";
                return (await conn.QueryFirstOrDefaultAsync<string>(sql, new { merchantId, scheduleTemplateGuid })) != null;
            }
        }

        /// <summary>
        /// 检查商户某一天是否有消费者预约项目
        /// </summary>
        /// <param name="merchantId">商户id</param>
        /// <param name="scheduleDate">日期</param>
        /// <returns></returns>
        public async Task<bool> CheckConsumerScheduledOnDay(string merchantId, DateTime scheduleDate)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            a.schedule_detail_guid 
                            FROM
	                            t_merchant_schedule_detail a
	                            INNER JOIN t_merchant_schedule b ON a.schedule_guid = b.schedule_guid 
                            WHERE
	                            b.merchant_guid = @merchantId 
	                            AND schedule_date = @scheduleDate
	                            and a.consumption_guid<>'Lock'
	                            limit 1";
                return (await conn.QueryFirstOrDefaultAsync<string>(sql, new { merchantId, scheduleDate = scheduleDate.Date })) != null;
            }
        }

        /// <summary>
        /// 查询店铺服务人员某端日期的预约排班明细
        /// </summary>
        /// <param name="scheduleSDate">排班起始日期</param>
        /// <param name="scheduleEDate">排班结束日期</param>
        /// <param name="targetGuid">被排班人guid</param>
        /// <returns></returns>
        public async Task<List<MerchantScheduleDetailDto>> GetScheduleDetailDtoByTargetGuid(DateTime scheduleSDate, DateTime scheduleEDate, string targetGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            detail.* ,
                                sche.schedule_date
                            FROM
	                            t_merchant_schedule sche
	                            INNER JOIN t_merchant_schedule_detail detail ON sche.schedule_guid = detail.schedule_guid 
                            WHERE
                                sche.target_guid = @targetGuid
	                            AND sche.schedule_date between @scheduleSDate and @scheduleEDate
	                            AND sche.`enable` = 1 
	                            AND detail.`enable` = 1;";
                var result = await conn.QueryAsync<MerchantScheduleDetailDto>(sql,
                    new
                    {
                        scheduleSDate = scheduleSDate.ToString("yyyy-MM-dd"),
                        scheduleEDate = scheduleEDate.ToString("yyyy-MM-dd"),
                        targetGuid
                    });
                return result?.ToList();
            }
        }
    }
}
