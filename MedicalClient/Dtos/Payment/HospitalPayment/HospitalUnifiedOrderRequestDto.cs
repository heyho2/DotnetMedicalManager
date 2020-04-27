using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Payment.HospitalPayment
{
    /// <summary>
    /// 医院公众号二维码扫码支付下单请求dto
    /// </summary>
    public class HospitalUnifiedOrderRequestDto
    {
        /// <summary>
        /// 付款用户OpenId
        /// </summary>
        [Required(ErrorMessage = "付款用户openid必填")]
        public string OpenId { get; set; }

        /// <summary>
        /// 支付金额，单位分
        /// </summary>
        [Required(ErrorMessage = "支付金额必填")]
        public int TotalFee { get; set; }

        /// <summary>
        /// 医院guid
        /// </summary>
        [Required(ErrorMessage = "医院guid必填")]
        public string HospitalGuid { get; set; }
    }
}
