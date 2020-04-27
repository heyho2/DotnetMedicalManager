using GD.Dtos.Doctor.Doctor;
using GD.Dtos.Enum;
using GD.Dtos.Enum.DoctorAppointment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 就诊档案列表查询Dto
    /// </summary>
    public class GetPatientRequestDto
    {
    }
    /// <summary>
    /// 添加就诊档案数据
    /// </summary>
    public class GetAddPatientRequestDto
    {
        /// <summary>
        /// 关系
        /// </summary>
        public InquiryRelationshipEnum? Relationship { get; set; }
        /// <summary>
        /// 就诊人姓名
        /// </summary>
        [Required(ErrorMessage = "姓名为必填值")]
        public string Name { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required(ErrorMessage = "手机号码必填")]
        public string Phone { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [Required(ErrorMessage = "性别必填")]
        public GenderEnum Gender { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 是否为默认就诊人
        /// </summary>
        public bool IsDefault { get; set; }
    }
    /// <summary>
    /// 就诊人查询Dto
    /// </summary>
    public class GetPatientResponDto
    {
        /// <summary>
        /// 关系
        /// </summary>
        public string Relationship { get; set; }
        /// <summary>
        /// id
        /// </summary>
        public string PatientGuid { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int? Age { get; set; }
        /// <summary>
        /// 就诊人姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public GenderEnum Gender { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 是否为默认就诊人
        /// </summary>
        public bool IsDefault { get; set; }
    }
    /// <summary>
    /// 修改Dto
    /// </summary>
    public class UpdatePatientRequestDto
    {
        /// <summary>
        /// id
        /// </summary>
        [Required(ErrorMessage = "ID为必填值")]
        public string PatientGuid { get; set; }
        /// <summary>
        /// 关系
        /// </summary>
        public InquiryRelationshipEnum? Relationship { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 就诊人姓名
        /// </summary>
        [Required(ErrorMessage = "姓名为必填值")]
        public string Name { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required(ErrorMessage = "手机号码必填")]
        public string Phone { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [Required(ErrorMessage = "性别必填")]
        public GenderEnum Gender { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 是否为默认就诊人
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
