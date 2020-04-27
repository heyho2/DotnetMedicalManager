using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Meal.MealClient
{
    /// <summary>
    /// 获取指定日期的菜单详情响应Dto
    /// </summary>
    public class GetMenuDetailOneDayResponseDto : BaseDto
    {
        /// <summary>
        /// 餐别guid
        /// </summary>
        public string CategoryGuid { get; set; }

        /// <summary>
        /// 餐别名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 点餐截止时间
        /// </summary>
        public DateTime BookingDeadline { get; set; }

        /// <summary>
        /// 用餐开始时间
        /// </summary>
        public string MealStartTime { get; set; }

        /// <summary>
        /// 用餐截止时间
        /// </summary>
        public string MealEndTime { get; set; }

        /// <summary>
        /// 菜品列表
        /// </summary>
        public List<MenuDishesDto> MenuDishes { get; set; }

        
        /// <summary>
        /// 菜单菜品信息
        /// </summary>
        public class MenuDishesDto
        {
            /// <summary>
            /// 餐品guid
            /// </summary>
            public string DishesGuid { get; set; }

            /// <summary>
            /// 餐品名称
            /// </summary>
            public string DishesName { get; set; }

            /// <summary>
            /// 餐品单价
            /// </summary>
            public decimal DishesPrice { get; set; }

            /// <summary>
            /// 菜品描述
            /// </summary>
            public string DishesDescription { get; set; }

            /// <summary>
            /// 菜品图片
            /// </summary>
            public string DishesImg { get; set; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MenuDetailOneDayQueryDto
    {
        /// <summary>
        /// 餐别guid
        /// </summary>
        public string CategoryGuid { get; set; }

        /// <summary>
        /// 餐别名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 提前几天可预订
        /// </summary>
        public int CategoryAdvanceDay { get; set; }

        /// <summary>
        /// 可预订截止时间
        /// </summary>
        public string CategoryScheduleTime { get; set; }

        /// <summary>
        /// 用餐开始时间
        /// </summary>
        public string MealStartTime { get; set; }

        /// <summary>
        /// 用餐截止时间
        /// </summary>
        public string MealEndTime { get; set; }

        /// <summary>
        /// 餐品guid
        /// </summary>
        public string DishesGuid { get; set; }

        /// <summary>
        /// 餐品名称
        /// </summary>
        public string DishesName { get; set; }

        /// <summary>
        /// 菜品图片
        /// </summary>
        public string DishesImg { get; set; }

        /// <summary>
        /// 餐品内部价
        /// </summary>
        public decimal DishesInternalPrice { get; set; }

        /// <summary>
        /// 餐品外部价
        /// </summary>
        public decimal DishesExternalPrice { get; set; }

        /// <summary>
        /// 菜品描述
        /// </summary>
        public string DishesDescription { get; set; }

    }
}
