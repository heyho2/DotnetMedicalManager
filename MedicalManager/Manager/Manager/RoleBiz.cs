using Dapper;
using GD.DataAccess;
using GD.Dtos.User;
using GD.Models.Manager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GD.Manager.Manager
{
    /// <summary>
    /// 角色业务类
    /// </summary>
    public class RoleBiz : BaseBiz<RoleModel>
    {
        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<RoleModel>> GetAllAsync(GetRoleListRequestDto request)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sortFields = new string[] { "sort".ToLower(), "creation_Date".ToLower() };

                string whereSql = $"WHERE 1=1";
                if (!string.IsNullOrEmpty(request.Name))
                {
                    whereSql = $" {whereSql} AND (role_name like @name)";
                }
                if (request.Enable != null)
                {
                    whereSql = $" {whereSql} AND (Enable = @Enable)";
                }
                var orderbySql = "sort desc";
                if (!string.IsNullOrWhiteSpace(request.SortField))
                {
                    orderbySql = $"{(sortFields.Contains(request.SortField.ToLower()) ? request.SortField : sortFields[0])} {(request.IsAscending ? "asc" : "desc")}";
                }
                return await conn.GetListAsync<RoleModel>($"{whereSql} ORDER BY {orderbySql}", new
                {
                    name = $"%{request.Name}%",
                    request.Enable
                });
            }
        }
    }
}
