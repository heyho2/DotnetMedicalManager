using Dapper;
using GD.DataAccess;
using GD.Dtos.Mall.Groupbuy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GD.Mall
{
    /// <summary>
    /// 团购 业务类
    /// </summary>
    public class GroupbuyBiz
    {
        /// <summary>
        /// 获取首页团购
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        public async Task<IEnumerable<GetHomeGroupbuyItemDto>> GetHomeGroupbuyAsync(GetHomeGroupbuyRequestDto request)
        {
            var sql = $@"
SELECT
	A.*,
	B.product_name,
	B.standerd,
	CONCAT( C.base_path, C.relative_path ) AS PictureUrl 
FROM
	t_mall_groupbuy A
	LEFT JOIN t_mall_product B ON B.product_guid = A.product_guid
	LEFT JOIN t_utility_accessory C ON C.accessory_guid = B.picture_guid 
WHERE
	A.end_date >= now( ) 
	AND A.ENABLE =1
ORDER BY
	A.sort DESC 
LIMIT {request.Take}
";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<GetHomeGroupbuyItemDto>(sql);
                return result;
            }
        }

        /// <summary>
        /// 生美- 拼团聚划算列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetRecommendGroupBuyProductResponseDto> GetRecommendGroupBuyProductListAsync(GetRecommendGroupBuyProductRequestDto requestDto)
        {
            string sql = @"SELECT
	                                    gb.groupbuy_guid,
	                                    gb.NAME,
	                                    gb.product_guid,
	                                    CONCAT( acc.base_path, acc.relative_path ) AS PictureUrl 
                                    FROM
	                                    t_mall_groupbuy AS gb
	                                    LEFT JOIN t_mall_product AS pro ON gb.product_guid = pro.product_guid
	                                    LEFT JOIN t_utility_accessory AS acc ON pro.picture_guid = acc.accessory_guid 
                                    WHERE
                                        gb.recommend = 1 
	                                    AND gb.lump = 0 
	                                    AND gb.ENABLE = @Enable 
	                                    and gb.platform_type = @PlatformType
                                    ORDER BY
	                                    gb.sort ";
            return await MySqlHelper.QueryByPageAsync<GetRecommendGroupBuyProductRequestDto, GetRecommendGroupBuyProductResponseDto, GetRecommendGroupBuyProductItem>(sql, requestDto);
        }
        /// <summary>
        /// 生美- 拼团聚划算列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetGroupBuyProductListResponseDto> GetGroupBuyListAsync(GetGroupBuyProductListRequestDto requestDto)
        {
            string sql = @"SELECT
	                                    gb.groupbuy_guid,
	                                    gb.NAME,
	                                    gb.product_guid,
	                                    CONCAT( acc.base_path, acc.relative_path ) AS PictureUrl,
	                                    gb.price,
	                                    pro.price,
	                                    gb.qty,
	                                    ( SELECT count( * ) FROM t_mall_groupbuy_detail WHERE gb.groupbuy_guid = groupbuy_guid ) AS bought 
                                    FROM
	                                    t_mall_groupbuy AS gb
	                                    LEFT JOIN t_mall_product AS pro ON gb.product_guid = pro.product_guid
	                                    LEFT JOIN t_utility_accessory AS acc ON pro.picture_guid = acc.accessory_guid 
                                    WHERE
	                                     gb.lump = 0 
	                                    AND gb.ENABLE = @Enable 
	                                    AND gb.platform_type = @PlatformType 
                                    ORDER BY
	                                    gb.sort ";
            return await MySqlHelper.QueryByPageAsync<GetGroupBuyProductListRequestDto, GetGroupBuyProductListResponseDto, GetGroupBuyProductItem>(sql, requestDto);
        }
        /// <summary>
        /// 生美-	获取商品团购信息
        /// 有疑问的接口
        /// </summary>
        /// <returns></returns>
        public async Task<GetProductGroupBuyInfoResponseDto> GetProductGroupBuyInfoAsync(GetProductGroupBuyInfoRequestDto requestDto)
        {
            var sql = @"SELECT
	                                `NAME`,
	                                qty,
	                                buy_qty,
	                                price 
                                FROM
	                                t_mall_groupbuy 
                                WHERE
	                                product_guid = @productGuid 
	                                AND `enable` = @enable 
                                ORDER BY
	                                sort    ";
            return await MySqlHelper.QueryByPageAsync<GetProductGroupBuyInfoRequestDto, GetProductGroupBuyInfoResponseDto, GetProductGroupBuyInfoItem>(sql, requestDto);
        }
    }
}
