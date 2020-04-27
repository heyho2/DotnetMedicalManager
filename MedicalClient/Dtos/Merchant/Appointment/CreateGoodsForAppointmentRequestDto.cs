using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Merchant.Appointment
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateGoodsForAppointmentRequestDto : BaseDto
    {
        /// <summary>
        /// 用户手机号
        /// </summary>
        [Required(ErrorMessage = "用户手机号必填")]
        public string Phone { get; set; }

        /// <summary>
        /// 项目guid
        /// </summary>
        [Required(ErrorMessage ="项目guid必填")]
        public string ProjectGuid { get; set; }

        /// <summary>
        /// 商户guid
        /// </summary>
        public string MerchantGuid { get; set; }


    }
}
