﻿using GD.Common.Base;
using GD.Dtos.Enum.HospitalScheduleEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Doctor.Hospital
{
    public class CreateCopyScheduleRequestDto : BaseDto
    {
        /// <summary>
        /// 班次模板guid
        /// </summary>
        [Required(ErrorMessage = "班次模板guid必填")]
        public string TemplateGuid { get; set; }

        /// <summary>
        /// 需要应用到的日期
        /// </summary>
        [Required(ErrorMessage = "日期必填")]
        public List<DateTime> ScheduleDates { get; set; }

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
            /// 医生guid
            /// </summary>
            public string DoctorGuid { get; set; }
        }
    }
}