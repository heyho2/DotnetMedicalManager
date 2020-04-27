
using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Meal
{
    ///<summary>
    ///点餐餐别
    ///</summary>
    [Table("t_meal_category")]
    public class MealCategoryModel : BaseModel
    {

        ///<summary>
        ///GUID
        ///</summary>
        [Column("category_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "GUID")]
        public string CategoryGuid { get; set; }

        ///<summary>
        ///医院GUID
        ///</summary>
        [Column("hospital_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "医院GUID")]
        public string HospitalGuid { get; set; }

        ///<summary>
        ///餐别名称
        ///</summary>
        [Column("category_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "餐别名称")]
        public string CategoryName { get; set; }

        ///<summary>
        ///用餐开始时间
        ///</summary>
        [Column("meal_startTime"), Required(ErrorMessage = "{0}必填"), Display(Name = "用餐开始时间")]
        public string MealStartTime { get; set; }

        ///<summary>
        ///用餐截止时间
        ///</summary>
        [Column("meal_endTime"), Required(ErrorMessage = "{0}必填"), Display(Name = "用餐截止时间")]
        public string MealEndTime { get; set; }

        ///<summary>
        ///提前几天，默认为0
        ///</summary>
        [Column("category_advance_day"), Required(ErrorMessage = "{0}必填"), Display(Name = "提前几天，默认为0")]
        public int CategoryAdvanceDay { get; set; }

        ///<summary>
        ///可预订时间
        ///</summary>
        [Column("category_schedule_time"), Required(ErrorMessage = "{0}必填"), Display(Name = "可预订时间")]
        public string CategoryScheduleTime { get; set; }
    }
}



