using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Meal.MealCanteen
{
    /// <summary>
    /// 已定餐查询
    /// </summary>
    public class GetBookMealListAsyncRequestDto
    {
        /// <summary>
        /// 日期 
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "日期")]
        public DateTime? Date { get; set; }

        ///// <summary>
        ///// 分类Guid 
        ///// </summary>
        //[Required(ErrorMessage = "{0}必填"), Display(Name = "分类Guid")]
        //public string CategoryGuid { get; set; }

        /// <summary>
        /// 医院Guid 
        /// </summary>
        [Display(Name = "医院Guid")]
        public string HospitalGuid { get; set; }
    }


    /// <summary>
    /// 响应
    /// </summary>
    public class GetBookMealListAsyncResponseDto
    {
        /// <summary>
        /// 医院名 
        /// </summary>
        public string HosName { get; set; }

        /// <summary>
        /// 日期 
        /// </summary>
        public DateTime? MealDate { get; set; }

        /// <summary>
        /// 菜Guid 
        /// </summary>
        public string DishesGuid { get; set; }

        /// <summary>
        /// 菜名 
        /// </summary>
        public string DishesName { get; set; }
        /// <summary>
        /// 分类Guid 
        /// </summary>
        public string CategoryGuid { get; set; }
        /// <summary>
        /// 分类名
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 提前几天
        /// </summary>
        public int CategoryAdvanceDay { get; set; }
        /// <summary>
        /// 可预订截止时间
        /// </summary>
        public string CategoryScheduleTime { get; set; }
        /// <summary>
        /// 已订数量
        /// </summary>
        public int BookedTotal { get; set; } = 0;
    }



    /// <summary>
    /// 响应返回
    /// </summary>
    public class GetBookMealListAsyncReturnResponseDto
    {
        /// <summary>
        /// 分类Guid 
        /// </summary>
        public string CategoryGuid { get; set; }
        /// <summary>
        /// 分类名
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 提前几天
        /// </summary>
        public int CategoryAdvanceDay { get; set; }
        /// <summary>
        /// 可预订截止时间
        /// </summary>
        public string CategoryScheduleTime { get; set; }
        /// <summary>
        /// 医院名 
        /// </summary>
        public string HosName { get; set; }
        /// <summary>
        /// 日期 
        /// </summary>
        public DateTime? MenuDate { get; set; }
        /// <summary>
        /// 是否已截单 (true:预定中，false:已截单)
        /// </summary>
        public bool IsExpirationBook { get; set; }

        /// <summary>
        /// 已定餐信息列表
        /// </summary>
        public List<BookedDishesInfo> BookedDishesInfoList { get; set; }

        /// <summary>
        /// 已定餐信息
        /// </summary>
        public class BookedDishesInfo
        {
            /// <summary>
            /// 菜Guid 
            /// </summary>
            public string DishesGuid { get; set; }

            /// <summary>
            /// 菜名 
            /// </summary>
            public string DishesName { get; set; }
            /// <summary>
            /// 已订数量
            /// </summary>
            public int BookedTotal { get; set; } = 0;
        }
    }





}
