using Dapper;
using GD.DataAccess;
using GD.Dtos.Admin.Banner;
using GD.Dtos.Manager.Banner;
using GD.Models.Manager;
using GD.Models.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Manager
{
    /// <summary>
    /// Banner业务类
    /// </summary>
    public class BannerBiz
    {
        /// <summary>
        /// 获取唯一Banner Model
        /// </summary>
        /// <param name="guid">主键guid</param>
        /// <returns></returns>
        public BannerModel GetModel(string guid)
        {
            return MySqlHelper.GetModelById<BannerModel>(guid);
        }

        /// <summary>
        /// 通过所有者Guid获取Banner列表
        /// </summary>
        /// <param name="ownerGuid">所有者Guid</param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public List<BannerModel> GetModelsByOwnerGuid(string ownerGuid, bool enable = true)
        {
            var sql = "select * from t_manager_banner where owner_guid=@ownerGuid and enable=@enable order by sort desc";
            return MySqlHelper.Select<BannerModel>(sql, new { ownerGuid, enable })?.ToList();
        }

        /// <summary>
        /// 修改目标的Banner数据
        /// </summary>
        /// <param name="ownerGuid">banner所有者guid</param>
        /// <param name="banners">banner列表</param>
        /// <returns></returns>
        public async Task<bool> ModifyBannerInfoAsync(string ownerGuid, List<BannerModel> banners)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                var result = await conn.DeleteListAsync<BannerModel>("where owner_guid=@ownerGuid", new { ownerGuid });
                foreach (var item in banners)
                {
                    var affect = await conn.InsertAsync<string, BannerModel>(item);
                    if (string.IsNullOrEmpty(affect)) return false;
                }
                return true;
            });
        }

        /// <summary>
        /// 修改目标的Banner数据
        /// </summary>
        /// <param name="ownerGuid">banner所有者guid</param>
        /// <param name="banners">banner列表</param>
        /// <returns></returns>
        public async Task<bool> ModifyBannerInfoAsync(List<string> ownerGuids, List<BannerModel> banners)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                var result = await conn.DeleteListAsync<BannerModel>("where owner_guid in @ownerGuids", new { ownerGuids });
                foreach (var item in banners)
                {
                    var affect = await conn.InsertAsync<string, BannerModel>(item);
                    if (string.IsNullOrEmpty(affect)) return false;
                }
                return true;
            });
        }


        /// <summary>
        /// 获取页面Banner信息
        /// </summary>
        /// <param name="pageId">页面Id</param>
        /// <returns></returns>
        public async Task<IEnumerable<BannerBaseDto>> GetBannerInfoAsync(string pageId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
                            banner.picture_guid,
	                        banner.banner_name ,
	                        CONCAT( acce.base_path, acce.relative_path ) AS Picture,
	                        banner.target_url,
	                        banner.sort 
                        FROM
	                        t_manager_banner AS banner
	                        LEFT JOIN t_utility_accessory acce ON banner.picture_guid = acce.accessory_guid 
	                        AND acce.`enable` = TRUE 
                        WHERE
	                        banner.owner_guid = @pageId 
	                        AND banner.`enable` = TRUE
                        ORDER BY
                            banner.sort DESC";
                ;
                return await conn.QueryAsync<BannerBaseDto>(sql, new { pageId });
            }

        }

        /// <summary>
        /// 获取首页Banner
        /// </summary>
        /// <param name="ownerGuid"></param>
        /// <returns></returns>
        public async Task<IEnumerable<GetHomeBannerItemDto>> GetHomeBannerAsync(string ownerGuid)
        {
            var sqlWhere = $@" 1=1 AND A.enable = @enable AND A.owner_guid=@ownerGuid ";
            var sql = $@"
SELECT
	A.*,
	B.accessory_guid,
	B.base_path,
	B.relative_path 
FROM
	t_manager_banner A
	LEFT JOIN t_utility_accessory B ON A.picture_guid = B.accessory_guid 
WHERE
	{sqlWhere}
ORDER BY
	A.sort DESC";
            var parameters = new
            {
                enable = true,
                ownerGuid
            };
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<GetHomeBannerItemDto, AccessoryModel, GetHomeBannerItemDto>(sql, (a, b) =>
                {
                    a.Picture = $"{b?.BasePath}{b?.RelativePath}";
                    return a;
                }, parameters, splitOn: "accessory_guid");

                return result;
            }
        }

        public async Task<GetBannerPageResponseDto> GetBannerPageAsync(GetBannerPageRequestDto request)
        {
            var whereSql = $@" 1=1";
            if (!string.IsNullOrWhiteSpace(request.Type))
            {
                whereSql = $"{whereSql} AND owner_guid=@Type";
            }
            var sortFields = new string[] { "sort".ToLower(), "creation_Date".ToLower() };
            var orderbySql = "sort DESC";
            if (!string.IsNullOrWhiteSpace(request.SortField))
            {
                orderbySql = $"{(sortFields.Contains(request.SortField.ToLower()) ? request.SortField : sortFields[0])} {(request.IsAscending ? "asc" : "desc")}";
            }
            var sql = $@"
SELECT * FROM(
    SELECT
	    A.*,
	    B.accessory_guid,
	    B.base_path,
	    B.relative_path,
CONCAT( B.base_path, B.relative_path ) as Picture
    FROM
	    t_manager_banner A
	    LEFT JOIN t_utility_accessory B ON A.picture_guid = B.accessory_guid 
) ____T
WHERE
	{whereSql}
ORDER BY 
	enable desc,{orderbySql}";
            return await MySqlHelper.QueryByPageAsync<GetBannerPageRequestDto, GetBannerPageResponseDto, GetBannerPageItemDto>(sql, request);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(BannerModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.InsertAsync<string, BannerModel>(model);
                return !string.IsNullOrWhiteSpace(result);
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(BannerModel model)
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
        public async Task<BannerModel> GetAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<BannerModel>(id);
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
                var result = await conn.DeleteAsync<BannerModel>(id);
                return result > 0;
            }
        }

    }
}
