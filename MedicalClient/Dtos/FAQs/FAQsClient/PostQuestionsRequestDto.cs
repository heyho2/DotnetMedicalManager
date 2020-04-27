using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.FAQs.FAQsClient
{
    /// <summary>
    /// 发布问题请求dto
    /// </summary>
    public class PostQuestionsRequestDto : BaseDto
    {
        /// <summary>
        /// 问题内容
        /// </summary>
        [Required(ErrorMessage ="问题内容必填")]
        [StringLength(500, ErrorMessage = "内容长度不能超过500")]
        public string Content { get; set; }

        /// <summary>
        /// 附件图片地址
        /// </summary>
        public List<string> AttachedPictures { get; set; }

        /// <summary>
        /// 悬赏积分
        /// </summary>
        [Required(ErrorMessage ="悬赏积分必填")]
        public int Score { get; set; }

    }
}
