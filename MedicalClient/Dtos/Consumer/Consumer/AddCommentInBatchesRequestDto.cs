using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 批量评论请求Dto
    /// </summary>
    public class AddCommentInBatchesRequestDto : BaseDto
    {
        /// <summary>
        /// 目标ID
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "目标ID")]
        public List<string> TargetGuids { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "评论内容")]
        public string Content { get; set; }

        /// <summary>
        /// 评论分数
        /// </summary>
        [Display(Name = "评论分数")]
        public int Score { get; set; } = 0;

        /// <summary>
        /// 是否匿名
        /// </summary>
        /// <returns></returns>
        [Display(Name = "是否匿名")]
        public bool Anonymous { get; set; }

    }
}
