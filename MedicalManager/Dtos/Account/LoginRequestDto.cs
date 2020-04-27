using System.ComponentModel.DataAnnotations;
using GD.Common.Base;
using GD.Common.EnumDefine;

namespace GD.Dtos.Account
{
    /// <summary>
    /// 登录请求参数
    /// </summary>
    public class LoginRequestDto : BaseDto
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
        /// 用户类型
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "用户类型")]
        public UserType UserType
        {
            get;
            set;
        }

        /// <summary>
        /// 登录有效天数，默认为1。非正数则表示永不过期
        /// </summary>
        public double Days { get; set; } = 1;
    }
}
