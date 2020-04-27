using Dapper;
using GD.DataAccess;
using GD.Dtos.Mall.Category;
using GD.Dtos.Mall.Mall;
using GD.Dtos.Mall.Product;
using GD.Dtos.Merchant.Merchant;
using GD.Models.CommonEnum;
using GD.Models.CrossTable;
using GD.Models.Mall;
using GD.Models.Manager;
using GD.Models.MySqlCommon;
using GD.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Mall
{
    public class ProductBiz
    {
        /// <summary>
        /// 根据Guid查询
        /// </summary>
        /// <param name="productGuid"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public ProductModel GetModelByGuid(string productGuid, bool enable = true)
        {
            const string sql = "select * from t_mall_product where product_guid=@productGuid and enable=@enable";
            var model = MySqlHelper.SelectFirst<ProductModel>(sql, new { productGuid, enable });
            return model;
        }
        /// <summary>
        /// 根据Guid查询
        /// </summary>
        /// <param name="productGuid"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task<ProductModel> GetModelByGuidAsync(string productGuid, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                string sql = "select * from t_mall_product where product_guid=@productGuid and enable=@enable";
                return await conn.QueryFirstOrDefaultAsync<ProductModel>(sql, new { productGuid, enable });
            }
        }
        /// <summary>
        /// 根据Guid查询
        /// </summary>
        /// <param name="productGuid"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task<ProductModel> GetModelByGuidAsyncNoEnable(string productGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                string sql = "select * from t_mall_product where product_guid=@productGuid ";
                return await conn.QueryFirstOrDefaultAsync<ProductModel>(sql, new { productGuid });
            }
        }
        /// <summary>
        /// 通过商品编号获取商品
        /// </summary>
        /// <param name="merchantGuid">商铺guid</param>
        /// <param name="productCode">商品编码</param>
        /// <param name="enalbe"></param>
        /// <returns></returns>
        public async Task<ProductModel> GetByCodeAsync(string merchantGuid, string productCode, bool enalbe = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var model = await conn.QueryFirstOrDefaultAsync<ProductModel>("select * from t_mall_product where merchant_guid=@merchantGuid and product_code=@productCode and enable=@enalbe", new { merchantGuid, productCode, enalbe });
                return model;
            }
        }

        /// <summary>
        /// 通过主键列表获取model集合
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<ProductModel>> GetModelsAsync(List<string> ids)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<ProductModel>("where product_guid in @ids", new { ids });
                return result?.AsList();
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Add(ProductModel model)
        {
            return model.Insert();

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int? Delete(ProductModel model)
        {
            return model.Delete();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int? Update(ProductModel model)
        {
            model.LastUpdatedDate = DateTime.Now;
            return model.Update();
        }
        /// <summary>
        /// 异步新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ProductModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.InsertAsync<string, ProductModel>(model);
                return !string.IsNullOrWhiteSpace(result);
            }
        }

        /// <summary>
        /// 异步新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(ProductModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.UpdateAsync(model);
                return result == 1;
            }
        }
        /// <summary>
        /// 商品补充库存
        /// </summary>
        /// <param name="productGuid">商品guid</param>
        /// <param name="replenishInventory">补充的库存数量</param>
        /// <returns></returns>
        public async Task<bool> ReplenishProductInventoryAsync(string productGuid, int replenishInventory)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = "update t_mall_product set inventory=inventory + @replenishInventory where product_guid = @productGuid";
                var result = await conn.ExecuteAsync(sql, new { productGuid, replenishInventory });
                return result == 1;
            }
        }

        /// <summary>
        /// 新增商品数据
        /// </summary>
        /// <param name="model">商品model</param>
        /// <param name="introduce">商品介绍富文本</param>
        /// <param name="productDetail">商品明细富文本</param>
        /// <param name="banners">banners</param>
        /// <returns></returns>
        public async Task<bool> CreateProductOfDoctorCloudAsync(ProductModel model, List<RichtextModel> richTexts, List<BannerModel> banners)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                if (string.IsNullOrEmpty(await conn.InsertAsync<string, ProductModel>(model))) return false;

                foreach (var item in richTexts)
                {
                    if (string.IsNullOrEmpty(await conn.InsertAsync<string, RichtextModel>(item))) return false;
                }

                foreach (var item in banners)
                {
                    if (string.IsNullOrEmpty(await conn.InsertAsync<string, BannerModel>(item))) return false;
                }
                return true;
            });

        }


        /// <summary>
        /// 分页查询产品列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public List<ProductModel> GetPageList(GetProductPageListRequestDto requestDto, bool enable = true)
        {
            var sql = @"select *from t_mall_product where ( category_guid=@categoryGuid or category_name=@categoryName )  
                                                and  enable=@enable and on_sale=1
                                                order by creation_date desc limit @pageIndex,@pageSize  ";
            if (!string.IsNullOrWhiteSpace(requestDto.Keyword))
            {
                sql = $@"select *from t_mall_product where product_name like '%{requestDto.Keyword}%'
                                                and  enable=@enable and on_sale=1
                                                order by creation_date desc limit @pageIndex,@pageSize  ";
            }

            var modelList = MySqlHelper.Select<ProductModel>(sql, new
            {
                categoryGuid = requestDto.CategoryGuid,
                categoryName = requestDto.CategoryName,
                enable,
                pageIndex = (requestDto.PageIndex - 1) * requestDto.PageSize,
                pageSize = requestDto.PageSize,
            }).ToList();
            return modelList;
        }

        ///待修改
        /// <summary>
        /// 分页查询产品列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public List<GetMerchantProductInfoModel> GetProListByMerchantGuidAndCategoryGuid(GetProductListInMerchantRequestDto requestDto, bool enable = true)
        {
            var sql = @"SELECT * FROM  (
                            SELECT
	                            pro.merchant_guid,
	                            mer.merchant_name,
	                            pro.category_guid,
	                            dic.config_name,
	                            pro.product_guid,
	                            CONCAT( acc.base_path, acc.relative_path ) AS ProductPicUrl,
	                            pro.product_name,
	                            pro.price,
	                            pro.product_form,
	                            0 AS SaleNum 
                            FROM
	                            t_mall_product AS pro
	                            LEFT JOIN t_merchant AS mer ON pro.merchant_guid = mer.merchant_guid
	                            LEFT JOIN t_manager_dictionary AS dic ON pro.category_guid = dic.dic_guid
	                            LEFT JOIN t_utility_accessory AS acc ON pro.picture_guid = acc.accessory_guid 
                            WHERE
	                            pro.`enable` = @ENABLE 
                                AND mer.`enable` = 1
		                        AND pro.on_sale =1
                            ORDER BY
	                            pro.creation_date DESC
                        ) AS T 
                        WHERE
                            T.merchant_guid = @merchantGuid ";
            if (!string.IsNullOrWhiteSpace(requestDto.ClassifyGuid))
            {
                sql += " AND T.category_guid =@categoryGuid ";
            }
            sql += " LIMIT @pageIndex,@pageSize ";

            var modelList = MySqlHelper.Select<GetMerchantProductInfoModel>(sql, new
            {
                merchantGuid = requestDto.MerchantGuid,
                categoryGuid = requestDto.ClassifyGuid,
                enable,
                pageIndex = (requestDto.PageIndex - 1) * requestDto.PageSize,
                pageSize = requestDto.PageSize
            }).ToList();
            return modelList;
        }
        /// <summary>
        /// 搜索商品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SearchProductResponseDto> SearchProductAsync(SearchProductRequestDto request)
        {
            var sqlWhere = $@"AND ENABLE = 1 ";

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                sqlWhere = $"{sqlWhere} AND (merchant_name like @Keyword  OR category_name like @Keyword  OR product_name like @Keyword OR product_label like @Keyword OR brand like @Keyword)";
            }
            var sql = $@"
                    SELECT * FROM(
                        SELECT
	                        A.*,
	                        CONCAT( B.base_path, B.relative_path ) AS LogoUrl,
	                        C.merchant_name 
                        FROM
	                        t_mall_product A
	                        LEFT JOIN t_utility_accessory B ON B.accessory_guid = A.picture_guid
	                        LEFT JOIN t_merchant C ON C.merchant_guid = A.merchant_guid
                    )T
                    WHERE
	                    1 = 1 {sqlWhere}
                    ORDER BY
	                    creation_date";
            request.Keyword = $"%{request.Keyword}%";

            return await MySqlHelper.QueryByPageAsync<SearchProductRequestDto, SearchProductResponseDto, SearchProductItemDto>(sql, request);

        }

        /// <summary>
        /// 检查商品库存，例如检查商品库存是否大于N件
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <param name="inventory">需要检查的数量</param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public bool CheckProductInventory(string productId, int inventory, bool enable = true)
        {
            return MySqlHelper.Count<ProductModel>("where product_guid=@productId and inventory>=@inventory and enable=@enable", new { productId, inventory, enable }) > 0;
        }
        /// <summary>
        /// 获取首页热门商品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<GetHomeHotProductItemDto>> GetHomeHotProductAsync(GetHomeHotProductRequestDto request)
        {
            var sql = $@"
                        SELECT * FROM(
                            SELECT
	                        A.*,
	                        CONCAT( B.base_path, B.relative_path ) AS LogoUrl,
	                        C.merchant_name,
	                        (SELECT COUNT(1) FROM t_mall_order_detail A1 INNER JOIN t_mall_order A2 ON A1.order_guid = A2.order_guid AND A2.order_status = '{OrderStatusEnum.Completed.ToString()}' WHERE A2.`enable` = 1 AND A1.product_guid = A.product_guid GROUP BY A1.product_guid) AS count 
                        FROM
	                        t_mall_product A
	                        LEFT JOIN t_utility_accessory B ON B.accessory_guid = A.picture_guid
	                        LEFT JOIN t_merchant C ON C.merchant_guid = A.merchant_guid
                        )T
                        WHERE
	                        1 = 1 
                        ORDER BY
	                        count DESC 
                            LIMIT {request.Take}";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<GetHomeHotProductItemDto>(sql);
                return result;
            }
        }

        public Task RemoveProductsAsync(string merchantGuid, string[] v1, bool v2)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 计算销售量
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <param name="inventory">需要检查的数量</param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public int SumProductSoldNum(string productId, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return conn.ExecuteScalar<int>(
                    @"SELECT
	                        SUM( od.product_count ) AS soldTotal 
                        FROM
	                        t_mall_order_detail AS od
	                        INNER JOIN t_mall_order AS o ON o.order_guid = od.order_guid AND o.`enable` = od.`enable` 
                        WHERE
	                        od.product_guid = @productId 
	                        and o.order_status='Completed'
	                        AND od.ENABLE = @enable",
                    //加入订单状态就能
                    new { productId, enable });
            }
        }
        /// <summary>
        /// 商家端获取商品列表(智慧云医)
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetProductListForMerchantManagementResponseDto> GetProductListForMerchantManagementAsync(GetProductListForMerchantManagementRequestDto requestDto)
        {
            var sqlWhere = string.Empty;
            if (requestDto.OnSale.HasValue)
            {
                sqlWhere = $"{sqlWhere} and a.on_sale = {(requestDto.OnSale.Value ? 1 : 0)}";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.ProductName))
            {
                sqlWhere = $"{sqlWhere} and a.product_name like '%{requestDto.ProductName}%'";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.ProductCode))
            {
                sqlWhere = $"{sqlWhere} and a.product_code like '%{requestDto.ProductCode}%'";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.CategoryGuid))
            {
                sqlWhere = $"{sqlWhere} and a.category_guid = '{requestDto.CategoryGuid}'";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.ProductBrandGuid))
            {
                sqlWhere = $"{sqlWhere} and a.brand = '{requestDto.ProductBrandGuid}'";
            }
            if (requestDto.WarningInventory)
            {
                sqlWhere = $"{sqlWhere} and a.inventory <= a.warning_inventory";
            }

            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT
	                            a.product_guid AS ProductGuid,
	                            a.product_name AS ProductName,
                                a.product_code as ProductCode,
	                            a.price AS Price,
	                            a.on_sale AS OnSale,
                                a.platform_on_sale,
                                a.category_name as CategoryName,
                                a.product_form AS ProductForm,
                               (
                                    CASE a.`product_form` WHEN 'Service' THEN '-' ELSE a.inventory              END
                               ) AS Inventory,
                                a.effective_days AS EffectiveDays,
	                           (
                                    SELECT IFNULL(sum(d.product_count ), 0 ) 
                                    FROM t_mall_order as o 
                                        INNER JOIN t_mall_order_detail as d ON o.order_guid = d.order_guid 
                                    WHERE a.product_guid = d.product_guid AND o.order_status = 'Completed' AND o.`enable` = 1 AND d.`enable` = 1
                              ) as SalesVolume      
                            FROM
	                            t_mall_product a
                            WHERE
	                            a.merchant_guid = '{requestDto.MerchantGuid}' 
	                            AND a.platform_type = 'CloudDoctor' 
	                            AND a.`enable` = 1 { sqlWhere } 
                            GROUP BY
	                            a.product_guid,
	                            a.product_name,
                                a.product_code,
	                            a.price,
	                            a.on_sale,
	                            a.inventory 
                            ORDER BY
	                            a.product_name";
                var result = await MySqlHelper.QueryByPageAsync<GetProductListForMerchantManagementRequestDto, GetProductListForMerchantManagementResponseDto, GetProductListForMerchantManagementItemDto>(sql, requestDto);
                return result;
            }
        }

        /// <summary>
        /// 获取商铺回收站中的商品列表（智慧云医）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetProductsOfMerchantRecycleBinResponseDto> GetProductsOfMerchantRecycleBinAsync(GetProductsOfMerchantRecycleBinRequestDto requestDto)
        {
            var sqlWhere = "";
            if (requestDto.StartRecycleDate.HasValue)
            {
                sqlWhere = $"{sqlWhere} and a.last_updated_date >= '{requestDto.StartRecycleDate.Value.ToString("yyyy-MM-dd HH:mm:ss")}'";
            }
            if (requestDto.EndRecycleDate.HasValue)
            {
                sqlWhere = $"{sqlWhere} and a.last_updated_date <= '{requestDto.EndRecycleDate.Value.ToString("yyyy-MM-dd HH:mm:ss")}'";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.ProductName))
            {
                sqlWhere = $"{sqlWhere} and a.product_name like '%{requestDto.ProductName}%'";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.ProductCode))
            {
                sqlWhere = $"{sqlWhere} and a.product_code like '%{requestDto.ProductCode}%'";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.CategoryGuid))
            {
                sqlWhere = $"{sqlWhere} and a.category_guid = '{requestDto.CategoryGuid}'";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.ProductBrandGuid))
            {
                sqlWhere = $"{sqlWhere} and a.brand = '{requestDto.ProductBrandGuid}'";
            }

            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT
	                            a.product_guid AS ProductGuid,
	                            a.product_name AS ProductName,
	                            a.price AS Price,
	                            a.on_sale AS OnSale,
	                            a.inventory AS Inventory 
                            FROM
	                            t_mall_product a 
                            WHERE
	                            a.merchant_guid = '{requestDto.MerchantGuid}' 
	                            AND a.platform_type = 'CloudDoctor' 
	                            AND a.`enable` = 0 { sqlWhere }
                            ORDER BY
	                            a.last_updated_by DESC";
                var result = await MySqlHelper.QueryByPageAsync<GetProductsOfMerchantRecycleBinRequestDto, GetProductsOfMerchantRecycleBinResponseDto, GetProductsOfMerchantRecycleBinItemDto>(sql, requestDto);
                return result;
            }

        }


        /// <summary>
        /// 批量修改商品的上下架属性
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<bool> ChangeProductsOnSaleStatusAsync(ChangeProductsOnSaleStatusRequestDto requestDto, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = "update t_mall_product set on_sale=@onSale where merchant_guid=@merchantGuid and `enable`=@enable and product_guid in @productIds";
                var result = await conn.ExecuteAsync(sql, new { onSale = requestDto.OnSale, merchantGuid = requestDto.MerchantGuid, productIds = requestDto.ProductIds.ToArray(), enable });
                return result > 0;
            }
        }

        /// <summary>
        /// 真删除商铺的商品(必须加入到回收站后才能真删除)
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<bool> DeleteProductsCompletelyAsync(DeleteProductsCompletelyRequestDto requestDto)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.DeleteListAsync<ProductModel>("where merchant_guid=@merchantGuid and  `enable`=0 and product_guid in @productIds", new { merchantGuid = requestDto.MerchantGuid, productIds = requestDto.ProductIds.ToArray() });
                return result > 0;
            }
        }

        /// <summary>
        /// 移除/恢复商品（假删除）
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public async Task<bool> RemoveOrRestoreProductsAsync(string merchantGuid, string[] productIds, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = "update t_mall_product set `enable`=@enable  where merchant_guid=@merchantGuid  and product_guid in @productIds";
                var result = await conn.ExecuteAsync(sql, new { merchantGuid, productIds, enable });
                return result > 0;
            }
        }

        /// <summary>
        /// 分页查询产品列表,只显示总店商品
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public GetProductListByCategoryAndSortResponseDto GetProductListByCategory(GetProductListByCategoryAndSortRequestDto requestDto)
        {
            var isHaveKey = !string.IsNullOrWhiteSpace(requestDto.Keyword) ? $@" LOCATE( '{requestDto.Keyword}', product_name ) " : $@" ( category_guid = @CategoryGuid or category_name=@CategoryName )  ";
            if (requestDto.CategoryGuid.ToLower().Equals("all"))
            {
                isHaveKey = " 1=1 ";
            }
            if (string.IsNullOrWhiteSpace(requestDto.OrderBy))
            {
                requestDto.OrderBy = "p.creation_date ";
            }
            if (string.IsNullOrWhiteSpace(requestDto.DescOrAsc))
            {
                requestDto.DescOrAsc = " desc ";
            }
            var sql = $@"SELECT
	                                p.product_guid,
	                                p.product_name,
	                                p.product_title,
	                                p.standerd,
	                                p.product_label,
	                                p.price,
	                                CONCAT( acc.base_path, acc.relative_path ) AS ProPictureUrl,
	                                sold.soldTotal AS soldTotal,
	                                p.creation_date 
                                FROM
	                                t_mall_product AS p
	                                LEFT JOIN t_merchant AS m ON m.merchant_guid = p.merchant_guid
	                                LEFT JOIN t_utility_accessory AS acc ON p.picture_guid = acc.accessory_guid
	                                LEFT JOIN (
                                SELECT
	                                SUM( a.product_count ) AS soldTotal,
	                                a.product_guid 
                                FROM
	                                t_mall_order_detail a
	                                INNER JOIN t_mall_order b ON a.order_guid = b.order_guid 
	                                AND a.`enable` = b.`enable` 
                                WHERE
	                                b.order_status = 'Completed' 
	                                AND a.`enable` = 1 
                                GROUP BY
	                                a.product_guid 
	                                ) AS sold ON sold.product_guid = p.product_guid 
                                WHERE
	                                { isHaveKey } 
	                                	AND p.`enable` = 1 
	                                    AND p.on_sale = 1 
                                        AND p.platform_on_sale = 1 
	                                    AND m.`enable` = 1 
                                ORDER BY
	                                { requestDto.OrderBy } { requestDto.DescOrAsc } 
	                                 ";
            return MySqlHelper.QueryByPage<GetProductListByCategoryAndSortRequestDto, GetProductListByCategoryAndSortResponseDto, GetProductListByCategoryAndSortItemDto>(sql, requestDto);
            //using (var conn = MySqlHelper.GetConnection())
            //{
            //    return conn.Query<GetProductListByCategoryAndSortResponseDto>(sql, new
            //    {
            //        categoryGuid = requestDto.CategoryGuid,
            //        categoryName = requestDto.CategoryName,
            //        enable,
            //        pageIndex = (requestDto.PageIndex - 1) * requestDto.PageSize,
            //        pageSize = requestDto.PageSize,
            //    }).ToList();
            //}
        }

        /// <summary>
        /// 根据商品类型获取商品列表
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <param name="classifyGuid"></param>
        /// <returns></returns>
        public async Task<List<ProductModel>> GetProductsByClassifyGuidAsync(string merchantGuid, string classifyGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryAsync<ProductModel>(@"SELECT DISTINCT t.* FROM `t_mall_product` as t
                WHERE t.category_guid = @classifyGuid AND t.merchant_guid = @merchantGuid and
                t.enable = 1 and t.on_sale = 1", new { classifyGuid, merchantGuid }))
                .ToList();
            }
        }
        /// <summary>
        /// 分页-查询上架类目商品
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetClassifyProductListResponse> GetClassifyProductListAsync(GetClassifyProductListRequest requestDto)
        {
            var sql = @"SELECT
	                        p.product_guid,
	                        p.product_name,
	                        p.product_title,
	                        p.price,
	                        CONCAT( acc.base_path, acc.relative_path ) AS PictureURL 
                        FROM
	                        t_mall_product AS p
	                        LEFT JOIN t_merchant AS m ON p.merchant_guid = m.merchant_guid
	                        LEFT JOIN t_utility_accessory AS acc ON p.picture_guid = acc.accessory_guid 
                        WHERE
	                        p.merchant_guid = @MerchantGuid 
	                        AND p.category_guid = @DicGuid 
	                        AND p.`enable` = 1 
	                        AND p.product_form = 'Service' 
	                        AND p.on_sale = 1 
	                        AND m.`enable` = 1
                            AND p.`platform_on_sale` = 1 order by p.product_name";
            return await MySqlHelper.QueryByPageAsync<GetClassifyProductListRequest, GetClassifyProductListResponse, GetClassifyProductListItem>(sql, requestDto);
        }

        /// <summary>
        /// 检查门店下商品名是否有重复
        /// </summary>
        /// <param name="name">商品名称</param>
        /// <param name="merchantGuid">门店guid</param>
        /// <param name="productGuid">若不为空，即比对时需排除此商品</param>
        /// <returns>是否重复：true表示重复，false表示没有重复</returns>
        public async Task<bool> CheckProductNameRepeatAsync(string name, string merchantGuid, string productGuid = null)
        {
            var sqlWhere = "";
            if (!string.IsNullOrWhiteSpace(productGuid))
            {
                sqlWhere = "and product_guid<>@productGuid";
            }
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $"select * from t_mall_product where merchant_guid=@merchantGuid and product_name =@name {sqlWhere} limit 1";
                var result = await conn.QueryFirstOrDefaultAsync<ProductModel>(sql, new { name, merchantGuid, productGuid });
                return result != null;
            }
        }

        /// <summary>
        /// 新增商品数据
        /// </summary>
        /// <param name="models"></param>
        /// <param name="richTexts"></param>
        /// <param name="banners"></param>
        /// <returns></returns>
        public async Task<bool> CreateProductAsync(List<ProductModel> models, List<RichtextModel> richTexts, List<BannerModel> banners, List<ProductProjectModel> pps = null)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                await ExcuteBatchSqlAsync(MySqlHelperExtension.CreateBatchInsertSqls(models), conn);
                await ExcuteBatchSqlAsync(MySqlHelperExtension.CreateBatchInsertSqls(richTexts), conn);
                await ExcuteBatchSqlAsync(MySqlHelperExtension.CreateBatchInsertSqls(banners), conn);
                if (pps != null && pps.Any())
                {
                    await ExcuteBatchSqlAsync(MySqlHelperExtension.CreateBatchInsertSqls(pps), conn);
                }
                return true;
            });

        }

        /// <summary>
        /// 批量运行sql
        /// </summary>
        /// <param name="sqls"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        private async Task<bool> ExcuteBatchSqlAsync(List<string> sqls, MySql.Data.MySqlClient.MySqlConnection conn)
        {
            if (!sqls.Any())
            {
                return true;
            }
            foreach (var sql in sqls)
            {
                await conn.ExecuteAsync(sql);
            }
            return true;

        }
        /// <summary>
        /// 获取推荐商品数据列表集合
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetProductListResponseDto> GetRecommendProductListAsync(GetRecommendProductListRequestDto request)
        {
            var sql = $@"
                   SELECT
                        a.product_guid,
	                    a.product_name,
	                    a.price,
                        a.product_form,
	                    CONCAT( acc.base_path, acc.relative_path ) AS PicturePath 
                    FROM
                      t_mall_product a LEFT JOIN t_utility_accessory AS acc ON a.picture_guid = acc.accessory_guid inner JOIN t_merchant m on a.merchant_guid=m.merchant_guid and m.`enable`=a.`enable`
                       where  a.`enable` = 1 and a.recommend = 1 and a.on_sale = 1 AND a.`platform_on_sale` = 1 ORDER BY sort desc";
            return await MySqlHelper.QueryByPageAsync<GetRecommendProductListRequestDto, GetProductListResponseDto, GetProductListItemDto>(sql, request);
        }
        /// <summary>
        /// 获取商品数据列表集合
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetProductListResponseDto> GetProductListAsync(GetByCategoryProductListRequestDto request)
        {
            var sql = $@"
                   SELECT
                      a.product_guid,
	                    a.product_name,
	                    a.price,
                        a.product_form,
	                    CONCAT( acc.base_path, acc.relative_path ) AS PicturePath 
                    FROM
                      t_mall_product a LEFT JOIN t_utility_accessory AS acc ON a.picture_guid = acc.accessory_guid 
	                    LEFT JOIN t_manager_dictionary b ON a.category_guid = b.dic_guid inner JOIN t_merchant m on a.merchant_guid=m.merchant_guid and m.`enable`=a.`enable`
                       where b.parent_guid=@CategoryGuid  and  a.`enable` = 1 and a.on_sale = 1 AND a.`platform_on_sale` = 1 ORDER BY a.sort desc";
            return await MySqlHelper.QueryByPageAsync<GetByCategoryProductListRequestDto, GetProductListResponseDto, GetProductListItemDto>(sql, request);
        }
    }
}
