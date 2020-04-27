using Dapper;
using GD.DataAccess;
using GD.Dtos.Meal.MealAdmin;
using GD.Models.Meal;
using System.Threading.Tasks;

namespace GD.Meal
{
    /// <summary>
    /// 管理员相关操作
    /// </summary>
    public class MealAdminBiz
    {
        /// <summary>
        /// 获取管理员信息
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<MealAdminModel> GetMealAdmin(MealAdminLoginRequestDto requestDto)
        {
            var model = (MealAdminModel)null;

            var sql = @"select 
                            admin_guid,admin_name, password, hos_name, hospital_guid 
                        from t_meal_admin
                        where admin_name = @name and enable = 1";

            using (var conn = MySqlHelper.GetConnection())
            {
                model = await conn.QueryFirstOrDefaultAsync<MealAdminModel>(sql, new { name = requestDto.UserName });
            }

            if (model is null)
            {
                return null;
            }

            var saltPwd = Common.Helper.CryptoHelper.AddSalt(model.AdminGuid, requestDto.Password);

            if (!saltPwd.Equals(model.Password))
            {
                return null;
            }
            return model;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<bool> UpdateMealAdminPassword(string userId, string newPassword)
        {
            var saltPwd = Common.Helper.CryptoHelper.AddSalt(userId, newPassword);

            var sql = "update t_meal_admin set password = @password where admin_guid = @admin_guid";

            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.ExecuteAsync(sql, new { admin_guid = userId, password = saltPwd });

                return result > 0;
            }
        }
    }
}
