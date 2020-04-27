using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;
using GD.Models.CommonEnum;

namespace GD.Dtos.Mall.Mall
{
    /// <inheritdoc />
    /// <summary>
    /// 生美-获取超值优惠首页商品推荐 
    /// </summary>
    public class GetRecommendProductRequestDto: BasePageRequestDto
    {
        /// <summary>
        /// 分类枚举
        /// </summary>
        public string ClassifyName { get; set; } = ClassifyEnum.SuperValu.ToString();

        /// <summary>
        /// 平台类型
        /// </summary>
        public string PlatformType { get; set; } = GD.Common.EnumDefine.PlatformType.LifeCosmetology.ToString();

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable { get; set; } = true;
    }
    /// <inheritdoc />
    /// <summary>
    /// 
    /// </summary>
    public class GetRecommendProductResponseDto : BasePageResponseDto<GetRecommendProductItem>
    {

    }
    /// <inheritdoc />
    /// <summary>
    /// 子项
    /// </summary>
    public class GetRecommendProductItem : BaseDto
    {
        /// <summary>
        /// 分类Guid
        /// </summary>
        public string ClassifyGuid { get; set; }
        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool Recommend { get; set; }
        /// <summary>
        /// 产品Guid
        /// </summary>
        public string TargetGuid { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string TargetName { get; set; }
        /// <summary>
        /// 产品URL
        /// </summary>
        public string TargetPicUrl { get; set; }
    }
}
