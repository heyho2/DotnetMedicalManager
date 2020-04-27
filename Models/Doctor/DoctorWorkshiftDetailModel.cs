
using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Doctor
{
    /// <summary>
    /// 医生班次模板详情
    /// </summary>
    [Table("t_doctor_workshift_detail")]
    public class DoctorWorkshiftDetailModel : BaseModel
    {
        /// <summary>
        /// 班次明细guid
        /// </summary>
        [Column("workshift_detail_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "班次明细guid")]
        public string WorkshiftDetailGuid { get; set; }

        /// <summary>
        /// 班次模板guid
        /// </summary>
        [Column("template_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "班次模板guid")]
        public string TemplateGuid { get; set; }

        /// <summary>
        /// 班次类别
        /// </summary>
        [Column("workshift_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "班次类别")]
        public string WorkshiftType { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Column("start_time"), Required(ErrorMessage = "{0}必填"), Display(Name = "开始时间")]
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Column("end_time"), Required(ErrorMessage = "{0}必填"), Display(Name = "结束时间")]
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// 可预约数量限制
        /// </summary>
        [Column("appointment_limit"), Required(ErrorMessage = "{0}必填"), Display(Name = "可预约数量限制")]
        public int AppointmentLimit { get; set; }

        /// <summary>
        /// 预约编号前缀
        /// </summary>
        [Column("appointment_no_prefix"), Required(ErrorMessage = "{0}必填"), Display(Name = "预约编号前缀")]
        public string AppointmentNoPrefix { get; set; }
    }
}



