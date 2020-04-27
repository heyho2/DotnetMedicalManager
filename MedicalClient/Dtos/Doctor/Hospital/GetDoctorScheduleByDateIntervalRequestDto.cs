using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Doctor.Hospital
{
    /// <summary>
    /// 获取时间段内医生排班信息
    /// </summary>
    public class GetDoctorScheduleByDateIntervalRequestDto : BaseDto
    {
        /// <summary>
        /// 医生guid
        /// </summary>
        [Required]
        public string DoctorGuid { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate { get; set; }
    }

    /// <summary>
    /// 获取时间段内医生排班信息
    /// </summary>
    public class GetDoctorScheduleByDateIntervalResponseDto : BaseDto
    {
        /// <summary>
        /// 排班日期
        /// </summary>
        public DateTime ScheudleDate { get; set; }

        /// <summary>
        /// 排班时间段列表
        /// </summary>
        public List<ScheduleDetail> ScheduleDetails { get; set; }

        public class ScheduleDetail
        {
            /// <summary>
            /// 排班guid
            /// </summary>
            public string ScheduleGuid { get; set; }

            /// <summary>
            /// 时段开始时间
            /// </summary>
            public string StartTime { get; set; }

            /// <summary>
            /// 时段结束时间
            /// </summary>
            public string EndTime { get; set; }

            /// <summary>
            /// 剩余号数量
            /// </summary>
            public int AppointmentLimit { get; set; }

        }


    }
}
