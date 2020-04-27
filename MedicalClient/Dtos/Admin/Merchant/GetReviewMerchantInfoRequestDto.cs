using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Admin.Merchant
{
    /// <summary>
    /// 商家详细
    /// </summary>
    public class GetReviewMerchantInfoRequestDto : BaseDto
    {
        /// <summary>
        /// id
        /// </summary>
        public string MerchantGuid { get; set; }
    }
    /// <summary>
    /// 商家详细
    /// </summary>
    public class GetReviewMerchantInfoResponseDto : BaseDto
    {
    }
}
