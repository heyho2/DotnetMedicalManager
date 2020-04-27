using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Questionnaire
{
    /// <summary>
    /// 问卷问题答案选项表
    /// </summary>
    [Table("t_questionnaire_answer")]
    public class QuestionnaireAnswerModel : BaseModel
    {
        /// <summary>
        /// 问卷答案主键
        /// </summary>
        [Column("answer_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "问卷答案主键")]
        public string AnswerGuid { get; set; }

        /// <summary>
        /// 所属问卷guid
        /// </summary>
        [Column("questionnaire_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "所属问卷guid")]
        public string QuestionnaireGuid { get; set; }

        /// <summary>
        /// 所属问卷问题guid
        /// </summary>
        [Column("question_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "所属问卷问题guid")]
        public string QuestionGuid { get; set; }

        /// <summary>
        /// 答案名称
        /// </summary>
        [Column("answer_label"), Required(ErrorMessage = "{0}必填"), Display(Name = "答案名称")]
        public string AnswerLabel { get; set; }

        /// <summary>
        /// 是否是默认选项
        /// </summary>
        [Column("is_default"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否是默认选项")]
        public bool IsDefault { get; set; }

        /// <summary>
        /// 答案顺序序号
        /// </summary>
        [Column("sort"), Required(ErrorMessage = "{0}必填"), Display(Name = "答案顺序序号")]
        public int Sort { get; set; }
    }
}



