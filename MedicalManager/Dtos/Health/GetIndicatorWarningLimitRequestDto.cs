using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 
    /// </summary>
    public class GetIndicatorWarningLimitRequestDto
    {
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "会员参数未提供")]
        public string ConsumerGuid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "指标参数未提供")]
        public string IndicatorGuid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetIndicatorWarningLimitResponseDto
    {
        /// <summary>
        /// 是否开启预警
        /// </summary>
        public bool TurnOnWarning { get; set; }
        /// <summary>
        /// 预警设置
        /// </summary>
        public List<GetIndicatorWarningLimit> Limits { get; set; } = new List<GetIndicatorWarningLimit>();
    }
}
