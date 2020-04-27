using GD.Common.Base;
using GD.Dtos.Enum.HospitalScheduleEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Hospital
{
    /// <summary>
    /// 获取医院模板下的班次详情响应
    /// </summary>
    public class GetWorkShiftDetailsResponseDto : BaseDto
    {
        /// <summary>
        /// 班次详情guid
        /// </summary>
        public string WorkshiftDetailGuid { get; set; }

        /// <summary>
        /// 班次类别
        /// </summary>
        public WorkshiftTypeEnum WorkshiftType { get; set; }

        /// <summary>
        /// 时段开始时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 时段结束时间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 号源数量
        /// </summary>
        public int AppointmentLimit { get; set; }
    }
}
