using Dapper;
using GD.DataAccess;
using GD.Dtos.Meal.MealAdmin;
using GD.Dtos.Meal.MealCanteen;
using GD.Models.Meal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Meal
{
    /// <summary>
    /// 点餐餐别业务类
    /// </summary>
    public class MealCategoryBiz
    {
        /// <summary>
        /// 获取唯一model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MealCategoryModel> GetModelAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<MealCategoryModel>(id);
            }
        }

        /// <summary>
        /// 通过主键集合获取models
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<MealCategoryModel>> GetModelsByIdsAsync(List<string> ids)
        {
            if (!ids.Any())
            {
                return new List<MealCategoryModel>();
            }
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<MealCategoryModel>("where category_guid in @ids", new { ids });
                return result?.ToList();
            }
        }

        /// <summary>
        /// 类别是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<bool> ExistCategoryName(string hospitalGuid, string name,
            string categoryGuid = null)
        {
            var parameters = new DynamicParameters();

            var sql = @"select 1 from t_meal_category 
                      where hospital_guid = @hospitalGuid and category_name = @name and enable = 1";

            parameters.Add("@hospitalGuid", hospitalGuid);
            parameters.Add("@name", name);

            if (!string.IsNullOrEmpty(categoryGuid))
            {
                sql += " and category_guid <> @categoryGuid";
                parameters.Add("@categoryGuid", categoryGuid);
            }

            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.ExecuteScalarAsync(sql, parameters);

                return (result is null) ? false : true;
            }
        }

        /// <summary>
        /// 创建餐别
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task<string> CreateCategory(MealCategoryModel category)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.InsertAsync<string, MealCategoryModel>(category);
            }
        }

        /// <summary>
        /// 更新餐别
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> UpdateCategory(MealCategoryModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.UpdateAsync(model);
            }
        }

        /// <summary>
        /// 根据医院Id和主键获取指定餐别
        /// </summary>
        /// <param name="hospitalGuid"></param>
        /// <param name="categoryGuid"></param>
        /// <returns></returns>
        public async Task<MealCategoryModel> GetCategoryModelById(string hospitalGuid,
            string categoryGuid)
        {
            var sql = @"select *
                        from t_meal_category 
                        where category_guid = @categoryGuid and hospital_guid = @hospitalGuid";

            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<MealCategoryModel>(sql, new { categoryGuid, hospitalGuid });
            }
        }

        /// <summary>
        /// 校验订单是否已存在待删除餐别
        /// </summary>
        /// <param name="categoryGuid"></param>
        /// <returns></returns>
        public async Task<bool> ExistOrder(string categoryGuid)
        {
            var sql = "select 1 from t_meal_order where category_guid = @categoryGuid";

            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.ExecuteScalarAsync(sql, new { categoryGuid });

                return (result is null) ? false : true;
            }
        }

        /// <summary>
        /// 删除餐别
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <param name="categoryGuid"></param>
        /// <returns></returns>
        public async Task<bool> DeleteCategory(MealCategoryModel model)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                await conn.DeleteListAsync<MealMenuModel>("where category_guid = @categoryGuid", new { model.CategoryGuid });

                await UpdateCategory(model);

                return true;
            });
        }

        /// <summary>
        /// 获取餐别列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<List<MealCategoryModel>> GetMealCategories(GetCategoryListRequestDto requestDto)
        {
            var sql = @"where hospital_guid = @HospitalGuid and enable = 1";
            var parameters = new DynamicParameters();
            parameters.Add("@HospitalGuid", requestDto.HospitalGuid);

            if (!string.IsNullOrEmpty(requestDto.CategoryName))
            {
                sql += " and category_name LIKE @categoryName";
                parameters.Add("@categoryName", "%" + requestDto.CategoryName + "%");
            }

            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.GetListAsync<MealCategoryModel>(sql, parameters)).AsList();
            }
        }

        /// <summary>
        /// 获取分类List
        /// </summary>
        /// <returns></returns>
        public async Task<List<GetCategoryListAsyncResponseDto>> GetCategoryListAsync(GetCategoryListAsyncRequestDto request)
        {
            var sql = @"SELECT DISTINCT
	                                category_guid,
	                                category_name 
                                FROM
	                                t_meal_category 
                                WHERE
	                                hospital_guid =@HospitalGuid
	                                AND ENABLE = 1 ";

            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryAsync<GetCategoryListAsyncResponseDto>(sql, new { request.HospitalGuid })).ToList();
            }
        }
        /// <summary>
        /// 获取分类List
        /// </summary>
        /// <returns></returns>
        public async Task<List<GetUseFullDateListAsyncResponseDto>> GetUseFullDateListAsync(GetUseFullDateListAsyncRequestDto request)
        {
            string sqlWhere = "";
            if (request.IsShowNullDate)
            {
                sqlWhere += " AND ENABLE = 1 ";
            }
            var date = DateTime.Now.AddDays(-2).Date;
            var sql = $@"SELECT DISTINCT
	                                menu_date 
                                FROM
	                                t_meal_menu 
                                WHERE
	                                hospital_guid = @HospitalGuid
	                                AND menu_date>=@date
                                    {sqlWhere}
                                GROUP BY
	                                menu_date ";
            //AND ENABLE = 1
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryAsync<GetUseFullDateListAsyncResponseDto>(sql, new { request.HospitalGuid,date })).ToList();
            }
        }
        

    }
}
