using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Meal.MealClient
{
    /// <summary>
    ///提交点餐订单请求Dto
    /// </summary>
    public class SubmitMealOrderRequestDto : BaseDto
    {
        /// <summary>
        /// 医院guid
        /// </summary>
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 菜品详情
        /// </summary>
        public List<OrderDishes> DishesDetails { get; set; }

        /// <summary>
        /// 订单菜品
        /// </summary>
        public class OrderDishes
        {
            /// <summary>
            /// 预订的用餐日期
            /// </summary>
            public DateTime MealDate { get; set; }

            /// <summary>
            /// 餐别guid
            /// </summary>
            public string CategoryGuid { get; set; }

            /// <summary>
            /// 菜品guid
            /// </summary>
            public string DishesGuid { get; set; }

            /// <summary>
            /// 数量
            /// </summary>
            public int Quantity { get; set; }
        }
    }
}
