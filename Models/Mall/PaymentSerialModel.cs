
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
    /// 支付流水Model
    ///</summary>
    [Table("t_mall_payment_serial")]
    public class PaymentSerialModel : BaseModel
    {
        ///<summary>
        ///付款流水GUID
        ///</summary>
        [Column("payment_serial_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "付款流水GUID")]
        public string PaymentSerialGuid { get; set; }

        ///<summary>
        ///付款流水号
        ///</summary>
        [Column("payment_serial_no"), Required(ErrorMessage = "{0}必填"), Display(Name = "付款流水号")]
        public string PaymentSerialNo { get; set; }

        ///<summary>
        ///应付
        ///</summary>
        [Column("payable_amount"), Required(ErrorMessage = "{0}必填"), Display(Name = "应付")]
        public decimal PayableAmount { get; set; }

        ///<summary>
        ///实付
        ///</summary>
        [Column("paid_amount"), Required(ErrorMessage = "{0}必填"), Display(Name = "实付")]
        public decimal PaidAmount { get; set; }

        ///<summary>
        ///是否存在异常
        ///</summary>
        [Column("abnormal"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否存在异常")]
        public bool Abnormal { get; set; } = false;
    }
}



