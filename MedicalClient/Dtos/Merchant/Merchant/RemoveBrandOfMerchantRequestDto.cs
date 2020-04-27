using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 移除商户品牌数据请求Dto
    /// </summary>
    public class RemoveBrandOfMerchantRequestDto : BaseDto
    {
        /// <summary>
        /// 商户guid
        /// </summary>
        [Required(ErrorMessage ="商户guid必填")]
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 品牌guid
        /// </summary>
        [Required(ErrorMessage = "品牌guid必填")]
        public List<string> BrandGuids { get; set; }
    }
}
