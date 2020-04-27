using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Mall.LivingBeautyMall
{
    /// <summary>
    /// 生美-商品banner简介 请求Dto
    /// </summary>
    public class GetCommonProductDetailBannerRequestDto 
    {
        /// <summary>
        /// 产品Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "产品Guid")]
        public string ProductGuid { get; set; }
        /// <summary>
        /// 平台类型
        /// </summary>
        public string PlatformType { get; set; } = GD.Common.EnumDefine.PlatformType.LifeCosmetology.ToString();
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable { get; set; } = true;

    }
    /// <summary>
    /// 响应Dto
    /// </summary>
    public class GetCommonProductDetailBannerResponseDto
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
        /// 产品title
        /// </summary>
        public string ProductTitle { get; set; }
        /// <summary>
        /// 销量
        /// </summary>
        public int SoldNum { get; set; }
        /// <summary>
        /// 商品形态(服务，实体)
        /// </summary>
        public string ProductForm { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Standard { get; set; }
        /// <summary>
        /// 图片Url
        /// </summary>
        public string ProductPicURL { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 运费？
        /// </summary>
        public decimal Freight { get; set; }
        /// <summary>
        /// 产品banner信息
        /// </summary>
        public List<GetCommonProductDetailBannerItem> ProBannerInfo { get; set; }
        public string MerchantGuid { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// 子项
        /// </summary>
        public class GetCommonProductDetailBannerItem : BaseDto
        {
            /// <summary>
            /// 商品bannerID
            /// </summary>
            public string BannerGuid { get; set; }
            /// <summary>
            /// 商品banner图片地址
            /// </summary>
            public string BannerPicUrl { get; set; }
            /// <summary>
            /// 目标地址
            /// </summary>
            public string BannerTargetUrl { get; set; }
        }
    }
}
