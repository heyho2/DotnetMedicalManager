using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Meal.MealCanteen
{
    /// <summary>
    /// 菜品维护-新增
    /// </summary>
    public class AddDisheMaintenanceAsyncRequestDto
    {
        /// <summary>
        /// 日期 
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "日期")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// 分类Guid 
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "分类Guid")]
        public string CategoryGuid { get; set; }

        /// <summary>
        /// 医院Guid 
        /// </summary>
        [Display(Name = "医院Guid")]
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 医院菜Guid数组 
        /// </summary>
        [Display(Name = "菜Guid数组")]
        public string[] DishesGuidArr { get; set; }

    }

    /// <summary>
    /// 响应
    /// </summary>
    public class AddDisheMaintenanceAsyncResponseDto
    {

    }
}
