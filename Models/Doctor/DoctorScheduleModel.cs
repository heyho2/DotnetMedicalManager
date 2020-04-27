
using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Doctor
{
    /// <summary>
    /// 医生排班表
    /// </summary>
    [Table("t_doctor_schedule")]
    public class DoctorScheduleModel : BaseModel
    {
        /// <summary>
        /// 医生排班guid
        /// </summary>
        [Column("schedule_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "医生排班guid")]
        public string ScheduleGuid { get; set; }

        /// <summary>
        /// 医院guid
        /// </summary>
        [Column("hospital_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "医院guid")]
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 班次模板guid
        /// </summary>
        [Column("template_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "班次模板guid")]
        public string TemplateGuid { get; set; }

        /// <summary>
        /// 排班日期
        /// </summary>
        [Column("schedule_date"), Required(ErrorMessage = "{0}必填"), Display(Name = "排班日期")]
        public DateTime ScheduleDate { get; set; }

        /// <summary>
        /// 排班周期guid
        /// </summary>
        [Column("cycle_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "排班周期guid")]
        public string CycleGuid { get; set; }

        /// <summary>
        /// 班次明细guid
        /// </summary>
        [Column("workshift_detail_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "班次明细guid")]
        public string WorkshiftDetailGuid { get; set; }

        /// <summary>
        /// 医生guid
        /// </summary>
        [Column("doctor_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "医生guid")]
        public string DoctorGuid { get; set; }

        /// <summary>
        /// 预约数量限制
        /// </summary>
        [Column("appointment_limit"), Required(ErrorMessage = "{0}必填"), Display(Name = "预约数量限制")]
        public int AppointmentLimit { get; set; }

        /// <summary>
        /// 已挂号数量
        /// </summary>
        [Column("appointment_quantity"), Required(ErrorMessage = "{0}必填"), Display(Name = "已挂号数量")]
        public int AppointmentQuantity { get; set; }
    }
}



