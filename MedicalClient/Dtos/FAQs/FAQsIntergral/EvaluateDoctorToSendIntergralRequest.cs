using GD.Common.EnumDefine;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.FAQs.FAQsIntergral
{
    /// <summary>
    /// 评论送积分
    /// </summary>
    public class EvaluateDoctorToSendIntergralRequest
    {
        /// <summary>
        /// 评论Guid 
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "评论Guid")]
        public string CommentGuid { get; set; }

        /// <summary>
        /// 评论Guid 
        /// </summary>
        [Display(Name = "用户类型")]
        public UserType UserType { get; set; } = UserType.Consumer;

    }


    /// <summary>
    /// 
    /// </summary>
    public class EvaluateDoctorToSendIntergralResponse
    {


    }
}
