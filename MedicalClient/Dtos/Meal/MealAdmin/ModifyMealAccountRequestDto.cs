using GD.Common.Base;
using GD.Models.Meal;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Meal.MealAdmin
{
    /// <summary>
    /// 更新账户请求
    /// </summary>
    public class ModifyMealAccountRequestDto : BaseDto
    {
        /// <summary>
        /// 医院GUID
        /// </summary>
        [Required(ErrorMessage = "医院参数不正确")]
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 用户GUID
        /// </summary>
        [Required(ErrorMessage = "用户参数不正确")]
        public string UserGuid { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        [Required(ErrorMessage = "用户身份参数为空")]
        public MealUserTypeEnum UserType { get; set; }
    }
}
