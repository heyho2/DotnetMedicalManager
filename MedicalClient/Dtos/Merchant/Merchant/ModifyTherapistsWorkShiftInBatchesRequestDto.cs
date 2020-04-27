using System.Collections.Generic;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 服务人员批量排班请求Dto
    /// </summary>
    public class ModifyTherapistsWorkShiftInBatchesRequestDto
    {
        /// <summary>
        /// 周期排班guid
        /// </summary>
        public string ScheduleTemplateGuid { get; set; }

        /// <summary>
        /// 班次模板
        /// </summary>
        public string TemplateGuid { get; set; }

        /// <summary>
        ///服务人员批次排班详情
        /// </summary>
        public List<TherapistWorkShiftForScheduleDto> TherapistWorkShifts { get; set; }
    }
}
