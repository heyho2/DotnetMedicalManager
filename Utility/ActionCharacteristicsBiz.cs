using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using GD.Models.Utility;
using GD.DataAccess;

namespace GD.Utility
{
    /// <summary>
    /// 操作逻辑处理类
    /// </summary>
    public class ActionCharacteristicsBiz
    {
        /// <summary>
        /// 获取操作model
        /// </summary>
        /// <returns></returns>
        public async Task<List<ActionCharacteristicsModel>> GetOperateModels()
        {
            using (MySql.Data.MySqlClient.MySqlConnection conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryAsync<ActionCharacteristicsModel>("select * from t_utility_operate")).AsList();
            }
        }
    }
}
