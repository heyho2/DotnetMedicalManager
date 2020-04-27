using GD.Common.Base;
using GD.Dtos.CommonEnum;
using GD.Models.CommonEnum;
using System;

namespace GD.Dtos.Mall.Mall
{
    /// <inheritdoc />
    /// <summary>
    /// 获取订单列表请求Dto
    /// </summary>
    public class GetMyOrderListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 订单类型(未支付，待收货，已完成，已取消)
        /// </summary>
        public OrderStatusEnum OrderStatus { get; set; }

        /// <summary>
        /// 用户id(选填，默认为登录用户)
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 筛选关键字（可支持订单号、产品名称）
        /// </summary>
        public string Keyword { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class GetMyOrderListResponseTmpDto : BasePageResponseDto<GetMyOrderListItemTmpDto>
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class GetMyOrderListItemTmpDto : BaseDto
    {
        /// <summary>
        /// 订单guid
        /// </summary>
        public string OrderGuid { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderStatus { get; set; }

        /// <summary>
        /// 门店guid
        /// </summary>
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 门店名称
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 付款金额
        /// </summary>
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int ProductCount { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal Freight { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单key
        /// </summary>
        public string OrderKey { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public string OrderType { get; set; }

        /// <summary>
        /// 订单分类
        /// </summary>
        public string OrderCategory { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayType { get; set; }



        /// <summary>
        /// 订单标记
        /// </summary>
        public string OrderMark { get; set; }

        /// <summary>
        /// 折扣金额
        /// </summary>
        public decimal DiscountAmout { get; set; }

        /// <summary>
        /// 门店是否启用
        /// </summary>
        public bool MerchantEnable { get; set; }

       

    }
}