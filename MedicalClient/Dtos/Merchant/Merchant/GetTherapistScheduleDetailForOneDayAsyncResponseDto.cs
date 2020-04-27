using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 获取美疗师某一天的排班详情（时间刻度列表）响应Dto
    /// </summary>
    public class GetTherapistScheduleDetailForOneDayAsyncResponseDto : BaseDto
    {
        /// <summary>
        /// 排班guid
        /// </summary>
        public string ScheduleGuid { get; set; }

        /// <summary>
        /// 排班详情
        /// </summary>
        public List<ScheduleTimeDetailDto> ScheduleDetails { get; set; }
    }
}
