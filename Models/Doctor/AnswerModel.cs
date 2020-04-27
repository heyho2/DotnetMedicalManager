using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Doctor
{
    ///<summary>
    ///问题回答表模型
    ///</summary>
    [Table("t_doctor_answer")]
    public class AnswerModel : BaseModel
    {
        ///<summary>
        ///常问医生回答GUID
        ///</summary>
        [Column("answer_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "常问医生回答GUID")]
        public string AnswerGuid
        {
            get;
            set;
        }

        ///<summary>
        ///问题GUID
        ///</summary>
        [Column("question_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "问题GUID")]
        public string QuestionGuid
        {
            get;
            set;
        }

        ///<summary>
        ///
        ///</summary>
        [Column("answer"), Required(ErrorMessage = "{0}必填"), Display(Name = "")]
        public string Answer
        {
            get;
            set;
        }
    }
}