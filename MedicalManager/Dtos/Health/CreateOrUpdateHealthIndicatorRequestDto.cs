using GD.Models.Health;
using System.Collections.Generic;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 创建或更新健康指标
    /// </summary>
    public class CreateOrUpdateHealthIndicatorRequestDto
    {
        /// <summary>
        /// 
        /// </summary>
        public List<HealthIndicatorItem> Items { get; set; } = new List<HealthIndicatorItem>();
    }

    /// <summary>
    /// 指标项
    /// </summary>
    public class HealthIndicatorItem
    {
        /// <summary>
        /// 指标标识
        /// </summary>
        public string IndicatorGuid { get; set; }
        /// <summary>
        /// 指标名称
        /// </summary>
        public string IndicatorName { get; set; }
        /// <summary>
        /// 指标类型（0：单个值，1：多个值，默认为0）
        /// </summary>
        public int IndicatorType { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Display { get; set; }
        /// <summary>
        /// （不用传）
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<HealthIndicatorItemOption> Options { get; set; } = new List<HealthIndicatorItemOption>();
    }

    /// <summary>
    /// 指标项选项
    /// </summary>
    public class HealthIndicatorItemOption
    {
        /// <summary>
        /// 选项Guid
        /// </summary>
        public string OptionGuid { get; set; }
        /// <summary>
        /// 选项单位
        /// </summary>
        public string OptionUnit { get; set; }
        /// <summary>
        /// 选项名称
        /// </summary>
        public string OptionName { get; set; }
        /// <summary>
        /// 是否必填
        /// </summary>
        public bool Required { get; set; }
        /// <summary>
        ///参考范围最小值
        /// </summary>
        public decimal? MinValue { get; set; }
        /// <summary>
        /// 参考范围最大值
        /// </summary>
        public decimal? MaxValue { get; set; }
        /// <summary>
        /// （不用传）
        /// </summary>
        public int Sort { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HealthIndicatorContext
    {
        /// <summary>
        /// 
        /// </summary>
        public List<HealthIndicatorModel> InsertIndicatorModels { get; set; } = new List<HealthIndicatorModel>();
        /// <summary>
        /// 
        /// </summary>
        public List<HealthIndicatorModel> UpdateIndicatorModels { get; set; } = new List<HealthIndicatorModel>();
        /// <summary>
        /// 
        /// </summary>
        public List<HealthIndicatorOptionModel> InsertIndicatorOptionModels { get; set; } = new List<HealthIndicatorOptionModel>();
        /// <summary>
        /// 
        /// </summary>
        public List<HealthIndicatorOptionModel> UpdateIndicatorOptionModels { get; set; } = new List<HealthIndicatorOptionModel>();
        /// <summary>
        /// 
        /// </summary>
        public List<string> DeleteIndicatorGuids { get; set; } = new List<string>();
    }

    /// <summary>
    /// 
    /// </summary>
    public class HealthIndicatorItemContext
    {
        /// <summary>
        /// 
        /// </summary>
        public HealthIndicatorContext Context { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public HealthIndicatorItem Item { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<HealthIndicatorOptionModel> DbOptions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public HealthIndicatorItemOption SubmitOption { get; set; }
    }
}
