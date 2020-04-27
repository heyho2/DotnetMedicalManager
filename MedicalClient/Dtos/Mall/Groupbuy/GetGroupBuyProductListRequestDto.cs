using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;
using GD.Models.CommonEnum;

namespace GD.Dtos.Mall.Groupbuy
{
    /// <inheritdoc />
    /// <summary>
    /// 生美-获取拼团列表Dto
    /// </summary>
    public class GetGroupBuyProductListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 分类枚举
        /// </summary>
        public string ClassifyName { get; set; } = ClassifyEnum.GroupBuy.ToString();

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
    /// 响应Dto
    /// </summary>
    public class GetGroupBuyProductListResponseDto : BasePageResponseDto<GetGroupBuyProductItem>
    {

    }

    /// <inheritdoc />
    /// <summary>
    /// 子项
    /// </summary>
    public class GetGroupBuyProductItem : BaseDto
    {
        /// <summary>
        /// 团购Guid
        /// </summary>
        public string GroupBuyGuid { get; set; }
        /// <summary>
        /// 团购名称
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
        /// <summary>
        /// 团购价
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        public string OriginalPrice { get; set; }
        /// <summary>
        /// 团购总数量
        /// </summary>
        public string Qty { get; set; }
        /// <summary>
        /// 已参团人数
        /// </summary>
        public string Bought { get; set; }

    }
}
