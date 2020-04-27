using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Mall
{
    ///<summary>
    ///
    ///</summary>
    [Table("t_mall_aftersale_detail")]
    public class AfterSaleDetailModel : BaseModel
    {

        ///<summary>
        ///售后详情ID
        ///</summary>
        [Column("detail_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "售后详情ID")]
        public string DetailGuid { get; set; }

        ///<summary>
        ///售后服务单ID
        ///</summary>
        [Column("service_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "售后服务单ID")]
        public string ServiceGuid { get; set; }

        ///<summary>
        ///订单详情ID
        ///</summary>
        [Column("order_detail_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "订单详情ID")]
        public string OrderDetailGuid { get; set; }

        ///<summary>
        ///商品ID
        ///</summary>
        [Column("product_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "商品ID")]
        public string ProductGuid { get; set; }

        ///<summary>
        ///商品单价
        ///</summary>
        [Column("unit_price"), Required(ErrorMessage = "{0}必填"), Display(Name = "商品单价")]
        public int UnitPrice { get; set; }

        ///<summary>
        ///商品数量
        ///</summary>
        [Column("product_count"), Required(ErrorMessage = "{0}必填"), Display(Name = "商品数量")]
        public int ProductCount { get; set; }

        ///<summary>
        ///退款金额,单位分，若无退款，默认为0
        ///</summary>
        [Column("refund_fee"), Required(ErrorMessage = "{0}必填"), Display(Name = "退款金额,单位分，若无退款，默认为0")]
        public int RefundFee { get; set; }

    }
}



