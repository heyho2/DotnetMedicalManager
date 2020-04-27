using Dapper;
using GD.DataAccess;
using GD.Models.Meal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GD.Meal
{
    /// <summary>
    /// 点餐订单明细业务类
    /// </summary>
    public class MealOrderDetailBiz
    {
        /// <summary>
        /// 通过订单guid获取models
        /// </summary>
        /// <param name="orderGuid"></param>
        /// <returns></returns>
        public async Task<List<MealOrderDetailModel>> GetModelsByOrderGuidAsync(string orderGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<MealOrderDetailModel>("where order_guid=@orderGuid and `enable`=1", new { orderGuid });
                return result.AsList();
            }
        }
    }
}
