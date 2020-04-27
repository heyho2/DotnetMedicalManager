using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.DtoIn
{
    /// <summary>
    /// 商户信息输入Dto
    /// </summary>
    public class MerchantDto : BaseDto
    {
        /// <summary>
        /// 真实姓名
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "真实姓名")]
        public string UserName { get; set; }

        /// <summary>
        /// 商户电话
        /// </summary>
        public string Telephone { get; set; }

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
        /// 商户名称
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "商户名称")]
        public string MerchantName { get; set; }

        /// <summary>
        /// 签名附件guid
        /// </summary>
        public string SignatureGuid { get; set; }

        /// <summary>
        /// 字典配置项，例如证书
        /// </summary>
        public List<CertificateDto> Certificates { get; set; }

        /// <summary>
        /// 经营范围
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "经营范围")]
        public List<ScopeDto> Scopes { get; set; }
    }
}
