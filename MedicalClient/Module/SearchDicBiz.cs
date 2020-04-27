using GD.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using GD.Models.Mall;
using System.Threading.Tasks;
using System.Linq;
using StackExchange.Redis;

namespace GD.Module
{
    /// <summary>
    /// 搜索词典业务类
    /// </summary>
    public class SearchDicBiz
    {
        /// <summary>
        /// 获取智慧云医搜索引擎热词列表Top10
        /// </summary>
        /// <returns>List<string></returns>
        public async Task<List<string>> GetSearchHotWordListAsync(string searchHotWordKey)
        {
            
            List<SearchDicModel> models = new List<SearchDicModel>();
            if (!(await RedisHelper.Database.KeyExistsAsync(searchHotWordKey)))
            {
                using (var conn = MySqlHelper.GetConnection())
                {
                    var sql = @"select * from t_utility_search_dic order by frequency desc limit 0,10";
                    models = (await conn.QueryAsync<SearchDicModel>(sql))?.ToList();
                }
                List<SortedSetEntry> sse = new List<SortedSetEntry>();
                sse.AddRange(models.Select(item => new SortedSetEntry(item.Word, Convert.ToDouble(item.Frequency))));
                if (sse.Count > 0)
                {
                    await RedisHelper.Database.SortedSetAddAsync(searchHotWordKey, sse.ToArray());
                    RedisHelper.Database.KeyExpire(searchHotWordKey, new TimeSpan(24, 0, 0));
                }
            }
            var sortedList = await RedisHelper.Database.SortedSetRangeByRankAsync(searchHotWordKey, 0, 9, Order.Descending);
            //RedisHelper.Database.ad
            return sortedList.Select(a => a.ToString()).ToList();

        }
    }
}
