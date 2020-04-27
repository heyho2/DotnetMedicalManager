using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Meal.MealCanteen
{
    /// <summary>
    /// 食堂端登录
    /// </summary>
    public class MealCanteenLoginRequestDto
    {
        /// <summary>
        ///用户名（手机号）
        /// </summary>
        [Required(ErrorMessage = "用户名（手机号）")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码必填")]
        public string Password { get; set; }
    }

    /// <summary>
    /// 响应
    /// </summary>
    public class MealCanteenLoginResponseDto
    {
        /// <summary>
        /// 用户guid
        /// </summary>
        public string OperatorGuid { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 医院Guid
        /// </summary>
        public string HospitalGuid { get; set; }
        /// <summary>
        /// token
        /// </summary>
        public string Token { get; set; }
    }
}
