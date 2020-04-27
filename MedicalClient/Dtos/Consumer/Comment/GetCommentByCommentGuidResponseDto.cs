using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Comment
{
    /// <summary>
    /// 根据guid获取评论数据响应Dto
    /// </summary>
    public class GetCommentByCommentGuidResponseDto : BaseDto
    {
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 评价打分
        /// </summary>
        public int Score { get; set; }
    }
}
