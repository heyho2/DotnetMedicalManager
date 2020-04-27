using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Health
{
    /// <summary>
    /// 健康基础信息表
    /// </summary>
    [Table("t_health_information")]
    public class HealthInformationModel : BaseModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column("information_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "主键")]
        public string InformationGuid { get; set; }

        /// <summary>
        /// 数值指标:numericalvalue,单选指标: singleelection,多选指标:multipleselection,问答指标:questionsandanswers
        /// </summary>
        [Column("information_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "题目类型：单选、判断、数值、文本、多选")]
        public string InformationType { get; set; }

        /// <summary>
        /// 问题名称
        /// </summary>
        [Column("subject_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "问题名称")]
        public string SubjectName { get; set; }

        /// <summary>
        /// 问题单位名称
        /// </summary>
        [Column("subject_unit"), Display(Name = "问题单位名称")]
        public string SubjectUnit { get; set; }

        /// <summary>
        /// 问题提示语
        /// </summary>
        [Column("subject_prompt_text")]
        public string SubjectPromptText { get; set; }
        /// <summary>
        /// 是否单行文本
        /// </summary>
        [Column("is_single_line"), Required(ErrorMessage = "{0}必填"), Display(Name = "文本类型")]
        public bool IsSingleLine { get; set; } = false;

        /// <summary>
        /// 排序
        /// </summary>
        [Column("sort"), Required(ErrorMessage = "{0}必填"), Display(Name = "排序")]
        public int Sort { get; set; }
    }
}



