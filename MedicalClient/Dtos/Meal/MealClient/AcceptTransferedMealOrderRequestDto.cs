using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Meal.MealClient
{
    /// <summary>
    /// 接收转让订单请求
    /// </summary>
    public class AcceptTransferedMealOrderRequestDto : BaseDto
    {
        /// <summary>
        /// 订单guid
        /// </summary>
        [Required(ErrorMessage ="订单guid必填")]
        public string OrderGuid { get; set; }
    }
}
