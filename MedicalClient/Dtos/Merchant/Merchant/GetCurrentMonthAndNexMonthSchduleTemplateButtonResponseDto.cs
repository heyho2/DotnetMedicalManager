using GD.Common.Base;
using System;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 获取当月和下月排班周期数据响应Dto
    /// </summary>
    public class GetCurrentMonthAndNexMonthSchduleTemplateButtonResponseDto : BaseDto
    {
        /// <summary>
        /// 月份
        /// </summary>
        public string MonthText { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime StarDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 是否排班
        /// </summary>
        public bool ScheduleState { get; set; }

        /// <summary>
        /// 排班周期guid
        /// </summary>
        public string ScheduleTemplateGuid { get; set; }

        /// <summary>
        /// 排班周期选择的班次模板名称
        /// </summary>
        public string TemplateName { get; set; }
    }
}
