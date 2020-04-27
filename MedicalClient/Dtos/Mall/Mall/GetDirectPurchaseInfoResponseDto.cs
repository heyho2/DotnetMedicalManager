using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 一键购页面数据显示响应Dto
    /// </summary>
    public class GetDirectPurchaseInfoResponseDto : BaseDto
    {
        /// <summary>
        /// 商品Guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string ProductPicture { get; set; }

        /// <summary>
        /// 商品单价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// 预付款比例
        /// </summary>
        public decimal Rate { get; set; }

        
    }
}
