using Dapper;
using GD.BizBase;
using GD.DataAccess;
using GD.Dtos.Merchant.Merchant;
using GD.Models.Merchant;
using GD.Models.MySqlCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Merchant
{
    /// <summary>
    /// 排班模板业务类
    /// </summary>
    public class ScheduleTemplateBiz : BaseBiz<ScheduleTemplateModel>
    {
        /// <summary>
        /// 获取排班周期数据
        /// </summary>
        /// <param name="scheduleTemplateId"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task<ScheduleTemplateModel> GetModelAsync(string scheduleTemplateId, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = "select * from t_merchant_schedule_template where schedule_template_guid=@scheduleTemplateId and `enable`=@enable ";
                return await conn.QueryFirstOrDefaultAsync<ScheduleTemplateModel>(sql, new { scheduleTemplateId, enable });
            }
        }

        /// <summary>
        /// 获取店铺指定日期的排班周期模板数据
        /// </summary>
        /// <param name="merchantId">店铺guid</param>
        /// <param name="scheduleDate">日期</param>
        /// <returns></returns>
        public async Task<ScheduleTemplateModel> GetModelByDateAsync(string merchantId, DateTime scheduleDate)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = "select * from t_merchant_schedule_template where merchant_guid=@merchantId and `enable`=1 and start_date<=@scheduleDate and end_date>=@scheduleDate";
                return await conn.QueryFirstOrDefaultAsync<ScheduleTemplateModel>(sql, new { merchantId, scheduleDate = scheduleDate.Date });
            }
        }

        /// <summary>
        /// 检查店铺周期排班是否与其他周期排班日期交叉
        /// </summary>
        /// <param name="merchantGuid">店铺guid</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public async Task<bool> CheckScheduleTemplateCrossAsync(string merchantGuid, DateTime startDate, DateTime endDate)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            schedule_template_guid 
                            FROM
	                            t_merchant_schedule_template 
                            WHERE
	                            merchant_guid = @merchantGuid 
	                            AND ( (start_date <= @startDate AND end_date >= @startDate) OR (start_date <= @endDate AND end_date >= @endDate) ) 
                            LIMIT 1";
                return (await conn.QueryFirstOrDefaultAsync<string>(sql, new { merchantGuid, startDate = startDate.Date, endDate = endDate.Date })) != null;
            }
        }

        /// <summary>
        /// 商户批次安排服务人员排班（周期性排班）
        /// </summary>
        /// <param name="scheduleTemplateModel"></param>
        /// <param name="merchantScheduleModels"></param>
        /// <param name="merchantScheduleDetailModels"></param>
        /// <returns></returns>
        public async Task<bool> ScheduleTherapistsWorkShiftInBatchesAsync(ScheduleTemplateModel scheduleTemplateModel, List<MerchantScheduleModel> merchantScheduleModels, List<MerchantScheduleDetailModel> merchantScheduleDetailModels)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                if (string.IsNullOrEmpty(await conn.InsertAsync<string, ScheduleTemplateModel>(scheduleTemplateModel)))
                {
                    return false;
                }


                await ExcuteBatchSqlAsync(MySqlHelperExtension.CreateBatchInsertSqls(merchantScheduleModels), conn);
                await ExcuteBatchSqlAsync(MySqlHelperExtension.CreateBatchInsertSqls(merchantScheduleDetailModels), conn);

                return true;
            });
        }

        /// <summary>
        /// 批量运行sql
        /// </summary>
        /// <param name="sqls"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        private async Task<bool> ExcuteBatchSqlAsync(List<string> sqls, MySql.Data.MySqlClient.MySqlConnection conn)
        {
            if (!sqls.Any())
            {
                return true;
            }
            foreach (var sql in sqls)
            {
                await conn.ExecuteAsync(sql);
            }
            return true;

        }



        /// <summary>
        /// 商户批次修改美疗师排班（一次提交一个周期；细节性修改排班）
        /// 商户对每一天每个班次进行覆盖性排班调整
        /// </summary>
        /// <param name="scheduleTemplateModel"></param>
        /// <param name="merchantScheduleModels"></param>
        /// <param name="merchantScheduleDetailModels"></param>
        /// <returns></returns>
        public async Task<bool> ModifyTherapistsWorkShiftInBatchesAsync(ScheduleTemplateModel scheduleTemplateModel, List<MerchantScheduleModel> merchantScheduleModels, List<MerchantScheduleDetailModel> merchantScheduleDetailModels)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                var sqlDeleteDetail = "DELETE a from t_merchant_schedule_detail a inner join t_merchant_schedule b on a.schedule_guid=b.schedule_guid where b.schedule_template_guid=@scheduleTemplateGuid";
                await conn.ExecuteAsync(sqlDeleteDetail, new { scheduleTemplateGuid = scheduleTemplateModel.ScheduleTemplateGuid });

                await conn.DeleteListAsync<MerchantScheduleModel>("where schedule_template_guid=@scheduleTemplateGuid", new { scheduleTemplateGuid = scheduleTemplateModel.ScheduleTemplateGuid });

                await conn.UpdateAsync(scheduleTemplateModel);

                await ExcuteBatchSqlAsync(MySqlHelperExtension.CreateBatchInsertSqls(merchantScheduleModels), conn);
                await ExcuteBatchSqlAsync(MySqlHelperExtension.CreateBatchInsertSqls(merchantScheduleDetailModels), conn);


                return true;
            });
        }

        /// <summary>
        /// 服务人员一日批量修改排班；一次提交一天的修改
        /// </summary>
        /// <param name="scheduleTemplateGuid">排班周期guid</param>
        /// <param name="scheduleDate"></param>
        /// <param name="merchantScheduleModels"></param>
        /// <param name="merchantScheduleDetailModels"></param>
        /// <returns></returns>
        public async Task<bool> ModifyTherapistsWorkShiftDailyInBatchesAsync(string scheduleTemplateGuid, DateTime scheduleDate, List<MerchantScheduleModel> merchantScheduleModels, List<MerchantScheduleDetailModel> merchantScheduleDetailModels)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                var sqlDeleteDetail = "DELETE a from t_merchant_schedule_detail a inner join t_merchant_schedule b on a.schedule_guid=b.schedule_guid where b.schedule_template_guid=@scheduleTemplateGuid and b.schedule_date=@scheduleDate";
                await conn.ExecuteAsync(sqlDeleteDetail, new { scheduleTemplateGuid, scheduleDate = scheduleDate.Date });

                await conn.DeleteListAsync<MerchantScheduleModel>("where schedule_template_guid=@scheduleTemplateGuid and schedule_date=@scheduleDate", new { scheduleTemplateGuid, scheduleDate = scheduleDate.Date });

                await ExcuteBatchSqlAsync(MySqlHelperExtension.CreateBatchInsertSqls(merchantScheduleModels), conn);
                await ExcuteBatchSqlAsync(MySqlHelperExtension.CreateBatchInsertSqls(merchantScheduleDetailModels), conn);

                return true;
            });
        }


        /// <summary>
        /// 分页获取排班周期数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetScheduleTemplateListResponseDto> GetScheduleTemplateListAsync(GetScheduleTemplateListRequestDto requestDto)
        {

            var sql = @"SELECT
	                            a.schedule_template_guid AS ScheduleTemplateGuid,
	                            a.start_date AS StartDate,
	                            a.end_date AS EndDate,
	                            a.creation_date AS CreationDate,
	                            a.template_guid AS TemplateGuid,
	                            b.template_name AS TemplateName 
                            FROM
	                            t_merchant_schedule_template a
	                            INNER JOIN t_merchant_work_shift_template b ON a.template_guid = b.template_guid 
                            WHERE
	                            a.merchant_guid = @MerchantGuid 
                            ORDER BY
	                            StartDate DESC";
            return await MySqlHelper.QueryByPageAsync<GetScheduleTemplateListRequestDto, GetScheduleTemplateListResponseDto, GetScheduleTemplateListItemDto>(sql, requestDto);

        }

        /// <summary>
        /// 商户删除排班周期数据
        /// </summary>
        /// <param name="scheduleTemplateModel"></param>
        /// <returns></returns>
        public async Task<bool> RemoveScheduleCycleAsync(ScheduleTemplateModel scheduleTemplateModel)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                var sqlDeleteDetail = "DELETE a from t_merchant_schedule_detail a inner join t_merchant_schedule b on a.schedule_guid=b.schedule_guid where b.schedule_template_guid=@scheduleTemplateGuid";
                await conn.ExecuteAsync(sqlDeleteDetail, new { scheduleTemplateGuid = scheduleTemplateModel.ScheduleTemplateGuid });

                await conn.DeleteListAsync<MerchantScheduleModel>("where schedule_template_guid=@scheduleTemplateGuid", new { scheduleTemplateGuid = scheduleTemplateModel.ScheduleTemplateGuid });

                await conn.DeleteAsync(scheduleTemplateModel);

                return true;
            });
        }

    }
}
