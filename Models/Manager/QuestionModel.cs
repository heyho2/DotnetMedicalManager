using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Manager
{
    ///<summary>
    ///常见问题
    ///</summary>
    [Table("t_manager_question")]
    public class QuestionModel : BaseModel
    {

        ///<summary>
        ///问题GUID
        ///</summary>
        [Column("question_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "问题GUID")]
        public string QuestionGuid { get; set; }

        ///<summary>
        ///问题
        ///</summary>
        [Column("question"), Required(ErrorMessage = "{0}必填"), Display(Name = "问题")]
        public string Question { get; set; }

        ///<summary>
        ///答案
        ///</summary>
        [Column("answer")]

        public string Answer { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Column("sort")]
        public int Sort { get; set; }

    }
}



