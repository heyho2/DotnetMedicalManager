using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 获取指定目标的评论树响应Dto
    /// </summary>
    public class GetTargetCommentRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 目标Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "目标Guid")]
        public string TargetGuid { get; set; }
    }
}
