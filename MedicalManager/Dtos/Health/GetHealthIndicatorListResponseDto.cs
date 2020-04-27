using Newtonsoft.Json;
using System.Collections.Generic;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 获取日常健康指标列表
    /// </summary>
    public class GetHealthIndicatorListResponseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public List<HealthIndicator> HealthIndicators { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HealthIndicator
    {
        /// <summary>
        /// 指标Id
        /// </summary>
        public string IndicatorGuid { get; set; }
        /// <summary>
        /// 指标名称
        /// </summary>
        public string IndicatorName { get; set; }
        /// <summary>
        /// 指标类型
        /// </summary>
        public int IndicatorType { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Display { get; set; }
        /// <summary>
        /// 指标项列表
        /// </summary>
        public List<HealthIndicatorOption> Options { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BaseHealthIndicatorOption
    {
        /// <summary>
        /// 选项Id
        /// </summary>
        public string OptionGuid { get; set; }
        /// <summary>
        /// 选项名称
        /// </summary>
        public string OptionName { get; set; }
        /// <summary>
        /// 选项单位
        /// </summary>
        public string OptionUnit { get; set; }
        /// <summary>
        /// 是否必填
        /// </summary>
        public bool Required { get; set; }
        /// <summary>
        /// 参考范围最小值
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MinValue { get; set; }
        /// <summary>
        /// 参考范围最大值
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MaxValue { get; set; }
    }

    /// <summary>
    /// 指标项
    /// </summary>
    public class HealthIndicatorOption : BaseHealthIndicatorOption
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public string IndicatorGuid { get; set; }
        /// <summary>
        /// 选项排序
        /// </summary>
        public int Sort { get; set; }
    }
}
