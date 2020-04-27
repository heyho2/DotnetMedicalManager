using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Health
{
    /// <summary>
    /// 健康基础信息选项表
    /// </summary>
    [Table("t_health_information_option")]
    public class HealthInformationOptionModel : BaseModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column("option_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "主键")]
        public string OptionGuid { get; set; }

        /// <summary>
        /// 健康基础信息表guid
        /// </summary>
        [Column("information_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "健康基础信息表guid")]
        public string InformationGuid { get; set; }

        /// <summary>
        /// 选项名称
        /// </summary>
        [Column("option_label"), Required(ErrorMessage = "{0}必填"), Display(Name = "选项名称")]
        public string OptionLabel { get; set; }

        /// <summary>
        /// 是否默认选项
        /// </summary>
        [Column("is_default"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否默认选项")]
        public bool IsDefault { get; set; }

        /// <summary>
        /// 排序默认值0
        /// </summary>
        [Column("sort"), Required(ErrorMessage = "{0}必填"), Display(Name = "排序默认值0")]
        public int Sort { get; set; }
    }
}



