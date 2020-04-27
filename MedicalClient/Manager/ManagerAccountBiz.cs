using GD.Models.Manager;
using Dapper;
using GD.DataAccess;
using System.Threading.Tasks;
using GD.Dtos.Admin.User;
using System;
using System.Collections.Generic;
using System.Linq;
using GD.Common.Helper;

namespace GD.Manager
{
    /// <summary>
    /// 账号
    /// </summary>
    public class ManagerAccountBiz
    {

        public async Task<AccountModel> GetAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<AccountModel>(id);
            }
        }
        public async Task<AccountModel> GetByAccountAsync(string account)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<AccountModel>("where Account=@Account", new { account });
                return result.FirstOrDefault();
            }
        }
        public async Task<List<AccountModel>> GetModelsAsync(string account, string phone)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<AccountModel>("where Account=@Account or Phone=@Phone", new { account, phone });
                return result.ToList();
            }
        }
        public async Task<List<AccountModel>> GetModelsAsync(string userGuid, string account, string phone)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<AccountModel>("where user_Guid<>@userGuid and (Account=@Account or Phone=@Phone)", new { account, phone, userGuid });
                return result.ToList();
            }
        }
        /// <summary>
        /// 获取用户信息列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetAccountListResponseDto> GetAccountListAsync(GetGetAccountListRequestDto request)
        {
            var whereSql = "1=1";
            if (!string.IsNullOrWhiteSpace(request.Account))
            {
                whereSql = $"{whereSql} AND Account=@Account";
            }
            if (!string.IsNullOrWhiteSpace(request.Phone))
            {
                whereSql = $"{whereSql} AND Phone=@Phone";
            }
            var sql = $@"
SELECT * FROM (
	SELECT
	a.*,
	b.org_name AS OrganizationName 
FROM
	t_manager_account a
	LEFT JOIN t_manager_organization b ON a.organization_guid = b.org_guid
) T 
WHERE {whereSql}
	ORDER BY creation_date
";
            return await MySqlHelper.QueryByPageAsync<GetGetAccountListRequestDto, GetAccountListResponseDto, GetAccountListItemDto>(sql, request);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="accountModel"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(AccountModel accountModel, List<AccountRoleModel> accountRoleModels)
        {
            var result = await MySqlHelper.TransactionAsync(async (conn, t) =>
            {
                await conn.InsertAsync<string, AccountModel>(accountModel);
                foreach (var item in accountRoleModels)
                {
                    await conn.InsertAsync<string, AccountRoleModel>(item);
                }
                return true;
            });
            return result;
        }
        public async Task<bool> UpdateAsync(AccountModel accountModel)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.UpdateAsync(accountModel);
                return result > 0;
            }
        }
        public async Task<bool> UpdateAsync(AccountModel accountModel, List<AccountRoleModel> accountRoleModels)
        {
            var result = await MySqlHelper.TransactionAsync(async (conn, t) =>
            {
                await conn.UpdateAsync(accountModel);
                await conn.DeleteListAsync<AccountRoleModel>("where user_guid=@UserGuid", new { accountModel.UserGuid });
                foreach (var item in accountRoleModels)
                {
                    await conn.InsertAsync<string, AccountRoleModel>(item);
                }
                return true;
            });
            return result;
        }
    }
}
