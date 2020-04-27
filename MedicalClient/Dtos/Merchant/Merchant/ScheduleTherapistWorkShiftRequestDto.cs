using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 安排美疗师班次 请求Dto
    /// </summary>
    public class ScheduleTherapistWorkShiftRequestDto : BaseDto
    {
        /// <summary>
        /// 排班日期
        /// </summary>
        public DateTime ScheduleDate { get; set; }

        /// <summary>
        /// 商铺guid
        /// </summary>
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 班次模板guid
        /// </summary>
        public string TemplateGuid { get; set; }

        /// <summary>
        /// 班次与人员
        /// </summary>
        public List<ScheduleWorkShiftDto> WorkShiftInfo { get; set; }
    }

    /// <summary>
    /// 班次和人员
    /// </summary>
    public class ScheduleWorkShiftDto : BaseDto
    {
        /// <summary>
        /// 班次guid
        /// </summary>
        public string WorkShiftGuid { get; set; }

        /// <summary>
        /// 美疗师guid
        /// </summary>
        public List<string> Therapists { get; set; }
    }

    /// <summary>
    /// 被排班人类型：美疗师/其他
    /// </summary>
    public enum ScheduleTargetType
    {
        /// <summary>
        /// 美疗师
        /// </summary>
        [Description("美疗师")]
        Therapist=1
    }
}
