using Dapper;
using GD.Common.Base;
using GD.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using GD.BizBase.Common;

namespace GD.BizBase
{
    public abstract class BaseBiz<T> where T : BaseModel
    {
        /// <summary>
        /// 异步获取唯一实例
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<T>(id);
            }
        }

        /// <summary>
        /// 异步插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> InsertAsync(T model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.InsertAsync<string, T>(model);
                return !string.IsNullOrWhiteSpace(result);
            }
        }

        /// <summary>
        /// 异步批量插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> InsertAsync(List<T> models)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                foreach (var model in models)
                {
                    var result = await conn.InsertAsync<string, T>(model);
                }
                return true;
            });
        }




        /// <summary>
        /// 异步更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(T model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.UpdateAsync(model);
                return result > 0;
            }
        }

        /// <summary>
        /// 查看数据是否存在
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns></returns>
        public async Task<bool> ExistAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var model = await conn.GetAsync<T>(id);
                return model != null;
            }
        }

        /// <summary>
        /// 删除model
        /// </summary>
        /// <param name="model">被删除model</param>
        /// <param name="userId">操作人</param>
        /// <param name="islogical">是否逻辑删除</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(T model, string userId, bool islogical = false)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var affectedRows = 0;
                if (islogical)
                {
                    model.LastUpdatedDate = DateTime.Now;
                    model.LastUpdatedBy = userId;
                    model.Enable = false;
                    affectedRows = await conn.UpdateAsync(model);
                }
                else
                {
                    affectedRows = await conn.DeleteAsync(model);
                }
                return affectedRows == 1;

            }
        }

        /// <summary>
        /// 通过主键集合获取models
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<T>> GetModelsByPrimaryKeyIdsAsync(IEnumerable<string> ids, bool? enable = true)
        {
            var keyName = DapperEx.GetTablePrimaryKey<T>();
            var distinctIds = ids.Distinct();
            using (var conn = MySqlHelper.GetConnection())
            {
                var sqlWhere = string.Empty;
                if (enable.HasValue)
                {
                    sqlWhere = " and `enable`=@enable";
                }
                var result = await conn.GetListAsync<T>($"where {keyName} in @distinctIds {sqlWhere}", new { distinctIds, enable });
                return result.ToList();
            }
        }

        /// <summary>
        /// Mysql真实批量插入
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public bool InsertBatch(List<T> models)
        {
            var count = models.InsertBatch();
            return count == models.Count;
        }
    }
}