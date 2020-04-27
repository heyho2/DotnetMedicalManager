using Dapper;
using GD.DataAccess;
using GD.Models.Utility;
using System.Threading.Tasks;

namespace GD.Manager.Utility
{
    public class RichtextBiz
    {
        /// <summary>
        /// 按ID查询
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task<RichtextModel> GetAsync(string guid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<RichtextModel>(guid);
            }
        }
    }
}
