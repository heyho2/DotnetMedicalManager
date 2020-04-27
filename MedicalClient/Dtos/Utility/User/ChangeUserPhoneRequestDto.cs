using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Utility.User
{
    /// <summary>
    /// 更新手机
    /// </summary>
    public class ChangeUserPhoneRequestDto : BaseDto
    {
        ///<summary>
        /// 用户Guid
        ///</summary>
        public string UserGuid { get; set; }

        /// <summary>
        /// 用户新手机号
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "用户新手机号")]
        public string Phone { get; set; }

        /// <summary>
        /// 手机验证码
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "手机验证码")]
        public int VerifyCode { get; set; }
    }
}
