using GD.Common.Base;
using GD.Dtos.Certificate;
using System;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Doctor
{
    /// <summary>
    /// 修改医生数据
    /// </summary>
    public class UpdateDoctorInfoRequestDto : BaseDto
    {

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "真实姓名")]
        public string UserName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "性别")]
        public string Gender { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "Birthday")]
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "身份证号")]
        public string IdentityNumber { get; set; }

        /// <summary>
        /// 所属医院
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "所属医院")]
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 工作城市 省-市-区
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "工作城市")]
        public string[] WorkCity { get; set; }

        /// <summary>
        /// 执业医院
        /// </summary>
        public string PractisingHospital { get; set; }

        /// <summary>
        /// 所在科室Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "所在科室")]
        public string OfficeGuid { get; set; }

        /// <summary>
        /// 职称Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "职称")]
        public string TitleGuid { get; set; }

        /// <summary>
        /// 所获荣誉
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "所获荣誉")]
        public string Honor { get; set; }

        /// <summary>
        /// 背景
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "背景")]
        public string Background { get; set; }

        /// <summary>
        /// 擅长
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "擅长")]
        public string[] AdeptTags { get; set; }
        /// <summary>
        /// 医生id
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "医生id")]
        public string DoctorGuid { get; set; }

        /// <summary>
        /// 签名附件guid
        /// </summary>
        public string SignatureGuid { get; set; }

        /// <summary>
        /// 一寸照附件Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "一寸照附件")]
        public string PortraitGuid { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "手机号码")]
        public string Phone { get; set; }
        /// <summary>
        /// 推荐
        /// </summary>
        public bool IsRecommend { get; set; }
        /// <summary>
        /// 推荐医生排序
        /// </summary>
        public int RecommendSort { get; set; }
        /// <summary>
        /// 工作年龄
        /// </summary>
        public int WorkAge { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string JobNumber { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 字典配置项，例如证书
        /// </summary>
        [MinLength(1, ErrorMessage = "{0}必填"), Display(Name = "证书")]
        public CertificateItemDto[] Certificates { get; set; }
    }
    /// <summary>
    /// 添加医生数据
    /// </summary>
    public class AddDoctorInfoRequestDto : BaseDto
    {
        /// <summary>
        /// 真实姓名
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "真实姓名")]
        public string UserName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "性别")]
        public string Gender { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "Birthday")]
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "身份证号")]
        public string IdentityNumber { get; set; }

        /// <summary>
        /// 所属医院
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "所属医院")]
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 工作城市 省-市-区
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "工作城市")]
        public string[] WorkCity { get; set; }

        /// <summary>
        /// 执业医院
        /// </summary>
        public string PractisingHospital { get; set; }

        /// <summary>
        /// 所在科室Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "所在科室")]
        public string OfficeGuid { get; set; }

        /// <summary>
        /// 职称Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "职称")]
        public string TitleGuid { get; set; }

        /// <summary>
        /// 所获荣誉
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "所获荣誉")]
        public string Honor { get; set; }

        /// <summary>
        /// 背景
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "背景")]
        public string Background { get; set; }

        /// <summary>
        /// 擅长
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "擅长")]
        public string[] AdeptTags { get; set; }

        /// <summary>
        /// 签名附件guid
        /// </summary>
        public string SignatureGuid { get; set; }

        /// <summary>
        /// 字典配置项，例如证书
        /// </summary>

        /// <summary>
        /// 一寸照附件Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "一寸照附件")]
        public string PortraitGuid { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [Phone]
        [Required(ErrorMessage = "{0}必填"), Display(Name = "手机号码")]
        public string Phone { get; set; }
        /// <summary>
        /// 推荐
        /// </summary>
        public bool IsRecommend { get; set; }
        /// <summary>
        /// 推荐医生排序
        /// </summary>
        public int RecommendSort { get; set; }
        /// <summary>
        /// 工作年龄
        /// </summary>
        public int WorkAge { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string JobNumber { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 字典配置项，例如证书
        /// </summary>
        [MinLength(1, ErrorMessage = "{0}必填"), Display(Name = "证书")]
        public CertificateItemDto[] Certificates { get; set; }
    }
}
