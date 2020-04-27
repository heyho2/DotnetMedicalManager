using GD.DataAccess;
using GD.Dtos.Mall.Mall;
using GD.Models.Mall;
using System.Threading.Tasks;

namespace GD.Manager.Mall
{
    public class ProductBiz : BaseBiz<ProductModel>
    {
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
        /// 商品中心查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ProductCenterResponseDto> ProductCenterAsync(ProductCenterRequestDto request)
        {
            var sqlWhere = $@"AND p.ENABLE = 1 ";

            if (!string.IsNullOrWhiteSpace(request.ProductName))
            {
                sqlWhere = $"{sqlWhere} AND p.product_name like @ProductName";
                request.ProductName = $"%{request.ProductName}";
            }
            if (!string.IsNullOrWhiteSpace(request.ProductCode))
            {
                sqlWhere = $"{sqlWhere} AND p.product_code like @ProductCode";
                request.ProductCode = $"%{request.ProductCode}";
            }
            if (!string.IsNullOrWhiteSpace(request.MerchantGuid))
            {
                sqlWhere = $"{sqlWhere} AND p.merchant_guid=@MerchantGuid";
            }
            if (!string.IsNullOrWhiteSpace(request.CategoryName))
            {
                sqlWhere = $"{sqlWhere} AND p.category_name=@CategoryName";
            }
            if (request.ProductStatus.HasValue)
            {
                sqlWhere = $"{sqlWhere} AND p.on_sale=@ProductStatus";
                request.ProductStatus = request.ProductStatus.Value;
            }
            var sql = $@"
                            SELECT
                            p.product_guid,
	                        m.merchant_name,
	                        p.product_form,
	                        p.category_name,
	                        product_code,
	                        p.product_name,
	                        p.price,
	                        p.inventory,
	                        a.salecount,
	                        p.on_sale,
	                        p.platform_on_sale,
	                        p.effective_days,
	                        p.recommend,
	                        p.sort 
                        FROM
	                        t_mall_product p
	                        LEFT JOIN t_merchant m ON p.merchant_guid = m.merchant_guid
	                        LEFT JOIN (
	                        SELECT
		                        od.product_guid,
	                          IFNULL(sum(od.product_count ), 0 ) salecount
	                        FROM
		                        t_mall_order_detail od RIGHT JOIN  t_mall_order o ON o.order_guid = od.order_guid 
		                        WHERE o.order_status ='Completed' AND o.`enable` = 1 AND od.`enable` = 1
	                        GROUP BY
	                        od.product_guid
	                        ) a on a.product_guid=p.product_guid
                                            WHERE
	                                            1 = 1 {sqlWhere}
                                            ORDER BY
	                                            p.creation_date";
            return await MySqlHelper.QueryByPageAsync<ProductCenterRequestDto, ProductCenterResponseDto, ProductCenterItemDto>(sql, request);

        }
    }
}
