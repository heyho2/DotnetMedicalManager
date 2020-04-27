using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Merchant.Therapist
{
    /// <summary>
    /// 重置密码
    /// </summary>
    public class TherapistResetPasswordAsyncRequestDto
    {
        ///<summary>
        ///手机号
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "手机号")]
        public string Phone { get; set; }

        ///<summary>
        ///验证码
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "验证码")]
        public int Code { get; set; }

        ///<summary>
        ///密码
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "密码")]
        public string Password { get; set; }
    }
}
