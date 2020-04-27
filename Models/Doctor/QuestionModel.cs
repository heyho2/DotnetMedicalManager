using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Doctor
{
    ///<summary>
    ///
    ///</summary>
    [Table("t_doctor_question")]
    public class QuestionModel : BaseModel
    {
        ///<summary>
        ///常问医生问题GUID
        ///</summary>
        [Column("question_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "常问医生问题GUID")]
        public string QuestionGuid
        {
            get;
            set;
        }

        ///<summary>
        ///医生GUID
        ///</summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "医生GUID")]
        public string UserGuid
        {
            get;
            set;
        }

        ///<summary>
        ///问题
        ///</summary>
        [Column("question"), Required(ErrorMessage = "{0}必填"), Display(Name = "问题")]
        public string Question
        {
            get;
            set;
        }
    }
}