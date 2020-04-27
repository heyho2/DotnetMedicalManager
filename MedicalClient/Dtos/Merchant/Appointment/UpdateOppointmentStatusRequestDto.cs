using GD.Models.CommonEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Merchant.Appointment
{
    /// <summary>
    /// 变更预约状态 - 请求Dto
    /// </summary>
    public class UpdateOppointmentStatusRequestDto
    {
        /// <summary>
        /// 预约Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "预约Guid")]
        public string ConsumptionGuid { get; set; }

        /// <summary>
        /// 来自哪个端口,默认为商户端
        /// </summary>
        public string FromPoint { get; set; } = "Merchant";

        ///// <summary>
        ///// 预约状态
        ///// </summary>
        //[Required(ErrorMessage = "{0}必填"), Display(Name = "预约状态")]
        //public ConsumptionStatusEnum ConsumptionStatus { get; set; }
    }
    /// <summary>
    /// 响应Dto
    /// </summary>
    public class UpdateOppointmentStatusResponseDto
    {

    }
}
