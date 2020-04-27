using Dapper;
using GD.DataAccess;
using GD.Models.Manager;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GD.Manager.Manager
{
    /// <summary>
    /// 系统菜单
    /// </summary>
    public class MenuBiz : BaseBiz<MenuModel>
    {
        /// <summary>
        /// 根据code 获取单个记录
        /// </summary>
        /// <param name="menuCode"></param>
        /// <returns></returns>
        public virtual async Task<MenuModel> GetByCodeAsync(string menuCode)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<MenuModel>("select * from t_manager_menu Where Menu_Code=@MenuCode", new { menuCode });
            }
        }
        public virtual async Task<IEnumerable<MenuModel>> GetByGuidsAsync(string[] menuGuids)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryAsync<MenuModel>("select * from t_manager_menu Where enable=1 and menu_guid  in @menuGuids", new { menuGuids });
            }
        }
    }
}
