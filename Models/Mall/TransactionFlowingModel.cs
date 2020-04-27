using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GD.Models.Mall
{
    /// <summary>
    /// 交易流水Model
    /// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("t_mall_transaction_flowing")]
    public class TransactionFlowingModel : BaseModel
    {
        /// <summary>
        /// 交易流水GUID
        /// </summary>
        [Column("transaction_flowing_guid"), System.ComponentModel.DataAnnotations.Key, Required(ErrorMessage = "{0}必填"), Display(Name = "交易流水GUID")]
        public string TransactionFlowingGuid { get; set; }

        /// <summary>
        /// 交易流水号(流水号)
        /// </summary>
        [Column("transaction_number"), Required(ErrorMessage = "{0}必填"), Display(Name = "交易流水号")]
        public string TransactionNumber { get; set; }

        /// <summary>
        /// 商品订单号(平台生产的生产订单号,传给外部支付平台)
        /// </summary>
        [Column("out_trade_no"), Required(ErrorMessage = "{0}必填"), Display(Name = "商品订单号")]
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 交易渠道(银行卡，现金，苹果支付，积分支付，微信支付，支付宝，线下支付)
        /// </summary>
        [Column("channel"), Required(ErrorMessage = "{0}必填"), Display(Name = "交易渠道")]
        public string Channel { get; set; }

        /// <summary>
        /// 交易渠道编号(外部支付平台生成编号)
        /// </summary>
        [Column("channel_number"), Required(ErrorMessage = "{0}必填"), Display(Name = "交易渠道")]
        public string ChannelNumber { get; set; }

        /// <summary>
        /// 支付账号(银行卡号、微信号等等)
        /// </summary>
        [Column("pay_account"), Required(ErrorMessage = "{0}必填"), Display(Name = "支付账号")]
        public string PayAccount { get; set; }

        /// <summary>
        /// 交易金额（分）
        /// </summary>
        [Column("amount"), Required(ErrorMessage = "{0}必填"), Display(Name = "交易金额")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 外部退款单号
        /// </summary>
        [Column("out_refund_no"), Required(ErrorMessage = "{0}必填"), Display(Name = "外部退款单号")]
        public string OutRefundNo { get; set; }

        /// <summary>
        /// 交易流水状态(WaitForPayment:等待支付, WaitingForTheAccount:等待到账, Success:成功, Expired:过期, Failure:失败, RequestRefund:退款申请, RefuseRefund:拒绝退款, WaitingForTheAccountByRefund:等待退款到账, RefundSuccess:退款成功, RefundExpired:退款支付过期, RefundFailure:退款失败, TradingClosed:交易关闭)
        /// </summary>
        [Column("transaction_status"), Required(ErrorMessage = "{0}必填"), Display(Name = "交易流水状态")]
        public string TransactionStatus { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("remarks"), Display(Name = "备注")]
        public string Remarks { get; set; }
    }

    /// <summary>
    /// 流水状态
    /// </summary>
    public enum TransactionStatusEnum
    {
        /// <summary>
        /// 等待支付
        /// </summary>
        WaitForPayment = 1,
        /// <summary>
        /// 等待到账
        /// </summary>
        WaitingForTheAccount = 2,
        /// <summary>
        /// Success
        /// </summary>
        Success = 3,
        /// <summary>
        /// 过期
        /// </summary>
        Expired = 4,
        /// <summary>
        /// 失败
        /// </summary>
        Failure,
        /// <summary>
        /// 退款申请
        /// </summary>
        RequestRefund,
        /// <summary>
        /// 拒绝退款
        /// </summary>
        RefuseRefund,
        /// <summary>
        /// 等待退款到账
        /// </summary>
        WaitingForTheAccountByRefund,
        /// <summary>
        /// 退款成功
        /// </summary>
        RefundSuccess,
        /// <summary>
        /// 退款支付过期
        /// </summary>
        RefundExpired,
        /// <summary>
        /// 退款失败
        /// </summary>
        RefundFailure,
        /// <summary>
        /// 交易关闭
        /// </summary>
        TradingClosed
    }
}