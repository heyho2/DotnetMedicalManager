using GD.Common.Base;
using GD.Models.CommonEnum;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Mall
{
    ///<summary>
    ///订单表模型
    ///</summary>
    [Table("t_mall_order")]
    public class OrderModel : BaseModel
    {
        ///<summary>
        ///订单GUID
        ///</summary>
        [Column("order_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "订单GUID")]
        public string OrderGuid { get; set; }

        ///<summary>
        ///订单编号
        ///</summary>
        [Column("order_no"), Required(ErrorMessage = "{0}必填"), Display(Name = "订单编号")]
        public string OrderNo { get; set; } = GD.Common.Helper.OrderNoCreater.Create(Common.EnumDefine.PlatformType.CloudDoctor);

        ///<summary>
        ///用户GUID
        ///</summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户GUID")]
        public string UserGuid { get; set; }

        ///<summary>
        ///商户guid
        ///</summary>
        [Column("merchant_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "商户GUID")]
        public string MerchantGuid { get; set; }

        ///<summary>
        ///活动类型
        ///</summary>
        [Column("order_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "订单类型")]
        public string OrderType { get; set; } = OrderTypeEnum.Normal.ToString();

        ///<summary>
        ///订单种类
        ///</summary>
        [Column("order_category"), Required(ErrorMessage = "{0}必填"), Display(Name = "订单种类")]
        public string OrderCategory { get; set; } = OrderCategoryEnum.Service.ToString();

        ///<summary>
        ///订单主次：Primary:主体订单、Secondary:明细订单
        ///</summary>
        [Column("order_mark"), Required(ErrorMessage = "{0}必填"), Display(Name = "订单种类")]
        public string OrderMark { get; set; } = OrderMarkEnum.Primary.ToString();

        /// <summary>
        /// 统一下单key
        /// </summary>
        [Column("order_key"), Required(ErrorMessage = "{0}必填"), Display(Name = "统一下单key")]
        public string OrderKey { get; set; }

        ///<summary>
        ///商品总数量
        ///</summary>
        [Column("product_count"), Required(ErrorMessage = "{0}必填"), Display(Name = "商品总数量")]
        public int ProductCount { get; set; }

        ///<summary>
        ///
        ///</summary>
        [Column("order_phone"), Required(ErrorMessage = "{0}必填"), Display(Name = "")]
        public string OrderPhone { get; set; }

        ///<summary>
        ///收货地址
        ///</summary>
        [Column("order_address"), Required(ErrorMessage = "{0}必填"), Display(Name = "收货地址")]
        public string OrderAddress { get; set; }

        ///<summary>
        ///订单收件人
        ///</summary>
        [Column("order_receiver"), Required(ErrorMessage = "{0}必填"), Display(Name = "订单收件人")]
        public string OrderReceiver { get; set; }

        ///<summary>
        ///订单状态：待付款，待发货，待收货，退单
        ///</summary>
        [Column("order_status"), Required(ErrorMessage = "{0}必填"), Display(Name = "订单状态：待付款，待发货，待收货，退单")]
        public string OrderStatus { get; set; } = OrderStatusEnum.Obligation.ToString();

        /// <summary>
        /// 付款日期
        /// </summary>
        [Column("payment_date"), Required(ErrorMessage = "{0}必填"), Display(Name = "付款日期")]
        public DateTime? PaymentDate { get; set; }

        ///<summary>
        ///支付方式：银行卡，现金，苹果支付，积分支付，微信支付，支付宝, 线下支付
        ///</summary>
        [Column("pay_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "支付方式：银行卡，现金，苹果支付，积分支付，微信支付，支付宝，线下支付")]
        public string PayType { get; set; } = PayTypeEnum.Wechat.ToString();
        ///<summary>
        ///应付金额
        ///</summary>
        [Column("payables_amount"), Required(ErrorMessage = "{0}必填"), Display(Name = "应付金额")]
        public decimal PayablesAmount { get; set; }

        ///<summary>
        ///实付金额
        ///</summary>
        [Column("paid_amount"), Required(ErrorMessage = "{0}必填"), Display(Name = "实付金额")]
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// 欠款金额
        /// </summary>
        [Column("debt"), Required(ErrorMessage = "{0}必填"), Display(Name = "欠款金额")]
        public decimal Debt { get; set; } = 0m;

        ///<summary>
        ///优惠金额
        ///</summary>
        [Column("discount_amout"), Required(ErrorMessage = "{0}必填"), Display(Name = "优惠金额")]
        public decimal DiscountAmout { get; set; } = 0m;
        ///<summary>
        ///运费
        ///</summary>
        [Column("freight"), Required(ErrorMessage = "{0}必填"), Display(Name = "运费")]
        public decimal Freight { get; set; } = 0M;

        ///<summary>
        ///快递公司
        ///</summary>
        [Column("express_company"), Display(Name = "快递公司")]
        public string ExpressCompany { get; set; }

        ///<summary>
        ///快递单号
        ///</summary>
        [Column("express_no"), Display(Name = "快递单号")]
        public string ExpressNo { get; set; }

        ///<summary>
        ///订单备注
        ///</summary>
        [Column("remark"), Required(ErrorMessage = "{0}必填"), Display(Name = "订单备注")]
        public string Remark { get; set; }

        /// <summary>
        /// 交易流水GUID
        /// </summary>
        [Column("transaction_flowing_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "交易流水号")]
        public string TransactionFlowingGuid { get; set; }

        ///<summary>
        ///平台类型
        ///</summary>
        [Column("platform_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "平台类型")]
        public string PlatformType { get; set; } = GD.Common.EnumDefine.PlatformType.CloudDoctor.ToString();

        /// <summary>
        /// 订单类型
        /// </summary>
        public enum OrderTypeEnum
        {
            /// <summary>
            /// 普通订单
            /// </summary>
            [Description("普通订单")]
            Normal = 1,
            /// <summary>
            /// 团购订单
            /// </summary>
            [Description("团购订单")]
            Group
        }

        /// <summary>
        /// 订单种类
        /// </summary>
        public enum OrderCategoryEnum
        {
            /// <summary>
            /// 服务类
            /// </summary>
            [Description("服务类")]
            Service = 1,
            /// <summary>
            /// 实体类
            /// </summary>
            [Description("实体类")]
            Physical

        }
        /// <summary>
        ///订单主次枚举
        /// </summary>
        public enum OrderMarkEnum
        {
            /// <summary>
            /// 主体订单
            /// </summary>
            [Description("主体订单")]
            Primary = 1,
            /// <summary>
            /// 明细订单
            /// </summary>
            [Description("明细订单")]
            Secondary

        }
    }


}