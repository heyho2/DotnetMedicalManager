using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Questionnaire
{
    /// <summary>
    /// 问卷问题表
    /// </summary>
    [Table("t_questionnaire_question")]
    public class QuestionnaireQuestionModel : BaseModel
    {
        /// <summary>
        /// 问卷题目主键
        /// </summary>
        [Column("question_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "问卷题目主键")]
        public string QuestionGuid { get; set; }

        /// <summary>
        /// 所属问卷guid
        /// </summary>
        [Column("questionnaire_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "所属问卷guid")]
        public string QuestionnaireGuid { get; set; }

        /// <summary>
        /// 题目名称
        /// </summary>
        [Column("question_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "题目名称")]
        public string QuestionName { get; set; }

        /// <summary>
        /// 问题分组guid
        /// </summary>
        [Column("dimension_guid"), Display(Name = "问题分组guid")]
        public string DimensionGuid { get; set; }

        /// <summary>
        /// 题目类型：单选、判断、数值、文本、多选
        /// </summary>
        [Column("question_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "题目类型：单选、判断、数值、文本、多选")]
        public string QuestionType { get; set; }

        /// <summary>
        /// 依赖答案guid
        /// </summary>
        [Column("depend_answer")]
        public string DependAnswer { get; set; }

        /// <summary>
        /// 依赖答案所属问题guid
        /// </summary>
        [Column("depend_question")]
        public string DependQuestion { get; set; }

        /// <summary>
        /// 是否依赖
        /// </summary>
        [Column("is_depend"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否依赖")]
        public bool IsDepend { get; set; } = false;

        /// <summary>
        /// 排序序号
        /// </summary>
        [Column("sort"), Required(ErrorMessage = "{0}必填"), Display(Name = "排序序号")]
        public int Sort { get; set; }

        /// <summary>
        /// 问题单位：仅数值问题有此项
        /// </summary>
        [Column("unit")]
        public string Unit { get; set; }

        /// <summary>
        /// 提示文字：仅问答题由此项
        /// </summary>
        [Column("prompt_text")]
        public string PromptText { get; set; }
    }
}



