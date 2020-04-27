using Dapper;
using GD.DataAccess;
using GD.Models.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GD.Module
{
    public class CodeBiz
    {
        const int _length = 8;
        const string _startCode = "";

        static readonly object _lock = new object();

        static CodeBiz()
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var count = conn.QueryFirstOrDefault<int>("select count(1) from t_utility_code");
                if (count == 0)
                {
                    conn.Insert(new CodeModel
                    {
                        MealOrderMaxId = 0
                    });
                }
            }
        }

        public string GetMealOrderCode(string _startCode = _startCode, int length = _length)
        {
            lock (_lock)
            {
                var maxid = GetMealOrderMaxIdAsync().Result;
                var ss = UpdateMealOrderMaxIdAsync(++maxid).Result;
                return $"{_startCode}{ (maxid).ToString().PadLeft(_length, '0')}";
            }
        }

        public async Task<int> GetMealOrderMaxIdAsync()
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<int>("select meal_order_max_id from t_utility_code LIMIT 1");
            }
        }

        public async Task<int> UpdateMealOrderMaxIdAsync(int maxid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.ExecuteAsync("update t_utility_code set meal_order_max_id =@maxid  LIMIT 1", new { maxid });
            }
        }
    }
}
