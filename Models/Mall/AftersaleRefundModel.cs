using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Mall
{
    ///<summary>
    ///
    ///</summary>
    [Table("t_mall_aftersale_refund")]
    public class AfterSaleRefundModel : BaseModel
    {

        ///<summary>
        ///售后退款记录主键
        ///</summary>
        [Column("refund_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "售后退款记录主键")]
        public string RefundGuid { get; set; }

        ///<summary>
        ///服务单ID
        ///</summary>
        [Column("service_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "服务单ID")]
        public string ServiceGuid { get; set; }

        ///<summary>
        ///所属支付流水ID
        ///</summary>
        [Column("flowing_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "所属支付流水ID")]
        public string FlowingGuid { get; set; }

        ///<summary>
        ///所属订单GUID
        ///</summary>
        [Column("order_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "所属订单GUID")]
        public string OrderGuid { get; set; }

        ///<summary>
        ///退款金额，单位分
        ///</summary>
        [Column("refund_fee"), Required(ErrorMessage = "{0}必填"), Display(Name = "退款金额，单位分")]
        public int RefundFee { get; set; }

        ///<summary>
        ///退款中、已成功、已失败
        ///</summary>
        [Column("status"), Required(ErrorMessage = "{0}必填"), Display(Name = "退款中、已成功、已失败")]
        public string Status { get; set; }

        ///<summary>
        ///商户退款单号(由业务平台提供给微信的退款唯一标识)
        ///</summary>
        [Column("out_refund_no")]
        public string OutRefundNo { get; set; }
    }
}



