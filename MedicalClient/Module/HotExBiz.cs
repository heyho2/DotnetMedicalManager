using Dapper;
using GD.DataAccess;
using GD.Models.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GD.Module
{
    public class HotExBiz
    {
        /// <summary>
        /// 获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<HotModel> GetModelAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<HotModel>(id);
            }
        }

        /// <summary>
        /// 更新hot表收藏量
        /// </summary>
        /// <param name="targetGuid"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        public async Task<bool> UpdateCollectTotalAsync(string targetGuid, bool isAdd = true)
        {
            try
            {
                var modifyStr = isAdd ? "+1" : "-1";
                using (var conn = MySqlHelper.GetConnection())
                {
                    var sql = $"insert into t_utility_hot (owner_guid,collect_count) values('{targetGuid}',1)  ON DUPLICATE KEY UPDATE collect_count=collect_count{modifyStr},last_updated_date=NOW();";
                    var result = await conn.ExecuteAsync(sql);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Common.Helper.Logger.Error($"更新hot表收藏量失败 at {nameof(HotExBiz)}.{nameof(UpdateCollectTotalAsync)}:{Environment.NewLine} {ex.Message}");
                return false;
            }

        }

        /// <summary>
        /// 更新hot表点赞量
        /// </summary>
        /// <param name="targetGuid"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        public async Task<bool> UpdateLikeTotalAsync(string targetGuid, bool isAdd = true)
        {
            try
            {
                var modifyStr = isAdd ? "+1" : "-1";
                using (var conn = MySqlHelper.GetConnection())
                {
                    var sql = $"insert into t_utility_hot (owner_guid,like_count) values('{targetGuid}',1)  ON DUPLICATE KEY UPDATE like_count=like_count{modifyStr},last_updated_date=NOW();";
                    var result = await conn.ExecuteAsync(sql);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Common.Helper.Logger.Error($"更新hot表点赞量失败 at {nameof(HotExBiz)}.{nameof(UpdateLikeTotalAsync)}:{Environment.NewLine} {ex.Message}");
                return false;
            }

        }

        /// <summary>
        /// 更新hot表浏览量
        /// </summary>
        /// <param name="targetGuid"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        public async Task<bool> UpdateVisitTotalAsync(string targetGuid)
        {
            try
            {
                using (var conn = MySqlHelper.GetConnection())
                {
                    var sql = $"insert into t_utility_hot (owner_guid,visit_count) values('{targetGuid}',1)  ON DUPLICATE KEY UPDATE visit_count=visit_count+1,last_updated_date=NOW();";
                    var result = await conn.ExecuteAsync(sql);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Common.Helper.Logger.Error($"更新hot表浏览量失败 at {nameof(HotExBiz)}.{nameof(UpdateVisitTotalAsync)}:{Environment.NewLine} {ex.Message}");
                return false;
            }

        }

    }
}
