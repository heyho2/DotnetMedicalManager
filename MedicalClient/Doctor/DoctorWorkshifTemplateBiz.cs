using Dapper;
using GD.DataAccess;
using GD.Models.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Doctor
{
    /// <summary>
    /// 班次模板业务类
    /// </summary>
    public class DoctorWorkshifTemplateBiz : BizBase.BaseBiz<DoctorWorkshifTemplateModel>
    {
        /// <summary>
        /// 通过医院guid获取班次模板
        /// </summary>
        /// <returns></returns>
        public async Task<List<DoctorWorkshifTemplateModel>> GetModelsByHospitalGuidAsync(string hospitalGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<DoctorWorkshifTemplateModel>("where hospital_guid=@hospitalGuid and `enable`=1", new { hospitalGuid });
                return result.ToList();
            }
        }

        /// <summary>
        /// 新增或编辑班次模板
        /// </summary>
        /// <param name="model"></param>
        /// <param name="detailModels"></param>
        /// <param name="isCreate"></param>
        /// <returns></returns>
        public async Task<bool> CreateEditWorkshiftTemplateAsync(DoctorWorkshifTemplateModel model, List<DoctorWorkshiftDetailModel> detailModels, bool isCreate)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                if (isCreate)
                {
                    await conn.InsertAsync<string, DoctorWorkshifTemplateModel>(model);
                }
                else
                {
                    await conn.UpdateAsync(model);
                }
                await conn.DeleteListAsync<DoctorWorkshiftDetailModel>("where template_guid=@TemplateGuid", new { model.TemplateGuid });
                detailModels.InsertBatch(conn);
                return true;
            });
        }
    }
}
