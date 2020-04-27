using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Doctor
{
    ///<summary>
    ///问答表模型
    ///</summary>
    [Table("t_doctor_qa")]
    public class QaModel : BaseModel
    {
        ///<summary>
        ///问答表GUID
        ///</summary>
        [Column("qa_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "问答表GUID")]
        public string QaGuid
        {
            get;
            set;
        }

        ///<summary>
        ///医生GUID
        ///</summary>
        [Column("author_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "医生GUID")]
        public string AuthorGuid
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

        ///<summary>
        ///回答
        ///</summary>
        [Column("answer"), Required(ErrorMessage = "{0}必填"), Display(Name = "回答")]
        public string Answer
        {
            get;
            set;
        }

        ///<summary>
        ///排序
        ///</summary>
        [Column("sort"), Required(ErrorMessage = "{0}必填"), Display(Name = "排序")]
        public int Sort
        {
            get;
            set;
        }
    }
}