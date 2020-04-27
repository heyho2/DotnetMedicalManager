using GD.Common.EnumDefine;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.FAQs.FAQsIntergral
{
    /// <summary>
    /// 取消关注
    /// </summary>
    public  class CancelAttentionToDoctorReduceIntergralRequest
    {
        /// <summary>
        /// 取消关注医生Guid 
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "取消关注医生Guid")]
        public string DoctorGuid { get; set; }

        /// <summary>
        /// 评论Guid 
        /// </summary>
        [Display(Name = "用户类型")]
        public UserType UserType { get; set; } = UserType.Consumer;
    }
}
