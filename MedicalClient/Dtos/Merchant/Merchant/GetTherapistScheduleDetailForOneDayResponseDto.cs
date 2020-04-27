using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 获取美疗师某一天的排班详情 响应Dto
    /// </summary>
    public class GetTherapistScheduleDetailForOneDayResponseDto : BaseDto
    {
        /// <summary>
        /// 美疗师guid
        /// </summary>
        public string TherapistGuid { get; set; }

        /// <summary>
        /// 排班日期
        /// </summary>
        public DateTime ScheduleDate { get; set; }

        /// <summary>
        /// 排班guid
        /// </summary>
        public string ScheduleGuid { get; set; }

        /// <summary>
        /// 工作班次时间详情
        /// </summary>
        public List<TimeDto> WorkShiftDetails { get; set; }

        /// <summary>
        /// 预约排班时间详情
        /// </summary>
        public List<TimeDto> ScheduleDetails { get; set; }


    }

    /// <summary>
    /// 时间段Dto
    /// </summary>
    public class TimeDto : BaseDto
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
    }
    /// <summary>
    /// 预约排班时间详情
    /// </summary>
    public class ScheduleTimeDetailDto : TimeDto
    {
        /// <summary>
        /// 排班明细guid
        /// </summary>
        public string ScheduleDetailGuid { get; set; }
        /// <summary>
        /// 消费者预约的消费记录Id
        /// </summary>
        public string ConsumptionGuid { get; set; }

        /// <summary>
        /// 当前时间刻度是否被占用
        /// </summary>
        public bool Occupy { get; set; }

    }
}
