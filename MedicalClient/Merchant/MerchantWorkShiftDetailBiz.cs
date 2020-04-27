using Dapper;
using GD.BizBase;
using GD.DataAccess;
using GD.Dtos.Merchant.Merchant;
using GD.Models.Merchant;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Merchant
{
    /// <summary>
    /// 班次明细业务类
    /// </summary>
    public class MerchantWorkShiftDetailBiz : BaseBiz<MerchantWorkShiftDetailModel>
    {
        /// <summary>
        /// 获取店铺班次最大时间区间
        /// </summary>
        /// <param name="merchant"></param>
        /// <returns></returns>
        public async Task<TimeDto> GetMaxDuration(string merchantGuid, string templateGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            min( start_time ) AS StartTime,
	                            max( end_time ) AS EndTime 
                            FROM
	                            t_merchant_work_shift_detail a
	                            INNER JOIN t_merchant_work_shift b ON a.work_shift_guid = b.work_shift_guid 
                            WHERE
	                            merchant_guid = @merchantGuid and b.template_guid=@templateGuid
	                            AND a.`enable` = 1 
	                            AND b.`enable` = 1";
                return await conn.QueryFirstOrDefaultAsync<TimeDto>(sql, new { merchantGuid, templateGuid });
            }
        }

        /// <summary>
        /// 获取模板下班次详情
        /// </summary>
        /// <param name="templateGuid">班次模板guid</param>
        /// <returns></returns>
        public async Task<List<WorkShiftDetailsOfTemplateGuidSourceDto>> GetWorkShiftDetailsOfTemplateGuidAsync(string templateGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            a.work_shift_guid,
	                            a.work_shift_name,
	                            b.start_time,
	                            b.end_time 
                            FROM
	                            t_merchant_work_shift a
	                            INNER JOIN t_merchant_work_shift_detail b ON a.work_shift_guid = b.work_shift_guid 
	                            AND a.`enable` = b.`enable` 
                            WHERE
	                            a.template_guid = @templateGuid 
	                            AND a.`enable` =1";
                var result = await conn.QueryAsync<WorkShiftDetailsOfTemplateGuidSourceDto>(sql, new { templateGuid });
                return result?.ToList();

            }
        }

        /// <summary>
        /// 通过模板guid获取班次明细
        /// </summary>
        /// <param name="templateId">模板guid</param>
        /// <returns></returns>
        public async Task<List<MerchantWorkShiftDetailModel>> GetModelsByTemplateGuidAsync(string templateId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            b.* 
                            FROM
	                            t_merchant_work_shift a
	                            INNER JOIN t_merchant_work_shift_detail b ON a.work_shift_guid = b.work_shift_guid 
	                            AND a.`enable` = b.`enable` 
                            WHERE
	                            a.template_guid = @templateId 
	                            AND a.`enable` =1";
                var result = await conn.QueryAsync<MerchantWorkShiftDetailModel>(sql, new { templateId });
                return result?.ToList();
            }
        }
    }
}
