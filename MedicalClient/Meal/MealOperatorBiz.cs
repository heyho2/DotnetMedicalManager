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
    /// 食堂操作员业务类
    /// </summary>
    public class MealOperatorBiz
    {
        /// <summary>
        /// 获取食堂操作员列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<GetMealOperatorListResponseDto>> GetMealOperators(GetMealOperatorListRequestDto request)
        {
            var parameters = new DynamicParameters();

            var sql = @"select 
                            operator_guid,user_name,creation_date,last_updated_date
                        from t_meal_operator where hospital_guid = @hospitalGuid";

            parameters.Add("@hospitalGuid", request.HospitalGuid);

            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<GetMealOperatorListResponseDto>(sql, parameters);

                return result;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="hospitalGuid"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task<bool> Delete(string hospitalGuid, string guid)
        {
            var parameters = new DynamicParameters();

            var sql = @"delete from t_meal_operator
                        where operator_guid = @operatorGuid and hospital_guid = @hospitalGuid";

            parameters.Add("@hospitalGuid", hospitalGuid);
            parameters.Add("@operatorGuid", guid);

            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.ExecuteAsync(sql, parameters);

                return result > 0;
            }
        }

        /// <summary>
        /// 操作员是否存在
        /// </summary>
        /// <param name="hospitalGuid"></param>
        /// <param name="operatorGuid"></param>
        /// <returns></returns>
        public async Task<bool> ExistOperator(string hospitalGuid, string operatorGuid)
        {
            var sql = @"select 1 from t_meal_operator 
                        where operator_guid = @operatorGuid and hospital_guid = @hospitalGuid";

            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.ExecuteScalarAsync(sql, new { hospitalGuid, operatorGuid });

                return (result is null) ? false : true;
            }
        }

        /// <summary>
        /// 操作员用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<bool> ExistUserName(string userName)
        {
            var sql = @"select 1 from t_meal_operator 
                        where user_name = @userName";

            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.ExecuteScalarAsync(sql, new { userName });

                return (result is null) ? false : true;
            }
        }

        /// <summary>
        /// 更新操作员密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdatePassword(MealOperatorModel model)
        {
            var saltPwd = Common.Helper.CryptoHelper.AddSalt(model.OperatorGuid, model.Password);

            var sql = @"update t_meal_operator 
                        set password = @password, last_updated_by = @lastUpdatedBy 
                        where operator_guid = @operatorGuid and hospital_guid = @hospitalGuid";

            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.ExecuteAsync(sql,
                    new
                    {
                        password = saltPwd,
                        operatorGuid = model.OperatorGuid,
                        lastUpdatedBy = model.LastUpdatedBy,
                        hospitalGuid = model.HospitalGuid
                    });

                return result > 0;
            }
        }

        /// <summary>
        /// 创建操作员账户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> CreateMealOperator(MealOperatorModel model)
        {
            model.Password = Common.Helper.CryptoHelper.AddSalt(model.OperatorGuid, model.Password);

            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.InsertAsync<string, MealOperatorModel>(model);
            }
        }

        /// <summary>
        /// 根据id获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MealOperatorModel> GetModelAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<MealOperatorModel>(id);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(MealOperatorModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.UpdateAsync(model);
                return result > 0;
            }
        }
        /// <summary>
        /// 根据id获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MealOperatorModel> GetModelAsync(string id, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<MealOperatorModel>("select * from t_meal_operator where operator_guid=@id and `enable`=@enable", new { id, enable });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""> </param>
        /// <returns></returns>
        public async Task<List<MealOperatorModel>> GetModelListByCondition(string userName)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.GetListAsync<MealOperatorModel>("  where user_name=@userName and enable=1", new { userName })).ToList();
            }
        }


    }
}
