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
    /// 班次模板明细业务类
    /// </summary>
    public class DoctorWorkshiftDetailBiz : BizBase.BaseBiz<DoctorWorkshiftDetailModel>
    {
        /// <summary>
        /// 通过模板guid获取班次模板明细
        /// </summary>
        /// <param name="templateGuid"></param>
        /// <returns></returns>
        public async Task<List<DoctorWorkshiftDetailModel>> GetModelsByTemplateGuidAsync(string templateGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<DoctorWorkshiftDetailModel>("where template_guid=@templateGuid and `enable`=1", new { templateGuid });
                return result.ToList();
            }

        }
    }
}
