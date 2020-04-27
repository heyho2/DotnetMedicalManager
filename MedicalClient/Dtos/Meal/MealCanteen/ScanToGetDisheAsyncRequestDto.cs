using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Meal.MealCanteen
{
    /// <summary>
    /// 扫码取餐
    /// </summary>
    public class ScanToGetDisheAsyncRequestDto
    {
        /// <summary>
        /// 扫码信息（OrderGuid） 
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "扫码信息（OrderGuid） ")]
        public string OrderGuid { get; set; }

    }

    /// <summary>
    ///响应
    /// </summary>
    public class ScanToGetDisheAsyncResponseDto
    {
        /// <summary>
        /// 订单Guid 
        /// </summary>
        public string OrderGuid { get; set; }
        /// <summary>
        /// 订单总价
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// 菜信息详情
        /// </summary>
        public List<MealOrderDetailInfo> DishesDetail { get; set; }


        /// <summary>
        /// 多个餐品
        /// </summary>
        public class MealOrderDetailInfo
        {
            /// <summary>
            /// 菜Guid
            /// </summary>
            public string DishesGuid { get; set; }
            /// <summary>
            /// 菜名称
            /// </summary>
            public string DishesName { get; set; }
            /// <summary>
            /// 数量
            /// </summary>
            public int Quantity { get; set; }
            /// <summary>
            /// 餐品单价
            /// </summary>
            public decimal UnitPrice { get; set; }

        }

    }
}
