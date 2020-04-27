using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Utility.Account
{
    /// <summary>
    /// 登录请求参数
    /// </summary>
    public class LoginAdminRequestDto : BaseDto
    {
        /// <summary>
        /// 管理员账号
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "管理员账号")]
        public string Account
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
        /// 登录有效天数，默认为1。非正数则表示永不过期
        /// </summary>
        public double Days { get; set; } = 1;
    }
}