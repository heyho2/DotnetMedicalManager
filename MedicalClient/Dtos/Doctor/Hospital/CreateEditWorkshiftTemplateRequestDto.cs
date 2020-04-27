using GD.Common.Base;
using GD.Dtos.Enum.HospitalScheduleEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Doctor.Hospital
{
    /// <summary>
    /// 创建或编辑班次模板
    /// </summary>
    public class CreateEditWorkshiftTemplateRequestDto : BaseDto
    {
        /// <summary>
        /// 班次模板guid
        /// </summary>
        public string TemplateGuid { get; set; }

        /// <summary>
        /// 班次模板名称
        /// </summary>
        [Required(ErrorMessage = "模板名称必填")]
        [StringLength(10, ErrorMessage = "模板名称最长为10个字")]
        public string TemplateName { get; set; }

        /// <summary>
        /// 班次详情
        /// </summary>
        [Required(ErrorMessage = "班次详情必填")]
        public List<WorkShift> Details { get; set; }


        /// <summary>
        /// 班次
        /// </summary>
        public class WorkShift : BaseDto
        {
            /// <summary>
            /// 班次类别
            /// </summary>
            [Required(ErrorMessage = "班次类别必填")]
            [Range(1, 2)]
            public WorkshiftTypeEnum WorkshiftType { get; set; }

            /// <summary>
            /// 时段开始时间
            /// </summary>
            [Required(ErrorMessage = "时段开始时间必填")]
            public TimeSpan StartTime { get; set; }

            /// <summary>
            /// 时段结束时间
            /// </summary>
            [Required(ErrorMessage = "时段结束时间必填")]
            public TimeSpan EndTime { get; set; }

            /// <summary>
            /// 号源数量
            /// </summary>
            [Required(ErrorMessage = "号源数量必填")]
            [Range(1, int.MaxValue)]
            public int AppointmentLimit { get; set; }
        }
    }
}
