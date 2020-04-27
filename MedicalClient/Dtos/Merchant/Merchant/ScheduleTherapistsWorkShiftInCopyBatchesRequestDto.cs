using GD.Common.Base;
using System;
using System.Collections.Generic;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 商户批次安排服务人员（一次提交一个周期，将一天的排班复制到整个周期上）请求Dto
    /// </summary>
    public class ScheduleTherapistsWorkShiftInCopyBatchesRequestDto : BaseDto
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
        /// 服务人员排班详情
        /// </summary>
        public List<TherapistWorkShiftForCopyScheduleDto> TherapistWorkShifts { get; set; }


    }

    /// <summary>
    /// 复制性批次排班——美疗师排班详情
    /// </summary>
    public class TherapistWorkShiftForCopyScheduleDto : BaseDto
    {
        /// <summary>
        /// 班次guid
        /// </summary>
        public string WorkShiftGuid { get; set; }

        /// <summary>
        /// 服务人员guid集合
        /// </summary>
        public List<string> TherapistGuids { get; set; }
    }
}
