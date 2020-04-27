using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor
{
    /// <summary>
    /// 获取医生注册资料配置项明细
    /// </summary>
    public class SaveDoctorCertificateDetailRequestDto : BaseDto
    {
        /// <summary>
        /// 证书guid
        /// </summary>
        public string CertificateGuid { get; set; }
        /// <summary>
        /// 类型id
        /// </summary>
        public string OwnerGuid { get; set; }
        
        /// <summary>
        /// 证书项guid
        /// </summary>
        public string DicGuid { get; set; }

        /// <summary>
        /// 证书项名称
        /// </summary>
        public string ConfigName { get; set; }

        /// <summary>
        /// 证书附件地址
        /// </summary>
        public string CertificateUrl { get; set; }

        /// <summary>
        /// 附件guid
        /// </summary>
        public string AccessoryGuid { get; set; }
    }
}
