using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 获取美疗师某一天的排班详情 请求Dto
    /// </summary>
    public class GetTherapistScheduleDetailForOneDayRequestDto:BaseDto
    {
        /// <summary>
        /// 美疗师guid
        /// </summary>
        public string TherapistGuid { get; set; }

        /// <summary>
        /// 排班日期
        /// </summary>
        public DateTime ScheduleDate { get; set; }
    }
}
