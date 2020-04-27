using GD.Common.Base;
using GD.Dtos.Doctor.Doctor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Doctor.Hospital
{
    /// <summary>
    /// 新增预约
    /// </summary>
    public class CreateAppointmentRequestDto : BaseDto
    {
        /// <summary>
        /// 医生guid
        /// </summary>
        [Required(ErrorMessage = "医生guid必填")]
        public string DoctorGuid { get; set; }

        /// <summary>
        /// 医生排班guid
        /// </summary>
        [Required(ErrorMessage = "医生排班guid必填")]
        public string ScheduleGuid { get; set; }

        /// <summary>
        /// 挂号手机号
        /// </summary>
        [Required(ErrorMessage = "挂号手机号必填")]
        public string Phone { get; set; }

        /// <summary>
        /// 患者姓名
        /// </summary>
        [Required(ErrorMessage = "患者姓名必填")]
        public string PatientName { get; set; }

        /// <summary>
        /// 患者出生日期
        /// </summary>
        public DateTime? PatientBirthday { get; set; }

        /// <summary>
        /// 患者性别
        /// </summary>
        [Required(ErrorMessage = "患者性别必填")]
        public GenderEnum PatientGender { get; set; }

        /// <summary>
        /// 患者证件号
        /// </summary>
        public string PatientCardNo { get; set; }
    }
}
