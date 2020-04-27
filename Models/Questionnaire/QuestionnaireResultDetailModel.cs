using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Questionnaire
{
    /// <summary>
    /// 用户填写问卷详情
    /// </summary>
    [Table("t_questionnaire_result_detail")]
    public class QuestionnaireResultDetailModel : BaseModel
    {
        /// <summary>
        /// 用户填写问卷详情主键
        /// </summary>
        [Column("detail_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "用户填写问卷详情主键")]
        public string DetailGuid { get; set; }

        /// <summary>
        /// 用户填写问卷结果guid
        /// </summary>
        [Column("result_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户填写问卷结果guid")]
        public string ResultGuid { get; set; }

        /// <summary>
        /// 问卷问题guid
        /// </summary>
        [Column("question_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "问卷问题guid")]
        public string QuestionGuid { get; set; }

        /// <summary>
        /// 问题答案项guid json数组  ["1","2","3"]
        /// </summary>
        [Column("answer_guids")]
        public string AnswerGuids { get; set; }

        /// <summary>
        /// 回答结果
        /// </summary>
        [Column("result")]
        public string Result { get; set; }

        /// <summary>
        /// 答题顺序
        /// </summary>
        [Column("sort"), Required(ErrorMessage = "{0}必填"), Display(Name = "答题顺序")]
        public int Sort { get; set; }
    }
}



