using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 美疗师锁定排班时间请求dto
    /// </summary>
    public class LockScheduleDetailTimesRequestDto : BaseDto
    {

        /// <summary>
        /// 排班guid
        /// </summary>
        public string ScheduleGuid { get; set; }

        /// <summary>
        /// 锁定时间列表
        /// </summary>
        public List<TimeDto> LockTimes { get; set; }
    }
}
