using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Consumer.ActuatingStation
{
    /// <summary>
    /// 操作人员提交完成预约操作请求Dto
    /// </summary>
    public class CompleteConsumptionRequestDto : BaseDto
    {
        /// <summary>
        /// 预约guid
        /// </summary>
        [Required(ErrorMessage = "预约guid必填")]
        public string ConsumptionGuid { get; set; }

        /// <summary>
        /// 商户备注
        /// </summary>
        [StringLength(500)]
        public string MerchantRemark { get; set; }
    }
}
