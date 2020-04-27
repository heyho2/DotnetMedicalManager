using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Merchant.Appointment
{
    /// <summary>
    /// 执行端-查看当天预约班次信息等 请求Dto
    /// </summary>
    public class SelectOneDayMySchduleInfoRequestDto
    {

        /// <summary>
        /// 美疗师Guid
        /// </summary>
        public string TherapistGuid { get; set; }

        /// <summary>
        /// 班别开始日期
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "班别开始日期")]
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 班别结束日期
        /// </summary>
        [Display(Name = "班别结束日期")]
        public DateTime? EndDate { get; set; }
    }

    /// <summary>
    /// 响应Dto
    /// </summary>
    public class SelectOneDayMySchduleInfoResponseDto
    {
        /// <summary>
        /// 班别日期
        /// </summary>
        [Display(Name = "班别日期")]
        public DateTime ScheduleDate { get; set; }

        /// <summary>
        /// 工作班次guid
        /// </summary>
        [Display(Name = "工作班次guid")]
        public string WorkShiftGuid { get; set; }

        /// <summary>
        /// 班次名称
        /// </summary>
        [Display(Name = "班次名称")]
        public string WorkShiftName { get; set; }

        /// <summary>
        /// 美疗师Guid
        /// </summary>
        [Display(Name = "美疗师Guid")]
        public string TherapistGuid { get; set; }

        /// <summary>
        /// 美疗师名称
        /// </summary>
        [Display(Name = "美疗师名称")]
        public string TherapistName { get; set; }
        
        /// <summary>
        /// 时间段
        /// </summary>
        [Display(Name = "时间段")]
        public string WorkShiftTimeDuration { get; set; }

    }
    
}
