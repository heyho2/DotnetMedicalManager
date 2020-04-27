using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 创建用户指标项
    /// </summary>
    public class CreateHealthIndicatorRequestDto
    {
        /// <summary>
        /// 会员标识
        /// </summary>
        [Required(ErrorMessage = "会员标识未提供")]
        public string UserGuid { get; set; }
        /// <summary>
        /// 健康指标Id
        /// </summary>
        public string IndicatorGuid { get; set; }
        /// <summary>
        /// 健康指标选项数据
        /// </summary>
        public List<HealthIndicatorOptionRequest> Options { get; set; } = new List<HealthIndicatorOptionRequest>();

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

    /// <summary>
    /// 
    /// </summary>
    public class UpdateHealthIndicatorSuggestion
    {
        /// <summary>
        /// 会员标识
        /// </summary>
        [Required(ErrorMessage = "会员标识未提供")]
        public string UserGuid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "健康建议必填"), MaxLength(500, ErrorMessage = "健康建议超过最大长度限制，请检查！")]
        public string Suggestion { get; set; }
    }
}
