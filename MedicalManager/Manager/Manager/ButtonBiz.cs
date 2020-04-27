using Dapper;
using GD.DataAccess;
using GD.Models.Manager;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GD.Manager.Manager
{
    /// <summary>
    /// 系统按钮
    /// </summary>
    public class ButtonBiz : BaseBiz<ButtonModel>
    {
        public override async Task<bool> DeleteAsync(string id)
        {
            //t_manager_grant_role
            var result = await MySqlHelper.TransactionAsync(async (conn, t) =>
            {
                await conn.DeleteAsync<ButtonModel>(id, t);
                await conn.DeleteListAsync<GrantRoleModel>("where right_guid=@id", new { id }, t);
                return true;
            });
            return result;
        }
        /// <summary>
        /// 根据code 获取单个记录
        /// </summary>
        /// <param name="menuCode"></param>
        /// <returns></returns>
        public async Task<ButtonModel> GetByCodeAsync(string buttonCode)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<ButtonModel>("select * from t_manager_button Where button_code=@buttonCode", new { buttonCode });
            }
        }
        public async Task<bool> AnyByCodeAsync(string buttonCode, string menuGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryFirstOrDefaultAsync<int?>("select count(1) from t_manager_button Where button_code=@buttonCode and  menu_guid=@menuGuid", new { buttonCode, menuGuid });
                return (result ?? 0) > 0;
            }
        }

        public async Task<IEnumerable<ButtonModel>> GetListAsync(string menuGuid, bool? enable = null)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sqlWhere = "1=1 AND menu_guid=@menuGuid";
                if (enable != null)
                {
                    sqlWhere = $"{sqlWhere} AND {nameof(ButtonModel.Enable)}={(enable.Value ? "1" : "0")}";
                }
                return await conn.QueryAsync<ButtonModel>($"select button_guid,menu_guid,button_code,button_name,sort from t_manager_button Where {sqlWhere}", new { menuGuid });
            }
        }
        public virtual async Task<IEnumerable<ButtonModel>> GetByGuidsAsync(string[] buttonGuids)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryAsync<ButtonModel>("select * from t_manager_button Where enable=1 and button_guid  in @buttonGuids", new { buttonGuids });
            }
        }
    }
}
