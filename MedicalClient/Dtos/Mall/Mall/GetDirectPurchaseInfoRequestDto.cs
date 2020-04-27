using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 一键购页面数据显示请求Dto
    /// </summary>
    public class GetDirectPurchaseInfoRequestDto : BaseDto
    {
        /// <summary>
        /// 产品guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
    }
}
