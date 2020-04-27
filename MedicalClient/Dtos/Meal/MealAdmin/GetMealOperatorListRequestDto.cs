using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Meal.MealAdmin
{
    /// <summary>
    /// 获取食堂操作员列表请求
    /// </summary>
    public class GetMealOperatorListRequestDto : BaseDto
    {
        /// <summary>
        /// 医院GUID
        /// </summary>
        [Required(ErrorMessage = "医院参数需提供")]
        public string HospitalGuid { get; set; }
    }
}
