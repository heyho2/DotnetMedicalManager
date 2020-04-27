using GD.Common.Base;

namespace GD.Dtos.WechatSubscription
{
    /// <summary>
    /// 用户微信公证号推广量
    /// </summary>
    public class WechatSubscriptionRecommendItemsDto : BaseDto
    {
        /// <summary>
        /// 推荐人guid
        /// </summary>
        public string UserGuid { get; set; }
        /// <summary>
        /// 推荐数量
        /// </summary>
        public int Count { get; set; }
    }
}
