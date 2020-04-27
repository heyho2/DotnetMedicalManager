using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Meal.MealAdmin
{
    /// <summary>
    /// 添加食堂操作员实体
    /// </summary>
    public class AddMealOperatorRequestDto : BaseDto
    {
        /// <summary>
        /// 医院GUID
        /// </summary>
        [Required(ErrorMessage = "医院参数需提供")]
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "手机号必填")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码必填")]
        public string Password { get; set; }
    }
}
