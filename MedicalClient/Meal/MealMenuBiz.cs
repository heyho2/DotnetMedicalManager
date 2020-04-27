using Dapper;
using GD.DataAccess;
using GD.Dtos.Meal.MealCanteen;
using GD.Dtos.Meal.MealClient;
using GD.Models.Meal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Meal
{
    /// <summary>
    /// 点餐菜单业务类
    /// </summary>
    public class MealMenuBiz
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(MealMenuModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.InsertAsync<string, MealMenuModel>(model);
                return !string.IsNullOrWhiteSpace(result);
            }
        }

        /// <summary>
        /// 根据id获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MealMenuModel> GetModelAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<MealMenuModel>(id);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(MealMenuModel model)
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
        public async Task<MealMenuModel> GetModelAsync(string id, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<MealMenuModel>("select * from t_meal_menu where menu_guid=@id and `enable`=@enable", new { id, enable });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""> </param>
        /// <returns></returns>
        public async Task<List<MealMenuModel>> GetModelListByOrderGuid(string categoryGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.GetListAsync<MealMenuModel>("  where category_guid=@categoryGuid and enable=1", new { categoryGuid })).ToList();
            }
        }

        /// <summary>
        /// 根据条件查询MealModel
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<List<MealMenuModel>> GetModelListByCondition(string CategoryGuid, string HospitalGuid, DateTime? Date)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sqlWhere = @"  where category_guid=@CategoryGuid and  hospital_guid=@HospitalGuid and menu_date=@Date and enable=1";
                return (await conn.GetListAsync<MealMenuModel>(sqlWhere, new { CategoryGuid, HospitalGuid, Date = Date.Value.ToString("yyyy/MM/dd") })).ToList();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.DeleteAsync<MealMenuModel>(id);
                return result > 0;
            }
        }


        /// <summary>
        /// 根据获取已选餐
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<List<GetBookMealListAsyncResponseDto>> GetBookMealListAsync(GetBookMealListAsyncRequestDto request)
        {
            string sql = $@"SELECT
	                                    dh.hos_name,
	                                    a.meal_date,
	                                    b.dishes_guid,
	                                    b.dishes_name,
	                                    a.category_guid,
	                                    a.category_name,
	                                    mc.category_advance_day,
	                                    mc.category_schedule_time,
	                                    SUM( b.quantity ) AS BookedTotal 
                                    FROM
	                                    t_meal_order AS a
	                                    LEFT JOIN t_meal_order_detail b ON a.order_guid = b.order_guid 
	                                    AND a.`enable` = b.`enable`
	                                    LEFT JOIN t_meal_category AS mc ON a.category_guid = mc.category_guid
	                                    LEFT JOIN t_doctor_hospital AS dh ON a.hospital_guid = dh.hospital_guid 
                                    WHERE
	                                    a.order_status = 'Paided' 
	                                    AND a.meal_date =@Date
	                                    AND a.hospital_guid = @HospitalGuid
                                    GROUP BY
	                                    dh.hos_name,
	                                    a.meal_date,
	                                    b.dishes_guid,
	                                    b.dishes_name,
	                                    a.category_guid,
	                                    a.category_name,
	                                    mc.category_advance_day,
	                                    mc.category_schedule_time ";
            //AND mm.ENABLE = 1
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryAsync<GetBookMealListAsyncResponseDto>(sql,
                    new {Date = request.Date.Value.ToString("yyyy/MM/dd"), request.HospitalGuid })).ToList();
            }
        }

        /// <summary>
        /// 扫码取餐
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<ScanToGetDisheAsyncResponseDto>> ScanToGetDisheAsync(ScanToGetDisheAsyncRequestDto request)
        {
            var sql = $@"  ";
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryAsync<ScanToGetDisheAsyncResponseDto>(sql, new { })).ToList();
            }
        }

        /// <summary>
        /// 获取食堂已安排菜单的日期，默认获取明天和明天以后安排的菜单日期
        /// </summary>
        /// <param name="hospitalGuid">医院guid</param>
        /// <param name="sMenuDate">起始日期</param>
        /// <returns></returns>
        public async Task<List<DateTime>> GetLatestModelsByMenuDateAsync(string hospitalGuid, DateTime sMenuDate)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<DateTime>("select distinct menu_date from t_meal_menu where hospital_guid=@hospitalGuid and `enable`=1 and menu_date>=@sMenuDate order by menu_date", new { hospitalGuid, sMenuDate });
                return result?.ToList();
            }
        }

        /// <summary>
        /// 获取某天某医院的菜单详情列表
        /// </summary>
        /// <param name="hospitalGuid"></param>
        /// <param name="menuDate"></param>
        /// <returns></returns>
        public async Task<List<MenuDetailOneDayQueryDto>> GetMenuDetailOneDayQueryAsync(string hospitalGuid, DateTime menuDate)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            a.category_guid,
	                            b.category_name,
	                            b.category_advance_day,
	                            b.category_schedule_time,
                                b.meal_startTime,
	                            b.meal_endTime,
	                            c.dishes_guid,
	                            c.dishes_name,
	                            c.dishes_internal_price,
	                            c.dishes_external_price,
	                            c.dishes_description ,
                                CONCAT(d.base_path,d.relative_path) as dishes_img
                            FROM
	                            t_meal_menu a
	                            INNER JOIN t_meal_category b ON a.category_guid = b.category_guid 
	                            AND a.`enable` = b.`enable`
	                            INNER JOIN t_meal_dishes c ON a.dishes_guid = c.dishes_guid 
	                            AND a.`enable` = c.`enable`
	                            LEFT JOIN t_utility_accessory d ON d.accessory_guid = c.dishes_img 
                            WHERE
	                            a.hospital_guid = @hospitalGuid
	                            AND c.dishes_onsale = 1 
	                            AND a.menu_date = @menuDate
	                            AND a.`enable` =1";
                var res = await conn.QueryAsync<MenuDetailOneDayQueryDto>(sql, new { hospitalGuid, menuDate = menuDate.Date });
                return res?.ToList();
            }
        }

        /// <summary>
        /// 菜品维护-查询
        /// </summary>
        /// <param name="hospitalGuid"></param>
        /// <param name="menuDate"></param>
        /// <returns></returns>
        public async Task<List<GetDisheMaintenanceAsyncResponseDto>> GetDisheMaintenanceAsync(GetDisheMaintenanceAsyncRequestDto request)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                                    md.dishes_guid,
	                                    md.dishes_name 
                                    FROM
	                                    t_meal_menu AS mm
	                                    LEFT JOIN t_meal_dishes AS md ON mm.dishes_guid = md.dishes_guid 
                                    WHERE
	                                    mm.hospital_guid = @HospitalGuid
	                                    AND mm.menu_date = @Date
	                                    AND mm.category_guid =@CategoryGuid
	                                    AND mm.ENABLE = 1 
                                    GROUP BY
	                                    md.dishes_guid,
	                                    md.dishes_name  ";

                var res = await conn.QueryAsync<GetDisheMaintenanceAsyncResponseDto>(sql, new { request.HospitalGuid, Date = request.Date.Value.ToString("yyyy/MM/dd"), request.CategoryGuid });
                return res?.ToList();

            }
        }

        /// <summary>
        /// 菜品维护-已选
        /// </summary>
        /// <returns></returns>
        public async Task<List<GetCheckedDisheMaintenanceAsyncResponseDto>> GetCheckedDisheMaintenanceAsync(GetCheckedDisheMaintenanceAsyncRequestDto request)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"";

                var res = await conn.QueryAsync<GetCheckedDisheMaintenanceAsyncResponseDto>(sql, new { });
                return res?.ToList();
            }
        }

        /// <summary>
        /// 菜品维护-添加
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AddDisheMaintenanceAsync(List<MealMenuModel> newModelList, List<MealMenuModel> oldModelList)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                //await conn.DeleteListAsync<MealMenuModel>("where item_guid in @itemIds", new { itemIds });
                foreach (var model in oldModelList)
                {
                    model.Enable = false;
                    if (await conn.UpdateAsync(model) < 1)
                    {
                        return false;
                    }
                }
                foreach (var model in newModelList)
                {
                    if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, MealMenuModel>(model)))
                    {
                        return false;
                    }
                }
                return true;
            });
        }


    }
}
