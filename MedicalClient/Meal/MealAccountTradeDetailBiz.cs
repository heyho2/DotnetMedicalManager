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
    /// 订单交易明细guid
    /// </summary>
    public class MealAccountTradeDetailBiz
    {
        /// <summary>
        /// 通过订单guid获取
        /// </summary>
        /// <param name="orderGuid"></param>
        /// <param name="AccountTradeType">选填，消费0 或 退款1</param>
        /// <returns></returns>
        public async Task<List<MealAccountTradeDetailModel>> GetModelsByOrderGuidAsync(string orderGuid, sbyte? AccountTradeType = null)
        {
            var sqlWhere = string.Empty;
            if (AccountTradeType.HasValue)
            {
                sqlWhere = "and b.account_trade_type=@AccountTradeType";
            }
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT
	                            a.* 
                            FROM
	                            t_meal_account_trade_detail a
	                            INNER JOIN t_meal_account_trade b ON a.account_trade_guid = b.account_trade_guid 
	                            AND a.`enable` = b.`enable` 
                            WHERE
	                            b.order_guid = @orderGuid {sqlWhere}
	                            AND a.`enable` =1";
                var result = await conn.QueryAsync<MealAccountTradeDetailModel>(sql, new { orderGuid, AccountTradeType });
                return result.AsList();
            }
        }
    }
}
