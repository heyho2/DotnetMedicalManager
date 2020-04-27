using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Health
{
    /// <summary>
    /// 日常健康指标选项表
    /// </summary>
    [Table("t_health_indicator_option")]
    public class HealthIndicatorOptionModel : BaseModel
    {
        /// <summary>
        /// guid
        /// </summary>
        [Column("option_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "guid")]
        public string OptionGuid { get; set; }

        /// <summary>
        /// 健康指标guid
        /// </summary>
        [Column("indicator_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "健康指标guid")]
        public string IndicatorGuid { get; set; }

        /// <summary>
        /// 指标项单位
        /// </summary>
        [Column("option_unit"), Display(Name = "指标项单位")]
        public string OptionUnit { get; set; }

        /// <summary>
        /// 指标项名称
        /// </summary>
        [Column("option_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "指标项名称")]
        public string OptionName { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        [Column("required"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否必填")]
        public bool Required { get; set; }

        /// <summary>
        /// 指标项最小值
        /// </summary>
        [Column("min_value"), Display(Name = "指标项最小值")]
        public decimal? MinValue { get; set; }

        /// <summary>
        /// 指标项最大值
        /// </summary>
        [Column("max_value"), Display(Name = "指标项最大值")]
        public decimal? MaxValue { get; set; }

        /// <summary>
        /// 排序默认值0
        /// </summary>
        [Column("sort"), Required(ErrorMessage = "{0}必填"), Display(Name = "排序默认值0")]
        public int Sort { get; set; }
    }
}



