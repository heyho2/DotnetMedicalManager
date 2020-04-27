using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Doctor
{
    /// <summary>
    /// 
    /// </summary>
    [Table("t_doctor_hospital_payment")]
    public class HospitalPaymentModel : BaseModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Column("payment_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "")]
        public string PaymentGuid { get; set; }

        /// <summary>
        /// 医院GUID
        /// </summary>
        [Column("hospital_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "医院GUID")]
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 微信支付订单号（由微信平台生成）
        /// </summary>
        [Column("transaction_id"), Required(ErrorMessage = "{0}必填"), Display(Name = "微信支付订单号（由微信平台生成）")]
        public string TransactionId { get; set; }

        /// <summary>
        /// 商户支付单号
        /// </summary>
        [Column("out_trade_no"), Required(ErrorMessage = "{0}必填"), Display(Name = "商户支付单号")]
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 商户退款单号
        /// </summary>
        [Column("out_refund_no")]
        public string OutRefundNo { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        [Column("merchant_no")]
        public string MerchantNo { get; set; }

        /// <summary>
        /// 用户付款OPENID
        /// </summary>
        [Column("pay_account"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户付款OPENID")]
        public string PayAccount { get; set; }

        /// <summary>
        /// 支付金额（分）
        /// </summary>
        [Column("amount"), Required(ErrorMessage = "{0}必填"), Display(Name = "支付金额（分）")]
        public int Amount { get; set; }

        /// <summary>
        /// 支付状态（等待支付：WAITFORPAYMENT；已付款：SUCCESS；支付失败：FAILURE 申请退款：REQUESTREFUND；退款成功：REFUNDSUCCESS）
        /// </summary>
        [Column("status"), Required(ErrorMessage = "{0}必填"), Display(Name = "支付状态（等待支付：WAITFORPAYMENT；已付款：SUCCESS；支付失败：FAILURE申请退款：REQUESTREFUND；退款成功：REFUNDSUCCESS）")]
        public string Status { get; set; }
    }
}
