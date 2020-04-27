using System.ComponentModel.DataAnnotations;
using GD.Common.Base;
using GD.Common.EnumDefine;

namespace GD.Dtos.Account
{
    /// <summary>
    /// 电话号码，密码，验证码
    /// </summary>
    public class PhonePasswordCodeRequestDto : BaseDto
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "电话号码"), Phone(ErrorMessage = "请输入正确的电话号码")]
        public string Phone
        {
            get;
            set;
        }

        /// <summary>
        /// 注册密码MD5值
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "注册密码MD5值"), StringLength(32, MinimumLength = 32, ErrorMessage = "请输入32位有效MD5值")]
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// 推荐人GUID
        /// </summary>
        [Display(Name = "推荐人GUID")]
        public string Referrer
        {
            get;
            set;
        }

        /// <summary>
        /// 注册来源平台类型
        /// </summary>
        [Display(Name = "注册来源平台类型")]
        public PlatformType PlatformType
        {
            get;
            set;
        }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "验证码"), Range(111111, 999999, ErrorMessage = "{0}无效")]
        public int Code
        {
            get;
            set;
        }

        /// <summary>
        /// 扩展参数
        /// </summary>
        [Display(Name = "扩展参数")]
        public object Parameters
        {
            get;
            set;
        }
    }
}
