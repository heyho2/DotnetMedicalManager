using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Meal.MealClient
{
    /// <summary>
    /// 获取用户点餐订单分页列表Item Dto
    /// </summary>
    public class GetUserMealOrderPageListItemDto : BaseDto
    {
        /// <summary>
        /// 订单guid
        /// </summary>
        public string OrderGuid { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 用餐日期
        /// </summary>
        public DateTime MealDate { get; set; }

        /// <summary>
        /// 用餐开始时间
        /// </summary>
        public DateTime MealStartTime { get; set; }

        /// <summary>
        /// 用餐截止时间
        /// </summary>
        public DateTime MealEndTime { get; set; }


        /// <summary>
        /// 能否取消订单
        /// </summary>
        public bool CanCancel { get; set; } = false;

        /// <summary>
        /// 餐别名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 订单总价格
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderStatus { get; set; }

        /// <summary>
        /// 订单创建日期
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 订单明细
        /// </summary>
        public List<GetUserMealOrderDetailResponseDto> OrderDetails { get; set; }
       

        /// <summary>
        /// 订单明细
        /// </summary>
        public class GetUserMealOrderDetailResponseDto
        {
            /// <summary>
            /// 菜品名称
            /// </summary>
            public string DishesName { get; set; }

            /// <summary>
            /// 数量
            /// </summary>
            public int Quantity { get; set; }
        }
    }

    /// <summary>
    /// 获取用户点餐订单分页列表Item Dto
    /// </summary>
    public class GetUserMealOrderPageListResponseDto : BasePageResponseDto<GetUserMealOrderPageListItemDto>
    {

    }

    /// <summary>
    /// 获取用户点餐订单分页列表请求Dto
    /// </summary>
    public class GetUserMealOrderPageListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 医院guid
        /// </summary>
        [Required(ErrorMessage ="医院guid必填")]
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 用户guid,非必填，默认为当前登录人
        /// </summary>
        public string UserGuid { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class GetUserMealOrderPageListQueryDto
    {
        /// <summary>
        /// 订单guid
        /// </summary>
        public string OrderGuid { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 用餐日期
        /// </summary>
        public DateTime MealDate { get; set; }

        /// <summary>
        /// 用餐开始时间
        /// </summary>
        public DateTime MealStartTime { get; set; }

        /// <summary>
        /// 用餐截止时间
        /// </summary>
        public DateTime MealEndTime { get; set; }

        /// <summary>
        /// 提前几天可预订
        /// </summary>
        public int CategoryAdvanceDay { get; set; }

        /// <summary>
        /// 可预订截止时间
        /// </summary>
        public string CategoryScheduleTime { get; set; }

        /// <summary>
        /// 餐别名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 订单总价格
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string RealOrderStatus { get; set; }

        /// <summary>
        /// 订单创建日期
        /// </summary>
        public DateTime CreationDate { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetUserMealOrderDetailQueryDto
    {
        /// <summary>
        /// 订单guid
        /// </summary>
        public string OrderGuid { get; set; }

        /// <summary>
        /// 菜品guid
        /// </summary>
        public string DishesGuid { get; set; }

        /// <summary>
        /// 菜品名称
        /// </summary>
        public string DishesName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
    }
}
