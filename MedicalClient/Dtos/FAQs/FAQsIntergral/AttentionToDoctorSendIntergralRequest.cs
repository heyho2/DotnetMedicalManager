using GD.Common.EnumDefine;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.FAQs.FAQsIntergral
{
    /// <summary>
    /// 加关注送积分
    /// </summary>
    public class AttentionToDoctorSendIntergralRequest
    {
        /// <summary>
        /// 收藏医生Guid 
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "收藏医生Guid")]
        public string DoctorGuid { get; set; }

        /// <summary>
        /// 评论Guid 
        /// </summary>
        [Display(Name = "用户类型")]
        public UserType UserType { get; set; } = UserType.Consumer;
    }
}
