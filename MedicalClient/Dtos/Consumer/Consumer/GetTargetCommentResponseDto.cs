using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 获取指定目标的评论树响应Dto
    /// </summary>
    public class GetTargetCommentResponseDto : BaseDto
    {
        /// <summary>
        /// 评论Guid
        /// </summary>
        public string CommentGuid { get; set; }

        /// <summary>
        /// 目标Guid
        /// </summary>
        public string TargetGuid { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 评论打分
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Portrait { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 点赞数
        /// </summary>
        public int LikeNumber  { get; set; }

        /// <summary>
        /// 回复
        /// </summary>
        public List<GetTargetCommentResponseDto> SonComments { get; set; }
    }
}
