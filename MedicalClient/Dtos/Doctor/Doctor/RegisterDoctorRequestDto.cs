using GD.Common.Base;
using GD.Dtos.DtoIn;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 注册医生请求Dto
    /// </summary>
    public class RegisterDoctorRequestDto : BaseDto
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
        public string City { get; set; }

        /// <summary>
        /// 执业医院
        /// </summary>
        public string PractisingHospital { get; set; }

        /// <summary>
        /// 所在科室Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "所在科室")]
        public string DocOffice { get; set; }

        /// <summary>
        /// 职称Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "职称")]
        public string DocTitle { get; set; }

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
        public string[] Adepts { get; set; }


        /// <summary>
        /// 签名附件guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "签名附件guid")]
        public string SignatureGuid { get; set; }

        /// <summary>
        /// 字典配置项，例如证书
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "证书配置项必填")]
        public List<CertificateDto> Certificates { get; set; }

        /// <summary>
        /// 一寸照附件Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "一寸照附件")]
        public string PortraitGuid { get; set; }

        /// <summary>
		/// 数据校验
		/// </summary>
		/// <param name="validationContext"></param>
		/// <returns></returns>
		/// <remarks>
		/// 子类可重载此方法，执行自定义的有效性校验
		/// </remarks>
		protected override IEnumerable<ValidationResult> Verify(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();
            if (Certificates==null || !Certificates.Any())
            {
                result.Add(new ValidationResult("请上传证书"));
            }
            return result;
        }
    }
}
