using Dapper;
using GD.DataAccess;
using GD.Dtos.Meal.MealAdmin;
using GD.Models.Meal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Meal
{
    /// <summary>
    /// 点餐钱包账户业务类
    /// </summary>
    public class MealAccountBiz
    {
        /// <summary>
        /// 通过用户guid获取models
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task<List<MealAccountModel>> GetModelsByUserIdAsync(string userId, string hospitalGuid, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<MealAccountModel>("where user_guid=@userId and hospital_guid=@hospitalGuid and `enable`=@enable", new { userId, hospitalGuid, enable });
                return result?.ToList();
            }
        }

        /// <summary>
        /// 获取指定医院用户钱包账户列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetMealAccountListResponseDto<MealAccountItem>> GetMealAccounts(GetMealAccountListRequestDto requestDto)
        {
            var response = new GetMealAccountListResponseDto<MealAccountItem>();

            var result = (IEnumerable<MealAccountItem>)null;

            var sql = $@"SELECT 	
                            a.user_guid,
						    u.user_name,
	                        u.phone,
                            a.user_type,
							a.`enable`,
							a.account_type,
							a.account_balance
                        FROM t_meal_account as a
	                        INNER JOIN t_utility_user as u on u.user_guid  = a.user_guid
                        WHERE a.hospital_guid = '{requestDto.HospitalGuid}'";

            using (var conn = MySqlHelper.GetConnection())
            {
                if (!string.IsNullOrEmpty(requestDto.Phone))
                {
                    sql += $" and u.phone = '{requestDto.Phone}'";

                    sql += " ORDER BY a.creation_date DESC";

                    result = await conn.QueryAsync<MealAccountItem>(sql);

                    if (result is null || result.Count() <= 0)
                    {
                        sql = $"SELECT user_guid,phone, user_name FROM t_utility_user where phone = '{requestDto.Phone}' and `enable` = 1";

                        result = await conn.QueryAsync<MealAccountItem>(sql);
                    }
                }
                else
                {
                    sql += " ORDER BY a.creation_date DESC";

                    result = await conn.QueryAsync<MealAccountItem>(sql);
                }
            }

            if (result is null || result.Count() <= 0)
            {
                return response;
            }

            var accounts = result.GroupBy(d => d.UserGuid).ToList();

            var items = new List<MealAccountItem>();

            foreach (var account in accounts)
            {
                var accountItem = new MealAccountItem();

                var first = account.FirstOrDefault();

                accountItem.UserGuid = first.UserGuid;
                accountItem.UserName = first.UserName;
                accountItem.UserType = first.UserType;
                accountItem.Enable = first.Enable;
                accountItem.Phone = first.Phone;

                foreach (var mealAccount in account.Distinct())
                {
                    if (mealAccount.AccountType == MealAccountTypeEnum.Recharge)
                    {
                        accountItem.RechargeBalance = mealAccount.Accountbalance;
                    }
                    else if (mealAccount.AccountType == MealAccountTypeEnum.Grant
                        && mealAccount.Enable)
                    {
                        accountItem.GrantBalance = mealAccount.Accountbalance;
                    }
                }

                items.Add(accountItem);
            }

            var total = items.Count();

            response.CurrentPage = items.Skip((requestDto.PageIndex - 1) * requestDto.PageSize).Take(requestDto.PageSize);

            response.Total = total;

            return response;
        }

        /// <summary>
        /// 获取指定用户账户信息
        /// </summary>
        /// <param name="hospitalGuid"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MealAccountModel>> GetAccountTypes(string hospitalGuid, string userGuid)
        {
            var parameters = new DynamicParameters();

            var sql = @"select *
                        from t_meal_account 
                        where hospital_guid = @hospitalGuid and user_guid = @userGuid";

            parameters.Add("@hospitalGuid", hospitalGuid);
            parameters.Add("@userGuid", userGuid);

            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<MealAccountModel>(sql, parameters);

                return result;
            }
        }

        /// <summary>
        /// 获取指定医院下所有赠款账户
        /// </summary>
        /// <param name="hospitalGuid"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MealAccountModel>> GetGrantAccounts(string hospitalGuid)
        {
            var parameters = new DynamicParameters();

            var sql = @"select *
                        from t_meal_account 
                        where hospital_guid = @hospitalGuid and user_type = 'Internal'
                        and account_type = 'Grant' and enable = 1";

            parameters.Add("@hospitalGuid", hospitalGuid);

            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<MealAccountModel>(sql, parameters);

                return result;
            }
        }


        /// <summary>
        /// 获取批量用户用户账户信息
        /// </summary>
        /// <param name="hospitalGuid"></param>
        /// <param name="userGuids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MealAccountModel>> GetAccountTypes(string hospitalGuid,
            List<string> userGuids)
        {
            var guids = string.Join(",", userGuids.Select(x => "'" + x + "'").ToArray());

            var sql = $@"select *
                        from t_meal_account 
                        where hospital_guid = '{hospitalGuid}' and user_guid in ({guids})";

            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<MealAccountModel>(sql);

                return result;
            }
        }

        /// <summary>
        /// 创建账户
        /// </summary>
        /// <param name="accounts"></param>
        /// <returns></returns>
        public async Task<bool> CreateAccount(List<MealAccountModel> accounts)
        {
            var sql = @"insert into t_meal_account(account_guid,user_guid,hospital_guid,user_type,
                    account_type,account_balance,enable,created_by,last_updated_by,org_guid) values
                (@AccountGuid,@UserGuid,@HospitalGuid,@UserType,
                    @AccountType,@AccountBalance,@Enable,@CreatedBy,@LastUpdatedBy,@OrgGuid)";

            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.ExecuteAsync(sql, accounts) > 0;
            }
        }

        /// <summary>
        /// 批量创建用户钱包账户
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<bool> CreateAccount(MySql.Data.MySqlClient.
            MySqlConnection conn, List<MealAccountModel> accounts)
        {
            var sql = @"insert into t_meal_account
                        (account_guid,
                        user_guid,
                        hospital_guid,
                        user_type,
                        account_type,
                        account_balance,
                        enable,
                        created_by,
                        last_updated_by,
                        org_guid) values
                        (@AccountGuid,
                        @UserGuid,
                        @HospitalGuid,
                        @UserType,
                        @AccountType,
                        @AccountBalance,
                        @Enable,    
                        @CreatedBy,
                        @LastUpdatedBy,
                        @OrgGuid)";

            return await conn.ExecuteAsync(sql, accounts) > 0;
        }

        /// <summary>
        /// 批量更新用户钱包账户信息
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="accounts"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAccounts(MySql.Data.MySqlClient.
            MySqlConnection conn, List<MealAccountModel> accounts)
        {
            var sql = @"update t_meal_account 
                        set account_balance = @AccountBalance,
                            user_type = @UserType,
                            last_updated_by = @LastUpdatedBy,
                            enable = @Enable,
                            account_balance = @AccountBalance
                        where account_guid = @AccountGuid";

            return await conn.ExecuteAsync(sql, accounts) > 0;
        }

        /// <summary>
        /// 批量创建用户钱包账户明细
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="accountDetails"></param>
        /// <returns></returns>
        public async Task<bool> CreateAccountDetails(MySql.Data.MySqlClient.
           MySqlConnection conn, List<MealAccountDetailModel> accountDetails)
        {
            var sql = @"insert into t_meal_account_detail 
                        (account_detail_guid,
                        account_guid,
                        account_detail_type,
                        account_detail_income_type,
                        account_detail_before_fee,
                        account_detail_fee,
                        account_detail_after_fee,
                        account_detail_description,
                        created_by,
                        last_updated_by,
                        org_guid) 
                        values
                        (@AccountDetailGuid,
                        @AccountGuid,
                        @AccountDetailType,
                        @AccountDetailIncomeType,
                        @AccountDetailBeforeFee,
                        @AccountDetailFee,
                        @AccountDetailAfterFee,
                        @AccountDetailDescription,
                        @CreatedBy,
                        @LastUpdatedBy,
                        @OrgGuid)";

            return await conn.ExecuteAsync(sql, accountDetails) > 0;
        }

        /// <summary>
        /// 更新账户状态（将个人账户和赠款账户同时禁用或启用）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAccountStatus(ModifyMealAccountRequestDto requestDto)
        {
            var parameters = new DynamicParameters();

            var sql = @"update t_meal_account
                      set  `enable` =  (IF(`enable` = 0, 1, 0))
                      where hospital_guid = @hospitalGuid and user_guid = @userGuid";

            parameters.Add("@hospitalGuid", requestDto.HospitalGuid);
            parameters.Add("@userGuid", requestDto.UserGuid);

            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.ExecuteAsync(sql, parameters);

                return result > 0;
            }
        }

        /// <summary>
        /// 更新用户账户身份
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAccountIdentity(List<MealAccountModel> addModels,
            List<MealAccountModel> updateModels, MealAccountDetailModel addAccountDetailModel)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                // 添加账户信息
                if (addModels != null && addModels.Count > 0)
                {
                    await CreateAccount(conn, addModels);
                }

                // 更新账户状态
                if (updateModels != null)
                {
                    await UpdateAccounts(conn, updateModels);
                }

                // 添加账户清零明细
                if (addAccountDetailModel != null)
                {
                    await conn.InsertAsync<string, MealAccountDetailModel>(addAccountDetailModel);
                }

                return true;
            });
        }

        /// <summary>
        /// 个人账户/赠款账户充值或扣减
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AccountRechargeOrDeduction(List<MealAccountModel> addAccountModels,
           List<MealAccountModel> updateAccountModels, List<MealAccountDetailModel> accountDetailModels)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                // 添加账户信息
                if (addAccountModels != null && addAccountModels.Count() > 0)
                {
                    await CreateAccount(conn, addAccountModels);
                }

                // 更新账户余额
                if (updateAccountModels != null && updateAccountModels.Count() > 0)
                {
                    await UpdateAccounts(conn, updateAccountModels);
                }

                // 添加赠款账户充值或扣减明细
                if (accountDetailModels != null && accountDetailModels.Count() > 0)
                {
                    await CreateAccountDetails(conn, accountDetailModels);
                }

                return true;
            });
        }
    }
}
