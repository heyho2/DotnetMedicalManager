using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Doctor
{
    ///<summary>
    ///每天用户咨询统计
    ///</summary>
    [Table("t_doctor_historical_consult_statistic")]
    public class HistoricalConsultStatisticModel : BaseModel
    {
        ///<summary>
        ///
        ///</summary>
        [Column("consult_statistic_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "")]
        public string ConsultStatisticGuid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("hospital_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "")]
        public string HospitalGuid { get; set; }

         ///<summary>
        ///每天有效用户咨询人数
        ///</summary>
        [Column("number"), Required(ErrorMessage = "{0}必填"), Display(Name = "每天有效用户咨询人数")]
        public int Number { get; set; }

        ///<summary>
        ///统计日期
        ///</summary>
        [Column("statistic_date"), Required(ErrorMessage = "{0}必填"), Display(Name = "统计日期")]
        public DateTime StatisticDate { get; set; }
    }
}



