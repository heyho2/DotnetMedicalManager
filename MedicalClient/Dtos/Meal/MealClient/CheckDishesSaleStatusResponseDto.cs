using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Meal.MealClient
{
    /// <summary>
    /// 检测菜品是否在售响应Dto
    /// </summary>
    public class CheckDishesSaleStatusResponseDto
    {
        /// <summary>
        /// 检测结果，true表示正常，false表示有异常
        /// </summary>
        public bool CheckResult { get; set; }

        /// <summary>
        /// 不在售的菜品列表
        /// </summary>
        public List<CheckDishes> NotSaleDishes { get; set; }

        /// <summary>
        /// 不在售的菜品
        /// </summary>
        public class CheckDishes
        {
            /// <summary>
            /// 菜品guid
            /// </summary>
            public string DishesGuid { get; set; }

            /// <summary>
            /// 菜品名称
            /// </summary>
            public string DishesName { get; set; }
        }
    }
}
