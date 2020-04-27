using Dapper;
using GD.Dtos.Common;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GD.Manager.Utility
{
    public class ExecuteSqlBiz
    {
        public async Task<SqlPageResponseDto> QueryAsync(SqlPageRequestDto request)
        {

            if (CheckWords(request.Sql))
            {
                var sss = await ExecuteAsync(request.Sql);
                var val = new SqlPageResponseDto
                {
                    CurrentPage = new List<IDictionary<string, object>> {
                        new Dictionary<string, object>{ { "result", $"成功执行{sss}" }, { "sql", request.Sql } }
                    },
                    Total = 1
                };
                return val;
            }
            else
                return await QueryByPageAsync(request);

        }
        private bool CheckWords(string sql)
        {
            string regex = "update|CREATE|delete|DROP";
            return Regex.IsMatch(sql, regex, RegexOptions.IgnoreCase);//返回ture
        }
        private async Task<SqlPageResponseDto> QueryByPageAsync(SqlPageRequestDto request)
        {
            string orderbySql = null;
            if (!string.IsNullOrWhiteSpace(request.SortField))
            {
                orderbySql = $"ORDER BY { request.SortField} {(request.IsAscending ? "ASC" : "DESC")}";
            }
            string pageSql = $"SELECT * FROM ({request.Sql})__TTTTT {orderbySql} limit {(request.PageIndex - 1) * request.PageSize}, {request.PageSize}";
            string countSql = "SELECT count(1) FROM (" + request.Sql + ") __t";
            using (var conn = DataAccess.MySqlHelper.GetConnection())
            {
                var CurrentPage = await conn.QueryAsync(pageSql, request);
                var Total = await conn.QueryFirstOrDefaultAsync<int>(countSql, request);
                var val = new SqlPageResponseDto
                {
                    CurrentPage = CurrentPage,
                    Total = Total
                };
                return val;
            }
        }
        private async Task<int> ExecuteAsync(string sql)
        {
            using (var conn = DataAccess.MySqlHelper.GetConnection())
            {
                return await conn.ExecuteAsync(sql);
            }
        }
    }
}
