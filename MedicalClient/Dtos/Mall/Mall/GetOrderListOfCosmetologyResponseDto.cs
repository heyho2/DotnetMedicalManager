using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 双美-获取订单列表响应Dto
    /// </summary>
    public class GetOrderListOfCosmetologyResponseDto : BaseDto
    {
        /// <summary>
        /// 订单Guid
        /// </summary>
        public string OrderGuid { get; set; }

        /// <summary>
        /// 订单是否有效
        /// </summary>
        public bool OrderEnable { get; set; }

        /// <summary>
        /// 统一下单KEY
        /// </summary>
        public string OrderKey { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单日期
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal PaidAmount { get; set; }
        /// <summary>
        /// 订单优惠金额
        /// </summary>
        public decimal DiscountAmout { get; set; }
        /// <summary>
        /// 欠款
        /// </summary>
        public decimal Debt { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderStatus { get; set; }

        /// <summary>
        ///订单类型（服务类，实体类）
        /// </summary>
        public string OrderCategory { get; set; }

        /// <summary>
        ///订单主次：Primary:主体订单、Secondary:明细订单
        /// </summary>
        public string OrderMark { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int ProductCount { get; set; }

        /// <summary>
        /// 订单明细
        /// </summary>
        public List<OrderDetailsOfCosmetologyResponseDto> OrderDetails { get; set; }


    }

    /// <summary>
    /// 订单明细商品详情
    /// </summary>
    public class OrderDetailsOfCosmetologyResponseDto : BaseDto
    {
        /// <summary>
        /// 商品Guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int ProductCount { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string ProductPicture { get; set; }

        /// <summary>
        /// 商品是否上架
        /// </summary>
        public bool OnSale { get; set; }
    }

    /// <summary>
    /// 双美订单明细Dto
    /// </summary>
    public class OrderDetailsOfCosmetologyDto : BaseDto
    {
        /// <summary>
        /// 订单Guid
        /// </summary>
        public string OrderGuid { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 商户是否可用
        /// </summary>
        public bool MerchantEnable { get; set; }

        /// <summary>
        /// 统一下单KEY
        /// </summary>
        public string OrderKey { get; set; }

        /// <summary>
        /// 订单日期
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal PaidAmount { get; set; }
        /// <summary>
        /// 订单优惠金额
        /// </summary>
        public decimal DiscountAmout { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderStatus { get; set; }

        /// <summary>
        ///订单类型（服务类，实体类）
        /// </summary>
        public string OrderCategory { get; set; }

        /// <summary>
        ///订单主次：Primary:主体订单、Secondary:明细订单
        /// </summary>
        public string OrderMark { get; set; }

        /// <summary>
        /// 商品Guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品是否上架
        /// </summary>
        public bool OnSale { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string ProductPicture { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int ProductCount { get; set; }

        /// <summary>
        /// 欠款
        /// </summary>
        public decimal Debt { get; set; }
    }
}
