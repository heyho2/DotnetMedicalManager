using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;
using GD.Models.CommonEnum;

namespace GD.Dtos.Mall.Groupbuy
{
    /// <inheritdoc />
    /// <summary>
    /// 生美-获取拼团首页商品推荐 请求Dto
    /// </summary>
    public class GetRecommendGroupBuyProductRequestDto : BasePageRequestDto
    {
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
    public class GetRecommendGroupBuyProductResponseDto : BasePageResponseDto<GetRecommendGroupBuyProductItem>
    {

    }
    /// <summary>
    /// 子项
    /// </summary>
    public class GetRecommendGroupBuyProductItem : BaseDto
    {
        /// <summary>
        /// 团购ID
        /// </summary>
        public string GroupBuyGuid { get; set; }
        /// <summary>
        /// 团购标题
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 产品Guid
        /// </summary>
        public string ProductGuid { get; set; }
        /// <summary>
        /// 图片Url
        /// </summary>
        public string PictureUrl { get; set; }
      
    }
}
