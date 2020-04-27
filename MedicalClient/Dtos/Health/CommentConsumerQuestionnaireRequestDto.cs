using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 评论用户填写的问卷
    /// </summary>
    public class CommentConsumerQuestionnaireRequestDto : BaseDto
    {
        /// <summary>
        /// 用户问卷结果guid
        /// </summary>
        [Required(ErrorMessage = "问卷结果guid必填")]
        public string ResultGuid { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        [Required(ErrorMessage = "评论内容必填")]
        [StringLength(1000)]
        public string Comment { get; set; }
    }
}
