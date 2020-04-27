using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Meal.MealClient
{
    /// <summary>
    /// 获取订单详情响应Dto
    /// </summary>
    public class GetMealOrderDetailResponseDto : BaseDto
    {
        /// <summary>
        /// 订单guid
        /// </summary>
        public string OrderGuid { get; set; }

        /// <summary>
        /// 订单总价
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// 用餐开始时间
        /// </summary>
        public DateTime MealStartTime { get; set; }

        /// <summary>
        /// 用餐截止时间
        /// </summary>
        public DateTime MealEndTime { get; set; }

        /// <summary>
        /// 订单菜品列表
        /// </summary>
        public List<MealOrderDetailDishesDto> Dishees { get; set; }

        /// <summary>
        /// 订单菜品
        /// </summary>
        public class MealOrderDetailDishesDto : BaseDto
        {
            /// <summary>
            /// 菜品名称
            /// </summary>
            public string DishesName { get; set; }

            /// <summary>
            /// 单价
            /// </summary>
            public decimal UnitPrice { get; set; }

            /// <summary>
            /// 数量
            /// </summary>
            public int Quantity { get; set; }
        }
    }
}
