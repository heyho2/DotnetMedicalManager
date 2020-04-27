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
    /// 健康指标选项业务类
    /// </summary>
    public class HealthIndicatorOptionBiz : BizBase.BaseBiz<HealthIndicatorOptionModel>
    {
        /// <summary>
        /// 查找对应的健康指标选项
        /// </summary>
        /// <param name="indicatorOptionGuid"></param>
        /// <returns></returns>
        public async Task<List<HealthIndicatorOptionModel>> GetHealthIndicatorOptionAsync(string indicatorOptionGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<HealthIndicatorOptionModel>("where indicator_guid=@indicatorOptionGuid", new { indicatorOptionGuid });
                return result?.ToList();
            }
        }
    }
}
