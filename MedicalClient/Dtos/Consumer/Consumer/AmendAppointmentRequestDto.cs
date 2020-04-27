using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 变更预约时间请求dto
    /// </summary>
    public class AmendAppointmentRequestDto : BaseDto
    {
        /// <summary>
        /// 排班guid
        /// </summary>
        [Required(ErrorMessage = "排班guid必填")]
        public string ScheduleGuid { get; set; }

        /// <summary>
        /// 消费预约guid
        /// </summary>
        [Required(ErrorMessage = "消费预约guid必填")]
        public string ConsumptionGuid { get; set; }

        /// <summary>
        /// 预约时间（开始时间）
        /// </summary>
        [Required(ErrorMessage = "预约时间（开始时间）必填")]
        public string StartTime { get; set; }

        /// <summary>
        /// 预约时间（结束时间）
        /// </summary>
        [Required(ErrorMessage = "预约时间（结束时间）必填")]
        public string EndTime { get; set; }
    }

    /// <summary>
    /// 变更预约时间响应dto
    /// </summary>
    public class AmendAppointmentResponseDto : BaseDto
    {
        /// <summary>
        /// 预约的店铺名称
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 商户地址
        /// </summary>
        public string MerchantAddress { get; set; }

        /// <summary>
        /// 预约的美疗师名称
        /// </summary>
        public string TherapistName { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 消费码
        /// </summary>
        public string ConsumptionNo { get; set; }

        /// <summary>
        /// 预约日期
        /// </summary>
        public DateTime AppointmentDate { get; set; }

        /// <summary>
        /// 美疗师头像url
        /// </summary>
        public string ProjectPictureUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ServiceMember { get; set; }
    }
}
