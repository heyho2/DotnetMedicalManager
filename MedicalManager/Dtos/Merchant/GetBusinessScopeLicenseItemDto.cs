using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Merchant
{
    /// <summary>
    /// 经营范围证件
    /// </summary>
    public class GetBusinessScopeLicenseItemDto : BaseDto
    {
        /// <summary>
        /// 经营范围名称
        /// </summary>
        public string ConfigName { get; set; }
        /// <summary>
        /// 图片id
        /// </summary>
        public string AccessoryGuid { get; set; }
        /// <summary>
        /// 经营范围字典
        /// </summary>
        public string ScopeDicGuid { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string CertificateUrl { get; set; }

        /// <summary>
        /// guid
        /// </summary>
        public string ScopeGuid { get; set; }
    }
}
