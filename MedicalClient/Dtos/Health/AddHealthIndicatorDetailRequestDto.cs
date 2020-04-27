using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 新增健康指标数据Dto
    /// </summary>
    public class AddHealthIndicatorRequestDto
    {
        /// <summary>
        /// 用户guid
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 健康指标Id
        /// </summary>
        [Required(ErrorMessage = "健康指标Id必填")]
        public string IndicatorGuid { get; set; }
        /// <summary>
        /// 健康指标选项数据
        /// </summary>
        public List<HealthIndicatorOptionRequest> HealthIndicatorOptionList { get; set; } = new List<HealthIndicatorOptionRequest>();
    }
    /// <summary>
    /// 指标选项
    /// </summary>
    public class HealthIndicatorOptionRequest
    {
        /// <summary>
        /// 指标选项Id
        /// </summary>
        public string OptionGuid { get; set; }
        /// <summary>
        /// 选项值
        /// </summary>
        public decimal? IndicatorValue { get; set; }
    }
}
