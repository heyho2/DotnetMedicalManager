using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Meal.MealAdmin
{
    /// <summary>
    /// 餐别请求
    /// </summary>
    public class AddCategoryRequestDto : BaseDto
    {
        /// <summary>
        /// 餐别GUID
        /// </summary>
        public string CategoryGuid { get; set; }
        /// <summary>
        /// 医院Id
        /// </summary>
        [Required(ErrorMessage = "医院Id参数不正确")]
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 餐别名称
        /// </summary>
        [Required(ErrorMessage = "名称必填")]
        [MaxLength(50, ErrorMessage = "超过长度最大限制")]
        public string CategoryName { get; set; }

        /// <summary>
        /// 用餐开始时间
        /// </summary>
        [Required(ErrorMessage = "用餐开始时间必填")]
        public string MealStartTime { get; set; }

        /// <summary>
        /// 用餐结束时间
        /// </summary>
        [Required(ErrorMessage = "用餐结束时间必填")]
        public string MealEndTime { get; set; }

        /// <summary>
        /// 提前多少天
        /// </summary>
        public int CategoryAdvanceDay { get; set; }

        /// <summary>
        /// 可预订时间
        /// </summary>
        [Required(ErrorMessage = "可预订时间必填")]
        public string CategoryScheduleTime { get; set; }
    }
}
