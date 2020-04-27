using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Meal.MealCanteen
{
    /// <summary>
    /// 获取可用日期
    /// </summary>
    public class GetUseFullDateListAsyncRequestDto
    {
        /// <summary>
        /// 医院Guid 
        /// </summary>
        [Display(Name = "医院Guid")]
        public string HospitalGuid { get; set; }
        /// <summary>
        /// 菜品维护true，已订菜品:false
        /// </summary>
        [Display(Name = "医院Guid")]
        public bool IsShowNullDate { get; set; }

    }
    /// <summary>
    /// 响应
    /// </summary>
    public class GetUseFullDateListAsyncResponseDto
    {
        ///// <summary>
        ///// 主键
        ///// </summary>
        //public string MenuGuid { get; set; }
        /// <summary>
        /// 选餐日期
        /// </summary>
        public DateTime MenuDate { get; set; }


    }
}
