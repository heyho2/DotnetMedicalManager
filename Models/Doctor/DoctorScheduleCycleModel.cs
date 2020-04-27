
using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Doctor
{
    /// <summary>
    /// 医院排班周期表
    /// </summary>
    [Table("t_doctor_schedule_cycle")]
    public class DoctorScheduleCycleModel : BaseModel
    {
        /// <summary>
        /// 医院排班周期
        /// </summary>
        [Column("cycle_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "医院排班周期")]
        public string CycleGuid { get; set; }

        /// <summary>
        /// 医院guid
        /// </summary>
        [Column("hospital_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "医院guid")]
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 起始日期
        /// </summary>
        [Column("start_date"), Required(ErrorMessage = "{0}必填"), Display(Name = "起始日期")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        [Column("end_date"), Required(ErrorMessage = "{0}必填"), Display(Name = "结束日期")]
        public DateTime EndDate { get; set; }

        
    }
}



