
using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Doctor
{
    ///<summary>
    ///医生回答用户咨询问题统计
    ///</summary>
    [Table("t_doctor_consult_statistic")]
    public class ConsultStatisticModel : BaseModel
    {

        ///<summary>
        ///
        ///</summary>
        [Column("doctor_consult_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "")]
        public string DoctorConsultGuid { get; set; }

        ///<summary>
        ///医生GUID
        ///</summary>
        [Column("doctor_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "医生GUID")]
        public string DoctorGuid { get; set; }

        ///<summary>
        ///咨询次数
        ///</summary>
        [Column("times"), Required(ErrorMessage = "{0}必填"), Display(Name = "咨询次数")]
        public int Times { get; set; }

        ///<summary>
        ///统计日期
        ///</summary>
        [Column("statistic_date"), Required(ErrorMessage = "{0}必填"), Display(Name = "统计日期")]
        public DateTime StatisticDate { get; set; }

    }
}



