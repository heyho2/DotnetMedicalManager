using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GD.Models.Mall
{
    /// <summary>
    /// 订单-支付
    /// </summary>
    [Table("t_mall_finance_pay")]
    public class FinancePayModel : BaseModel
    {
        ///<summary>
        ///支付主键
        ///</summary>
        [Column("pay_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "支付主键")]
        public string PayGuid { get; set; }

        ///<summary>
        ///订单Guid
        ///</summary>
        [Column("order_guid"), Display(Name = "订单Guid")]
        public string OrderGuid { get; set; }

        ///<summary>
        ///渠道ID
        ///</summary>
        [Column("channel_id"), Display(Name = "渠道ID")]
        public string ChannelID { get; set; }
        ///<summary>
        ///订单号
        ///</summary>
        [Column("trade_no"), Display(Name = "订单号")]
        public string TradeNo { get; set; }
        ///<summary>
        ///支付方式（微信，支付宝）
        ///</summary>
        [Column("pay_way"), Display(Name = "支付方式（微信，支付宝）")]
        public string PayWay { get; set; }
        ///<summary>
        ///是否银盛支付（可不填）
        ///</summary>
        [Column("pay_mode"), Display(Name = "是否银盛支付（可不填）")]
        public string PayMode { get; set; }
        ///<summary>
        ///支付名称
        ///</summary>
        [Column("subject"), Display(Name = "支付名称")]
        public string Subject { get; set; }
        ///<summary>
        ///支付金额
        ///</summary>
        [Column("amount"), Display(Name = "支付金额")]
        public string Amount { get; set; }
        ///<summary>
        ///支付码（仅扫码枪支付）
        ///</summary>
        [Column("authcode"), Display(Name = "支付码（仅扫码枪支付）")]
        public string AuthCode { get; set; }
        ///<summary>
        ///支付类型（normal_pay，刷脸支付smilepay）扫码枪支付
        ///</summary>
        [Column("pay_type"), Display(Name = "支付类型（normal_pay，刷脸支付smilepay）扫码枪支付")]
        public string PayType { get; set; }
        ///<summary>
        ///默认null
        ///</summary>
        [Column("flag"), Display(Name = "默认null")]
        public string Flag { get; set; }
        ///<summary>
        ///支付时间
        ///</summary>
        [Column("pay_time"), Display(Name = "支付时间")]
        public string PayTime { get; set; }

        ///<summary>
        ///商户订单号
        ///</summary>
        [Column("out_trade_no"), Display(Name = "商户订单号")]
        public string OutTradeNo { get; set; }
        ///<summary>
        ///支付状态(closed关闭，cancel撤销)
        ///</summary>
        [Column("status"), Display(Name = "支付状态(closed关闭，cancel撤销)")]
        public string Status { get; set; }


        ///<summary>
        ///二维码base64
        ///</summary>
        [Column("qr_code"), Display(Name = "二维码base64")]
        public string Qr_Code { get; set; }
        
        ///<summary>
        ///返回码
        ///</summary>
        [Column("result_code"), Display(Name = "返回码")]
        public string ResultCode { get; set; }
        ///<summary>
        ///返回消息
        ///</summary>
        [Column("result_msg"), Display(Name = "返回消息")]
        public string ResultMsg { get; set; }
    }

    /// <summary>
    /// 支付状态枚举
    /// </summary>
    public enum PayStatusEnum
    {
        /// <summary>
        /// 支付中
        /// </summary>
        Paying,
        /// <summary>
        /// 支付完成
        /// </summary>
        PayOk,
        /// <summary>
        /// 已退款
        /// </summary>
        Refunded,
        /// <summary>
        /// 订单已关闭
        /// </summary>
        Closed,
        /// <summary>
        /// 订单已撤销
        /// </summary>
        Canceled
    }
}
