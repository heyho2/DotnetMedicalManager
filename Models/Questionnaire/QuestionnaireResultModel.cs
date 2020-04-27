using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Questionnaire
{
    /// <summary>
    /// 用户填写问卷结果
    /// </summary>
    [Table("t_questionnaire_result")]
    public class QuestionnaireResultModel : BaseModel
    {
        /// <summary>
        /// 用户回答问卷结果
        /// </summary>
        [Column("result_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "用户回答问卷结果")]
        public string ResultGuid { get; set; }

        /// <summary>
        /// 问卷guid
        /// </summary>
        [Column("questionnaire_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "问卷guid")]
        public string QuestionnaireGuid { get; set; }

        /// <summary>
        /// 用户guid
        /// </summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户guid")]
        public string UserGuid { get; set; }

        /// <summary>
        /// 填写状态:是否已提交
        /// </summary>
        [Column("fill_status"), Required(ErrorMessage = "{0}必填"), Display(Name = "填写状态")]
        public bool FillStatus { get; set; }

        /// <summary>
        /// 是否已评论
        /// </summary>
        [Column("commented"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否已评论")]
        public bool Commented { get; set; }

        /// <summary>
        /// 评论建议
        /// </summary>
        [Column("comment")]
        public string Comment { get; set; }
    }
}



