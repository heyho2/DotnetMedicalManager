
using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Doctor
{
    ///<summary>
    ///医生解答问题统计
    ///</summary>
    [Table("t_doctor_answer_question_statistic")]
    public class AnswerQuestionStatisticModel : BaseModel
    {

        ///<summary>
        ///
        ///</summary>
        [Column("answer_question_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "")]
        public string AnswerQuestionGuid { get; set; }

        ///<summary>
        ///医生GUID
        ///</summary>
        [Column("doctor_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "医生GUID")]
        public string DoctorGuid { get; set; }

        ///<summary>
        ///解答问题个数
        ///</summary>
        [Column("times")]
        public int Times { get; set; }

        ///<summary>
        ///统计日期
        ///</summary>
        [Column("statistic_date")]
        public DateTime StatisticDate { get; set; }
    }
}



