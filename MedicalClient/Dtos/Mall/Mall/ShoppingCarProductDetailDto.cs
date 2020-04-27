using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 购物车商品明细Dto
    /// </summary>
    public class ShoppingCarProductDetailDto : BaseDto
    {
        /// <summary>
        /// 购物车Guid
        /// </summary>
        public string ItemGuid { get; set; }
        
        /// <summary>
        /// 商品Guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品有效期天数（自购买日起）
        /// </summary>
        public int EffectiveDays { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal ProductPrice { get; set; }

        /// <summary>
        /// 商品平台
        /// </summary>
        public string PlatformType { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int ProductCount { get; set; }

        /// <summary>
        /// 商铺guid
        /// </summary>
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 商品项guid
        /// </summary>
        public string ProjectGuid { get; set; }

        /// <summary>
        /// 商品项目适用阈值
        /// </summary>
        public int ProjectThreshold { get; set; }

        /// <summary>
        /// 商品项名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 项目价格
        /// </summary>
        public decimal ProjectPrice { get; set; }

        /// <summary>
        /// 商品项次数
        /// </summary>
        public int ProjectTimes { get; set; }

        /// <summary>
        /// 是否允许转赠
        /// </summary>
        public bool AllowPresent { get; set; }

        /// <summary>
        /// 商品是否采用预付款方式购买
        /// </summary>
        public bool AdvancePayment { get; set; }

        /// <summary>
        /// 商品预付款比例
        /// </summary>
        public decimal AdvancePaymentRate { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal Freight { get; set; }
        /// <summary>
        /// 是否实体物品(服务Service，实体Physical)
        /// </summary>
        public string ProductForm { get; set; }

    }

    /// <summary>
    /// 多订单汇总支付信息
    /// </summary>
    public class OrdersPaidInfo : BaseDto {
        /// <summary>
        /// 付款流水号（由支付接口返回）
        /// </summary>
        public string PaymentSerialNo { get; set; }

        /// <summary>
        /// 订单支付总额
        /// </summary>
        public decimal PaidTotal { get; set; }

        /// <summary>
        /// 各订单支付额信息
        /// </summary>
        public List<OrderPaidInfo> OrderPaidInfos { get; set; }
    }
    /// <summary>
    /// 订单支付信息
    /// </summary>
    public class OrderPaidInfo : BaseDto {

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderGuid { get; set; }

        /// <summary>
        /// 订单支付金额
        /// </summary>
        public decimal PaidAmount { get; set; }
    }
}
