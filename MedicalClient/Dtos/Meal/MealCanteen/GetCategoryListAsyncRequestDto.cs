using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Meal.MealCanteen
{
    /// <summary>
    /// 获取分类
    /// </summary>
    public class GetCategoryListAsyncRequestDto
    {
        /// <summary>
        /// 医院Guid 
        /// </summary>
        [Display(Name = "医院Guid")]
        public string HospitalGuid { get; set; }


    }

    /// <summary>
    /// 响应
    /// </summary>
    public class GetCategoryListAsyncResponseDto
    {
        /// <summary>
        /// 分类Guid 
        /// </summary>
        public string CategoryGuid { get; set; }

        /// <summary>
        /// 分类名称 
        /// </summary>
        public string CategoryName { get; set; }

    }


}
