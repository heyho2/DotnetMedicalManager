using Dapper;
using GD.DataAccess;
using GD.Dtos.Common;
using GD.Models.Consumer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GD.Manager.Consumer
{
    /// <summary>
    /// 收藏
    /// </summary>
    public class CollectionBiz : BaseBiz<CollectionModel>
    {
        public async Task<IEnumerable<CollectionCount>> GetCollectionCountAsync(IEnumerable<string> targetGuids)
        {
            var sql = $@"SELECT target_guid, count(1) FROM t_consumer_collection WHERE target_guid in @targetGuids GROUP BY target_guid";

            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryAsync<CollectionCount>(sql, new { targetGuids });
            }
        }
    }
}
