using GD.Common.Base;
using System;
using System.Collections.Generic;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 获取商户排班日历响应Dto
    /// </summary>
    public class GetMerchantSchduleCalendarResponseDto : BaseDto
    {
        /// <summary>
        /// 排班日期
        /// </summary>
        public DateTime ScheduleDate { get; set; }

        /// <summary>
        /// 商户排班详情
        /// </summary>
        public List<MerchantSchduleCalendarWorkShiftDto> Details { get; set; }
    }

    /// <summary>
    /// 商户排班日历班次分组数据
    /// </summary>
    public class MerchantSchduleCalendarWorkShiftDto : BaseDto
    {
        /// <summary>
        /// 班次
        /// </summary>
        public string WorkShiftGuid { get; set; }

        /// <summary>
        /// 班次名称
        /// </summary>

        public string WorkShiftName { get; set; }

        /// <summary>
        /// 班次明细时间段
        /// </summary>
        public string WorkShiftTimeDuration { get; set; }

        /// <summary>
        /// 商户排班日历美疗师数据
        /// </summary>
        public List<MerchantSchduleCalendarTherapistDto> Therapists { get; set; }
    }

    /// <summary>
    /// 商户排班日历美疗师数据
    /// </summary>
    public class MerchantSchduleCalendarTherapistDto : BaseDto
    {
        /// <summary>
        /// 服务人员guid
        /// </summary>
        public string TherapistGuid { get; set; }

        /// <summary>
        /// 服务人员姓名
        /// </summary>
        public string TherapistName { get; set; }
    }

    /// <summary>
    /// 商户排班日历原始数据
    /// </summary>
    public class MerchantSchduleCalendarDetailDto : BaseDto
    {
        /// <summary>
        /// 排班日期
        /// </summary>
        public DateTime ScheduleDate { get; set; }

        /// <summary>
        /// 班次
        /// </summary>
        public string WorkShiftGuid { get; set; }

        /// <summary>
        /// 班次名称
        /// </summary>

        public string WorkShiftName { get; set; }

        /// <summary>
        /// 服务人员guid
        /// </summary>

        public string TherapistGuid { get; set; }

        /// <summary>
        /// 服务人员姓名
        /// </summary>

        public string TherapistName { get; set; }

        /// <summary>
        /// 班次明细时间段
        /// </summary>
        public string WorkShiftTimeDuration { get; set; }
    }
    /// <summary>
    /// 商户排班日历 日期班次项
    /// </summary>
    public class MerchantSchduleCalendarDateDto : BaseDto
    {
        /// <summary>
        /// 排班日期
        /// </summary>
        public DateTime ScheduleDate { get; set; }

        /// <summary>
        /// 周期开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 周期结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 班次
        /// </summary>
        public string WorkShiftGuid { get; set; }

        /// <summary>
        /// 班次名称
        /// </summary>

        public string WorkShiftName { get; set; }


        /// <summary>
        /// 班次明细时间段
        /// </summary>
        public string WorkShiftTimeDuration { get; set; }
    }

    /// <summary>
    /// 商户排班日历 班次美疗师项
    /// </summary>
    public class MerchantSchduleCalendarWorkShfitTherapistDto : BaseDto
    {
        /// <summary>
        /// 排班日期
        /// </summary>
        public DateTime ScheduleDate { get; set; }

        /// <summary>
        /// 班次
        /// </summary>
        public string WorkShiftGuid { get; set; }

        /// <summary>
        /// 服务人员uid
        /// </summary>

        public string TherapistGuid { get; set; }

        /// <summary>
        /// 服务人员姓名
        /// </summary>

        public string TherapistName { get; set; }

    }
}
