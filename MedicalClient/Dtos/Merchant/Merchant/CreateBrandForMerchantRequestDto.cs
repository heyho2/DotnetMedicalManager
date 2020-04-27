using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 商户创建品牌请求Dto
    /// </summary>
    public class CreateBrandForMerchantRequestDto:BaseDto
    {
        /// <summary>
        /// 商户guid
        /// </summary>
        [Required(ErrorMessage = "商户guid必填")]
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 品牌名称
        /// </summary>
        [Required(ErrorMessage = "品牌名称必填")]
        public string BrandName { get; set; }

        /// <summary>
        /// 图片guid
        /// </summary>
        public string PictureGuid { get; set; }
    }
}
