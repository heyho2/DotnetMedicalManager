using GD.Common.EnumDefine;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.FAQs.FAQsIntergral
{
    /// <summary>
    /// 浏览文章送积分
    /// </summary>
    public class ViewArticleSendIntergralRequest
    {
        /// <summary>
        /// 文章Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "文章Guid")]
        public string ArticleGuid { get; set; }
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
