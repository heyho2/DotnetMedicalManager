using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 获取商户经营范围详情响应Dto
    /// </summary>
    public class GetMerchantScopeDetailResponseDto : BaseDto
    {
        /// <summary>
        /// 经营范围配置项guid
        /// </summary>
        public string DicGuid { get; set; }

        /// <summary>
        /// 经营范围配置项名称
        /// </summary>
        public string ConfigName { get; set; }

        /// <summary>
        /// 经营范围图片附件guid
        /// </summary>
        public string PictureGuid { get; set; }

        /// <summary>
        /// 经营范围图片附件url
        /// </summary>
        public string PictureUrl { get; set; }
    }
}
