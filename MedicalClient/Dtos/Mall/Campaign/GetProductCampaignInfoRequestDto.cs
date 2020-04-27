using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Mall.Campaign
{
    /// <inheritdoc />
    /// <summary>
    /// 生美-获取商品活动 请求Dto
    /// </summary>
    public class GetProductCampaignInfoRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 产品Guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable { get; set; } = true;
    }
    /// <inheritdoc />
    /// <summary>
    /// 响应Dto
    /// </summary>
    public class GetProductCampaignInfoResponseDto : BasePageResponseDto<GetProductCampaignInfoItem>
    {

    }
    /// <inheritdoc />
    /// <summary>
    /// 子项
    /// </summary>
    public class GetProductCampaignInfoItem : BaseDto
    {
        /// <summary>
        /// 活动guid
        /// </summary>
        public string CampaignGuid { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string CampaignName { get; set; }
    }

}
