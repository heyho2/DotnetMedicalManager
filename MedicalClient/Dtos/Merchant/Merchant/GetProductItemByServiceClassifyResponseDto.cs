using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 通过类型获取商品列表
    /// </summary>
    public class GetProductItemByServiceClassifyResponseDto : BaseDto
    {
        /// <summary>
        /// 商品guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品单价
        /// </summary>
        public decimal ProductPrice { get; set; }
    }
}
