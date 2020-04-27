using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GD.Models.Mall
{
    /// <summary>
    /// 订单列表
    /// </summary>
    public class OrderListInfoModel : BaseModel
    {
        ///<summary>
        ///订单GUID
        ///</summary>
        [Column("order_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "订单GUID")]
        public string OrderGuid { get; set; }
        ///<summary>
        ///订单状态
        ///</summary>
        [Column("order_status"), Required(ErrorMessage = "{0}必填"), Display(Name = "订单状态")]
        public string OrderStatus { get; set; }

        /// <summary>
        /// 商户guid
        ///</summary>
        [Column("merchant_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "商户guid")]
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 商铺名称
        ///</summary>
        [Column("merchant_name"), Display(Name = "商铺名称")]
        public string MerchantName { get; set; }

        /// <summary>
        /// 订单金额（实付）
        ///</summary>
        [Column("paid_amount"), Display(Name = "订单金额（实付）")]
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// 商品总数量
        ///</summary>
        [Column("product_count"), Display(Name = "商品总数量")]
        public int ProductCount { get; set; }

        /// <summary>
        /// 购物车记录主键id
        ///</summary>
        [Column("detail_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "购物车记录主键id")]
        public string DetailGuid { get; set; }

        /// <summary>
        /// 产品Guid
        ///</summary>
        [Column("product_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "产品Guid")]
        public string ProductGuid { get; set; }
        /// <summary>
        /// 产品名称
        ///</summary>
        [Column("product_name"), Display(Name = "产品名称")]
        public string ProductName { get; set; }
        /// <summary>
        /// 产品图片url
        ///</summary>
        [Column("ProductPicUrl"), Display(Name = "产品图片url")]
        public string ProductPicUrl { get; set; }

        /// <summary>
        /// 产品价格
        ///</summary>
        [Column("detail_price"), Required(ErrorMessage = "{0}必填"), Display(Name = "产品价格")]
        public decimal DetailPrice { get; set; }
        /// <summary>
        /// 购买数量
        ///</summary>
        [Column("detail_count"), Required(ErrorMessage = "{0}必填"), Display(Name = "购买数量")]
        public int DetailCount { get; set; }
        /// <summary>
        /// 商品评价
        ///</summary>
        [Column("comment_guid"), Display(Name = "商品评价")]
        public string CommentGuid { get; set; }
        /// <summary>
        /// 商品运费
        ///</summary>
        [Column("freight"), Display(Name = "商品运费")]
        public decimal Freight { get; set; }

        /// <summary>
        ///订单时间
        ///</summary>
        public DateTime? CreationDate { get; set; }
        /// <summary>
        /// 商品编号
        ///</summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 订单Key
        ///</summary>
        public string OrderKey { get; set; }
        /// <summary>
        /// 订单种类（Normal/Group）
        ///</summary>
        public string OrderType { get; set; }
        /// <summary>
        /// 订单分类（Service/Physical）
        ///</summary>
        public string OrderCategory { get; set; }
        /// <summary>
        /// 支付方式
        /// （支付方式：银行卡Card，现金Cash，苹果支付Apple，积分支付Score，微信支付Wechat，支付宝Alipay，线下支付OffLinePay）
        ///</summary>
        public string PayType { get; set; }
        /// <summary>
        /// 订单类型（Primary主订单、Secondary从订单）
        ///</summary>
        public string OrderMark { get; set; }
        /// <summary>
        /// 优惠金额
        ///</summary>
        public decimal DiscountAmout { get; set; }
        /// <summary>
        /// 商品是否在售
        ///</summary>
        public bool OnSale { get; set; }
        /// <summary>
        /// 商家是否存在
        ///</summary>
        public bool? MerchantEnable { get; set; }
    }
}
