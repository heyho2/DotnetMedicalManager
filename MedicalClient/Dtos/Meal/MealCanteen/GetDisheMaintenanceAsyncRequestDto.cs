﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Meal.MealCanteen
{
    /// <summary>
    /// 菜品维护
    /// </summary>
    public class GetDisheMaintenanceAsyncRequestDto
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

    }

    /// <summary>
    /// 响应
    /// </summary>
    public class GetDisheMaintenanceAsyncResponseDto
    {
        /// <summary>
        /// 餐品Guid 
        /// </summary>
        public string DishesGuid { get; set; }
        /// <summary>
        /// 餐品名称
        /// </summary>
        public string DishesName { get; set; }

    }
}
