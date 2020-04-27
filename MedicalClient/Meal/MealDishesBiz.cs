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
    /// 点餐产品业务类
    /// </summary>
    public class MealDishesBiz
    {
        /// <summary>
        /// 通过主键集合获取models
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<MealDishesModel>> GetModelsByIdsAsync(List<string> ids)
        {
            if (!ids.Any())
            {
                return new List<MealDishesModel>();
            }
            using (var conn = MySqlHelper.GetConnection())
            {

                var result = await conn.GetListAsync<MealDishesModel>("where dishes_guid in @ids", new { ids });
                return result?.ToList();
            }
        }

        /// <summary>
        /// 获取菜品分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetMealDishesListResponseDto> GetMealDishes(GetMealDishesListRequestDto requestDto)
        {
            var sql = $@"select 
                            d.dishes_guid,d.dishes_name,
                            concat(acc.base_path, acc.relative_path) as dishes_img_path,
                            d.dishes_img,
                            d.dishes_internal_price,d.dishes_external_price,
                            d.dishes_description,d.dishes_onsale
                        from t_meal_dishes as d
                        left join t_utility_accessory AS acc on d.dishes_img = acc.accessory_guid 
                        where d.hospital_guid = '{requestDto.HospitalGuid}'";

            if (!string.IsNullOrEmpty(requestDto.DishesName))
            {
                sql += $" and d.dishes_name like '%{requestDto.DishesName}%'";
            }
            sql += " order by d.creation_date desc";

            return await MySqlHelper.QueryByPageAsync<GetMealDishesListRequestDto, GetMealDishesListResponseDto, GetMealDishesItem>(sql, requestDto);
        }

        /// <summary>
        /// 菜品是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<bool> ExistDishesName(string hospitalGuid, string name,
            string dishesGuid = null)
        {
            var parameters = new DynamicParameters();

            var sql = @"select 1 from t_meal_dishes 
                      where hospital_guid = @hospitalGuid and dishes_name = @name";

            parameters.Add("@hospitalGuid", hospitalGuid);
            parameters.Add("@name", name);

            if (!string.IsNullOrEmpty(dishesGuid))
            {
                sql += " and dishes_guid <> @dishesGuid";
                parameters.Add("@dishesGuid", dishesGuid);
            }

            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.ExecuteScalarAsync(sql, parameters);

                return (result is null) ? false : true;
            }
        }

        /// <summary>
        /// 创建菜品
        /// </summary>
        /// <param name="dish"></param>
        /// <returns></returns>
        public async Task<string> CreateDish(MealDishesModel dish)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.InsertAsync<string, MealDishesModel>(dish);
            }
        }

        /// <summary>
        /// 更新菜品信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> UpdateDish(MealDishesModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.UpdateAsync(model);
            }
        }

        /// <summary>
        /// 根据医院Id和主键获取指定菜品
        /// </summary>
        /// <param name="hospitalGuid"></param>
        /// <param name="dishesGuid"></param>
        /// <returns></returns>
        public async Task<MealDishesModel> GetMealDishesModelById(string hospitalGuid,
            string dishesGuid)
        {
            var sql = @"select *
                        from t_meal_dishes 
                        where dishes_guid = @dishesGuid and hospital_guid = @hospitalGuid";

            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<MealDishesModel>(sql, new { dishesGuid, hospitalGuid });
            }
        }

        /// <summary>
        /// 通过主键集合获取models
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<MealDishesModel>> GetModelsByIdArrAndHospitalGuidAsync(string[] disheArr,string hospitalGuid)
        {
            if (!disheArr.Any())
            {
                return new List<MealDishesModel>();
            }
            using (var conn = MySqlHelper.GetConnection())
            {

                var result = await conn.GetListAsync<MealDishesModel>(" where dishes_guid in @disheArr and hospital_guid = @hospitalGuid  and dishes_onsale=1", new { disheArr, hospitalGuid });
                return result?.ToList();
            }
        }
        /// <summary>
        /// 通过主键集合获取models
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<MealDishesModel>> GetModelsByHospitalGuidAsync(string hospitalGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<MealDishesModel>("  where  hospital_guid = @hospitalGuid  and dishes_onsale=1 ", new {  hospitalGuid });
                return result?.ToList();
            }
        }
    }
}
