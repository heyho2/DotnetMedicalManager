using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Mall.LivingBeautyMall
{
    /// <summary>
    /// 通过分类获取商品数据请求Dto
    /// </summary>
    public class GetProductListByCategoryRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 分类ID
        /// </summary>
        public string CategoryGuid { get; set; }
    }

    /// <summary>
    /// 通过分类获取商品数据响应Dto
    /// </summary>
    public class GetProductListByCategoryResponseDto : BaseDto
    {
        /// <summary>
        /// 产品Guid
        /// </summary>
        public string ProductGuid { get; set; }
        /// <summary>
        /// 商户Guid
        /// </summary>
        public string MerchantGuid { get; set; }
        /// <summary>
        /// 分类Guid
        /// </summary>
        public string CategoryGuid { get; set; }
        /// <summary>
        /// 图片URL
        /// </summary>
        public string PictureGuid { get; set; }
        /// <summary>
        /// 产品名
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Standerd { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 市场价
        /// </summary>
        public decimal MarketPrice { get; set; }
        /// <summary>
        /// 推荐
        /// </summary>
        public bool Recommend { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable { get; set; }
    }
}
