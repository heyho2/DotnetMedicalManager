using Dapper;
using GD.BizBase;
using GD.DataAccess;
using GD.Dtos.Consumer.Consumer;
using GD.Dtos.Mall.Mall;
using GD.Models.Consumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Consumer
{
    /// <summary>
    /// 个人卡实体业务类
    /// </summary>
    public class GoodsBiz : BaseBiz<GoodsModel>
    {
        /// <summary>
        /// 检测个人商品使用项目次数是否达到阈值
        /// </summary>
        /// <param name="goodsGuid"></param>
        /// <returns>若有记录返回，表示达到了阈值；若无记录返回表示未达到阈值</returns>
        public async Task<CheckGoodsThresholdIsExceededDto> CheckGoodsThresholdIsExceededAsync(string goodsGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            a.goods_guid,
	                            a.project_threshold,
	                            sum( b.used ) AS project_used_sum 
                            FROM
	                            t_consumer_goods a
	                            INNER JOIN t_consumer_goods_item b ON a.goods_guid = b.goods_guid 
                            WHERE
	                            a.goods_guid = @goodsGuid
	                            AND IFNULL( project_threshold, 0 ) > 0 
                            GROUP BY
	                            a.goods_guid,
	                            a.project_threshold 
                            HAVING
	                            sum( b.used ) >= project_threshold";
                var result = await conn.QueryFirstOrDefaultAsync<CheckGoodsThresholdIsExceededDto>(sql, new { goodsGuid });
                return result;
            }
        }

        /// <summary>
        /// 检测个人商品是否已经用完
        /// </summary>
        /// <param name="goodsGuid">个人商品guid</param>
        /// <returns>返回true表示已经用完，返回false表示没用完</returns>
        public async Task<bool> CheckGoodsHasRunOutAsync(string goodsGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            a.* 
                            FROM
	                            t_consumer_goods a
	                            INNER JOIN t_consumer_goods_item b ON a.goods_guid = b.goods_guid
                            WHERE
	                            a.goods_guid = @goodsGuid and b.count>0
	                            AND b.remain > 0 
                            LIMIT 1;";
                var result = await conn.QueryFirstOrDefaultAsync<GoodsModel>(sql, new { goodsGuid });
                return result == null;
            }
        }


        /// <summary>
        /// 查询个人可用商品和商品项明细
        /// </summary>
        /// <returns></returns>
        public async Task<(IEnumerable<GoodsItemDetailDto>, int)> GetUseableGoodsListOfCosmetologyAsync(string userId, GetUseableOrderListOfCosmetologyRequestDto requestDto)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var stateWhere = string.Empty;
                if (requestDto.GoodsState == GetUseableOrderListOfCosmetologyRequestDto.GoodsStateEnum.Usable)
                {
                    stateWhere = "AND goods.available = 1 AND ( goods.effective_end_date IS NULL OR (goods.effective_end_date is not null and goods.effective_end_date >= CONCAT( DATE_FORMAT( NOW( ), '%Y-%m-%d' ), ' ', '00:00:00' ) )) ";
                }
                var sql = $@"DROP TEMPORARY TABLE
                            IF
	                            EXISTS tmpGoods;
                            CREATE TEMPORARY TABLE tmpGoods AS
                            SELECT
                            goods.*,
                            o.order_no,
                            o.creation_date AS order_date ,
                            case when goods.effective_end_date is null then 0 when goods.effective_end_date<CONCAT(DATE_FORMAT(NOW(),'%Y-%m-%d'),' ','00:00:00') then 1 
                            else 0 
                            end as is_effective
                            FROM
	                            t_mall_order o
	                            INNER JOIN t_consumer_goods goods ON goods.order_guid = o.order_guid 
                            WHERE
	                            o.user_guid = @userId 
	                            AND o.order_status = 'Completed' 
	                            AND o.order_mark = 'Secondary' 
	                            AND o.order_category = 'Service' 
	                            AND o.`enable` = 1 
	                            AND goods.`enable` = 1 {stateWhere}
                            ORDER BY
	                            o.creation_date DESC
	                            LIMIT @pageIndex,
	                            @pageSize;

                            select count(1) as count
	                            FROM
	                            t_mall_order o
	                            INNER JOIN t_consumer_goods goods ON goods.order_guid = o.order_guid 
                            WHERE
	                            o.user_guid = @userId 
	                            AND o.order_status = 'Completed' 
	                            AND o.order_mark = 'Secondary' 
	                            AND o.order_category = 'Service' 
	                            AND o.`enable` = 1 
	                            AND goods.`enable` = 1 { stateWhere } 
                            ORDER BY
	                            o.creation_date DESC ;

                            SELECT
	                            goods.order_no,
	                            goods.goods_guid AS GoodsGuid,
                                goods.is_effective,
                                goods.available as Available,
	                            product.product_name AS ProductName,
                                goods.project_threshold as ProjectThreshold,
	                            goods.order_date AS OrderDate,
	                            goods.effective_start_date AS EffectiveStartDate,
	                            goods.effective_end_date AS EffectiveEndDate,
	                            goodsItem.goods_item_guid AS GoodsItemGuid,
	                            project.project_guid AS ProjectGuid,
	                            project.project_name AS ProjectName,
	                            project.operation_time AS OperationTime,
	                            goodsItem.count AS ItemCount,
	                            goodsItem.remain AS ItemRemain,
	                            goodsItem.available AS ItemAvailable ,
	                            CONCAT(acc.base_path,acc.relative_path) as ProductPicture
                            FROM
	                            tmpGoods goods
	                            INNER JOIN t_consumer_goods_item goodsItem ON goods.goods_guid = goodsItem.goods_guid
	                            INNER JOIN t_mall_project project ON goodsItem.project_guid = project.project_guid 
	                            AND project.`enable` = 1 
	                            left join t_mall_product product on product.product_guid=goods.product_guid
	                            left join t_utility_accessory acc on acc.accessory_guid=product.picture_guid
                            WHERE
	                            goodsItem.`enable` = 1;
                            DROP TEMPORARY TABLE tmpGoods;";
                var result = await conn.QueryMultipleAsync(sql, new { userId, pageIndex = (requestDto.PageIndex - 1) * requestDto.PageSize, pageSize = requestDto.PageSize, requestDto.GoodsState });
                var dataCount = (await result.ReadAsync<GetUseableOrderListCount>()).FirstOrDefault().Count;
                var dataList = await result.ReadAsync<GoodsItemDetailDto>();
                return (dataList, dataCount);
                //var lst = await conn.QueryAsync<GoodsItemDetailDto>(sql, new { userId, pageIndex = (requestDto.PageIndex - 1) * requestDto.PageSize, pageSize = requestDto.PageSize, requestDto.GoodsState });
                //return lst?.ToList();

            }
        }

        /// <summary>
        /// 获取用户服务项目可用的卡
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<List<GoodsModel>> GetAvailableGoods(string userId)
        {
            var sql = @"SELECT * FROM t_consumer_goods as g
                INNER JOIN t_mall_order as o ON g.order_guid = o.order_guid
            WHERE o.user_guid = @userId AND o.order_status = 'Completed' AND o.order_category = 'Service' AND available = 1
            ORDER BY g.creation_date DESC";

            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryAsync<GoodsModel>(sql, new { userId })).ToList();
            }
        }


        /// <summary>
        /// 获取当前用户待使用的项目数量
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetUseableGoodsItemNumAsync(string userId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                                    count( goodsItem.goods_item_guid ) AS remain 
                                    FROM
	                                    t_mall_order o
	                                    INNER JOIN t_consumer_goods goods ON goods.order_guid = o.order_guid
	                                    INNER JOIN t_consumer_goods_item goodsItem ON goods.goods_guid = goodsItem.goods_guid 
                                    WHERE
	                                    o.user_guid =@userId
	                                    AND o.order_status = 'Completed' 
	                                    AND o.order_mark = 'Secondary' 
	                                    AND o.order_category = 'Service' 
	                                    AND goods.available = 1 
	                                    AND o.`enable` = 1 
	                                    AND goods.`enable` = 1 
	                                    AND goodsItem.`enable` = 1 
	                                    AND ( goods.effective_end_date IS NULL OR goods.effective_end_date > CONCAT( DATE_FORMAT( NOW( ), '%Y-%m-%d' ), ' ', '23:59:59' ) ) 
	                                    AND ( o.platform_type = 'CloudDoctor' OR o.platform_type = 'MedicalCosmetology' ) ;";
                var countNum = await conn.QueryFirstOrDefaultAsync<int>(sql, new { userId });
                return countNum;

            }
        }

        /// <summary>
        /// 通过主键集合获取models
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<GoodsModel>> GetModelsAsync(IEnumerable<string> ids)
        {
            var distinctIds = ids.Distinct();
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<GoodsModel>("where goods_guid in @distinctIds", new { distinctIds });
                return result.ToList();
            }
        }

        /// <summary>
        /// 通过订单明细Id集合获取model集合
        /// </summary>
        /// <param name="orderDetailIds"></param>
        /// <returns></returns>
        public async Task<List<GoodsModel>> GetModelsByOrderDetailIdAsync(IEnumerable<string> orderDetailIds, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<GoodsModel>("where detail_guid in @orderDetailIds and `enable`= @enable ", new { orderDetailIds, enable });
                return result.ToList();
            }
        }

        /// <summary>
        /// 获取用户临近过期的卡数据列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<GetMyGoodsListNearExpirationResponseDto>> GetMyGoodsListNearExpirationAsync(string userGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT
	                            b.goods_guid,
	                            b.product_name,
	                            b.effective_end_date 
                            FROM
	                            t_mall_order a
	                            INNER JOIN t_consumer_goods b ON a.order_guid = b.order_guid 
	                            AND a.`enable` = b.`enable`
                            WHERE
	                            a.order_status = 'Completed' 
	                            AND a.`enable` = 1 
	                            AND b.available = 1 
	                            AND b.effective_start_date IS NOT NULL 
	                            AND b.effective_end_date > CONCAT( DATE_FORMAT( NOW( ), '%Y-%m-%d' ), ' ', '00:00:00' ) 
	                            AND b.effective_end_date < CONCAT( DATE_FORMAT( date_add( NOW( ), INTERVAL 15 DAY ), '%Y-%m-%d' ), ' ', '23:59:59' ) 
                                AND b.user_guid=@userGuid
                            ORDER BY
	                            b.effective_end_date;";
                var result = await conn.QueryAsync<GetMyGoodsListNearExpirationResponseDto>(sql, new { userGuid });
                return result?.ToList();
            }
        }

        /// <summary>
        /// 获取消费者从未使用过的项目列表
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public async Task<List<GetMyUnusedGoodsItemListResponseDto>> GetMyUnusedGoodsItemListAsync(string userGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT
                                c.goods_item_guid,
                                d.project_name,
                                c.project_guid,
                                d.operation_time,
                                c.remain,
                                b.effective_start_date,
                                b.effective_end_date
                            FROM
	                            t_mall_order a
	                            INNER JOIN t_consumer_goods b ON a.order_guid = b.order_guid 
	                            AND a.`enable` = b.`enable`
	                            INNER JOIN t_consumer_goods_item c ON c.goods_guid = b.goods_guid 
	                            AND b.`enable` = c.`enable` 
	                            left join t_mall_project d on d.project_guid=c.project_guid and d.`enable`=1
                            WHERE
	                            a.order_status = 'Completed' 
	                            AND a.`enable` = 1 
	                            AND b.available = 1 
	                            AND c.available = 1 
	                            AND c.count > 0 
	                            AND c.used = 0 
	                            AND IFNULL( b.effective_end_date, date_add( NOW( ), INTERVAL 1 DAY ) ) > CONCAT( DATE_FORMAT( NOW( ), '%Y-%m-%d' ), ' ', '00:00:00' )
	                            and b.user_guid=@userGuid;";
                var result = await conn.QueryAsync<GetMyUnusedGoodsItemListResponseDto>(sql, new { userGuid });
                return result?.ToList();
            }
        }
    }
}
