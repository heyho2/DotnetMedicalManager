using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.Order
{
    /// <summary>
    /// 
    /// </summary>
    public class GetOrderDetailsInfoResponseDto : BaseDto
    {
        /// <summary>
        /// 商户Id
        /// </summary>
        public string MerchantGuid { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 商品总数量
        /// </summary>
        public int ProductTotal { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderStatus { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单Guid
        /// </summary>
        public string OrderGuid { get; set; }

        /// <summary>
        /// 应付金额
        /// </summary>
        public string PayablesAmount { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal DiscountAmout { get; set; }

        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public string CreationDate { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayType { get; set; }
        /// <summary>
        /// 统一支付Key
        /// </summary>
        public string OrderKey { get; set; }
        /// <summary>
        /// 订单支付时间
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// 能否申请售后
        /// </summary>
        public bool CanApplyAfterSale { get; set; } = false;

        /// <summary>
        /// 商品明细
        /// </summary>
        public List<GetOrderDetailsInfoItemDto> Products { get; set; }


    }

    /// <summary>
    /// 订单商品明细
    /// </summary>
    public class GetOrderDetailsInfoItemDto : BaseDto
    {
        /// <summary>
        /// 商品图片
        /// </summary>
        public string ProductPicture { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal ProductPrice { get; set; }

        /// <summary>
        /// 订单详情数量
        /// </summary>
        public string ProductCount { get; set; }

        /// <summary>
        /// 服务单Id
        /// </summary>
        public string ServiceGuid { get; set; }

        /// <summary>
        /// 服务单详情Guid
        /// </summary>
        public string ServiceDetailGuid { get; set; }
        /// <summary>
        /// 订单明细Id
        /// </summary>
        public string OrderDetailGuid { get; set; }

        /// <summary>
        /// 售后单状态
        /// </summary>
        public string ServiceStatus { get; set; }

        /// <summary>
        /// 售后类型
        /// </summary>
        public string ServiceType { get; set; }

        /// <summary>
        /// 售后单状态名称
        /// </summary>
        public string ServiceStatusDisplay { get; set; }

        /// <summary>
        /// 能否显示售后状态按钮
        /// </summary>
        public bool CanApplyAfterSale { get; set; } = false;
    }

    /// <summary>
    /// 订单详情中间数据
    /// </summary>
    public class GetOrderDetailsInfoTempDto : BaseDto
    {
        /// <summary>
        /// 商户Id
        /// </summary>
        public string MerchantGuid { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 商品总数量
        /// </summary>
        public int ProductTotal { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderStatus { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单Guid
        /// </summary>
        public string OrderGuid { get; set; }
        /// <summary>
        /// 统一支付Key
        /// </summary>
        public string OrderKey { get; set; }
        /// <summary>
        /// 订单支付时间
        /// </summary>
        public DateTime PaymentDate { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal ProductPrice { get; set; }
        /// <summary>
        /// 应付金额
        /// </summary>
        public string PayablesAmount { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal DiscountAmout { get; set; }

        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public string CreationDate { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayType { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string ProductPicture { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 订单详情数量
        /// </summary>
        public string ProductCount { get; set; }
        /// <summary>
        /// 订单明细Id
        /// </summary>
        public string OrderDetailGuid { get; set; }

        /// <summary>
        /// 服务单Id
        /// </summary>
        public string ServiceGuid { get; set; }

        /// <summary>
        /// 服务单详情Guid
        /// </summary>
        public string ServiceDetailGuid { get; set; }

        /// <summary>
        /// 售后类型
        /// </summary>
        public string ServiceType { get; set; }

        /// <summary>
        /// 售后单状态
        /// </summary>
        public string ServiceStatus { get; set; }
    }
}
