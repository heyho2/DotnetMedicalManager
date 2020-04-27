using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Achievement
{
    /// <summary>
    /// 月业绩
    /// </summary>
    public class CreateMonthAchievementDto
    {
        /// <summary>
        /// 年月时间
        /// </summary>
        [Required(ErrorMessage = "时间必填")]
        public DateTime Date { get; set; }
        /// <summary>
        /// 月目标万元
        /// </summary>
        [Required(ErrorMessage = "目标必填")]
        public decimal MonthGoal { get; set; }
    }
    /// <summary>
    /// 日业绩目标
    /// </summary>
    public class CreateDayAchievementDto
    {
        /// <summary>
        /// 年月日时间
        /// </summary>
        [Required(ErrorMessage = "时间必填")]
        public DateTime Date { get; set; }
        /// <summary>
        /// 日目标万元
        /// </summary>
        [Required(ErrorMessage = "目标必填")]
        public decimal DayGoal { get; set; }
        /// <summary>
        /// 日完成万元
        /// </summary>
        [Required(ErrorMessage = "完成目标必填")]
        public decimal DayCompleteGoal { get; set; }
    }
    /// <summary>
    /// 修改月业绩目标数据值
    /// </summary>
    public class UpdateMonthAchievementDto
    {
        /// <summary>
        /// ID
        /// </summary>
        [Required(ErrorMessage = "月目标ID必填")]
        public string GoalGuid { get; set; }
        /// <summary>
        /// 月目标万元
        /// </summary>
        [Required(ErrorMessage = "目标必填")]
        public decimal MonthGoal { get; set; }
    }
    /// <summary>
    /// 修改日业绩数据
    /// </summary>
    public class UpdateDayAchievementDto
    {
        /// <summary>
        /// ID
        /// </summary>
        [Required(ErrorMessage = "日业绩目标ID必填")]
        public string AchievementGuid { get; set; }
        /// <summary>
        /// 今日目标万元
        /// </summary>
        [Required(ErrorMessage = "今日目标必填")]
        public decimal TodayGoal { get; set; }
        /// <summary>
        /// 今日完成目标数据值 
        /// </summary>
        [Required(ErrorMessage = "今日完成目标必填")]
        public decimal TodayCompleteGoal { get; set; }
    }
    /// <summary>
    /// 月目标Dto
    /// </summary>
    public class GetMonthAchievementDto
    {
        /// <summary>
        /// 月目标ID
        /// </summary>
        public string GoalGuid { get; set; }
        /// <summary>
        /// 目标年
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 目标月
        /// </summary>
        public int Month { get; set; }
    }
    /// <summary>
    /// 获取每个月的业绩数据
    /// </summary>
    public class GetAchievementDto
    {
        /// <summary>
        /// 计算后的值
        /// </summary>
        public DataSource DataSource { get; set; }
        /// <summary>
        /// 每天数据列表
        /// </summary>
        public List<AchievementDayDto> AchievementDayList { get; set; }
    }
    /// <summary>
    /// 获取每个月的业绩数据对应值DTo
    /// </summary>
    public class DataSource
    {
        /// <summary>
        /// 月目标
        /// </summary>
        public decimal MonthGoal { get; set; }
        /// <summary>
        /// 月目标完成
        /// </summary>
        public decimal TotalComplete { get; set; }
        /// <summary>
        /// 月目标完成率
        /// </summary>
        public decimal MonthScale { get; set; }
    }
    /// <summary>
    /// 每天数据Dto
    /// </summary>
    public class AchievementDayDto
    {
        /// <summary>
        /// 每天目标ID
        /// </summary>
        public string AchievementGuid { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime AchievementDate { get; set; }
        /// <summary>
        /// 今日目标
        /// </summary>
        public decimal TodayGoal { get; set; }
        /// <summary>
        /// 今日完成目标
        /// </summary>
        public decimal TodayComplete { get; set; }
        /// <summary>
        /// 日目标完成率
        /// </summary>
        public decimal DayScale { get; set; }
    }
}
