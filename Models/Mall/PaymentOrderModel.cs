
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Mall
{
    ///<summary>
    /// 支付订单Model
    ///</summary>
    [Table("t_mall_payment_order")]
    public class PaymentOrderModel : BaseModel
    {
        ///<summary>
        ///支付流水订单主键
        ///</summary>
        [Column("payment_order_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "支付流水订单主键")]
        public string PaymentOrderGuid { get; set; }

        ///<summary>
        ///支付流水GUID
        ///</summary>
        [Column("payment_serial_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "支付流水GUID")]
        public string PaymentSerialGuid { get; set; }

        ///<summary>
        ///订单GUID
        ///</summary>
        [Column("order_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "订单GUID")]
        public string OrderGuid { get; set; }

        ///<summary>
        ///支付金额
        ///</summary>
        [Column("order_paid"), Required(ErrorMessage = "{0}必填"), Display(Name = "支付金额")]
        public decimal OrderPaid { get; set; }
    }
}



