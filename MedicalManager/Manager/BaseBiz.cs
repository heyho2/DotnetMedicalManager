using Dapper;
using GD.Common.Base;
using GD.DataAccess;
using GD.Manager.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Manager
{
    public abstract class BaseBiz<T> where T : BaseModel
    {
        public virtual async Task<bool> AnyAsync(object id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.AnyAsync<T>(id);
            }
        }
        public virtual async Task<T> GetAsync(object id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<T>(id);
            }
        }
        public virtual async Task<bool> InsertAsync(T entity)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return !string.IsNullOrWhiteSpace(await conn.InsertAsync<string, T>(entity));
            }
        }
        public virtual async Task<bool> UpdateAsync(T entity)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.UpdateAsync(entity)) > 0;
            }
        }
        public virtual async Task<bool> UpdateAsync(IEnumerable<T> entitys)
        {
            var result = await MySqlHelper.TransactionAsync(async (conn, t) =>
            {
                foreach (var item in entitys)
                {
                    await conn.UpdateAsync(item);
                }
                return true;
            });
            return result;
        }
        public virtual async Task<int> RecordCountAsync(bool? enable = null)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sqlWhere = "1=1";
                if (enable != null)
                {
                    sqlWhere = $"{sqlWhere} AND {nameof(BaseModel.Enable)}={(enable.Value ? "1" : "0")}";
                }
                return await conn.RecordCountAsync<T>($"WHERE {sqlWhere}");
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.DeleteAsync<T>(id);
                return result > 0;
            }
        }

        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <param name="enable"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> GetListAsync(bool? enable = null)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sqlWhere = "1=1";
                if (enable != null)
                {
                    sqlWhere = $"{sqlWhere} AND {nameof(BaseModel.Enable)}={(enable.Value ? "1" : "0")}";
                }
                var result = await conn.GetListAsync<T>($"WHERE {sqlWhere}");
                return result;
            }
        }

        public virtual async Task<IEnumerable<T>> GetListAsync(IEnumerable<string> guids, bool? enable = null)
        {
            if (!guids.Any())
            {
                return new List<T>();
            }
            guids = guids.Distinct();
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetListAsync<T>(guids, enable);
            }
        }
        public virtual async Task<bool> DisableEnableAsync(string guid, bool enable, string userId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var entity = await conn.GetAsync<T>(guid);
                if (entity == null)
                {
                    return false;
                }
                entity.LastUpdatedBy = userId;
                entity.LastUpdatedDate = DateTime.Now;
                entity.Enable = enable;
                var result = await conn.UpdateAsync(entity);
                return result > 0;
            }
        }

        public virtual async Task<bool> InsertListAsync(IEnumerable<T> modelList, IDbConnection conn = null, int? commandTimeout = null)
        {
            if (conn == null)
            {
                using (conn = MySqlHelper.GetConnection())
                {
                    return (await conn.InsertListAsync(modelList)) > 0;
                }
            }
            else
            {
                return (await conn.InsertListAsync(modelList)) > 0;
            }
        }
    }
}
