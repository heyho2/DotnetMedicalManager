using Dapper;
using GD.BizBase;
using GD.DataAccess;
using GD.Dtos.Mall.Mall;
using GD.Dtos.Mall.Order;
using GD.Models.Consumer;
using GD.Models.Mall;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Mall
{
    /// <summary>
    ///售后业务类
    /// </summary>
    public class AfterSaleServiceBiz : BaseBiz<AfterSaleServiceModel>
    {
        public async Task<GetAfterSaleServiceListResponseDto> GetServicePageListAsync(GetAfterSaleServiceListRequestDto requestDto)
        {
            var sqlWhere = string.Empty;

            if (!string.IsNullOrEmpty(requestDto.ServiceNo))
            {
                sqlWhere = "and ass.service_no = @ServiceNo";
            }

            if (requestDto.AfterSaleStatus.HasValue)
            {
                sqlWhere = $"{sqlWhere} and ass.`status` = @AfterSaleStatus";
            }

            if (!string.IsNullOrEmpty(requestDto.OrderNo))
            {
                sqlWhere = $"{sqlWhere} and o.order_no = @OrderNo";
            }

            if (requestDto.ApplicationBeginTime.HasValue)
            {
                sqlWhere = $"{sqlWhere} and ass.creation_date >= @ApplicationBeginTime";
            }

            if (requestDto.ApplicationEndTime.HasValue)
            {
                requestDto.ApplicationEndTime = requestDto.ApplicationEndTime.Value.AddDays(1);

                sqlWhere = $"{sqlWhere} and ass.creation_date <= @ApplicationEndTime";
            }

            var sql = $@"SELECT
                                ass.service_guid as ServiceGuid, -- 服务单Guid
                                ass.service_no as ServiceNo,-- 服务编号
	                            o.order_no AS OrderNo,-- 订单编号
	                            o.order_status as OrderStatus,-- 订单状态
	                            u.phone, -- 用户手机号码
	                            u.nick_name as NickName,-- 用户昵称
	                            ass.type AS `Type`,-- 售后类型
	                            ass.creation_date as ApplicationTime, -- 申请时间
	                            ass.`status` as AfterSaleStatus -- 退款状态
                            FROM t_mall_aftersale_service AS ass
	                            INNER JOIN t_mall_order as o ON ass.order_guid = o.order_guid AND ass.user_guid = o.user_guid AND ass.merchant_guid = o.merchant_guid
	                            INNER JOIN t_utility_user as u ON u.user_guid = ass.user_guid
                            WHERE ass.merchant_guid = @MerchantGuid {sqlWhere}
                        ORDER BY
	                        ass.creation_date desc";

            return await MySqlHelper.QueryByPageAsync<GetAfterSaleServiceListRequestDto, GetAfterSaleServiceListResponseDto, AfterSaleServiceItem>(sql, requestDto);
        }


        /// <summary>
        /// 获取服务详情
        /// </summary>
        /// <param name="serviceGuid"></param>
        /// <returns></returns>
        public async Task<GetServiceDetailResponseDto> GetServiceDetail(string merchantGuid, string serviceGuid)
        {
            var detail = (GetServiceDetailResponseDto)null;

            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"
                        SELECT
                                ass.service_no as ServiceNo,-- 服务编号
	                            o.order_no AS OrderNo,-- 订单编号
	                            o.order_status as OrderStatus,-- 订单状态
	                            u.phone, -- 用户手机号码
	                            u.nick_name as NickName,-- 用户昵称
	                            ass.creation_date as ApplicationTime, -- 申请时间
	                            ass.`status` as AfterSaleStatus, -- 退款状态
                                ass.`Type` as AfterSaleType,-- 售后类型
                                (r.refund_fee/100) as RefundFee, -- 退款金额
                                r.creation_date as RefundTime, -- 退款时间
                                ass.Reason, -- 退款原因
                                ass.refuse_reason as RefuseReason -- 拒绝原因
                        FROM t_mall_aftersale_service AS ass
	                            INNER JOIN t_mall_order as o ON ass.order_guid = o.order_guid AND ass.user_guid = o.user_guid AND ass.merchant_guid = o.merchant_guid
	                            INNER JOIN t_utility_user as u ON u.user_guid = ass.user_guid
                                LEFT JOIN t_mall_aftersale_refund as r ON ass.service_guid = r.service_guid
                        WHERE ass.merchant_guid = @MerchantGuid AND ass.service_guid = @ServiceGuid";

                detail = (await conn.QueryFirstOrDefaultAsync<GetServiceDetailResponseDto>(sql, new { merchantGuid, serviceGuid }));

                if (detail is null)
                {
                    return null;
                }

                sql = @"
                    SELECT 
	                    CONCAT(a.base_path, a.relative_path) as Pic,
	                    p.product_name as `Name`,
	                    afd.product_count as `Count`,
	                    (afd.unit_price/100) as UnitPrice,
	                    (afd.refund_fee/100) as RefundFee
                    FROM t_mall_aftersale_detail as afd
	                    INNER JOIN t_mall_product as p ON p.product_guid = afd.product_guid
	                    LEFT JOIN  t_utility_accessory as a ON p.picture_guid = a.accessory_guid
                    WHERE afd.service_guid = @serviceGuid";

                detail.Items = (await conn.QueryAsync<ProductItem>(sql,
                    new { serviceGuid })).ToList();

            }
            return detail;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<AfterSaleDetailModel>> GetAfterSaleDetailModels(string serviceGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT * FROM t_mall_aftersale_detail WHERE service_guid = @serviceGuid";

                return (await conn.QueryAsync<AfterSaleDetailModel>(sql,
                    new { serviceGuid })).ToList();

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetAfterSaleOrderServicesCount(string orderGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT COUNT(order_guid) FROM t_mall_aftersale_service WHERE order_guid = @orderGuid AND `status` = 3";

                return (await conn.ExecuteScalarAsync<int>(sql,
                    new { orderGuid }));

            }
        }

        /// <summary>
        /// 处理服务单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<bool> ProcessAfterSaleService(ProcessAfterSaleServiceContext context)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                await conn.UpdateAsync(context.AfterSaleService);

                if (context.AfterSaleRefund != null)
                {
                    context.AfterSaleRefund.Insert<string, AfterSaleRefundModel>();
                }

                if (context.AfterSaleConsultations.Count > 0)
                {
                    context.AfterSaleConsultations.InsertBatch();
                }

                if (context.Goods != null && context.Goods.Count > 0)
                {
                    await UpdateGoodsModels(conn, context.Goods);
                }

                if (context.Comments != null && context.Comments.Count > 0)
                {
                    await UpdateGoodsModels(conn, context.Comments);
                }

                await conn.UpdateAsync(context.Order);

                return true;
            });
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public async Task<GetAfterSaleServiceOrderDetailResponseDto> GetAfterSaleServiceOrderDetail(string orderNo)
        {
            var detail = (GetAfterSaleServiceOrderDetailResponseDto)null;

            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"
                        SELECT
                            o.order_guid as OrderGuid,
	                        o.order_no,
	                        u.nick_name as NickName,
	                        u.phone,
	                        o.discount_amout as DiscountAmount,
	                        o.creation_date as OrderTime,
	                        o.payment_date as PayTime,
	                        o.paid_amount as PaidAmount,
	                        o.payables_amount as PayablesAmount
                        FROM t_mall_order as o
	                        INNER JOIN t_utility_user as u ON o.user_guid = u.user_guid
                        WHERE o.order_no = @orderNo";

                detail =
                    (await conn.QueryFirstOrDefaultAsync<GetAfterSaleServiceOrderDetailResponseDto>(sql, new { orderNo }));

                if (detail is null)
                {
                    return null;
                }

                sql = @"
                    SELECT 
	                    p.product_name as `Name`,
                        p.category_name as CategoryName,
	                    d.product_count as `Count`,
	                    d.product_price as UnitPrice
                    FROM t_mall_order_detail as d
	                    INNER JOIN t_mall_product as p ON d.product_guid = p.product_guid
                    WHERE d.order_guid = @OrderGuid";

                detail.Items = (await conn.QueryAsync<ProductDetailItem>(sql,
                    new { detail.OrderGuid })).ToList();

            }
            return detail;
        }

        /// <summary>
        /// 提交售后服务单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<bool> SubmitAfterSaleServiceAsync(SubmitAfterSaleServiceContext context)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                await conn.InsertAsync<string, AfterSaleServiceModel>(context.ServiceModel);
                context.ServiceDetailModels.InsertBatch(conn);
                context.ConsultationModels.InsertBatch(conn);
                foreach (var item in context.GoodsModels)
                {
                    await conn.UpdateAsync(item);
                }
                foreach (var item in context.CommentModels)
                {
                    await conn.UpdateAsync(item);
                }
                return true;

            });
        }

        /// <summary>
        /// 批量更新商品卡项
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="goodsModels"></param>
        /// <returns></returns>
        async Task<bool> UpdateGoodsModels(MySql.Data.MySqlClient.MySqlConnection conn, List<GoodsModel> goodsModels)
        {
            var sql = @"update t_consumer_goods 
                        set 
                            `enable` = @Enable,
                            last_updated_by = @LastUpdatedBy,
                            last_updated_date = @LastUpdatedDate
                        where goods_guid = @GoodsGuid";

            return await conn.ExecuteAsync(sql, goodsModels) > 0;
        }

        /// <summary>
        /// 批量更新商品评论
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="commentModels"></param>
        /// <returns></returns>
        async Task<bool> UpdateGoodsModels(MySql.Data.MySqlClient.MySqlConnection conn, List<OrderProductCommentModel> commentModels)
        {
            var sql = @"update t_consumer_order_product_comment 
                        set 
                            `enable` = @Enable,
                            last_updated_by = @LastUpdatedBy,
                            last_updated_date = @LastUpdatedDate
                        where product_comment_guid = @ProductCommentGuid";

            return await conn.ExecuteAsync(sql, commentModels) > 0;
        }
    }
}
