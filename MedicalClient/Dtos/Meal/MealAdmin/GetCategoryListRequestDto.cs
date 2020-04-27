using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Meal.MealAdmin
{
    /// <summary>
    /// 获取餐别列表
    /// </summary>
    public class GetCategoryListRequestDto : BaseDto
    {
        /// <summary>
        /// 医院GUID
        /// </summary>
        [Required(ErrorMessage = "医院参数需提供")]
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 餐别名称
        /// </summary>
        public string CategoryName { get; set; }
    }
}