using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GD.DataAccess;
using GD.Dtos.Mall.Mall;
using GD.Models.CrossTable;
using GD.Models.Mall;
using Dapper;
using System.Data;
using System.Threading.Tasks;
using GD.Models.Manager;

namespace GD.Mall
{
    public class ShoppingCarBiz
    {
        /// <summary>
        /// 通过订单明细Id获取订单明细记录
        /// </summary>
        /// <returns></returns>
        public async Task<List<ShoppingCarModel>> GetModelListAsyncBuUserGuid(string userGuid, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.GetListAsync<ShoppingCarModel>(" where user_guid=@userGuid and `enable`=1", new { userGuid, enable })).ToList();
            }
        }

        /// <summary>
        /// 根据UserGuid查询列表
        /// </summary>
        /// <param name="pageDto"></param>
        /// <param name="userGuid"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public List<GetShoppingCarInfoModel> GetModelListByUserGuid(GetShoppingCartInfoListRequestDto pageDto, string userGuid, bool enable = true, string platformType = "CloudDoctor")
        {
            const string sql = @"SELECT * FROM ( SELECT
	                                                    	scar.user_guid,
	                                                        pro.merchant_guid,
	                                                        mer.merchant_name,
	                                                        pro.product_guid,
	                                                        pro.product_name,
	                                                        CONCAT( acc.base_path, acc.relative_path ) AS ProductPicUrl,
	                                                        pro.standerd,
	                                                        richtxt.content,
	                                                        pro.price,
	                                                        scar.count,
                                                            pro.freight,
                                                            scar.is_valid,
                                                            scar.item_guid
                                                        FROM
	                                                        t_mall_shopping_car AS scar
	                                                        LEFT JOIN t_mall_product AS pro ON scar.product_guid = pro.product_guid
	                                                        LEFT JOIN t_merchant AS mer ON pro.merchant_guid = mer.merchant_guid
	                                                        LEFT JOIN t_utility_accessory AS acc ON pro.picture_guid = acc.accessory_guid 
	                                                        left join t_utility_richtext as richtxt on pro.introduce_guid=richtxt.text_guid
                                                        WHERE
	                                                        scar.`enable` = @enable and scar.platform_type=@platformType
                                                        ORDER BY scar.creation_date DESC 
	                                                        ) AS T 
                                                    WHERE
	                                                    T.user_guid = @userGuid 
	                                                    LIMIT @pageInde,@pageSize";
            var modelList = MySqlHelper.Select<GetShoppingCarInfoModel>(sql, new { userGuid, enable, pageInde = (pageDto.PageIndex - 1) * pageDto.PageSize, pageSize = pageDto.PageSize, platformType }).ToList();
            return modelList;
        }

        /// <summary>
        /// 获取用户购物车记录总数
        /// </summary>
        /// <param name="userId">用户Guid</param>
        /// <param name="enable"></param>
        /// <param name="platformType"></param>
        /// <returns></returns>
        public GetMyShoppingCarTotalResponseDto GetMyShoppingCarTotal(string userId, bool enable = true, string platformType = "CloudDoctor")
        {
            const string sql = "select Sum(count) as ShoppingCarNum from t_mall_shopping_car where user_guid=@userId and enable=@enable and platform_type=@platformType";
            return MySqlHelper.SelectFirst<GetMyShoppingCarTotalResponseDto>(sql, new { userId, enable, platformType });
        }

        /// <summary>
        /// 获取用户购物车中指定商品的记录
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="productId">产品Id</param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task<ShoppingCarModel> GetShoppingCarProductByUserId(string userId, string productId, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<ShoppingCarModel>(" select * from t_mall_shopping_car  where user_guid = @userId and product_guid=@productId and enable = @enable", new { userId, productId, enable });
            }
        }

        /// <summary>
        /// 根据Guid查询
        /// </summary>
        /// <param name="itemGuid"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public ShoppingCarModel GetModelByGuid(string itemGuid, bool enable = true)
        {
            const string sql = "select * from t_mall_shopping_car where item_guid=@itemGuid and enable=@enable";
            var model = MySqlHelper.SelectFirst<ShoppingCarModel>(sql, new { itemGuid, enable });
            return model;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Add(ShoppingCarModel model)
        {
            return model.Insert();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int? Delete(ShoppingCarModel model)
        {
            return model.Delete();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int? Update(ShoppingCarModel model)
        {
            model.LastUpdatedDate = DateTime.Now;
            return model.Update();
        }
        /// <summary>
        /// 批量更新model
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public bool UpdateModels(List<ShoppingCarModel> models)
        {
            if (!models.Any())
            {
                return true;
            }
            return MySqlHelper.Transaction((conn, tran) =>
             {
                 foreach (var model in models)
                 {
                     if (model.Update(conn) != 1)
                     {
                         return false;
                     }
                 }
                 return true;
             });
        }

        /// <summary>
        /// 批量更新model
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task<bool> AddShoppingCarModelListAsnyc(List<ShoppingCarModel> modelList)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                foreach (var model in modelList)
                {
                    if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, ShoppingCarModel>(model))) { return false; }
                }
                return true;
            });
        }
        /// <summary>
        /// 修改购物车商品数量
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool ChangeShoppingCarProductNumber(string itemId, int number)
        {

            return true;
            //var sql = @"UPDATE t_mall_shopping_car AS car
            //            INNER JOIN t_mall_product AS product ON car.product_guid = product.product_guid 
            //            SET car.count = @number,
            //            car.is_valid =
            //            CASE

            //             WHEN product.inventory >= @number THEN
            //             1 ELSE 0 
            //            END 
            //            WHERE
            //             car.item_guid = @itemId 
            //             AND car.`enable` = 1 
            //             AND product.`enable` = 1;";
            //using (IDbConnection cnn = MySqlHelper.GetConnection())
            //{
            //    return cnn.Execute(sql, new { itemId, number })==1;
            //}

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> DeleteListByIdsAsync(string[] ItemGuidArr)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.DeleteListAsync<ShoppingCarModel>("where item_guid in @ItemGuidArr", new { ItemGuidArr });
            }
        }
        ///// <summary>
        ///// 云医-获取购物车列表数据
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public async Task<List<ShowMyShoppingCartListOfCosmetologyResponseDto>> GetMyShoppingCartListOfCosmetologyAsync(string userId)
        //{
        //    using (var conn = MySqlHelper.GetConnection())
        //    {
        //        var sql = @"SELECT
	       //                     car.item_guid AS ItemGuid,
	       //                     car.merchant_guid AS MerchantGuid,
	       //                     merchant.merchant_name AS MerchantName,
        //                        merchant.`enable` as MerchantEnable,
	       //                     car.product_guid AS ProductGuid,
	       //                     product.product_name AS ProductName,
	       //                     product.price AS ProductPrice,
        //                        product.on_sale as OnSale,
        //                        product.product_form AS ProductForm,
	       //                     CONCAT( acc.base_path, acc.relative_path ) AS ProductPicture,
	       //                     product.allow_advance_payment AS AllowAdvancePayment,
	       //                     product.advance_payment_rate AS AdvancePaymentRate,
	       //                     car.count AS ProductCount
        //                    FROM
	       //                     t_mall_shopping_car car
	       //                     INNER JOIN t_merchant merchant ON car.merchant_guid = merchant.merchant_guid
	       //                     INNER JOIN t_mall_product product ON car.product_guid = product.product_guid
	       //                     INNER JOIN t_utility_accessory acc ON product.picture_guid = acc.accessory_guid 
	       //                     AND acc.`enable` = 1
	
        //                    WHERE
	       //                     car.user_guid = @userId
	       //                     AND car.`enable` = 1 
	       //                     AND ( car.platform_type = 'CloudDoctor' OR car.platform_type = 'MedicalCosmetology' ) 
	       //                     AND product.`enable` = 1 ;
	
        //                    SELECT DISTINCT
	       //                     car.product_guid,
	       //                     project.project_guid AS ProjectGuid,
	       //                     project.project_name AS ProjectName,
	       //                     pp.project_times AS ProjectTimes 
        //                    FROM
	       //                     t_mall_shopping_car car
	       //                     LEFT JOIN t_mall_product_project pp ON car.product_guid = pp.product_guid
	       //                     LEFT JOIN t_mall_project project ON pp.project_guid = project.project_guid 
        //                    WHERE
	       //                     car.user_guid = @userId 
	       //                     AND car.`enable` = 1 
	       //                     AND ( car.platform_type = 'CloudDoctor' OR car.platform_type = 'MedicalCosmetology' );";
        //        var reader = await conn.QueryMultipleAsync(sql, new { userId });
        //        var shopcarProducts = (await reader.ReadAsync<ShoppingCartListDetailOfCosmetologyDto>())?.ToList();
        //        var productProjects = (await reader.ReadAsync<ShoppingCarListProductProjectInfo>())?.ToList();

        //        var merchantGroup = shopcarProducts.GroupBy(a => a.MerchantGuid);

        //        List<ShowMyShoppingCartListOfCosmetologyResponseDto> response = new List<ShowMyShoppingCartListOfCosmetologyResponseDto>();
        //        foreach (IGrouping<string, ShoppingCartListDetailOfCosmetologyDto> item in merchantGroup)
        //        {
        //            if (response.Where(a => a.MerchantGuid == item.Key).Count() > 0)
        //            {
        //                continue;
        //            }

        //            var merchant = item.First();
        //            var dto = new ShowMyShoppingCartListOfCosmetologyResponseDto
        //            {
        //                MerchantGuid = item.Key,
        //                MerchantName = merchant.MerchantName,
        //                MerchantEnable = merchant.MerchantEnable,
        //                Products = item.ToList().Select(a => new ShoppingCarListProductInfo
        //                {
        //                    ItemGuid = a.ItemGuid,
        //                    ProductGuid = a.ProductGuid,
        //                    OnSale = a.OnSale,
        //                    ProductName = a.ProductName,
        //                    ProductPrice = a.ProductPrice,
        //                    ProductForm = a.ProductForm,
        //                    ProductPicture = a.ProductPicture,
        //                    AllowAdvancePayment = a.AllowAdvancePayment,
        //                    AdvancePaymentRate = a.AdvancePaymentRate,
        //                    ProductCount = a.ProductCount,
        //                    Projects = productProjects.Where(b => b.ProductGuid == a.ProductGuid).Select(b => new ShoppingCarListProductProjectInfo
        //                    {
        //                        ProductGuid = b.ProductGuid,
        //                        ProjectGuid = b.ProjectGuid,
        //                        ProjectName = b.ProjectName,
        //                        ProjectTimes = b.ProjectTimes
        //                    }).ToList()
        //                }).ToList()
        //            };
        //            response.Add(dto);
        //        }
        //        return response;
        //    }
        //}


        ///// <summary>
        ///// 获取购物车列表数据
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public async Task<List<ShowMyShoppingCartListOfCosmetologyResponseDto>> GetMyShoppingCartListByCosmetologyAsync(string userId)
        //{
        //    using (var conn = MySqlHelper.GetConnection())
        //    {
        //        var sql = @"SELECT
	       //                     car.item_guid AS ItemGuid,
	       //                     car.merchant_guid AS MerchantGuid,
	       //                     merchant.merchant_name AS MerchantName,
        //                        merchant.`enable` as MerchantEnable,
	       //                     car.product_guid AS ProductGuid,
	       //                     product.product_name AS ProductName,
	       //                     product.price AS ProductPrice,
        //                        product.on_sale as OnSale,
	       //                     CONCAT( acc.base_path, acc.relative_path ) AS ProductPicture,
	       //                     product.allow_advance_payment AS AllowAdvancePayment,
	       //                     product.advance_payment_rate AS AdvancePaymentRate,
	       //                     car.count AS ProductCount
        //                    FROM
	       //                     t_mall_shopping_car car
	       //                     INNER JOIN t_merchant merchant ON car.merchant_guid = merchant.merchant_guid
	       //                     INNER JOIN t_mall_product product ON car.product_guid = product.product_guid
	       //                     LEFT JOIN t_utility_accessory acc ON product.picture_guid = acc.accessory_guid 
	       //                     AND acc.`enable` = 1
	
        //                    WHERE
	       //                     car.user_guid = @userId
	       //                     AND car.`enable` = 1 
	       //                     AND ( car.platform_type = 'LifeCosmetology' OR car.platform_type = 'MedicalCosmetology' ) 
	       //                     AND product.`enable` = 1 ;
	
        //                    SELECT DISTINCT
	       //                     car.product_guid,
	       //                     project.project_guid AS ProjectGuid,
	       //                     project.project_name AS ProjectName,
	       //                     pp.project_times AS ProjectTimes 
        //                    FROM
	       //                     t_mall_shopping_car car
	       //                     INNER JOIN t_mall_product_project pp ON car.product_guid = pp.product_guid
	       //                     INNER JOIN t_mall_project project ON pp.project_guid = project.project_guid 
        //                    WHERE
	       //                     car.user_guid = @userId 
	       //                     AND car.`enable` = 1 
	       //                     AND ( car.platform_type = 'LifeCosmetology' OR car.platform_type = 'MedicalCosmetology' );";
        //        var reader = await conn.QueryMultipleAsync(sql, new { userId });
        //        var shopcarProducts = (await reader.ReadAsync<ShoppingCartListDetailOfCosmetologyDto>())?.ToList();
        //        var productProjects = (await reader.ReadAsync<ShoppingCarListProductProjectInfo>())?.ToList();

        //        var merchantGroup = shopcarProducts.GroupBy(a => a.MerchantGuid);

        //        List<ShowMyShoppingCartListOfCosmetologyResponseDto> response = new List<ShowMyShoppingCartListOfCosmetologyResponseDto>();
        //        foreach (IGrouping<string, ShoppingCartListDetailOfCosmetologyDto> item in merchantGroup)
        //        {
        //            if (response.Where(a => a.MerchantGuid == item.Key).Count() > 0)
        //            {
        //                continue;
        //            }

        //            var merchant = item.First();
        //            var dto = new ShowMyShoppingCartListOfCosmetologyResponseDto
        //            {
        //                MerchantGuid = item.Key,
        //                MerchantName = merchant.MerchantName,
        //                MerchantEnable = merchant.MerchantEnable,
        //                Products = item.ToList().Select(a => new ShoppingCarListProductInfo
        //                {
        //                    ItemGuid = a.ItemGuid,
        //                    ProductGuid = a.ProductGuid,
        //                    OnSale = a.OnSale,
        //                    ProductName = a.ProductName,
        //                    ProductPrice = a.ProductPrice,
        //                    ProductForm = a.ProductForm,
        //                    ProductPicture = a.ProductPicture,
        //                    AllowAdvancePayment = a.AllowAdvancePayment,
        //                    AdvancePaymentRate = a.AdvancePaymentRate,
        //                    ProductCount = a.ProductCount,
        //                    Projects = productProjects.Where(b => b.ProductGuid == a.ProductGuid).Select(b => new ShoppingCarListProductProjectInfo
        //                    {
        //                        ProductGuid = b.ProductGuid,
        //                        ProjectGuid = b.ProjectGuid,
        //                        ProjectName = b.ProjectName,
        //                        ProjectTimes = b.ProjectTimes
        //                    }).ToList()
        //                }).ToList()
        //            };
        //            response.Add(dto);
        //        }
        //        return response;
        //    }
        //}

        /// <summary>
        /// 双美-获取购物车已选商品详细信息
        /// </summary>
        /// <param name="itemIds">购物车记录Id数组</param>
        /// <returns></returns>
        public async Task<List<ShoppingCarProductDetailDto>> GetShoppingCarSelectedProductDetail(string[] itemIds)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"DROP TEMPORARY TABLE
                            IF
	                            EXISTS tmp_shopping_car;
                            CREATE TEMPORARY TABLE tmp_shopping_car AS SELECT
                            * 
                            FROM
	                            t_mall_shopping_car 
                            WHERE
	                            `enable` = 1 
	                            AND item_guid IN @itemIds;
                            SELECT
                                car.item_guid,

	                            car.product_guid AS ProductGuid,
                                car.advance_payment as AdvancePayment,
                                product.product_name AS ProductName,
	                            product.price AS ProductPrice,
                                product.effective_days as EffectiveDays,
                                product.platform_type as PlatformType,
                                product.advance_payment_rate as AdvancePaymentRate,
                                product.project_threshold as ProjectThreshold,
                                product.freight,
                                product.product_form,

	                            car.count AS ProductCount,
	                            car.merchant_guid AS MerchantGuid ,
	                            project.project_guid as ProjectGuid,
	                            project.project_name as ProjectName,
	                            pp.project_times as ProjectTimes,
                                pp.allow_present as AllowPresent,
                                project.price as ProjectPrice
                            FROM
	                            tmp_shopping_car car
	                            INNER JOIN t_mall_product product ON car.product_guid = product.product_guid
	                            LEFT JOIN t_mall_product_project pp ON car.product_guid = pp.product_guid  AND pp.`enable` = 1 
	                            LEFT JOIN t_mall_project project ON pp.project_guid = project.project_guid   AND project.`enable` = 1
                            WHERE
	                            product.`enable` = 1    ;
                            DROP TEMPORARY TABLE tmp_shopping_car;";
                return (await conn.QueryAsync<ShoppingCarProductDetailDto>(sql, new { itemIds }))?.ToList();
            }
        }
    }
}
