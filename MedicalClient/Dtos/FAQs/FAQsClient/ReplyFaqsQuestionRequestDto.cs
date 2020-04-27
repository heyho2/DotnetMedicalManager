using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.FAQs.FAQsClient
{
    /// <summary>
    /// 回答问题 请求
    /// </summary>
    public class ReplyFaqsQuestionRequestDto : BaseDto
    {
        /// <summary>
        /// 问题guid
        /// </summary>
        [Required( ErrorMessage = "{0}必填")]
        public string QuestionGuid { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [StringLength(500, ErrorMessage = "内容长度不能超过500")]
        public string Content { get; set; }
    }
}
