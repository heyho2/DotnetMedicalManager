using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Meal.MealClient
{
    /// <summary>
    /// 获取指定日期的菜单详情请求Dto
    /// </summary>
    public class GetMenuDetailOneDayRequestDto : BaseDto
    {
        /// <summary>
        /// 医院guid
        /// </summary>
        [Required(ErrorMessage ="医院guid必填")]
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 菜单日期
        /// </summary>
        [Required(ErrorMessage = "菜单日期必填")]
        public DateTime MenuDate { get; set; }
    }
}
