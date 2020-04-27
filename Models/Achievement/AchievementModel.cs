
using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Achievement
{
    /// <summary>
    /// 
    /// </summary>
    [Table("t_achievement")]
    public class AchievementModel : BaseModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Column("achievement_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "")]
        public string AchievementGuid { get; set; }

        /// <summary>
        /// 月目标GUID
        /// </summary>
        [Column("goal_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "月目标GUID")]
        public string GoalGuid { get; set; }

        /// <summary>
        /// 业绩日期
        /// </summary>
        [Column("achievement_date"), Required(ErrorMessage = "{0}必填"), Display(Name = "业绩日期")]
        public DateTime AchievementDate { get; set; }

        /// <summary>
        /// 今日目标万元
        /// </summary>
        [Column("today_goal"), Required(ErrorMessage = "{0}必填"), Display(Name = "今日目标万元")]
        public decimal TodayGoal { get; set; }

        /// <summary>
        /// 今日完成目标万元
        /// </summary>
        [Column("today_complete"), Required(ErrorMessage = "{0}必填"), Display(Name = "今日完成目标万元")]
        public decimal TodayComplete { get; set; }

    }
}



