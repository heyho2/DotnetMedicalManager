using GD.Common.Base;
using System;
using System.Collections.Generic;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 服务人员批量排班请求Dto
    /// </summary>
    public class ScheduleTherapistsWorkShiftInBatchesResquestDto : BaseDto
    {
        /// <summary>
        /// 班次模板guid
        /// </summary>
        public string TemplateGuid { get; set; }


        /// <summary>
        /// 排班起始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 排班结束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 服务人员批次排班详情
        /// </summary>
        public List<TherapistWorkShiftForScheduleDto> TherapistWorkShifts { get; set; }


    }
    /// <summary>
    /// 批次排班——服务人员排班详情
    /// </summary>
    public class TherapistWorkShiftForScheduleDto : BaseDto
    {
        /// <summary>
        /// 服务人员guid
        /// </summary>
        public string TherapistGuid { get; set; }

        /// <summary>
        /// 排班日期
        /// </summary>
        public DateTime ScheduleDate { get; set; }

        /// <summary>
        /// 班次guid
        /// </summary>
        public string WorkShiftGuid { get; set; }
    }
}
