using Dapper;
using GD.Common.Base;
using GD.DataAccess;
using GD.Models.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Module
{
    public class UserExBiz
    {
        public virtual async Task<IEnumerable<UserModel>> GetListAsync(IEnumerable<string> guids, bool? enable = null)
        {
            if (!guids.Any())
            {
                return new List<UserModel>();
            }
            guids = guids.Distinct();
            using (var conn = MySqlHelper.GetConnection())
            {
                var sqlWhere = $"1=1 AND user_guid in @guids";
                if (enable != null)
                {
                    sqlWhere = $"{sqlWhere} AND {nameof(BaseModel.Enable)}={(enable.Value ? "1" : "0")}";
                }
                var result = await conn.GetListAsync<UserModel>($"WHERE {sqlWhere}", new { guids });
                return result;
            }
        }
    }
}
