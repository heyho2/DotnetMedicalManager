using Dapper;
using GD.DataAccess;
using GD.Dtos.Admin.User;
using GD.Models.Manager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GD.Manager
{
    public class OrganizationBiz
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
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(OrganizationModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.InsertAsync<string, OrganizationModel>(model);
                return !string.IsNullOrWhiteSpace(result);
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(OrganizationModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.UpdateAsync(model);
                return result > 0;
            }
        }
        /// <summary>
        /// 根据id获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OrganizationModel> GetAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<OrganizationModel>(id);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.DeleteAsync<OrganizationModel>(id);
                return result > 0;
            }
        }
    }
}
