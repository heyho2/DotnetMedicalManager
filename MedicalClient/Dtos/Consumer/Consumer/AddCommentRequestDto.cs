using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 添加评论请求dto
    /// </summary>
    public class AddCommentRequestDto : BaseDto
    {
        /// <summary>
        /// 目标ID
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "目标ID")]
        public string TargetGuid { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 评论分数
        /// </summary>
        [ Display(Name = "评论分数")]
        public int Score { get; set; } = 5;

        /// <summary>
        /// 是否匿名
        /// </summary>
        /// <returns></returns>
        [Display(Name = "是否匿名")]
        public bool Anonymous { get; set; }

        ///// <summary>
        ///// 评论图片列表
        ///// </summary>
        ///// <returns></returns>
        //[Display(Name = "评论分数")]
        //public List<CommentPic> CommentPicList { get; set; }

        //public class CommentPic
        //{
        //    public string BasePath { get; set; }
        //    public string RelativePath { get; set; }
        //}
    }
}
