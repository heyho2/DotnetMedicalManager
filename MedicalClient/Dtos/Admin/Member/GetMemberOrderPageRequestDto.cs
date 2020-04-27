using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Admin.Member
{
    /// <summary>
    /// 用户订单信息
    /// </summary>
    public class GetMemberOrderPageRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// id
        /// </summary>
        public string UserGuid { get; set; }
    }

    /// <summary>
    /// 用户订单信息
    /// </summary>
    public class GetMemberOrderPageResponseDto : BasePageResponseDto<GetMemberOrderPageItemDto>
    {

    }
    /// <summary>
    /// 用户订单信息
    /// </summary>
    public class GetMemberOrderPageItemDto : BaseDto
    {
        ///<summary>
        ///订单GUID
        ///</summary>
        public string OrderGuid { get; set; }

        ///<summary>
        ///订单编号
        ///</summary>
        public string OrderNo { get; set; }

        ///<summary>
        ///用户GUID
        ///</summary>
        public string UserGuid { get; set; }

        ///<summary>
        ///商户guid
        ///</summary>
        public string MerchantGuid { get; set; }


        ///<summary>
        ///商品总数量
        ///</summary>
        public int ProductCount { get; set; }

        ///<summary>
        ///
        ///</summary>
        public string OrderPhone { get; set; }

        ///<summary>
        ///收货地址
        ///</summary>
        public string OrderAddress { get; set; }

        ///<summary>
        ///订单收件人
        ///</summary>
        public string OrderReceiver { get; set; }

        ///<summary>
        ///订单状态：待付款，待发货，待收货，退单
        ///</summary>
        public string OrderStatus { get; set; }

        /// <summary>
        /// 付款日期
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        ///<summary>
        ///支付方式：银行卡，现金，苹果支付，积分支付，微信支付，支付宝
        ///</summary>
        public string PayType { get; set; }
        ///<summary>
        ///应付金额
        ///</summary>
        public decimal PayablesAmount { get; set; }

        ///<summary>
        ///实付金额
        ///</summary>
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// 欠款金额
        /// </summary>
        public decimal Debt { get; set; }

        ///<summary>
        ///优惠金额
        ///</summary>
        public decimal DiscountAmout { get; set; }
        ///<summary>
        ///运费
        ///</summary>
        public decimal Freight { get; set; }

        ///<summary>
        ///订单备注
        ///</summary>
        public string Remark { get; set; }
        ///<summary>
        ///平台类型
        ///</summary>
        public string PlatformType { get; set; }

        /// <summary>
        /// 创建时间，默认为系统当前时间   
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 使能标志，默认为 true
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 最后一次消费时间
        /// </summary>
        public DateTime? LastBuyDate { get; set; }
    }
}
