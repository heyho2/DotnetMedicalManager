using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 健康指标Dto
    /// </summary>
    public class GetHealthIndicatorResponseDto
    {
        /// <summary>
        /// 医生建议
        /// </summary>
        public string Suggestion { get; set; }
        /// <summary>
        /// 健康指标集合
        /// </summary>
        public List<GetHealthIndicatorItem> Items { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetHealthIndicatorItem
    {
        /// <summary>
        /// 健康指标Id
        /// </summary>
        public string IndicatorGuid { get; set; }
        /// <summary>
        /// 健康指标名称
        /// </summary>
        public string IndicatorName { get; set; }
        /// <summary>
        /// 类型 false:为单个值 true:多个值
        /// </summary>
        public bool IndicatorType { get; set; }
        /// <summary>
        /// 选项名称(后台使用)
        /// </summary>
        public string OptionName { get; set; }
        /// <summary>
        /// 选项Id(指标选项Id)
        /// </summary>
        public string OptionGuid { get; set; }
        /// <summary>
        /// 指标单位(单指标使用)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OptionUnit { get; set; }
        /// <summary>
        /// 指标值是否必填
        /// </summary>
        public bool Required { get; set; }
        /// <summary>
        /// 用户填写最近一次值
        /// </summary>
        public decimal? ResultVale { get; set; }
        /// <summary>
        /// 指标最小值
        /// </summary>
        public decimal? MinValue { get; set; }
        /// <summary>
        /// 指标最大值
        /// </summary>
        public decimal? MaxValue { get; set; }
    }
    /// <summary>
    /// 指标详情数据请求Dto
    /// </summary>
    public class GetHealthIndicatorDetailRequestDto
    {
        /// <summary>
        /// 会员标识
        /// </summary>
        [Required(ErrorMessage = "会员标识未提供")]
        public string UserGuid { get; set; }
        /// <summary>
        /// 健康指标Id
        /// </summary>
        [Required(ErrorMessage = "健康指标标识未提供")]
        public string IndicatorGuid { get; set; }
        /// <summary>
        /// 当前页码值
        /// </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// 页码大小
        /// </summary>
        public int PageSize { get; set; } = 5;
    }
    /// <summary>
    /// 指标记录数据
    /// </summary>
    public class GetHealthIndicatorRecordResponseDto
    {
        /// <summary>
        /// 时间数据
        /// </summary>
        public List<DateTime> DateList { get; set; }
        /// <summary>
        /// 指标详情数据
        /// </summary>
        public List<GetHealthIndicatorDetailResponseDto> DetailList { get; set; }
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; }
    }
    /// <summary>
    /// 指标详情列表
    /// </summary>
    public class GetHealthIndicatorDetailResponseDto
    {
        /// <summary>
        /// 指标选项Id
        /// </summary>
        public string OptionGuid { get; set; }
        /// <summary>
        /// 指标选项名称
        /// </summary>
        public string OptionName { get; set; }
        /// <summary>
        /// 指标选项单位
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OptionUnit { get; set; }

        /// <summary>
        /// 指标项最小值
        /// </summary>
        public decimal? MinValue { get; set; }

        /// <summary>
        /// 指标项最大值
        /// </summary>
        public decimal? MaxValue { get; set; }
        /// <summary>
        /// 选项数据列表
        /// </summary>
        public List<KeyValuePair<DateTime, decimal?>> OptionList { get; set; }
    }

    /// <summary>
    /// 指标项数据
    /// </summary>
    public class ConsumerHealthIndicatorOption : BaseHealthIndicatorOption
    {
        /// <summary>
        /// 用户填写最近一次值
        /// </summary>
        public decimal? ResultVale { get; set; }
    }
}
