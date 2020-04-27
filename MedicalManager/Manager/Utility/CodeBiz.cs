using Dapper;
using GD.DataAccess;
using GD.Models.Utility;
using System.Threading.Tasks;
namespace GD.Manager.Utility
{
    public class CodeBiz
    {
        const int _length = 6;
        const string _startCode = "A";

        static readonly object _lock = new object();
        static readonly object _lock2 = new object();
        static readonly object _lock3 = new object();
        static readonly object _lock4 = new object();

        static CodeBiz()
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var count = conn.QueryFirstOrDefault<int>("select count(1) from t_utility_code");
                if (count == 0)
                {
                    conn.Insert(new CodeModel
                    {
                        DictionaryMaxId = 0,
                        MealOrderMaxId = 0
                    });
                }
            }
        }
       
        /// <summary>
        /// 获取字典编码
        /// </summary>
        /// <param name="_startCode"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public string GetDictionaryCode(string _startCode = _startCode, int length = _length)
        {
            lock (_lock2)
            {
                var maxid = GetDictionaryMaxIdAsync().Result;
                var ss = UpdateDictionaryMaxIdAsync(++maxid).Result;
                return $"{_startCode}{ maxid.ToString().PadLeft(_length, '0')}";
            }
        }
        
        public async Task<int> GetDictionaryMaxIdAsync()
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<int>("select dictionary_max_id from t_utility_code LIMIT 1");
            }
        }
        public async Task<int> UpdateDictionaryMaxIdAsync(int maxid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.ExecuteAsync("update t_utility_code set dictionary_max_id =@maxid  LIMIT 1", new { maxid });
            }
        }
    }
}
