using Dapper;
using GD.AppSettings;
using GD.Common.Helper;
using GD.DataAccess;
using GD.Dtos.Mall.BackStage;
using GD.Dtos.Mall.Mall;
using GD.Dtos.Mall.Order;
using GD.Dtos.Merchant.Merchant;
using GD.Models.CommonEnum;
using GD.Models.Consumer;
using GD.Models.Mall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GD.Dtos.Merchant.Merchant.GetMerchantOrderPageListRequestDto;
using static GD.Models.Mall.ProductModel;

namespace GD.Mall
{
    /// <summary>
    /// 订单相关操作(存储过程操作)
    /// </summary>
    public class OrderBiz
    {


        /// <summary>
        /// 获取订单唯一实例
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns></returns>
        public OrderModel Getmodel(string orderId)
        {
            return MySqlHelper.GetModelById<OrderModel>(orderId);
        }

        /// <summary>
        /// 更新Model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateModel(OrderModel model)
        {
            return model.Update() == 1;
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CancelOrderAsync(OrderModel order)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                var productBiz = new ProductBiz();
                var orderDetailModelList = await new OrderDetailBiz().GetModelsByOrderIdAsync(order.OrderGuid);
                foreach (var item in orderDetailModelList)
                {
                    var productModel = await productBiz.GetModelByGuidAsync(item.ProductGuid);
                    if (productModel.ProductForm.Equals(ProductFormEnum.Physical.ToString()))
                    {
                        productModel.Inventory += item.ProductCount;
                        if (await conn.UpdateAsync(productModel) < 1) { return false; }
                    }
                }
                if (await conn.UpdateAsync(order) < 1) { return false; }
                return true;
            });
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UpdateModelsAsync(List<OrderModel> orders)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
             {
                 foreach (var item in orders)
                 {
                     await conn.UpdateAsync(item);
                 }
                 return true;
             });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<OrderModel>> GetModelsByIds(string[] ids)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.GetListAsync<OrderModel>("where order_guid in @ids", new { ids }))?.ToList();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<OrderModel>> GetModelsByIds(string ids)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.GetListAsync<OrderModel>("where order_guid in @ids", new { ids }))?.ToList();
            }
        }

        /// <summary>
        ///  根据订单编号
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OrderModel> GetModelsByOrderNo(string OrderNo)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryFirstAsync<OrderModel>("where order_no=@OrderNo", new { OrderNo }));
            }
        }

        public async Task<int> RecordCountAsync()
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.RecordCountAsync<OrderModel>("where enable=@enable", new { enable = true });
            }
        }

        /// <summary>
        /// 根据统一下单KEY查询订单
        /// </summary>
        /// <param name="transactionFlowinGuid">流水号GUID</param>
        /// <returns></returns>
        public async Task<List<OrderModel>> GetGetModelsByKey(string orderKey)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.GetListAsync<OrderModel>("where order_key=@orderKey", new { orderKey })).ToList();
            }
        }

        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="order"></param>
        /// <param name="orderDetailList"></param>
        /// <returns></returns>
        public bool SubmitOrderAndOrderDetail(OrderModel order, List<OrderDetailModel> orderDetailList)
        {
            var result = MySqlHelper.Transaction((conn, tran) =>
            {
                //主订单信息
                if (string.IsNullOrWhiteSpace(order.Insert(conn)))
                {
                    return false;
                }
                //订单详情信息
                if (orderDetailList.Any())
                {
                    foreach (var orderDetail in orderDetailList)
                    {
                        if (string.IsNullOrWhiteSpace(orderDetail.Insert(conn)))
                        {
                            return false;
                        }
                        //扣库存
                        var productBiz = new ProductBiz();
                        var productModel = productBiz.GetModelByGuid(orderDetail.ProductGuid);
                        if (productModel == null) return false;
                        productModel.Inventory = productModel.Inventory - orderDetail.ProductCount;
                        var isProUpdateSuccess = conn.UpdateAsync(productModel).Result;
                        if (isProUpdateSuccess < 1)
                        {
                            return false;
                        }
                    }
                }
                return true;
            });
            return result;
        }

        /// <summary>
        /// 双美-提交购物车到订单
        /// </summary>
        /// <param name="orders">订单列表</param>
        /// <param name="orderDetails">订单明细列表</param>
        /// <param name="goodsModels">个人商品列表</param>
        /// <param name="goodsItemModels">个人商品项列表</param>
        /// <param name="itemIds">购物车Id数组</param>
        /// <returns></returns>
        public async Task<bool> SubmitShoppingCartSeletedProductOfCosmetologyAsync(List<Dictionary<OrderModel, List<OrderDetailModel>>> orderInfoList, List<List<Dictionary<GoodsModel, List<GoodsItemModel>>>> goodsModelInfoList, Dictionary<OrderModel, List<OrderDetailModel>> primaryOrderInfo, string[] itemIds)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                foreach (var item in primaryOrderInfo)
                {
                    if (string.IsNullOrEmpty(await conn.InsertAsync<string, OrderModel>(item.Key))) { return false; }
                    foreach (var detailItem in item.Value)
                    {
                        if (string.IsNullOrEmpty(await conn.InsertAsync<string, OrderDetailModel>(detailItem))) { return false; }
                    }
                }
                foreach (var item in orderInfoList)
                {
                    foreach (var order in item)
                    {
                        if (string.IsNullOrEmpty(await conn.InsertAsync<string, OrderModel>(order.Key))) { return false; }
                        if (order.Value.Count < 1) { return false; }
                        foreach (var orderDetail in order.Value)
                        {
                            if (string.IsNullOrEmpty(await conn.InsertAsync<string, OrderDetailModel>(orderDetail))) { return false; }
                            //扣库存
                            var productModel = new ProductBiz().GetModelByGuid(orderDetail.ProductGuid);
                            if (productModel == null) return false;
                            if (productModel.ProductForm.Equals(ProductFormEnum.Physical.ToString()))
                            {
                                if (!productModel.OnSale) { return false; }
                                if (productModel.Inventory < orderDetail.ProductCount) { return false; }
                                productModel.Inventory = productModel.Inventory - orderDetail.ProductCount;
                                if (await conn.UpdateAsync(productModel) < 1) { return false; }
                            }
                        }
                    }
                }
                foreach (var itemList in goodsModelInfoList)
                {
                    foreach (var itemDictionary in itemList)
                    {
                        foreach (var itemD in itemDictionary)
                        {
                            if (string.IsNullOrEmpty(await conn.InsertAsync<string, GoodsModel>(itemD.Key))) { return false; }
                            foreach (var item in itemD.Value)
                            {
                                if (string.IsNullOrEmpty(await conn.InsertAsync<string, GoodsItemModel>(item))) { return false; }
                            }
                        }
                    }
                }

                await conn.DeleteListAsync<ShoppingCarModel>("where item_guid in @itemIds", new { itemIds });
                return true;
            });
        }

        /// <summary>
        /// 异步 添加订单
        /// </summary>
        /// <param name="order"></param>
        /// <param name="orderDetailList"></param>
        /// <returns></returns>
        public async Task<bool> SubmitOrderAndOrderDetailAsync(OrderModel order, List<OrderDetailModel> orderDetailList)
        {
            var result = await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                //主订单信息
                if (string.IsNullOrEmpty(await conn.InsertAsync<string, OrderModel>(order))) { return false; }
                //订单详情信息
                if (orderDetailList.Any())
                {
                    foreach (var orderDetail in orderDetailList)
                    {
                        if (string.IsNullOrEmpty(await conn.InsertAsync<string, OrderDetailModel>(orderDetail))) { return false; }
                        //扣库存
                        var productBiz = new ProductBiz();
                        var productModel = await productBiz.GetModelByGuidAsync(orderDetail.ProductGuid);
                        if (productModel == null) return false;
                        productModel.Inventory = productModel.Inventory - orderDetail.ProductCount;

                        var isProUpdateSuccess = await conn.UpdateAsync(productModel);
                        if (isProUpdateSuccess < 1)
                        {
                            return false;
                        }
                    }
                }
                return true;
            });
            return result;
        }

        /// <summary>
        /// 获取我的订单列表
        /// </summary>
        /// <returns></returns>
        public GetMyOrderListResponseTmpDto GetMyOrderList(GetMyOrderListRequestDto request)
        {
            string orderStatusCondition = " Where  o.user_guid = @UserID And o.`enable`=1 ";

            if (!string.Equals(request.OrderStatus.ToString().ToLower(), "all", StringComparison.OrdinalIgnoreCase))
            {
                orderStatusCondition = $"{orderStatusCondition} and o.order_status='{request.OrderStatus.ToString()}'";
            }
            //if (!string.IsNullOrWhiteSpace(request.Keyword))
            //{
            //    orderStatusCondition = $@"{orderStatusCondition}	AND (
	           //                                                                                         o.order_guid IN (
            //                                                                                        SELECT DISTINCT
	           //                                                                                         o.order_guid
            //                                                                                        FROM
	           //                                                                                         t_mall_order AS o
	           //                                                                                         LEFT JOIN t_mall_order_detail AS detail ON o.order_guid = detail.order_guid
	           //                                                                                         LEFT JOIN t_mall_product AS pro ON detail.product_guid = pro.product_guid
            //                                                                                        WHERE
	           //                                                                                         o.user_guid = @UserID
	           //                                                                                         AND pro.product_name LIKE '%{request.Keyword}%'
	           //                                                                                         )
	           //                                                                                         OR o.order_no LIKE '%{request.Keyword}%'
	           //                                                                                         ) ";
            //    //orderStatusCondition = $"{orderStatusCondition} and (pro.product_name like '%{request.Keyword}%' or o.order_no like '%{request.Keyword}%')";
            //}
            var sql = $@"SELECT
	                                o.order_guid,
	                                o.order_status,
	                                o.merchant_guid,
	                                m.merchant_name,
	                                o.paid_amount,
	                                o.product_count,
	                                o.freight,
	                                o.creation_date,
	                                o.order_no,
	                                o.order_key,
	                                o.order_type,
	                                o.order_category,
	                                o.pay_type,
	                                o.order_mark,
	                                o.discount_amout,
	                                m.`enable` AS MerchantEnable
                                FROM
	                                t_mall_order AS o
	                                LEFT JOIN t_merchant m ON o.merchant_guid = m.merchant_guid
	                                AND m.`enable` = 1
                        {orderStatusCondition} ORDER BY  o.creation_date DESC ";
            return MySqlHelper.QueryByPage<GetMyOrderListRequestDto, GetMyOrderListResponseTmpDto, GetMyOrderListItemTmpDto>(sql, request);
            //return MySqlHelper.Select<OrderListInfoModel>(sql, new { request.UserID, platformType, PageIndex = (request.PageIndex - 1) * request.PageSize, request.PageSize })?.ToList();
        }

        /// <summary>
        /// 根据用户Id,订单状态，平台类型筛选用户消费实付总额
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="platformType">平台类型</param>
        /// <returns></returns>
        public async Task<decimal> GetOrderPaidAmount(string userId, string orderStatus, string platformType)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"select SUM(paid_amount) from t_mall_order where user_guid=@userId and `enable`=1 and order_status=@orderStatus and platform_type=@platformType";
                var count = await conn.QueryFirstOrDefaultAsync<decimal?>(sql, new { userId, orderStatus, platformType });
                return count ?? 0;
            }
        }

        /// <summary>
        /// 后台-订单列表查询
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<BackStageGetOrderListResponseDto> BackStageGetOrderListAsync(BackStageGetOrderListRequestDto requestDto, string userId)
        {
            var sqlWhere = $" and  od.merchant_guid='{userId}' ";

            if (!string.IsNullOrWhiteSpace(requestDto.OrderStatus.ToString()) && !requestDto.OrderStatus.ToString().ToLower().Equals("all"))
            {
                sqlWhere += "  and  od.order_status=@OrderStatus ";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.OderNo))
            {
                sqlWhere += $"  and  od.order_no ='{requestDto.OderNo}' ";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.UserName))
            {
                sqlWhere += "  and  u.nick_name=@UserName ";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.Phone))
            {
                sqlWhere += " and  u.phone=@Phone ";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.CampaignGuid))
            {
                sqlWhere += " and  cam.campaign_guid =@CampaignGuid ";
            }
            var isST = !string.IsNullOrWhiteSpace(requestDto.StartTime.ToString());
            var isET = !string.IsNullOrWhiteSpace(requestDto.EndTime.ToString());
            if (isST)
            {
                sqlWhere += $" and  (od.creation_date >'{requestDto.StartTime}' )";
            }
            if (isET)
            {
                sqlWhere += $" and  (od.creation_date<'{requestDto.EndTime}' )";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.OrderReceiver))
            {
                sqlWhere += $" and  od.order_receiver  like '%{requestDto.OrderReceiver}%'  ";
            }

            if (!string.IsNullOrWhiteSpace(requestDto.ProductName))
            {
                sqlWhere += $" and  pro.product_name like '%{requestDto.ProductName}%' ";
            }

            var sql = $@"SELECT distinct
	                                    od.order_guid,
	                                    od.user_guid,
	                                    u.nick_name,
	                                    od.pay_type,
	                                    od.paid_amount,
	                                    od.order_status,
	                                    od.creation_date
                                    FROM
	                                    t_mall_order AS od
	                                    LEFT JOIN t_mall_order_detail AS detail ON od.order_guid = detail.order_guid
	                                    LEFT JOIN t_utility_user AS u ON od.user_guid = u.user_guid
	                                    LEFT JOIN t_mall_product AS pro ON pro.product_guid = detail.product_guid
	                                    LEFT JOIN t_mall_campaign AS cam ON cam.product_guid = pro.product_guid
                                    WHERE
	                                    1 = 1
                                    and  od.platform_type='{requestDto.PlatformType}'
                                    and  od.enable={requestDto.Enable}
                                        {sqlWhere}
                                    ORDER BY
	                                    od.creation_date ";

            return await MySqlHelper.QueryByPageAsync<BackStageGetOrderListRequestDto, BackStageGetOrderListResponseDto, BackStageGetOrderListItem>(sql, requestDto);
        }

        /// <summary>
        /// 后台查询订单详情
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<BackStageGetOrderDetailListResponseDto> BackStageGetOrderDetailListAsync(BackStageGetOrderDetailListRequestDto requestDto)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                                od.order_guid,
	                                od.order_receiver,
	                                od.order_phone,
	                                od.order_address,
	                                od.remark,
	                                detail.product_name,
	                                detail.product_count,
	                                detail.product_price,
	                                detail.product_price,
	                                cam.campaign_guid,
	                                cam.special_prices,
	                                od.freight,
	                                od.paid_amount,
	                                od.order_no,
	                                '' AS logistics_company,
	                                '' AS logistics_no,
	                                com.comment_guid,
	                                com.content
                                FROM
	                                t_mall_order AS od
	                                LEFT JOIN t_mall_order_detail AS detail ON od.order_guid = detail.order_guid
	                                LEFT JOIN t_utility_user AS u ON od.user_guid = u.user_guid
	                                LEFT JOIN t_mall_product AS pro ON pro.product_guid = detail.product_guid
	                                LEFT JOIN t_mall_campaign AS cam ON cam.product_guid = pro.product_guid
	                                LEFT JOIN t_consumer_comment AS com ON od.order_guid = com.target_guid
                                WHERE
	                                 od.order_guid= @OrderGuid
	                                 and od.enable=@Enable
	                                 and od.platform_type=@PlatformType
                                ORDER BY
	                                od.creation_date   ";

                return await conn.QueryFirstAsync<BackStageGetOrderDetailListResponseDto>(sql,
                    new { orderGuid = requestDto.OrderGuid, enable = requestDto.Enable, requestDto.PlatformType });
            }
        }

        public async Task<OrderModel> GetAsync(string guid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<OrderModel>(guid);
            }
        }

        /// <summary>
        /// 获取商户订单基本统计数据
        /// </summary>
        /// <param name="merchantGuid">商户guid</param>
        /// <returns></returns>
        public async Task<GetMerchantOrderBasicStatisticsDataResponseDto> GetMerchantOrderBasicStatisticsDataAsync(string merchantGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            merchant_guid,
	                            count( order_guid ) AS OrderCount,
	                            sum( product_count ) AS ProductCount,
	                            sum( paid_amount ) AS PaidAmount
                            FROM
	                            t_mall_order
                            WHERE
	                            merchant_guid = @merchantGuid
	                            AND `enable` = 1
                            GROUP BY
	                            merchant_guid";
                return await conn.QueryFirstOrDefaultAsync<GetMerchantOrderBasicStatisticsDataResponseDto>(sql, new { merchantGuid });
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <returns></returns>
        public async Task<List<GetMerchantOrderStatisticsDataSomeDaysResponseDto>> GetMerchantOrderStatisticsDataSomeDaysAsync(string merchantGuid, DateTime startDate, DateTime endDate)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            merchant_guid,
	                            date_format(creation_date, '%Y-%m-%d' ) AS OrderDate,
	                            count( order_guid ) AS OrderCount,
	                            sum( product_count ) AS ProductCount,
	                            sum( paid_amount ) AS PaidAmount
                            FROM
	                            t_mall_order
                            WHERE
	                            merchant_guid = @merchantGuid
	                            AND date( creation_date ) BETWEEN @startDate
	                            AND @endDate
	                            AND `enable` = 1
                            GROUP BY
	                            merchant_guid,
	                            OrderDate
                            ORDER BY
	                            merchant_guid,
	                            OrderDate";
                return (await conn.QueryAsync<GetMerchantOrderStatisticsDataSomeDaysResponseDto>(sql, new { merchantGuid, startDate = startDate.ToString("yyyy-MM-dd"), endDate = endDate.ToString("yyyy-MM-dd") }))?.ToList();
            }
        }

        /// <summary>
        /// 根据用户Id,订单状态，平台类型筛选用户消费实付总额
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="platformType">平台类型</param>
        /// <returns></returns>
        public async Task<List<OrderModel>> GetOrderListByOrderKey(string orderKey)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sqlWhere = $@" where order_key=@orderKey  ";
                return (await conn.GetListAsync<OrderModel>(sqlWhere, new { orderKey })).ToList();
            }
        }

        /// <summary>
        /// 根据流水号查询订单
        /// </summary>
        /// <param name="transactionFlowinGuid">流水号GUID</param>
        /// <returns></returns>
        public async Task<List<OrderModel>> GetGetModelsByTransactionFlowingGuid(string transactionFlowingGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.GetListAsync<OrderModel>("where transaction_flowing_guid=@transactionFlowingGuid", new { transactionFlowingGuid })).ToList();
            }
        }

        /// <summary>
        /// 双美-查询订单列表数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<OrderDetailsOfCosmetologyDto>> GetOrderDetailsOfCosmetologyAsync(GetOrderListOfCosmetologyRequestDto requestDto)
        {
            string orderStatusCondition = "";

            if (!string.Equals(requestDto.OrderStatus.ToString(), "all", StringComparison.OrdinalIgnoreCase))
            {
                orderStatusCondition = $"and o.order_status='{requestDto.OrderStatus.ToString()}'";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.Keyword))
            {
                orderStatusCondition = $"{orderStatusCondition} and (product.product_name like '%{requestDto.Keyword}%' or o.order_no like '%{requestDto.Keyword}%')";
            }
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"DROP TEMPORARY TABLE
                                IF
	                                EXISTS tmpOrder;
                                CREATE TEMPORARY TABLE tmpOrder AS SELECT distinct
                                o.order_key,o.creation_date,o.order_category
                                FROM
	                                t_mall_order o inner join t_mall_order_detail od on o.order_guid=od.order_guid
                                    left join t_mall_product product on product.product_guid=od.product_guid
                                WHERE
	                                o.user_guid = @UserID
	                                AND ( o.platform_type = 'CloudDoctor' OR o.platform_type = 'MedicalCosmetology' ) { orderStatusCondition }
                                ORDER BY
	                                o.creation_date DESC
	                                LIMIT @pageIndex,
	                                @pageSize;
                                SELECT DISTINCT
	                                o.order_guid AS OrderGuid,
	                                o.order_no AS OrderNo,
                                    o.order_category,
                                    o.order_mark,
                                    m.`enable` as MerchantEnable,
									o.order_key AS OrderKey,
	                                o.creation_date AS OrderDate,
	                                o.paid_amount AS PaidAmount,
                                    o.discount_amout AS DiscountAmout,
	                                o.order_status AS OrderStatus,
                                    detail.product_guid as ProductGuid,
	                                product.product_name AS ProductName,
                                    product.on_sale as OnSale,
	                                detail.product_count AS ProductCount,
	                                ifnull( o.debt, 0 ) AS Debt,
                                    CONCAT(proacc.base_path,proacc.relative_path) ProductPicture
                                FROM
	                                t_mall_order AS o INNER JOIN tmpOrder as tmp on tmp.order_key=o.order_key
	                                INNER JOIN t_mall_order_detail detail ON o.order_guid = detail.order_guid
                                    INNER JOIN t_merchant m on m.merchant_guid=o.merchant_guid
	                                INNER JOIN t_mall_product product ON product.product_guid = detail.product_guid
                                    LEFT JOIN t_utility_accessory proacc ON product.picture_guid = proacc.accessory_guid
                                WHERE
	                                o.`enable` = 1
	                                AND detail.`enable` = 1
	                                AND product.`enable` = 1
								ORDER BY o.creation_date DESC;
                                DROP TEMPORARY TABLE tmpOrder;";
                var lst = await conn.QueryAsync<OrderDetailsOfCosmetologyDto>(sql, new { requestDto.UserID, pageIndex = (requestDto.PageIndex - 1) * requestDto.PageSize, pageSize = requestDto.PageSize });
                return lst?.ToList();
            }
        }

        /// <summary>
        /// 查询商户订单分页列表
        /// </summary>
        public async Task<GetMerchantOrderPageListResponseDto> GetMerchantOrderPageListAsync(GetMerchantOrderPageListRequestDto requestDto)
        {
            var sqlWhere = string.Empty;
            requestDto.StartDate = requestDto.StartDate.Date;
            requestDto.EndDate = requestDto.EndDate.AddDays(1).Date;
            if (!string.IsNullOrWhiteSpace(requestDto.Keyword))
            {
                requestDto.Keyword = $"%{requestDto.Keyword}%";
                sqlWhere = $"{sqlWhere} and (b.phone like @Keyword or b.nick_name like @Keyword)";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.OrderNo))
            {
                sqlWhere = $"{sqlWhere} and a.order_no=@OrderNo";
            }
            //待线下支付
            if (requestDto.OrderStatus == OrderStatusConditionEnum.OffLinePay)
            {
                sqlWhere = $"{sqlWhere} and a.order_status='{OrderStatusConditionEnum.Obligation.ToString()}' and a.pay_type='{PayTypeEnum.OffLinePay.ToString()}'";
            }
            else if (requestDto.OrderStatus == OrderStatusConditionEnum.Obligation)
            {
                sqlWhere = $"{sqlWhere} and a.order_status='{OrderStatusConditionEnum.Obligation.ToString()}' and a.pay_type<>'{PayTypeEnum.OffLinePay.ToString()}'";
            }
            else if (requestDto.OrderStatus == OrderStatusConditionEnum.Shipped)//待收货和待发货订单的状态值实际都为“待收货”，区别是待收货是填了物流单号，待发货没填
            {
                sqlWhere = $"{sqlWhere} and a.order_status='{OrderStatusConditionEnum.Received.ToString()}' and a.express_no is null";
            }
            else if (requestDto.OrderStatus == OrderStatusConditionEnum.Received)//待收货和待发货订单的状态值实际都为“待收货”，区别是待收货是填了物流单号，待发货没填
            {
                sqlWhere = $"{sqlWhere} and a.order_status='{OrderStatusConditionEnum.Received.ToString()}' and a.express_no is not null";
            }
            else if (requestDto.OrderStatus != OrderStatusConditionEnum.All)
            {
                sqlWhere = $"{sqlWhere} and a.order_status='{requestDto.OrderStatus.ToString()}'";
            }
            var sql = $@"SELECT
	                        a.order_guid,
	                        a.order_no,
	                        b.nick_name,
	                        b.phone,
	                        a.paid_amount,
	                        a.creation_date,
	                        a.order_status ,
	                        a.payment_date,
	                        a.pay_type,
	                        a.payables_amount,
                            a.freight,
	                        a.discount_amout,
	                        a.order_receiver,
	                        a.order_phone,
	                        a.express_company,
	                        a.express_no,
	                        a.order_address
                        FROM
	                        t_mall_order a
	                        INNER JOIN t_utility_user b ON a.user_guid = b.user_guid
                        WHERE
	                        a.merchant_guid = @MerchantGuid
	                        AND a.`enable` = 1
                            and a.creation_date BETWEEN @StartDate and @EndDate
                            {sqlWhere}
                        ORDER BY
	                        a.creation_date DESC";
            return await MySqlHelper.QueryByPageAsync<GetMerchantOrderPageListRequestDto, GetMerchantOrderPageListResponseDto, GetMerchantOrderPageListItemDto>(sql, requestDto);
        }

        /// <summary>
        /// 获取订单下的商品列表
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        public async Task<List<OrderProductDto>> GetOrderProductListAsync(List<string> orderIds)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            order_guid,
	                            product_guid,
	                            product_count,
	                            product_price,
	                            product_name
                            FROM
	                            t_mall_order_detail
                            WHERE
	                            order_guid IN @orderIds;";
                var result = await conn.QueryAsync<OrderProductDto>(sql, new { orderIds });
                return result?.AsList();
            }
        }

        /// <summary>
        /// 获取商品下包含的项目列表
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public async Task<List<OrderProductProjectsDto>> GetOrderProductProjectsAsync(List<string> productIds)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            a.product_guid,
	                            b.project_name,
	                            a.project_times
                            FROM
	                            t_mall_product_project a
	                            INNER JOIN t_mall_project b ON a.project_guid = b.project_guid
                            WHERE
	                            a.product_guid IN @productIds;";
                var result = await conn.QueryAsync<OrderProductProjectsDto>(sql, new { productIds });
                return result?.AsList();
            }
        }

        /// <summary>
        /// 门店开单（仅限服务类订单）
        /// </summary>
        /// <param name="order"></param>
        /// <param name="orderDetails"></param>
        /// <param name="goodsModels"></param>
        /// <param name="goodsItemModels"></param>
        /// <returns></returns>
        public async Task<bool> PlaceOrderAsync(OrderModel order, List<OrderDetailModel> orderDetails, List<GoodsModel> goodsModels, List<GoodsItemModel> goodsItemModels)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                await conn.InsertAsync<string, OrderModel>(order);
                if (orderDetails.InsertBatch(conn) == 0)
                {
                    return false;
                }
                if (goodsModels.InsertBatch(conn) == 0)
                {
                    return false;
                }
                if (goodsItemModels.InsertBatch(conn) == 0)
                {
                    return false;
                }
                return true;
            });
        }

        public async Task<List<GetDeadlineOrderResponseDto>> GetDeadlineOrderAsync(string userId = null)
        {
            var settings = Factory.GetSettings("host.json");
            var orderDeadline = settings["OrderDeadline"];
            var iOrderDeadline = 24 * 60 + 1;
            int.TryParse(orderDeadline, out iOrderDeadline);
            using (var conn = MySqlHelper.GetConnection())
            {
                var whereSql = string.Empty;
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    whereSql = " and user_guid=@userId";
                }
                //var sql = $@"SELECT DISTINCT
                //             order_key,
                //             transaction_flowing_guid
                //            FROM
                //             t_mall_order
                //            WHERE
                //             order_status = 'Obligation' { whereSql }
                //             AND creation_date < date_sub( NOW( ), INTERVAL ( 24 * 60+1 ) MINUTE )
                //                AND order_type = 'Normal' AND pay_type=@payType ;";
                var sql = $@"SELECT DISTINCT
	                            order_key,
	                            transaction_flowing_guid
                            FROM
	                            t_mall_order
                            WHERE
	                            order_status = 'Obligation' { whereSql }
	                            AND creation_date < date_sub( NOW( ), INTERVAL {iOrderDeadline} MINUTE )
                                AND order_type = 'Normal' AND pay_type=@payType ;";
                var result = await conn.QueryAsync<GetDeadlineOrderResponseDto>(sql, new { userId, payType = PayTypeEnum.Wechat.ToString() });
                return result?.ToList();
            }
        }

        public async Task<bool> CloseDeadlineOrderAsync(List<GetDeadlineOrderResponseDto> closeOrder)
        {
            if (!closeOrder.Any())
            {
                return true;
            }
            var orderKyes = closeOrder.Select(a => a.OrderKey).Distinct().ToList();
            var flowingGuids = closeOrder.Select(a => a.TransactionFlowingGuid).Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
            Logger.Info($"{nameof(CloseDeadlineOrderAsync)}-需要取消的订单一共{orderKyes.Count()}组");
            Logger.Info($"{nameof(CloseDeadlineOrderAsync)}-需要关闭的交易流水一共{flowingGuids.Count()}个");
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                await conn.ExecuteAsync("update t_mall_order set order_status='Canceled',last_updated_by='system',last_updated_date=NOW() where order_key in @orderKyes", new { orderKyes });
                await conn.ExecuteAsync("update t_mall_transaction_flowing set transaction_status='TradingClosed',last_updated_by='system',last_updated_date=NOW() where transaction_flowing_guid in @flowingGuids", new { flowingGuids });
                await conn.ExecuteAsync("update t_merchant_flowing set flow_status= 'TradingClosed',last_updated_by='system',last_updated_date=NOW() where transaction_flowing_guid in @flowingGuids", new { flowingGuids });

                return true;
            });
        }

        /// <summary>
        /// 查询超过15天还未确认收货的待收货订单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<OrderModel>> GetOrdersWithoutReceivingBeyondDeadlineAsync(string userId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"WHERE
	                            user_guid = @userId 
	                            AND `enable` = 1 
	                            AND order_category = 'Physical' 
	                            AND order_status = 'Received' 
	                            AND express_no IS NOT NULL 
	                            AND last_updated_by < DATE_SUB( NOW(), INTERVAL 15 DAY );";
                var result = await conn.GetListAsync<OrderModel>(sql, new { userId });
                return result.ToList();
            }
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="orderGuid"></param>
        /// <returns></returns>
        public async Task<List<GetOrderDetailsInfoTempDto>> GetOrderDetailsInfoAsync(string orderGuid)
        {
            var sql = @"SELECT
                            c.merchant_guid,
	                        IFNULL( c.merchant_name, '智慧云医' ) AS merchant_name,
	                        a.product_count AS product_total,
	                        a.order_status,
	                        a.order_no,
                            a.order_guid,
                            a.order_key,
                            a.payment_date,
	                        a.payables_amount,
	                        a.discount_amout,
	                        a.paid_amount,
	                        a.creation_date,
	                        case a.pay_type when 'Wechat' then '微信支付' when 'OffLinePay' then '线下支付' else '其他' end as pay_type,
	                        b.product_name,
	                        b.product_count,
                            b.product_price,
                            b.detail_guid as order_detail_guid,
	                        CONCAT(g.base_path,g.relative_path) as product_picture,
	                        e.service_guid,
                            d.detail_guid as service_detail_guid,
                            e.type as service_type,
	                        e.`status` AS service_status 
                        FROM
	                        t_mall_order a
	                        INNER JOIN t_mall_order_detail b ON a.order_guid = b.order_guid 
	                        AND a.`enable` = b.`enable`
	                        LEFT JOIN t_merchant c ON c.merchant_guid = a.merchant_guid
	                        LEFT JOIN t_mall_aftersale_detail d ON d.order_detail_guid = b.detail_guid
	                        LEFT JOIN t_mall_aftersale_service e ON e.service_guid = d.service_guid 
	                        left join t_mall_product f on f.product_guid=b.product_guid 
	                        left join t_utility_accessory g on g.accessory_guid=f.picture_guid
                        WHERE
	                        a.order_guid = @orderGuid
	                        AND a.`enable` = 1 
                        ORDER BY
	                        b.product_name";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<GetOrderDetailsInfoTempDto>(sql, new { orderGuid });
                return result.ToList();
            }
        }
    }
}