using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using GD.Common.Base;
using GD.Dtos.Consumer.Consumer;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 生美首页推荐产品请求Dto
    /// 分页
    /// </summary>
    public class GetFirstPageRecommendProductListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 行为类型
        /// </summary>
        public string BehaviorType { get; set; } = BehaviorTypeEnum.Product.ToString();

        /// <summary>
        /// 行为类型枚举
        /// </summary>
        public enum BehaviorTypeEnum
        {
            /// <summary>
            /// 刷卡
            /// </summary>
            [Description("页面")]
            Page,
            /// <summary>
            /// 刷卡
            /// </summary>
            [Description("产品")]
            Product,
            /// <summary>
            /// 文章
            /// </summary>
            [Description("文章")]
            Article,
            /// <summary>
            /// 动作、活动
            /// </summary>
            [Description("动作")]
            Movement
        }
    }
}
