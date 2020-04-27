using Dapper;
using GD.BizBase;
using GD.DataAccess;
using GD.Models.FAQs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.FAQs
{
    /// <summary>
    /// 
    /// </summary>
    public class DoctorBalanceBiz : BaseBiz<DoctorBalanceModel>
    {
        /// <summary>
        /// 根据id获取列表
        /// </summary>
        /// <param name="ids">ids</param>
        /// <returns></returns>
        public async Task<DoctorBalanceModel> GetModelsById(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<DoctorBalanceModel>("select * from t_doctor_balance where   balance_guid= @id", new { id });
            }
        }
        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="ids">ids</param>
        /// <returns></returns>
        public async Task<List<DoctorBalanceModel>> GetModels()
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.GetListAsync<DoctorBalanceModel>(" where   ", new { })).ToList();
            }
        }

             
    }
}
