using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Merchant.MerchantFlowing
{
    /// <summary>
    /// 商户流水报表请求DTO
    /// </summary>
    public class MerchantFlowingReportResquestDto : BaseDto
    {
        /// <summary>
        /// 商户GUID
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "商户GUID")]
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "开始时间")]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "结束时间")]
        public DateTime? EndTime { get; set; }
    }
}