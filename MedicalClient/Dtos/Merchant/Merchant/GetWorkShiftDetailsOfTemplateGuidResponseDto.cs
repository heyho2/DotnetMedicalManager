using GD.Common.Base;
using System.Collections.Generic;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 获取班次模板的班次详情
    /// </summary>
    public class GetWorkShiftDetailsOfTemplateGuidResponseDto : BaseDto
    {
        /// <summary>
        /// 班次guid
        /// </summary>
        public string WorkShiftGuid { get; set; }

        /// <summary>
        /// 班次guid
        /// </summary>
        public string WorkShiftName { get; set; }

        /// <summary>
        /// 班次明细时间段
        /// </summary>
        public List<TimeDto> WorkShiftDetailTimes { get; set; }
    }

    /// <summary>
    /// 获取班次模板的班次详情原始数据
    /// </summary>
    public class WorkShiftDetailsOfTemplateGuidSourceDto : TimeDto
    {
        /// <summary>
        /// 班次guid
        /// </summary>
        public string WorkShiftGuid { get; set; }

        /// <summary>
        /// 班次guid
        /// </summary>
        public string WorkShiftName { get; set; }

    }
}
