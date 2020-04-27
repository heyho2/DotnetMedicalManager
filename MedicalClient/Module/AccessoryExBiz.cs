using Dapper;
using GD.DataAccess;
using GD.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Module
{
    /// <summary>
    /// 附件业务扩展类
    /// </summary>
    public class AccessoryExBiz
    {
        /// <summary>
        /// 批量获取model
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<AccessoryModel>> GetModelsAsync(IEnumerable<string> guids)
        {
            if (!guids.Any())
            {
                return new List<AccessoryModel>();
            }
            guids = guids.Distinct();
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<AccessoryModel>("where accessory_guid in @guids", new { guids });
                return result.AsList();
            }
        }
    }
}
