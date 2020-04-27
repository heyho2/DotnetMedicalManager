using Dapper;
using GD.BizBase;
using GD.DataAccess;
using GD.Dtos.Mall.Mall;
using GD.Dtos.Mall.Project;
using GD.Models.Mall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Mall
{
    /// <summary>
    /// 服务项目实体业务类
    /// </summary>
    public class ProjectBiz : BaseBiz<ProjectModel>
    {
        /// <summary>
        /// 根据商户Id和主键获取指定服务项目
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <param name="proejectGuid"></param>
        /// <returns></returns>
        public async Task<ProjectModel> GetMerchantPorjectModelById(string merchantGuid,
            string proejectGuid)
        {
            var sql = @"select *
                        from t_mall_project 
                        where project_guid = @proejectGuid and merchant_guid = @merchantGuid";

            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<ProjectModel>(sql, new { proejectGuid, merchantGuid });
            }
        }
        /// <summary>
        /// 获取model
        /// </summary>
        /// <param name="projectId">主键guid</param>
        /// <returns></returns>
        public async Task<ProjectModel> GetModelAsync(string projectId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<ProjectModel>("select * from t_mall_project where project_guid=@projectId and `enable`=1", new { projectId });
            }
        }
      
        /// <summary>
        /// 获取服务项目列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetMerchantProjectResponseDto> GetMerchantProjects(GetMerchantProjectListRequestDto requestDto)
        {
            var sql = $@"SELECT project_guid, 
                 classify_name, operation_time, project_name,price
            FROM  t_mall_project 
            WHERE merchant_guid = @MerchantGuid AND enable = 1";

            if (!string.IsNullOrEmpty(requestDto.ClassifyGuid))
            {
                sql += " AND classify_guid = @ClassifyGuid";
            }

            if (!string.IsNullOrEmpty(requestDto.ProjectName?.Trim()))
            {
                sql += $" and project_name like '%{requestDto.ProjectName}%'";
            }

            sql += " ORDER BY creation_date DESC";

            return await MySqlHelper.QueryByPageAsync<GetMerchantProjectListRequestDto, GetMerchantProjectResponseDto, MerchantProjectItem>(sql, requestDto);
        }

        /// <summary>
        /// 商户下服务项目名称是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<bool> ExistMerchantProjectName(string merchantGuid, string name,
            string projectGuid = null)
        {
            var parameters = new DynamicParameters();

            var sql = @"select 1 from t_mall_project 
                      where merchant_guid = @merchantGuid and project_name = @name and enable = 1";

            parameters.Add("@merchantGuid", merchantGuid);
            parameters.Add("@name", name);

            if (!string.IsNullOrEmpty(projectGuid))
            {
                sql += " and project_guid <> @projectGuid";
                parameters.Add("@projectGuid", projectGuid);
            }

            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.ExecuteScalarAsync(sql, parameters);

                return (result is null) ? false : true;
            }
        }

        /// <summary>
        /// 删除指定服务项目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> DeleteMerchantProject(ProjectModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.DeleteListAsync<ProjectModel>("where project_guid = @projectGuid", new { model.ProjectGuid })) > 0;
            }
        }
        /// <summary>
        /// 获取门店适用的项目
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task<List<ProjectModel>> GetModelListByProductGuid(string productGuid, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT 
	                                    mp.* 
                                    FROM
	                                    t_mall_product_project AS mpp
	                                    RIGHT JOIN t_mall_project AS mp ON mpp.project_guid = mp.project_guid 
	                                    AND mpp.`enable` = mp.`enable` 
                                    WHERE
	                                    mpp.product_guid = @productGuid 
	                                    AND mp.`enable` = @enable ";
                var result = await conn.QueryAsync<ProjectModel>(sql, new { productGuid, enable });
                return result?.ToList();
            }
        }

        /// <summary>
        /// 根据商品guid集合获取包含项目列表
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task<List<ProductProjectDto>> GetDtoListByProductGuids(List<string> productGuids)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            a.product_guid,
	                            b.project_guid,
	                            b.project_name,
	                            b.operation_time,
                                a.project_times,
	                            b.price,
	                            a.allow_present 
                            FROM
	                            t_mall_product_project a
	                            INNER JOIN t_mall_project b ON a.project_guid = b.project_guid 
	                            AND a.`enable` = b.`enable` 
                            WHERE
	                            a.product_guid IN @productGuids
	                            AND a.`enable` =1";
                var result = await conn.QueryAsync<ProductProjectDto>(sql, new { productGuids });
                return result?.ToList();
            }
        }

        /// <summary>
        /// 根据产品Id获取商品项列表
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <returns></returns>
        public async Task<List<ProjectInfoDto>> GetModelsByProductIdAsync(string productId)
        {
            var sql = @"SELECT
	                        project.project_guid AS ProjectGuid,
	                        project.project_name AS ProjectName,
	                        ppr.project_times AS ProjectTimes ,
                            ppr.allow_present,
	                        project.price,
	                        project.platform_type,
                            case when ppr.project_times=999 then true else false end as Infinite
                        FROM
	                        t_mall_product_project ppr
	                        INNER JOIN t_mall_project project ON project.project_guid = ppr.project_guid 
                        WHERE
	                        ppr.product_guid = @productId 
	                        AND project.`enable` = 1 
	                        AND ppr.`enable` =1";

            using (var conn = MySqlHelper.GetConnection())
            {
                var projectInfo = await conn.QueryAsync<ProjectInfoDto>(sql, new { productId });
                return projectInfo?.ToList();
            }

        }

    }
}
