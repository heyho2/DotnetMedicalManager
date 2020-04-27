using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Consumer
{
    /// <summary>
    /// 挂号(就诊)记录表
    /// </summary>
    [Table("t_consumer_doctor_appointment")]
    public class DoctorAppointmentModel : BaseModel
    {
        /// <summary>
        /// 预约guid
        /// </summary>
        [Column("appointment_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "预约guid")]
        public string AppointmentGuid { get; set; }

        /// <summary>
        /// 医院guid
        /// </summary>
        [Column("hospital_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "医院guid")]
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 用户guid
        /// </summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户guid")]
        public string UserGuid { get; set; }

        /// <summary>
        /// 挂号编号
        /// </summary>
        [Column("appointment_no"), Required(ErrorMessage = "{0}必填"), Display(Name = "挂号编号")]
        public string AppointmentNo { get; set; }

        /// <summary>
        /// 挂号预约排班guid
        /// </summary>
        [Column("schedule_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "挂号预约排班guid")]
        public string ScheduleGuid { get; set; }

        /// <summary>
        /// 医生guid
        /// </summary>
        [Column("doctor_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "医生guid")]
        public string DoctorGuid { get; set; }

        /// <summary>
        /// 预约科室
        /// </summary>
        [Column("office_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "预约科室")]
        public string OfficeGuid { get; set; }

        /// <summary>
        /// 预约就诊时间
        /// </summary>
        [Column("appointment_time"), Required(ErrorMessage = "{0}必填"), Display(Name = "预约就诊时间")]
        public DateTime AppointmentTime { get; set; }

        /// <summary>
        /// 预约过号时间
        /// </summary>
        [Column("appointment_deadline"), Required(ErrorMessage = "{0}必填"), Display(Name = "预约过号时间")]
        public DateTime AppointmentDeadline { get; set; }



        /// <summary>
        /// 就诊人guid
        /// </summary>
        [Column("patient_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "就诊人guid")]
        public string PatientGuid { get; set; }

        /// <summary>
        /// 就诊人姓名
        /// </summary>
        [Column("patient_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "就诊人姓名")]
        public string PatientName { get; set; }

        /// <summary>
        /// 就诊人电话
        /// </summary>
        [Column("patient_phone"), Required(ErrorMessage = "{0}必填"), Display(Name = "就诊人电话")]
        public string PatientPhone { get; set; }

        /// <summary>
        /// 就诊人性别（M/F），默认为M
        /// </summary>
        [Column("patient_gender")]
        public string PatientGender { get; set; }

        /// <summary>
        /// 就诊人出生年月
        /// </summary>
        [Column("patient_birthday")]
        public DateTime? PatientBirthday { get; set; }

        /// <summary>
        /// 就诊人证件号
        /// </summary>
        [Column("patient_cardno")]
        public string PatientCardno { get; set; }

        /// <summary>
        /// 问诊人与当前用户关系
        /// Own:自己，Relatives:亲属，Friend:朋友,Other:其它
        /// </summary>
        [Column("patient_relationship")]
        public string PatientRelationship { get; set; }

        /// <summary>
        /// 状态Waiting:待就诊，Miss:爽约，Cancel：取消就诊，Treated：已就诊
        /// </summary>
        [Column("status"), Required(ErrorMessage = "{0}必填"), Display(Name = "状态Waiting:待就诊，Miss:爽约，Cancel：取消就诊，Treated：已就诊")]
        public string Status { get; set; }
    }
}



