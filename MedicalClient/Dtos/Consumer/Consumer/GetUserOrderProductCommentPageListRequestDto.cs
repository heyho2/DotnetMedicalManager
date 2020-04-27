using GD.Common.Base;
using System;
using System.ComponentModel;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 获取用户订单商品评价列表请求dto
    /// </summary>
    public class GetUserOrderProductCommentPageListRequestDto : PageRequestDto
    {
        /// <summary>
        /// 用户guid，非必填，默认为当前登录用户Id
        /// </summary>
        public string UserGuid { get; set; }
    }

    /// <summary>
    /// 获取用户订单商品评价列表响应dto
    /// </summary>
    public class GetUserOrderProductCommentPageListResponseDto : BasePageResponseDto<GetUserOrderProductCommentPageListItemDto>
    {
    }

    /// <summary>
    /// 获取用户订单商品评价列表数据明细dto
    /// </summary>
    public class GetUserOrderProductCommentPageListItemDto : BaseDto
    {
        /// <summary>
        /// 订单商品评价记录guid
        /// </summary>
        public string ProductCommentGuid { get; set; }

        /// <summary>
        /// 商品guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 下单日期
        /// </summary>
        public DateTime order_date { get; set; }

        /// <summary>
        /// 门店名称
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 评价状态
        /// </summary>
        public CommentStatusEnum CommentStatus { get; set; }

        /// <summary>
        /// 评价guid
        /// </summary>
        public string CommentGuid { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserPortrait { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 评论分数
        /// </summary>
        public int? Score { get; set; }

        /// <summary>
        /// 是否匿名
        /// </summary>
        /// <returns></returns>
        public bool? Anonymous { get; set; }

        /// <summary>
        /// 评价日期
        /// </summary>
        public DateTime? CommentDate { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string ProductPicture { get; set; }

        /// <summary>
        /// 评价状态
        /// </summary>
        public enum CommentStatusEnum
        {
            /// <summary>
            /// 未评价
            /// </summary>
            [Description("未评价")]
            NotEvaluate = 1,

            /// <summary>
            /// 已评价
            /// </summary>
            [Description("已评价")]
            Evaluate,

            /// <summary>
            /// 已过期
            /// </summary>
            [Description("已过期")]
            Expired
        }
    }
}