using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 
    /// </summary>
    public class GetHealthIndicatorOptionDetailResponseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string OptionName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OptionUnit { get; set; }

        /// <summary>
        /// 指标最小值
        /// </summary>
        public decimal? MinValue { get; set; }

        /// <summary>
        /// 指标最大值
        /// </summary>
        public decimal? MaxValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal? IndicatorValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IndicatorOptionGuid { get; set; }
    }
}
