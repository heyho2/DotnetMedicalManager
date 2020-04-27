using Dapper;
using GD.DataAccess;
using GD.Models.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Health
{
    /// <summary>
    /// 健康基础信息选项
    /// </summary>
    public class HealthInformationOptionBiz : BizBase.BaseBiz<HealthInformationOptionModel>
    {
        /// <summary>
        /// 获取基础健康信息问题选项
        /// </summary>
        /// <param name="informationGuid"></param>
        /// <returns></returns>
        public async Task<List<HealthInformationOptionModel>> GetHealthInformationOptionAsync(string informationGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<HealthInformationOptionModel>("where information_guid=@informationGuid and `enable`= 1", new { informationGuid });
                return result?.ToList();
            }
        }
    }
}
