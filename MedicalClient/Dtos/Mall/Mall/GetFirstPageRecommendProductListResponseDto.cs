using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 生美首页推荐产品响应Dto
    /// </summary>
    public class GetFirstPageRecommendProductListResponseDto : BaseDto
    {
        /// <summary>
        /// 产品Guid
        /// </summary>
        public string ProductGuid { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 产品价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 产品原价格
        /// </summary>
        public decimal NormalPrice { get; set; }
        /// <summary>
        /// 产品图片Url
        /// </summary>
        public string ProductPicUrl { get; set; }
    }
}
