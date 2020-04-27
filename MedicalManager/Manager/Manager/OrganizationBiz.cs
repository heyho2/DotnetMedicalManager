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
    /// 常见问题
    /// </summary>
    public class OrganizationBiz : BaseBiz<OrganizationModel>
    {
        /// <summary>
        /// 获取所有组织架构
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<OrganizationModel>> GetAllAsync(GetOrganizationTreeRequestDto request)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sortFields = new string[] { "sort".ToLower(), "creation_Date".ToLower() };
                string whereSql = $"WHERE 1=1";
                if (!string.IsNullOrEmpty(request.Name))
                {
                    whereSql = $" {whereSql} AND (org_name like @name or parentName like @name)";
                }
                if (request.Enable != null)
                {
                    whereSql = $" {whereSql} AND Enable = @Enable";
                }
                var orderbySql = "sort desc";
                if (!string.IsNullOrWhiteSpace(request.SortField))
                {
                    orderbySql = $"{(sortFields.Contains(request.SortField.ToLower()) ? request.SortField : sortFields[0])} {(request.IsAscending ? "asc" : "desc")}";
                }
                string sql = $@"
SELECT * FROM(
    SELECT
	    a.*,
	    b.org_name AS parentName 
    FROM
	    t_manager_organization a
	    LEFT JOIN t_manager_organization b ON a.parent_guid = b.org_guid
)__T
{whereSql}
ORDER BY {orderbySql}
";
                return await conn.QueryAsync<OrganizationModel>(sql, new
                {
                    name = $"%{request.Name}%",
                    request.Enable
                });
            }
        }
    }
}
