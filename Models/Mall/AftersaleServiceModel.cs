using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Mall
{
    ///<summary>
    ///
    ///</summary>
    [Table("t_mall_aftersale_service")]
    public class AfterSaleServiceModel : BaseModel
    {

        ///<summary>
        ///服务单主键ID
        ///</summary>
        [Column("service_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "服务单主键ID")]
        public string ServiceGuid { get; set; }

        ///<summary>
        ///服务单号
        ///</summary>
        [Column("service_no"), Required(ErrorMessage = "{0}必填"), Display(Name = "服务单号")]
        public string ServiceNo { get; set; }

        ///<summary>
        ///商户GUID
        ///</summary>
        [Column("merchant_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "商户GUID")]
        public string MerchantGuid { get; set; }

        ///<summary>
        ///所属订单GUID
        ///</summary>
        [Column("order_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "所属订单GUID")]
        public string OrderGuid { get; set; }

        ///<summary>
        ///用户GUID
        ///</summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户GUID")]
        public string UserGuid { get; set; }

        ///<summary>
        ///售后状态
        ///</summary>
        [Column("status"), Required(ErrorMessage = "{0}必填"), Display(Name = "售后状态")]
        public string Status { get; set; }

        ///<summary>
        ///售后类型:退款不退货、退款退货、换货
        ///</summary>
        [Column("type"), Required(ErrorMessage = "{0}必填"), Display(Name = "售后类型:退款不退货、退款退货、换货")]
        public string Type { get; set; }

        ///<summary>
        ///快递单号
        ///</summary>
        [Column("express_no")]
        public string ExpressNo { get; set; }

        ///<summary>
        ///售后原因
        ///</summary>
        [Column("reason")]
        public string Reason { get; set; }

        ///<summary>
        ///售后原因
        ///</summary>
        [Column("refuse_reason")]
        public string RefuseReason { get; set; }

        ///<summary>
        ///退款金额,单位分，若无退款，则默认为0
        ///</summary>
        [Column("refund_fee"), Required(ErrorMessage = "{0}必填"), Display(Name = "退款金额,单位分，若无退款，则默认为0")]
        public int RefundFee { get; set; }
    }
}



