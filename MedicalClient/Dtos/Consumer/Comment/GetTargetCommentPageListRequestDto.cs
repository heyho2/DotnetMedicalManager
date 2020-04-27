using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Consumer.Comment
{
    /// <summary>
    /// 获取目标评论分页列表请求Dto
    /// </summary>
    public class GetTargetCommentPageListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 评论目标guid
        /// </summary>
        public string TargetGuid { get; set; }

        /// <summary>
        /// 当前登录用户guid(用于判断用户是否点赞了指定的回答)
        /// </summary>
        public string UserId { get; set; }
    }

    /// <summary>
    /// 获取目标评论分页列表响应Dto
    /// </summary>
    public class GetTargetCommentPageListResponseDto : BasePageResponseDto<GetTargetCommentPageListItemDto>
    {

    }

    /// <summary>
    /// 获取目标评论分页列表ItemDto
    /// </summary>
    public class GetTargetCommentPageListItemDto : BaseDto
    {
        /// <summary>
        /// 评论guid
        /// </summary>
        public string CommentGuid { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 评价评分
        /// </summary>
        public string Score { get; set; }

        /// <summary>
        /// 评论日期
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 评论人昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 评论人头像
        /// </summary>
        public string Portrait { get; set; }

        /// <summary>
        /// 回复数量
        /// </summary>
        public int TotalReply { get; set; }

        /// <summary>
        /// 点赞数量
        /// </summary>
        public int TotalLike { get; set; }

        /// <summary>
        /// 如果用户已登录，则表示当前用户是否点赞了回答记录
        /// </summary>
        public bool IsLike { get; set; } = false;
    }
}
