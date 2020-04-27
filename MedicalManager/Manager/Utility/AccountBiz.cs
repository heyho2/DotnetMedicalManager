using GD.DataAccess;
using GD.Models.Manager;
using System.Collections.Generic;

namespace GD.Manager.Utility
{
    public class AccountBiz : BaseBiz<AccountModel>
    {
        /// <summary>
        /// 获取管理员信息
        /// </summary>
        /// <param name="account">管理员账号</param>
        /// <returns></returns>
        public IEnumerable<AccountModel> GetAdministrator(string account, bool enable = true)
        {
            const string sql = "select * from t_manager_account where account = @account and enable = @enable";
            return MySqlHelper.Select<AccountModel>(sql, new { account, enable });
        }
    }
}
