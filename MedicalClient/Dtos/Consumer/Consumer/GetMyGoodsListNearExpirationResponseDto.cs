using GD.Common.Base;
using System;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 获取用户临近过期的卡数据列表响应Dto
    /// </summary>
    public class GetMyGoodsListNearExpirationResponseDto : BaseDto
    {
        /// <summary>
        /// 个人卡guid
        /// </summary>
        public string GoodsGuid { get; set; }

        /// <summary>
        /// 卡名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 过期日期
        /// </summary>
        public DateTime EffectiveEndDate { get; set; }
    }
}
