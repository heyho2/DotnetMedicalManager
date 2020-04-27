using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 获取订单详情
    /// </summary>
    public class GetOrderDetailRequest
    {
        /// <summary>
        /// 订单Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "订单Guid")]
        public string OrderGuid { get; set; }

    }

    /// <summary>
    /// response
    /// </summary>
    public class GetOrderDetailResponse : BaseDto
    {
        /// <summary>
        /// 订单Guid
        /// </summary>
        public string OrderGuid { get; set; }
        /// <summary>
        /// OrderKey
        /// </summary>
        public string OrderKey { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 商户Guid
        /// </summary>
        public string MerchantGuid { get; set; }
        /// <summary>
        /// 商户名称
        /// </summary>
        public string MerchantName { get; set; }
        /// <summary>
        /// 订单类型：普通订单，团购订单
        /// 'Normal','Group'
        /// </summary>
        public string OrderType { get; set; }
        /// <summary>
        /// 订单种类：实体类、服务类
        /// 'Service','Physical'
        /// </summary>
        public string OrderCategory { get; set; }
        /// <summary>
        /// 订单主次
        /// </summary>
        public string OrderMark { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int ProductCount { get; set; }
        /// <summary>
        /// 订单状态：待付款，待发货，待收货，已完成，取消
        /// 'Obligation','Shipped','Received','Completed','Canceled'
        /// </summary>
        public string OrderStatus { get; set; }
        /// <summary>
        /// 支付方式：银行卡，现金，苹果支付，积分支付，微信支付，支付宝，线下支付
        /// 'Card','Cash','Apple','Score','Wechat','Alipay','OffLinePay'
        /// </summary>
        public string PayType { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PaymentDate { get; set; }
        /// <summary>
        /// 应付金额
        /// </summary>
        public decimal PayablesAmount { get; set; }
        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal PaidAmount { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal DiscountAmout { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        public decimal Freight { get; set; }
        /// <summary>
        /// 快递编号
        /// </summary>
        public string ExpressNo { get; set; }
        /// <summary>
        /// 快递公司
        /// </summary>
        public string ExpressCompany { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 下单日期
        /// </summary>
        public DateTime? CreationDate { get; set; }
        /// <summary>
        /// 平台
        /// </summary>
        public string PlatformType { get; set; }

        /// <summary>
        /// 待付款订单截止时间
        /// </summary>
        public DateTime? PaymentDeadline { get; set; }

        /// <summary>
        /// 订单详情列表
        /// </summary>
        public List<OrderDetailInfo> OrderDetailInfoList { get; set; }

        /// <summary>
        /// 订单详情
        /// </summary>
        public class OrderDetailInfo
        {
            /// <summary>
            /// 详情Guid
            /// </summary>
            public string DetailGuid { get; set; }
            /// <summary>
            /// 产品Guid
            /// </summary>
            public string ProductGuid { get; set; }
            /// <summary>
            /// 产品图片URL
            /// </summary>
            public string ProductPicURL { get; set; }
            /// <summary>
            /// 产品名称
            /// </summary>
            public string ProductName { get; set; }
            /// <summary>
            /// 产品数量
            /// </summary>
            public int ProductCount { get; set; }
            ///<summary>
            ///价格
            ///</summary>
            public decimal ProductPrice { get; set; }

        }
    }
}
