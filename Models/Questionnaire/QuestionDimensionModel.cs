using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Questionnaire
{
    /// <summary>
    /// 问卷问题维度
    /// </summary>
    [Table("t_questionnaire_question_dimension")]
    public class QuestionnaireQuestionDimensionModel : BaseModel
    {
        /// <summary>
        /// 问卷问题分组主键
        /// </summary>
        [Column("dimension_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "问卷问题分组主键")]
        public string DimensionGuid { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        [Column("dimension_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "分组名称")]
        public string DimensionName { get; set; }

        /// <summary>
        /// 问卷guid
        /// </summary>
        [Column("questionnaire_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "问卷guid")]
        public string QuestionnaireGuid { get; set; }
    }
}



