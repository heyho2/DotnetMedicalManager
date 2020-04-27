using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Consumer.Consumer
{
    /// <inheritdoc />
    /// <summary>
    ///生美-获取双美全部评价 请求Dto
    /// </summary>
    public class GetAllCLEvaluateRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 目标Guid
        /// </summary>
        public string TargetGuid { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable { get; set; } = true;
    }
    /// <inheritdoc />
    /// <summary>
    /// 响应Dto
    /// </summary>
    public class GetAllCLEvaluateResponseDto : BaseDto
    {
        /// <summary>
        /// UserId
        /// </summary>
        public string UserGuid { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 头像URL
        /// </summary>
        public string PortraitUrl { get; set; }

        /// <summary>
        /// 评论时间
        /// </summary>
        public string CommentDate { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 图片URL
        /// </summary>
        public string PictureUrl { get; set; }

        /// <summary>
        /// 目标名称
        /// </summary>
        public string TargetName { get; set; }

        /// <summary>
        /// 评分
        /// </summary>
        public string Score { get; set; }
    }
   
}