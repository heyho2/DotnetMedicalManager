using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Meal.MealAdmin
{
    /// <summary>
    /// 登录请求
    /// </summary>
    public class MealAdminLoginRequestDto : BaseDto
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "账号必填")]
        public string UserName { get; set; }


        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码必填")]
        public string Password { get; set; }

        /// <summary>
        /// 多少天过期
        /// </summary>
        public int Days { get; set; }
    }
}
