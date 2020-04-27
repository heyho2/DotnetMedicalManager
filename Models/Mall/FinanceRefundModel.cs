using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GD.Models.Mall
{
    /// <summary>
    /// 退款
    /// </summary>
    [Table("t_mall_finance_refund")]
    public class FinanceRefundModel : BaseModel
    {
        ///<summary>
        ///退款主键Guid
        ///</summary>
        [Column("refund_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "退款主键Guid")]
        public string RefundGuid { get; set; }

        ///<summary>
        ///订单号
        ///</summary>
        [Column("trade_no"), Display(Name = "订单号")]
        public string TradeNo { get; set; }

        ///<summary>
        ///原因
        ///</summary>
        [Column("reason"), Display(Name = "原因")]
        public string Reason { get; set; }
        ///<summary>
        ///退款订单号
        ///</summary>
        [Column("refund_no"), Display(Name = "退款订单号")]
        public string RefundNo { get; set; }
        ///<summary>
        ///退款金额
        ///</summary>
        [Column("refund_fee"), Display(Name = "退款金额")]
        public string RefundFee { get; set; }
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
}
