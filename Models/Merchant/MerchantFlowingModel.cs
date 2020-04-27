using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GD.Models.Merchant
{
    /// <summary>
    /// 商户流水表
    /// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("t_merchant_flowing")]
    public class MerchantFlowingModel : BaseModel
    {
        /// <summary>
        /// 商户流水表
        /// </summary>
        [Column("merchant_flowing_guid"), System.ComponentModel.DataAnnotations.Key, Required(ErrorMessage = "{0}必填"), Display(Name = "商户流水表")]
        public string MerchantFlowingGuid { get; set; }

        /// <summary>
        /// 用户支付流水号
        /// </summary>
        [Column("transaction_flowing_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "交易流水号")]
        public string TransactionFlowingGuid { get; set; }

        /// <summary>
        /// 订单GUID
        /// </summary>
        [Column("order_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "交易流水号")]
        public string OrderGuid { get; set; }

        /// <summary>
        /// 商户账号
        /// </summary>
        [Column("merchant_account"), Required(ErrorMessage = "{0}必填"), Display(Name = "交易流水号")]
        public string MerchantAccount { get; set; }

        /// <summary>
        /// 金钱流动状态(PlatformNotReceived:平台未收款, PlatformCollection:平台已收款, RefundSuccess:已退款, RefundFailure:退款失败, 钱已经回退至平台, TradingClosed:交易关闭(失效), BeingAllocated:正在划拨, AllocatedSuccess:划拨成功, TransferFailed:划拨失败, 钱已经回退至平台)
        /// </summary>
        [Column("flow_status"), Required(ErrorMessage = "{0}必填"), Display(Name = "交易流水号")]
        public string FlowStatus { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [Column("amount"), Required(ErrorMessage = "{0}必填"), Display(Name = "交易流水号")]
        public decimal Amount { get; set; }
    }

    /// <summary>
    /// 金钱流动状态
    /// </summary>
    public enum FlowStatus
    {
        /// <summary>
        /// 平台未收款
        /// </summary>
        PlatformNotReceived = 1,
        /// <summary>
        /// 平台已收款
        /// </summary>
        PlatformCollection = 2,
        /// <summary>
        /// 已退款
        /// </summary>
        RefundSuccess = 3,
        /// <summary>
        /// 退款失败
        /// </summary>
        RefundFailure = 4,
        /// <summary>
        /// 交易关闭
        /// </summary>
        TradingClosed = 5,
        /// <summary>
        /// 正在划拨
        /// </summary>
        BeingAllocated = 6,
        /// <summary>
        /// 划拨成功
        /// </summary>
        AllocatedSuccess = 7,
        /// <summary>
        /// 划拨失败
        /// </summary>
        TransferFailed = 8
    }
}