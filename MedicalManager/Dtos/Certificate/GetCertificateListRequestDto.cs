using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Certificate
{
    /// <summary>
    /// 获取证书列表
    /// </summary>
    public class GetCertificateListRequestDto : BaseDto
    {
        /// <summary>
        /// 证书持有人GUID
        /// </summary>
        public string OwnerGuid { get; set; }
        /// <summary>
        /// 证书类型
        /// </summary>
        [EnumDataType(typeof(CertificateType))]
        public CertificateType Type { get; set; }
        /// <summary>
        /// 证书类型
        /// </summary>
        public enum CertificateType
        {
            /// <summary>
            /// 医生
            /// </summary>
            Doctors,
            /// <summary>
            /// 
            /// </summary>
            Merchant
        }
    }
    /// <summary>
    /// 获取证书列表
    /// </summary>
    public class GetCertificateListItemDto : BaseDto
    {
        /// <summary>
        /// 证书持有人GUID
        /// </summary>
        public string OwnerGuid { get; set; }
        /// <summary>
        /// id
        /// </summary>
        public string CertificateGuid { get; set; }
        /// <summary>
        /// 证书名字
        /// </summary>
        public string CertificateName { get; set; }
        /// <summary>
        /// 证书图片地址
        /// </summary>
        public string PictureUrl { get; set; }
    }
}
