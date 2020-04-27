using Dapper;
using GD.DataAccess;
using GD.Models.Manager;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Manager
{
    public class RecommendDetailBiz
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddsAsync(List<RecommendDetailModel> models)
        {
            var result = await MySqlHelper.TransactionAsync(async (conn, t) =>
            {
                foreach (var item in models)
                {
                    if (string.IsNullOrEmpty(await conn.InsertAsync<string, RecommendDetailModel>(item, t))) { return false; }
                }
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
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int?> UpdateAsync(RecommendDetailModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.UpdateAsync(model);
            }
        }

        /// <summary>
        /// 按ID查询
        /// </summary>  
        /// <param name="dicGuid"></param>
        /// <returns></returns>
        public async Task<RecommendDetailModel> GetAsync(string guid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<RecommendDetailModel>(guid);
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
                var result = await conn.QueryAsync<string>("select owner_guid from t_manager_recommend_detail where recommend_guid=@guid and enable=@enable", new
                {
                    guid,
                    enable = true
                });
                return result.ToArray();
            }
        }
    }
}
