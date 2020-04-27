using GD.Common.Base;

namespace GD.Dtos.Meal.MealAdmin
{
    /// <summary>
    /// 餐别列表响应信息
    /// </summary>
    public class GetCategoryListResponseDto : BaseDto
    {
        /// <summary>
        /// 餐别GUID
        /// </summary>
        public string CategoryGuid { get; set; }

        /// <summary>
        /// 餐别名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 用餐开始时间
        /// </summary>
        public string MealStartTime { get; set; }

        /// <summary>
        /// 用餐结束时间
        /// </summary>
        public string MealEndTime { get; set; }

        /// <summary>
        /// 提前多少天
        /// </summary>
        public int CategoryAdvanceDay { get; set; }

        /// <summary>
        /// 可预订时间
        /// </summary>
        public string CategoryScheduleTime { get; set; }
    }
}
