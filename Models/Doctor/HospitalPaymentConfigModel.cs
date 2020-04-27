
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Doctor
{
    /// <summary>
    /// 医院支付相关参数
    /// </summary>
    [Table("t_doctor_hospital_payment_config")]
    public class HospitalPaymentConfigModel:BaseModel
    {
        
         /// <summary>
         /// 医院guid
         /// </summary>
         [Column("hospital_guid"),Key,Required(ErrorMessage = "{0}必填"), Display(Name = "医院guid")]
         public string HospitalGuid{ get;set; }
        
         /// <summary>
         /// 医院微信商户号Id
         /// </summary>
         [Column("merchant_id"),Required(ErrorMessage = "{0}必填"), Display(Name = "医院微信商户号Id")]
         public string MerchantId{ get;set; }
        
         /// <summary>
         /// 商户秘钥，已加密
         /// </summary>
         [Column("merchant_secret"),Required(ErrorMessage = "{0}必填"), Display(Name = "商户秘钥，已加密")]
         public string MerchantSecret{ get;set; }
        
        
    }
}



