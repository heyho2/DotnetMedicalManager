using Dapper;
using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GD.Manager.Common
{
    public static class DapperExpansion
    {
        public static string GetKeyName<T>() where T : BaseModel
        {
            var t = typeof(T);
            var name = string.Empty;
            foreach (var item in t.GetProperties())
            {
                var keyInfo = item.GetCustomAttributes<System.ComponentModel.DataAnnotations.KeyAttribute>().FirstOrDefault();
                if (keyInfo != null)
                {
                    var columnInfo = item.GetCustomAttributes<System.ComponentModel.DataAnnotations.Schema.ColumnAttribute>().FirstOrDefault();

                    name = columnInfo?.Name ?? item.Name;
                }
            }
            if (name == string.Empty)
            {
                throw new Exception("没有找到主键");
            }
            return name;
        }
        public static string GetTableName<T>() where T : BaseModel
        {
            var t = typeof(T);
            var tableInfo = t.GetCustomAttributes<System.ComponentModel.DataAnnotations.Schema.TableAttribute>(false).FirstOrDefault();
            return tableInfo?.Name ?? t.Name;
        }
        public static async Task<int> InsertListAsync<T>(this IDbConnection conn, IEnumerable<T> modelList, int? commandTimeout = null) where T : BaseModel
        {
            var t = typeof(T);
            var columns = new List<string>();
            var columnValues = new List<string>();
            foreach (var item in t.GetProperties())
            {
                var columnInfo = item.GetCustomAttributes<System.ComponentModel.DataAnnotations.Schema.ColumnAttribute>().FirstOrDefault();
                columnValues.Add($"@{item.Name}");
                columns.Add($"{columnInfo?.Name ?? item.Name}");
            }
            var sql = $"INSERT INTO {GetTableName<T>()} ({string.Join(",", columns)}) VALUES ({string.Join(",", columnValues)});";
            return await conn.ExecuteAsync(sql, modelList, null, commandTimeout);
        }

        public static async Task<IEnumerable<T>> GetListAsync<T>(this IDbConnection conn, IEnumerable<string> guids, bool? enable = null) where T : BaseModel
        {
            var sqlWhere = $"1=1 AND {GetKeyName<T>()} in @guids";
            if (enable != null)
            {
                sqlWhere = $"{sqlWhere} AND {nameof(BaseModel.Enable)}={(enable.Value ? "1" : "0")}";
            }
            var result = await conn.GetListAsync<T>($"WHERE {sqlWhere}", new { guids });
            return result;
        }
        public static async Task<bool> AnyAsync<T>(this IDbConnection conn, object id) where T : BaseModel
        {
            var sql = $"SELECT COUNT(1) FROM {GetTableName<T>()} WHERE {GetKeyName<T>()}=@id";
            int result = await conn.QueryFirstOrDefaultAsync<int>(sql, new { id });
            return result > 0;
        }
    }
}
