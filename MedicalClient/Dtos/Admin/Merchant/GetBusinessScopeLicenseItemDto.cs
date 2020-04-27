using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Admin.Merchant
{
    /// <summary>
    /// 经营范围证件
    /// </summary>
    public class GetBusinessScopeLicenseItemDto : BaseDto
    {
        /// <summary>
        /// 经营范围名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 图片id
        /// </summary>
        public string PictureGuid { get; set; }
        /// <summary>
        /// 商户id
        /// </summary>
        public string MerchantGuid { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string PictureUrl { get; set; }
        
    }
}
