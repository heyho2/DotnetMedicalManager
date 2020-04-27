using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Achievement
{
    /// <summary>
    /// 
    /// </summary>
    [Table("t_achievement_month_goal")]
    public class AchievementMonthGoalModel : BaseModel
    {

        /// <summary>
        /// 月目标GUID
        /// </summary>
        [Column("goal_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "月目标GUID")]
        public string GoalGuid { get; set; }

        /// <summary>
        /// 医院ID
        /// </summary>
        [Column("hospital_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "医院guid")]
        public string HospitalGuid { get; set; }
        /// <summary>
        /// 目标年份
        /// </summary>
        [Column("year"), Required(ErrorMessage = "{0}必填"), Display(Name = "目标年份")]
        public int Year { get; set; }

        /// <summary>
        /// 目标月份
        /// </summary>
        [Column("month"), Required(ErrorMessage = "{0}必填"), Display(Name = "目标月份")]
        public int Month { get; set; }

        /// <summary>
        /// 月目标万元
        /// </summary>
        [Column("month_goal"), Required(ErrorMessage = "{0}必填"), Display(Name = "月目标万元")]
        public decimal MonthGoal { get; set; }
    }
}



