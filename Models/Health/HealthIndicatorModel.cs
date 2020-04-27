using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Health
{
    /// <summary>
    /// 日常健康指标基础数据表
    /// </summary>
    [Table("t_health_indicator")]
    public class HealthIndicatorModel : BaseModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column("indicator_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "主键")]
        public string IndicatorGuid { get; set; }

        /// <summary>
        /// 指标名称
        /// </summary>
        [Column("indicator_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "指标名称")]
        public string IndicatorName { get; set; }

        /// <summary>
        /// 0：单个值，1：多个值，默认为0
        /// </summary>
        [Column("indicator_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "0：单个值，1：多个值，默认为0")]
        public bool IndicatorType { get; set; }

        /// <summary>
        /// 排序默认值0
        /// </summary>
        [Column("sort"), Required(ErrorMessage = "{0}必填"), Display(Name = "排序默认值0")]
        public int Sort { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        [Column("display"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否显示")]
        public bool Display { get; set; }
    }
}



