
using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Doctor
{
    ///<summary>
    ///医生在线时长统计
    ///</summary>
    [Table("t_doctor_online_statistic")]
    public class OnlineStatisticModel : BaseModel
    {

        ///<summary>
        ///
        ///</summary>
        [Column("online_time_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "")]
        public string OnlineTimeGuid { get; set; }

        ///<summary>
        ///
        ///</summary>
        [Column("doctor_guid")]
        public string DoctorGuid { get; set; }

        ///<summary>
        ///在线时长（分钟）
        ///</summary>
        [Column("duration"), Required(ErrorMessage = "{0}必填"), Display(Name = "在线时长（分钟）")]
        public decimal Duration { get; set; }

        ///<summary>
        ///统计日期
        ///</summary>
        [Column("statistic_date"), Required(ErrorMessage = "{0}必填"), Display(Name = "统计日期")]
        public DateTime StatisticDate { get; set; }

    }
}



