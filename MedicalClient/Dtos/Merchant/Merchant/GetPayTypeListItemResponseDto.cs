using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 支付方式枚举值列表
    /// </summary>
    public class GetPayTypeListItemResponseDto : BaseDto
    {
        /// <summary>
        /// 支付方式code
        /// </summary>
        public string PayTypeCode { get; set; }

        /// <summary>
        /// 支付方式name
        /// </summary>
        public string PayTypeName { get; set; }
    }
}
