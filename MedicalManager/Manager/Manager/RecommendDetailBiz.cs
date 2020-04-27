using Dapper;
using GD.DataAccess;
using GD.Models.Manager;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Manager.Manager
{
    public class RecommendDetailBiz : BaseBiz<RecommendDetailModel>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddsAsync(List<RecommendDetailModel> models, string recommendGuid, string[] ownerGuids)
        {
            var result = await MySqlHelper.TransactionAsync(async (conn, t) =>
            {
                foreach (var item in models)
                {
                    await conn.InsertAsync<string, RecommendDetailModel>(item, t);
                }
                var aa = await conn.DeleteListAsync<RecommendDetailModel>("where owner_guid in @ownerGuids and recommend_guid=@recommendGuid", new
                {
                    recommendGuid,
                    ownerGuids
                }, t);
                return true;
            });
            return result;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> DeleteListByIdsAsync(string[] detailGuids)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.DeleteListAsync<RecommendDetailModel>("where detail_guid in @detailGuids", new { detailGuids });
            }
        }
        public async Task<int> DeleteAsync(string recommendGuid, string ownerGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.DeleteListAsync<RecommendDetailModel>("where recommend_guid = @recommendGuid and owner_Guid = @ownerGuid", new
                {
                    recommendGuid,
                    ownerGuid
                });
            }
        }
        public async Task<IEnumerable<RecommendDetailModel>> GetListAsync(string recommendGuid, string ownerGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<RecommendDetailModel>("where recommend_guid=@recommendGuid and owner_guid=@ownerGuid and enable=@enable", new
                {
                    recommendGuid,
                    ownerGuid,
                    enable = true
                });
                return result;
            }
        }
        public async Task<string[]> GetOwnerGuidsAsync(string guid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<string>("select owner_guid from t_manager_recommend_detail where recommend_guid=@guid and enable=1", new
                {
                    guid
                });
                return result.ToArray();
            }
        }
    }
}
