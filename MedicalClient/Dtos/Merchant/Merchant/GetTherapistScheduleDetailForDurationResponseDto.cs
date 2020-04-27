using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 获取美疗师连续天数的排班明细数据响应Dto
    /// </summary>
    public class GetTherapistScheduleDetailForDurationResponseDto : BaseDto
    {
        /// <summary>
        /// 排班日期
        /// </summary>
        public DateTime ScheduleDate { get; set; }

        /// <summary>
        /// 指定日期的排班明细
        /// </summary>
        public List<ScheduleTimeDetailDto> ScheduleTimeDetails { get;set;}
    }
}
