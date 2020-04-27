using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Merchant.Therapist
{
    /// <summary>
    /// 获取商户某天某项目的美疗师和排班详情请求Dto
    /// </summary>
    public class GetTherapistsScheduleByProjectIdOneDayRequestDto : BaseDto
    {
        /// <summary>
        /// 商户guid
        /// </summary>
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 服务项目guid
        /// </summary>
        [Required(ErrorMessage = "服务项目guid必填")]
        public string ProjectGuid { get; set; }

        /// <summary>
        /// 排班日期
        /// </summary>
        [Required(ErrorMessage = "排班日期必填")]
        public DateTime ScheduleDate { get; set; }

    }
}
