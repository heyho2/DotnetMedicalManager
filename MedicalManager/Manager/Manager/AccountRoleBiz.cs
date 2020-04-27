using Dapper;
using GD.DataAccess;
using GD.Models.Manager;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GD.Manager.Manager
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class AccountRoleBiz : BaseBiz<AccountRoleModel>
    {
        public async Task<IEnumerable<string>> GetListAsync(string userGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var roleGuids = await conn.QueryAsync<string>("select role_guid from t_manager_account_role where enable=1 and user_Guid=@userGuid", new { userGuid });
                var result = await conn.QueryAsync<string>("select role_guid from t_manager_role where enable=1 and role_guid in @roleGuids", new { roleGuids });
                return result;
            }
        }
    }
}
