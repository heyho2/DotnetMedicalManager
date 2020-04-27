using GD.Models.CommonEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Merchant.Appointment
{
    /// <summary>
    /// 执行端-预约的开始与结束 请求Dto
    /// </summary>
    public class ChangeAppointmentStatusRequestDto
    {
        /// <summary>
        /// 登录人GUID，即商户GUID
        /// </summary>
        public string UserGuid { get; set; }

        /// <summary>
        /// 预约Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "预约Guid")]
        public string ConsumptionGuid { get; set; }

        /// <summary>
        /// 预约状态
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "预约状态")]
        public ConsumptionStatusEnum ConsumptionStatus { get; set; }
            //ConsumptionStatusEnum.Canceled.ToString();
    }
}
