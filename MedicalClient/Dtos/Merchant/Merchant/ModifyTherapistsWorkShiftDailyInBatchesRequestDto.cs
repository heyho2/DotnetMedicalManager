using GD.Common.Base;
using System;
using System.Collections.Generic;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 服务人员一日批量排班请求Dto
    /// </summary>
    public class ModifyTherapistsWorkShiftDailyInBatchesRequestDto : BaseDto
    {
        /// <summary>
        /// 周期排班guid
        /// </summary>
        public string ScheduleTemplateGuid { get; set; }

        /// <summary>
        /// 排班日期
        /// </summary>
        public DateTime ScheduleDate { get; set; }

        /// <summary>
        /// 服务人员批次排班详情
        /// </summary>
        public List<TherapistWorkShiftForDailyScheduleDto> TherapistWorkShifts { get; set; }
    }

    /// <summary>
    /// 一日批量排班——服务人员排班详情
    /// </summary>
    public class TherapistWorkShiftForDailyScheduleDto : BaseDto
    {
        /// <summary>
        /// 服务人员guid
        /// </summary>
        public string TherapistGuid { get; set; }

        /// <summary>
        /// 班次guid
        /// </summary>
        public string WorkShiftGuid { get; set; }
    }
}
