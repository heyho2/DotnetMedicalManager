using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 查询商户订单分页列表请求Dto
    /// </summary>
    public class GetMerchantOrderPageListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 门店guid
        /// </summary>
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 用户手机号、昵称
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单状态:All-全部、Obligation-待付款、OffLinePay-待线下付款、Shipped-待发货、Received-待收货、Completed-已完成、Canceled-已取消
        /// </summary>
        [Required(ErrorMessage = "订单状态条件必填")]
        public OrderStatusConditionEnum OrderStatus { get; set; }

        /// <summary>
        /// 下单日期（起始）
        /// </summary>
        [Required(ErrorMessage = "下单日期筛选条件必填")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 下单日期（结束）
        /// </summary>
        [Required(ErrorMessage = "下单日期筛选条件必填")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public enum OrderStatusConditionEnum
        {

            /// <summary>
            /// 全部 0
            /// </summary>
            [Description("全部")]
            All = 0,
            /// <summary>
            /// 待付款 1
            /// </summary>
            [Description("待付款")]
            Obligation,

            /// <summary>
            /// 线下付款 2
            /// </summary>
            [Description("线下付款")]
            OffLinePay,

            /// <summary>
            /// 待发货 3
            /// </summary>
            [Description("待发货")]
            Shipped,

            /// <summary>
            /// 待收货 4
            /// </summary>
            [Description("待收货")]
            Received,

            /// <summary>
            /// 已完成 5
            /// </summary>
            [Description("已完成")]
            Completed,

            /// <summary>
            /// 已取消 6
            /// </summary>
            [Description("已取消")]
            Canceled
        }
    }

    /// <summary>
    /// 查询商户订单分页列表响应Dto
    /// </summary>
    public class GetMerchantOrderPageListResponseDto : BasePageResponseDto<GetMerchantOrderPageListItemDto>
    {

    }

    /// <summary>
    /// 查询商户订单分页列表详情Dto
    /// </summary>
    public class GetMerchantOrderPageListItemDto : BaseDto
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderGuid { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 下单用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 下单用户手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// 下单日期
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderStatus { get; set; }

        /// <summary>
        /// 付款日期
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayType { get; set; }

        /// <summary>
        /// 订单金额（应付金额）
        /// </summary>
        public decimal PayablesAmount { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal? Freight { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal DiscountAmout { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public string OrderReceiver { get; set; }

        /// <summary>
        /// 收件人手机号
        /// </summary>
        public string OrderPhone { get; set; }

        /// <summary>
        /// 快递承运公司
        /// </summary>
        public string ExpressCompany { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public string ExpressNo { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        public string OrderAddress { get; set; }

        /// <summary>
        /// 订单商品列表
        /// </summary>
        public List<OrderProductDto> Products { get; set; }
    }

    /// <summary>
    /// 订单商品信息
    /// </summary>
    public class OrderProductDto : BaseDto
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderGuid { get; set; }

        /// <summary>
        /// 商品guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public string ProductCount { get; set; }

        /// <summary>
        /// 商品单价
        /// </summary>
        public string ProductPrice { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 订单商品包含项目列表
        /// </summary>
        public List<OrderProductProjectsDto> Projects { get; set; }


    }

    /// <summary>
    /// 订单商品包含的服务项目数据
    /// </summary>
    public class OrderProductProjectsDto : BaseDto
    {
        /// <summary>
        /// 商品guid
        /// </summary>
        public string ProductGuid { get; set; }
        /// <summary>
        /// 服务项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 服务项目次数
        /// </summary>
        public string ProjectTimes { get; set; }

    }

}
