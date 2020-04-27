using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 获取商户基础信息响应Dto
    /// </summary>
    public class GetMerchantBasicInfoResponseDto : BaseDto
    {
        /// <summary>
        /// 商户名称
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 商户图片url
        /// </summary>
        public string MerchantPictureUrl { get; set; }

        /// <summary>
        /// 商户电话
        /// </summary>
        public string Telephone { get; set; }
    }
}
