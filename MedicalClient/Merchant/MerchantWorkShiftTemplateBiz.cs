using Dapper;
using GD.DataAccess;
using GD.Dtos.Merchant.Merchant;
using GD.Models.Merchant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GD.Merchant
{
    /// <summary>
    /// 班次模板业务类
    /// </summary>
    public class MerchantWorkShiftTemplateBiz
    {
        /// <summary>
        /// 获取班次模板
        /// </summary>
        /// <param name="templateId">主键id</param>
        /// <param name="enalbe"></param>
        /// <returns></returns>
        public async Task<MerchantWorkShiftTemplateModel> GetModelAsync(string templateId, bool enalbe = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = "select * from t_merchant_work_shift_template where template_guid=@templateId and `enable`=@enalbe";
                return await conn.QueryFirstOrDefaultAsync<MerchantWorkShiftTemplateModel>(sql, new { templateId, enalbe });
            }
        }

        /// <summary>
        /// 判断班次模板是否被使用过
        /// </summary>
        /// <param name="templateId">班次模板guid</param>
        /// <returns></returns>
        public async Task<bool> CheckTemplateUsed(string templateId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = "select * from t_merchant_schedule_template where template_guid=@templateId limit 1";
                return (await conn.QueryFirstOrDefaultAsync<ScheduleTemplateModel>(sql, new { templateId })) != null;
            }
        }

        /// <summary>
        /// 新增商户班次模板
        /// </summary>
        /// <param name="model">班次模板model</param>
        /// <param name="workShifts">班次model集合</param>
        /// <param name="details">班次详细model集合</param>
        /// <returns></returns>
        public async Task<bool> AddWorkShiftTemplateAsync(MerchantWorkShiftTemplateModel model, List<MerchantWorkShiftModel> workShifts, List<MerchantWorkShiftDetailModel> details)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                if (string.IsNullOrEmpty(await conn.InsertAsync<string, MerchantWorkShiftTemplateModel>(model))) return false;

                foreach (var item in workShifts)
                {
                    if (string.IsNullOrEmpty(await conn.InsertAsync<string, MerchantWorkShiftModel>(item))) return false;
                }
                foreach (var item in details)
                {
                    if (string.IsNullOrEmpty(await conn.InsertAsync<string, MerchantWorkShiftDetailModel>(item))) return false;
                }
                return true;
            });
        }

        /// <summary>
        /// 指定商户下模板是否已存在
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <param name="name"></param>
        /// <param name="templateGuid"></param>
        /// <returns></returns>
        public async Task<bool> IsExistTemplate(string merchantGuid, string name, string templateGuid = null)
        {
            var parameters = new DynamicParameters();

            var sql = @"select 1 from t_merchant_work_shift_template 
                      where merchant_guid = @merchantGuid and template_name = @name and enable = 1";

            parameters.Add("@merchantGuid", merchantGuid);
            parameters.Add("@name", name);

            if (!string.IsNullOrEmpty(templateGuid))
            {
                sql += " and template_guid <> @templateGuid";
                parameters.Add("@templateGuid", templateGuid);
            }

            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.ExecuteScalarAsync(sql, parameters);

                return (result is null) ? false : true;
            }
        }

        /// <summary>
        /// 修改商户班次模板
        /// </summary>
        /// <param name="model"></param>
        /// <param name="workShifts"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        public async Task<bool> ModifyWorkShiftTemplateAsync(MerchantWorkShiftTemplateModel model, List<MerchantWorkShiftModel> workShifts, List<MerchantWorkShiftDetailModel> details)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                var para = new { templateGuid = model.TemplateGuid };
                if (await conn.UpdateAsync(model) == 0) return false;
                var sqlDeleteDetail = "delete a from t_merchant_work_shift_detail a inner join t_merchant_work_shift b on a.work_shift_guid=b.work_shift_guid where b.template_guid=@templateGuid";
                await conn.ExecuteAsync(sqlDeleteDetail, para);

                await conn.DeleteListAsync<MerchantWorkShiftModel>("where template_guid=@templateGuid", para);

                foreach (var item in workShifts)
                {
                    if (string.IsNullOrEmpty(await conn.InsertAsync<string, MerchantWorkShiftModel>(item))) return false;
                }
                foreach (var item in details)
                {
                    if (string.IsNullOrEmpty(await conn.InsertAsync<string, MerchantWorkShiftDetailModel>(item))) return false;
                }
                return true;
            });
        }

        /// <summary>
        /// 分页获取商户班次模板
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetWorkShiftTemplatePageListResponseDto> GetWorkShiftTemplatePageListAsync(GetWorkShiftTemplatePageListRequestDto requestDto)
        {
            var sql = @"SELECT
	                        template_guid,
	                        template_name,
	                        creation_date 
                        FROM
	                        t_merchant_work_shift_template 
                        WHERE
	                        merchant_guid = @MerchantGuid 
	                        AND `enable` =1";
            return await MySqlHelper.QueryByPageAsync<GetWorkShiftTemplatePageListRequestDto, GetWorkShiftTemplatePageListResponseDto, GetWorkShiftTemplatePageListItemDto>(sql, requestDto);
        }
    }
}
