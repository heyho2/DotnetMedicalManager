using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Questionnaire
{
    /// <summary>
    /// 问卷表
    /// </summary>
    [Table("t_questionnaire")]
    public class QuestionnaireModel : BaseModel
    {
        /// <summary>
        /// 问卷主键
        /// </summary>
        [Column("questionnaire_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "问卷主键")]
        public string QuestionnaireGuid { get; set; }

        /// <summary>
        /// 问卷名称
        /// </summary>
        [Column("questionnaire_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "问卷名称")]
        public string QuestionnaireName { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        [Column("subhead")]
        public string Subhead { get; set; }

        /// <summary>
        /// 是否已发放
        /// </summary>
        [Column("issuing_status"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否已发放")]
        public bool IssuingStatus { get; set; } = false;

        /// <summary>
        /// 是否已发放
        /// </summary>
        [Column("issuing_date"), Display(Name = "是否已发放")]
        public DateTime? IssuingDate { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        [Column("display"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否显示")]
        public bool Display { get; set; } = true;

        /// <summary>
        /// 问卷下是否存在依赖问题
        /// </summary>
        [Column("has_depend")]
        public bool HasDepend { get; set; }
    }
}



