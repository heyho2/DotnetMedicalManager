using Dapper;
using GD.DataAccess;
using GD.Models.Manager;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Manager.Manager
{
    /// <summary>
    /// 角色权限业务类
    /// </summary>
    public class GrantRoleBiz : BaseBiz<GrantRoleModel>
    {
        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="roleGuid"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetRoleRightAsync(string roleGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryAsync<string>("select right_guid from t_manager_grant_role where role_guid=@roleGuid", new { roleGuid });
            }
        }
        public async Task<IEnumerable<string>> GetRoleRightAsync(string[] roleGuids)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result= await conn.QueryAsync<string>("select right_guid from t_manager_grant_role where role_guid in @roleGuids", new { roleGuids });
                return result.Distinct();
            }
        }
        public async Task<bool> DeleteListAsync(string roleGuid, string[] rightGuids)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.DeleteListAsync<GrantRoleModel>("where role_guid=@roleGuid and right_guid in @rightGuids", new { roleGuid, rightGuids });
                return result > 0;
            }
        }
        public async Task<bool> SaveRoleMenuAsync(IEnumerable<GrantRoleModel> addRoleRights, string roleGuid, string[] deleteRoleRights)
        {
            return await MySqlHelper.TransactionAsync(async (conn, t) =>
            {
                if (addRoleRights.Any())
                {
                    await InsertListAsync(addRoleRights, conn);
                }
                if (deleteRoleRights.Any())
                {
                    await conn.DeleteListAsync<GrantRoleModel>("where role_guid=@roleGuid and right_guid in @deleteRoleRights", new { roleGuid, deleteRoleRights });
                }
                return true;
            });
        }
        
    }
}
