using Dapper;
using GD.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GD.Utility
{
    /// <summary>
    /// 服务单号业务类
    /// </summary>
    public class ServiceNoBiz
    {
        /// <summary>
        /// 获取服务订单号
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetServiceNo()
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"INSERT INTO t_serviceNo(Id) VALUES(NULL);SELECT @@IDENTITY";
                var result = await conn.QueryFirstOrDefaultAsync<string>(sql);
                return result;
            }
        }
    }
}
