using GD.Common.Base;
using GD.Dtos.Certificate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Merchant
{
    /// <summary>
    /// 注册商户
    /// </summary>
    public class RegisterMerchantRequestDto : BaseDto
    {
        /// <summary>
        /// 商户电话
        /// </summary>
        public string Telephone { get; set; }
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
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 商户图片
        /// </summary>
        public string MerchantPicture { get; set; }

        /// <summary>
        /// 字典配置项，例如证书
        /// </summary>
        [MinLength(1, ErrorMessage = "{0}必填"), Display(Name = "证书")]
        public CertificateItemDto[] Certificates { get; set; }

        /// <summary>
        /// 经营范围
        /// </summary>
        [MinLength(1, ErrorMessage = "{0}必填"), Display(Name = "经营范围")]
        public ScopeItemDto[] Scopes { get; set; }
        
        /// <summary>
        /// 经度
        /// </summary>
        public decimal Longitude { get; set; }
        /// <summary>
        /// 维度
        /// </summary>
        public decimal Latitude { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        ///<summary>
        ///市
        ///</summary>
        public string City { get; set; }
        ///<summary>
        ///省
        ///</summary>
        public string Province { get; set; }
        ///<summary>
        ///区
        ///</summary>
        public string Area { get; set; }
        ///<summary>
        ///街道
        ///</summary>
        public string Street { get; set; }
        ///<summary>
        ///医院GUID
        ///</summary>
        public string HospitalGuid { get; set; }
    }
    /// <summary>
    /// 修改商户
    /// </summary>
    public class UpdateMerchantRequestDto : BaseDto
    {
        /// <summary>
        /// 商户id
        /// </summary>
        [Required]
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 商户电话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "商户名称")]
        public string MerchantName { get; set; }
        /// <summary>
        /// 商户图片
        /// </summary>
        public string MerchantPicture { get; set; }


        /// <summary>
        /// 字典配置项，例如证书
        /// </summary>
        public CertificateItemDto[] Certificates { get; set; }

        /// <summary>
        /// 经营范围
        /// </summary>
        public ScopeItemDto[] Scopes { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal Longitude { get; set; }
        /// <summary>
        /// 维度
        /// </summary>
        public decimal Latitude { get; set; }

        ///<summary>
        ///市
        ///</summary>
        public string City { get; set; }
        ///<summary>
        ///省
        ///</summary>
        public string Province { get; set; }
        ///<summary>
        ///区
        ///</summary>
        public string Area { get; set; }
        ///<summary>
        ///街道
        ///</summary>
        public string Street { get; set; }
        ///<summary>
        ///医院GUID
        ///</summary>
        public string HospitalGuid { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
    }
    /// <summary>
    /// 经营范围传入Dto
    /// </summary>
    public class ScopeItemDto : BaseDto
    {
        /// <summary>
        /// 范围字典Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "范围字典Guid")]
        public string ScopeDicGuid { get; set; }

        /// <summary>
        /// 经营范围图片guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "经营范围图片guid")]
        public string AccessoryGuid { get; set; }
    }
    
}
