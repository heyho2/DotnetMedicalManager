using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 会员健康指标预警设置
    /// </summary>
    public class CreateIndicatorWarningLimitRequestDto
    {
        /// <summary>
        /// 会员Guid
        /// </summary>
        [Required(ErrorMessage = "会员参数未提供")]
        public string ConsumerGuid { get; set; }
        /// <summary>
        /// 是否开启预警
        /// </summary>
        public bool TurnOnWarning { get; set; }
        /// <summary>
        /// 预警设置
        /// </summary>
        public List<IndicatorWarningLimit> Limits { get; set; } = new List<IndicatorWarningLimit>();
    }

    /// <summary>
    /// 
    /// </summary>
    public class IndicatorWarningLimit
    {
        /// <summary>
        /// 指标项guid
        /// </summary>
        public string OptionGuid { get; set; }
        /// <summary>
        /// 预警最小值
        /// </summary>
        public decimal? MinValue { get; set; }
        /// <summary>
        /// 预警最大值
        /// </summary>
        public decimal? MaxValue { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetIndicatorWarningLimit
    {
        /// <summary>
        /// 
        /// </summary>
        public bool TurnOnWarning { get; set; }
        /// <summary>
        /// 指标项guid
        /// </summary>
        public string OptionGuid { get; set; }
        /// <summary>
        /// 预警最小值
        /// </summary>
        public decimal? MinValue { get; set; }
        /// <summary>
        /// 预警最大值
        /// </summary>
        public decimal? MaxValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OptionName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OptionUnit { get; set; }

    }
}
