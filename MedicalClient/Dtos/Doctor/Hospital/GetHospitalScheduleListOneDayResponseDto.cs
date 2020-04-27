using GD.Common.Base;
using GD.Dtos.Enum.HospitalScheduleEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Hospital
{
    /// <summary>
    /// 获取医院某天的排班详情（包括未排班的时间段）
    /// </summary>
    public class GetHospitalScheduleListOneDayResponseDto : BaseDto
    {
        /// <summary>
        /// 班次模板guid
        /// </summary>
        public string TemplateGuid { get; set; }

        public DateTime ScheduleDate { get; set; }

        /// <summary>
        /// 明细
        /// </summary>
        public List<ScheduleDetail> Details { get; set; }

        /// <summary>
        /// 排班详情
        /// </summary>
        public class ScheduleDetail : BaseDto
        {
            /// <summary>
            /// 模板班次guid
            /// </summary>
            public string WorkshiftDetailGuid { get; set; }

            /// <summary>
            /// 班别：1 上午 , 2 下午
            /// </summary>
            public WorkshiftTypeEnum WorkshiftType { get; set; }

            /// <summary>
            /// 开始时间
            /// </summary>
            public string StartTime { get; set; }

            /// <summary>
            /// 结束时间
            /// </summary>
            public string EndTime { get; set; }

            /// <summary>
            /// 当前班次时段内是否有预约
            /// </summary>
            public bool HasAppointment { get; set; }

            /// <summary>
            /// 号源量
            /// </summary>
            public int? AppointmentLimit { get; set; }

            /// <summary>
            /// 医生排班信息
            /// </summary>
            public List<DoctorSchedule> Doctors { get; set; }
        }

        /// <summary>
        /// 医生排班信息
        /// </summary>
        public class DoctorSchedule : BaseDto
        {
            /// <summary>
            /// 医生姓名
            /// </summary>
            public string DoctorName { get; set; }

            /// <summary>
            /// 医生guid
            /// </summary>
            public string DoctorGuid { get; set; }

            /// <summary>
            /// 医生排班guid
            /// </summary>
            public string ScheduleGuid { get; set; }
        }
    }

    /// <summary>
    /// 获取周期下的排班详情列表数据dto
    /// </summary>
    public class GetHospitalScheduleListOneDayItem : BaseDto
    {


        /// <summary>
        /// 班次模板guid
        /// </summary>
        public string TemplateGuid { get; set; }

        /// <summary>
        /// 排班日期
        /// </summary>
        public DateTime ScheduleDate { get; set; }

        /// <summary>
        /// 班次模板明细guid
        /// </summary>
        public string WorkshiftDetailGuid { get; set; }

        /// <summary>
        /// 班别：1 上午 , 2 下午
        /// </summary>
        public WorkshiftTypeEnum WorkshiftType { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 号源量
        /// </summary>
        public int AppointmentLimit { get; set; }

        /// <summary>
        /// 排班明细guid
        /// </summary>
        public string ScheduleGuid { get; set; }

        /// <summary>
        /// 医生guid
        /// </summary>
        public string DoctorGuid { get; set; }

        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }

        /// <summary>
        /// 预约数量
        /// </summary>
        public int? AppointmentCount { get; set; }
    }
}
