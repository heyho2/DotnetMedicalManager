using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 商品详情相应类
    /// </summary>
    public class GetProductDetailResponseDto : BaseDto
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
        /// 规格
        /// </summary>
        public string Standard { get; set; }
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
        public List<BannerInfo> ProBannerInfo { get; set; } = new List<BannerInfo>();

        /// <summary>
        /// 商品副标题
        /// </summary>
        public string ProductTitle { get; set; }

        /// <summary>
        /// 商户guid
        /// </summary>
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 产品banner信息
        /// </summary>
        public class BannerInfo
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
