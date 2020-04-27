using GD.Common.EnumDefine;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.FAQs.FAQsIntergral
{
    /// <summary>
    /// 浏览问题送积分
    /// </summary>
    public class ViewQuestionToSendIntergralRequest
    {
        /// <summary>
        /// 问题Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "问题Guid")]
        public string QuestionGuid { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        [Display(Name = "用户类型")]
        public UserType UserType { get; set; } = UserType.Consumer;
        /// <summary>
        /// 用户ID 
        /// </summary>
        [Display(Name = "用户ID")]
        public string UserID { get; set; }
    }
}
