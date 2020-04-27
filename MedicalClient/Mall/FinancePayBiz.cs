using Dapper;
using GD.DataAccess;
using GD.Models.Mall;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GD.Mall
{
    /// <summary>
    /// 支付记录表操作
    /// </summary>
    public class FinancePayBiz
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(FinancePayModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.InsertAsync<string, FinancePayModel>(model);
                return !string.IsNullOrWhiteSpace(result);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(FinancePayModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.UpdateAsync(model);
                return result > 0;
            }
        }

        /// <summary>
        /// 根据trade_no获取model
        /// </summary>
        /// <param name="id"></param>
        /// <param enable="enable"></param>
        /// <returns></returns>
        public async Task<FinancePayModel> GetModelAsyncByTradeNo(string id, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<FinancePayModel>("select * from t_mall_finance_pay where trade_no=@id and `enable`=@enable", new { id, enable });
            }
        }

        /// <summary>
        /// 根据id获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FinancePayModel> GetModelAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<FinancePayModel>(id);
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
                var result = await conn.DeleteAsync<FinancePayModel>(id);
                return result > 0;
            }
        }
    }
}
