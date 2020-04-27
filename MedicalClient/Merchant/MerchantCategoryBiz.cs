using Dapper;
using GD.BizBase;
using GD.DataAccess;
using GD.Dtos.Mall.Category;
using GD.Dtos.Merchant.Category;
using GD.Dtos.Merchant.Merchant;
using GD.Models.Manager;
using GD.Models.Merchant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Merchant
{
    /// <summary>
    /// 类别业务类
    /// </summary>
    public class MerchantCategoryBiz : BaseBiz<CategoryExtensionModel>
    {
        /// <summary>
        /// 获取服务类型一级分类或指定商户所关联的一级分类列表
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> GetClassifies(string merchantGuid = ""/*商户Guid*/,
            bool isMerchantClassify = false/*是否获取指定商户已关联类别的分类*/,
            IEnumerable<string> classfieds = null/*获取指定分类*/)
        {
            var sql = @"SELECT DISTINCT d.dic_guid as id, d.config_name as `name` FROM t_manager_dictionary as d";

            if (isMerchantClassify)
            {
                sql += " INNER JOIN t_merchant_category_extension as c ON c.classify_guid = d.dic_guid";
            }

            sql += $" WHERE d.parent_guid = '{DictionaryType.ServiceClassifyGuid}'";

            if (isMerchantClassify && !string.IsNullOrEmpty(merchantGuid))
            {
                sql += $" AND c.merchant_guid = '{merchantGuid}' AND c.enable = 1";
            }

            var list = new List<object>();

            using (var conn = MySqlHelper.GetConnection())
            {
                var pairs = (await conn.QueryAsync(sql)).ToDictionary(
                        row => (string)row.id,
                        row => (string)row.name);

                if (pairs?.Count <= 0)
                {
                    return list;
                }

                foreach (var pair in pairs)
                {
                    if (classfieds?.Count() > 0)
                    {
                        if (!classfieds.Contains(pair.Key))
                        {
                            continue;
                        }
                    }

                    list.Add(new { id = pair.Key, name = pair.Value });
                }
            }

            return list;
        }

        /// <summary>
        /// 获取指定商户服务类型下二级分类列表
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <param name="dictId"></param>
        /// <returns></returns>
        public async Task<List<GetServiceClassifysResponseDto>> GetServiceTwoLevelClassifys(string merchantGuid, string dictId)
        {
            var sql = $@"SELECT DISTINCT d.dic_guid as id, d.config_name as `name` FROM t_manager_dictionary as d 
                    INNER JOIN t_merchant_category_extension as c ON c.classify_guid = d.dic_guid
            WHERE d.parent_guid = '{dictId}' AND c.merchant_guid = '{merchantGuid}' AND c.enable = 1";

            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryAsync<GetServiceClassifysResponseDto>(sql)).ToList();
            }
        }

        /// <summary>
        /// 获取商户指定平台分类下类别列表
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <param name="classifyGuid"></param>
        /// <returns></returns>
        public async Task<List<object>> GetCategoriesByClassify(string merchantGuid,
            string classifyGuid)
        {
            var sql = @"select category_guid as id,category_name as `name` from t_merchant_category_extension  
                WHERE merchant_guid = @MerchantGuid and classify_guid = @classifyGuid";

            var list = new List<object>();

            using (var conn = MySqlHelper.GetConnection())
            {
                var pairs = (await conn.QueryAsync(sql, new { merchantGuid, classifyGuid }))
                    .ToDictionary(
                        row => (string)row.id,
                        row => (string)row.name);

                if (pairs?.Count <= 0)
                {
                    return list;
                }

                foreach (var pair in pairs)
                {
                    list.Add(new { id = pair.Key, name = pair.Value });
                }
            }

            return list;
        }

        /// <summary>
        /// 获取类别详细信息
        /// </summary>
        /// <param name="categoryGuid"></param>
        /// <returns></returns>
        public async Task<GetMerchantCategoryDetailInfoResponseDto> GetMerchantCategoryDetailInfo(string merchantGuid, string categoryGuid)
        {
            var model = (GetMerchantCategoryDetailInfoResponseDto)null;

            var sql = @"SELECT 
                 classify_name AS ClassifyName, telephone, category_name AS CategoryName,
                 address,detail_address AS DetailAddress,
                 limit_time as LimitTime,
                 cover_guid AS CoverGuid,
                 introduction,
                 CONCAT('[',latitude,',',longitude,']') AS LongLatitude, 
                 (SELECT CONCAT(acc.base_path, acc.relative_path) AS cover FROM t_utility_accessory AS acc WHERE cover_guid = acc.accessory_guid) AS CoverUrl
            FROM  t_merchant_category_extension 
            WHERE merchant_guid = @MerchantGuid and category_guid = @CategoryGuid";

            using (var conn = MySqlHelper.GetConnection())
            {
                model = (await conn.QueryFirstOrDefaultAsync<GetMerchantCategoryDetailInfoResponseDto>(sql, new { merchantGuid, categoryGuid }));

                if (model is null)
                {
                    return null;
                }

                sql = "select picture_guid as value, target_url as url, sort from t_manager_banner where owner_guid = @categoryGuid and enable = 1 order by sort desc";

                model.Banners = (await conn.QueryAsync<MerchantCategoryBanner>(sql, new { categoryGuid })).ToList();
            }

            return model;
        }

        /// <summary>
        /// 创建类别
        /// </summary>
        /// <param name="model"></param>
        /// <param name="banners"></param>
        /// <returns></returns>
        public async Task<bool> CreateCategoryAsync(CategoryExtensionModel model, List<BannerModel> banners)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                var insertResult = await conn.InsertAsync<string, CategoryExtensionModel>(model);

                if (string.IsNullOrEmpty(insertResult)) { return false; }

                foreach (var item in banners)
                {
                    var affect = await conn.InsertAsync<string, BannerModel>(item);
                    if (string.IsNullOrEmpty(affect)) return false;
                }
                return true;
            });
        }


        /// <summary>
        /// 更新类别
        /// </summary>
        /// <param name="model"></param>
        /// <param name="banners"></param>
        /// <returns></returns>
        public async Task<bool> UpdateCategoryAsync(CategoryExtensionModel model, List<BannerModel> banners)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                var updateResult = await conn.UpdateAsync(model);

                if (updateResult <= 0) { return false; }

                var result = await conn.DeleteListAsync<BannerModel>("where owner_guid=@categoryGuid", new { model.CategoryGuid });

                foreach (var item in banners)
                {
                    var affect = await conn.InsertAsync<string, BannerModel>(item);
                    if (string.IsNullOrEmpty(affect)) return false;
                }
                return true;
            });
        }

        /// <summary>
        /// 获取商户指定平台分类下对应项目列表
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <returns></returns>
        public async Task<List<GetMerchantClassifyProjectListResponseDto>> GetMerchantClassifyProjects(string merchantGuid, IEnumerable<string> classfieds, string name)
        {
            var classifies = await GetClassifies(merchantGuid, true, classfieds);

            if (classifies?.Count() <= 0)
            {
                return null;
            }

            var ids = string.Join(",", classifies.Select(x => "'" + ((dynamic)x).id + "'").ToArray());

            var sql = $@"SELECT 
                        project_guid as PorjectId, 
                        classify_guid as ClassifyGuid,
                        project_name as PorjectName 
                     FROM t_mall_project 
                     WHERE classify_guid IN ({ids}) and enable = 1 and merchant_guid = '{merchantGuid}'";

            var items = (List<MerchantClassifyProjectItem>)null;

            using (var conn = MySqlHelper.GetConnection())
            {
                items = (await conn.QueryAsync<MerchantClassifyProjectItem>(sql)).ToList();
            }

            if (items?.Count() <= 0)
            {
                return null;
            }

            var classifyProjects = new List<GetMerchantClassifyProjectListResponseDto>();

            foreach (var classify in classifies)
            {
                var classifyProject = new GetMerchantClassifyProjectListResponseDto()
                {
                    Id = ((dynamic)classify).id,
                    Name = ((dynamic)classify).name
                };

                classifyProject.Items = items.Where(d => d.ClassifyGuid == classifyProject.Id).ToList();

                if (!string.IsNullOrEmpty(name?.Trim()))
                {
                    classifyProject.Items = classifyProject.Items.Where(d => d.PorjectName.Contains(name)).ToList();
                }

                classifyProjects.Add(classifyProject);

            }

            return classifyProjects;
        }

        /// <summary>
        /// 获取指定商户下用户（手机号码）已购服务大类列表
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<GetMerchantConsumerClassifyListResponseDto>> GetMerchantConsumerClassifies(string merchantGuid, string userId)
        {
            var sql = $@"SELECT DISTINCT 
                                d.category_guid as ClassifyGuid,
                                d.category_name as ClassifyName
                            FROM
                              t_consumer_goods a
                              INNER JOIN t_mall_order b ON a.order_guid = b.order_guid 
                              AND a.`enable` = b.`enable`
                              INNER JOIN t_mall_product c ON c.product_guid = a.product_guid
                              INNER JOIN t_merchant_category_extension d ON c.category_guid = d.classify_guid and d.merchant_guid=c.merchant_guid
                              INNER JOIN t_consumer_goods_item e ON e.goods_guid = a.goods_guid 
                              AND a.available = e.available 
                            WHERE
                              b.order_status = 'Completed' 
                              AND a.`enable` = 1 
                              and a.user_guid='{userId}'
                              AND c.merchant_guid='{merchantGuid}'
                              AND a.available = 1 
                              AND e.remain > 0;";

            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryAsync<GetMerchantConsumerClassifyListResponseDto>(sql)).ToList();
            }
        }

        /// <summary>
        /// 获取指定商户下用户（手机号码）已购服务项目列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="classifyId"></param>
        /// <returns></returns>
        public async Task<List<GetConsumerProjectsResponseDto>> GetMerchantConsumerProjects
            (string merchantGuid, string categoryId, string userId)
        {
            var sql = $@"
                        SELECT
                          e.goods_item_guid,
                          f.project_guid,
                          f.project_name,
                          e.remain 
                        FROM
                          t_consumer_goods a
                          INNER JOIN t_mall_order b ON a.order_guid = b.order_guid 
                          AND a.`enable` = b.`enable`
                          INNER JOIN t_mall_product c ON c.product_guid = a.product_guid
                          INNER JOIN t_merchant_category_extension d ON c.category_guid = d.classify_guid and c.merchant_guid = d.merchant_guid
                          INNER JOIN t_consumer_goods_item e ON e.goods_guid = a.goods_guid 
                          AND a.available = e.available 
                          INNER JOIN t_mall_project f on f.project_guid = e.project_guid
                        WHERE
                          b.order_status = 'Completed' 
                          AND a.`enable` = 1 
                          AND a.user_guid= @userId
                          AND d.category_guid = @categoryId
                          AND a.available = 1
                          AND c.merchant_guid = @merchantGuid
                          AND e.remain > 0 
                          AND (a.effective_end_date is null or a.effective_end_date>=CONCAT(DATE_FORMAT(NOW(),'%Y-%m-%d'),' 00:00:00'))
                          ORDER BY f.project_guid, e.remain;";

            using (var conn = MySqlHelper.GetConnection())
            {
                var items = (await conn.QueryAsync<ConsumerProjectItem>(sql,
                    new { userId, categoryId, merchantGuid })).ToList();

                if (items == null || items.Count <= 0)
                {
                    return null;
                }

                return items.GroupBy(d => d.ProjectGuid).Select(k => new GetConsumerProjectsResponseDto
                {
                    ProjectGuid = k.Key,
                    GoodsItemGuid = Min(k).GoodsItemGuid,
                    ProjectName = Min(k).ProjectName
                }).ToList();

                ConsumerProjectItem Min(IGrouping<string, ConsumerProjectItem> k)
                {
                    return k.ToList().OrderBy(d => d.Remain).FirstOrDefault();
                }
            }
        }

        /// <summary>
        /// 根据商户Id和主键获取指定类别
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <param name="categoryGuid"></param>
        /// <returns></returns>
        public async Task<CategoryExtensionModel> GetCategoryModelById(string merchantGuid,
            string categoryGuid)
        {
            var sql = @"select *
                        from t_merchant_category_extension 
                        where category_guid = @categoryGuid and merchant_guid = @merchantGuid";

            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<CategoryExtensionModel>(sql, new { categoryGuid, merchantGuid });
            }
        }

        /// <summary>
        /// 获取类别列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetMerchantCategoryListResponseDto> GetCategories(GetMerchantCategoryListRequestDto requestDto)
        {
            var sql = @"SELECT category_guid, 
                 classify_name, telephone, category_name,`enable`    
            FROM  t_merchant_category_extension 
            WHERE merchant_guid = @MerchantGuid";

            if (!string.IsNullOrEmpty(requestDto.ClassifyGuid))
            {
                sql += " AND classify_guid = @ClassifyGuid";
            }

            sql += " ORDER BY creation_date DESC";

            return await MySqlHelper.QueryByPageAsync<GetMerchantCategoryListRequestDto, GetMerchantCategoryListResponseDto, CategoryItem>(sql, requestDto);
        }

        /// <summary>
        /// 判断（商户+平台分类）唯一
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <returns></returns>
        public async Task<bool> MerchantUnitqueClassify(string merchantGuid, string classifyGuid, string categoryGuid = null)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@merchantGuid", merchantGuid);
            parameters.Add("@classifyGuid", classifyGuid);

            var sql = @"select 1 from t_merchant_category_extension 
                      where merchant_guid = @merchantGuid and classify_guid = @classifyGuid and enable = 1";

            if (!string.IsNullOrEmpty(categoryGuid))
            {
                sql += " and category_guid <> @categoryGuid";
                parameters.Add("@categoryGuid", categoryGuid);
            }

            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.ExecuteScalarAsync(sql, parameters);

                return (result is null) ? false : true;
            }
        }

        /// <summary>
        /// 类别是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<bool> ExistCategoryName(string merchantGuid, string name,
            string categoryGuid = null)
        {
            var parameters = new DynamicParameters();

            var sql = @"select 1 from t_merchant_category_extension 
                      where merchant_guid = @merchantGuid and category_name = @name and enable = 1";

            parameters.Add("@merchantGuid", merchantGuid);
            parameters.Add("@name", name);

            if (!string.IsNullOrEmpty(categoryGuid))
            {
                sql += " and category_guid <> @categoryGuid";
                parameters.Add("@categoryGuid", categoryGuid);
            }

            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.ExecuteScalarAsync(sql, parameters);

                return (result is null) ? false : true;
            }
        }

        /// <summary>
        /// 通过商品类别和门店guid获取类别扩展信息
        /// </summary>
        /// <param name="classifyId"></param>
        /// <param name="merchangId"></param>
        /// <returns></returns>
        public async Task<CategoryExtensionModel> GetModelByClassifyGuidAsync(string classifyId, string merchangId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryFirstOrDefaultAsync<CategoryExtensionModel>("select * from t_merchant_category_extension where classify_guid=@classifyId and merchant_guid=@merchangId", new { classifyId, merchangId });
                return result;
            }
        }


        #region 客户端类目相关
        /// <summary>
        /// 二级分类获取类目
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <param name="categoryGuid"></param>
        /// <returns></returns>
        public async Task<CategoryClassifyListResponse> GetCategoryClassifyListAsync(CategoryClassifyListRequest request)
        {
            var sqlWhere = $@" WHERE  ce.ENABLE = 1 AND mer.ENABLE =1 ";
            if (!string.IsNullOrWhiteSpace(request.DicGuid))
            {
                sqlWhere = $@" {sqlWhere} AND  ce.classify_guid = @DicGuid ";
            }
            var sql = $@"SELECT
	                                ce.category_guid,
	                                ce.classify_guid,
	                                ce.category_name,
	                                ce.address,
	                                ce.detail_address,
	                                CONCAT( acc.base_path, acc.relative_path ) AS CoverURL,
	                                ce.latitude,
	                                ce.longitude,
	                                ce.sort 
                                FROM
	                                t_merchant_category_extension AS ce
                                    LEFT JOIN t_merchant AS mer ON ce.merchant_guid = mer.merchant_guid
	                                LEFT JOIN t_utility_accessory AS acc ON ce.cover_guid = acc.accessory_guid 
                                {sqlWhere}  ";

            return await MySqlHelper.QueryByPageAsync<CategoryClassifyListRequest, CategoryClassifyListResponse, CategoryClassifyListItem>(sql, request);
        }


        #endregion
    }
}
