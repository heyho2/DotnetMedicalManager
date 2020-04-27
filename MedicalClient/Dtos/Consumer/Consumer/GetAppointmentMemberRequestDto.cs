using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 挂号列表请求Dto
    /// </summary>
    public class GetAppointmentMemberRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 状态
        /// </summary>
        public AppointmentRequestStatusEnum? AppointmentRequestStatus { get; set; }
    }
    public enum AppointmentRequestStatusEnum
    {
        /// <summary>
        /// 待就诊
        /// </summary>
        [Description("待就诊")]
        Waiting = 1,
        /// <summary>
        /// 已就诊
        /// </summary>
        [Description("已就诊")]
        Treated
    }
    /// <summary>
    /// 挂号列表返回Dto
    /// </summary>
    public class GetAppointmentMemberPageListResponseDto : BasePageResponseDto<GetAppointmentMemberItemDto>
    {

    }
    public class GetAppointmentMemberItemDto : BaseDto
    {
        /// <summary>
        /// 挂号guid
        /// </summary>
        public string AppointmentGuid { get; set; }
        /// <summary>
        /// 挂号编号
        /// </summary>
        public string AppointmentNo { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 挂号时间
        /// </summary>
        public DateTime AppointmentTime { get; set; }
        /// <summary>
        /// 挂号结束时间
        /// </summary>
        public DateTime AppointmentDeadline { get; set; }
        /// <summary>
        /// 挂号人
        /// </summary>
        public string PatientName { get; set; }
        /// <summary>
        /// 医院名称
        /// </summary>
        public string HosName { get; set; }
        /// <summary>
        /// 医院地址
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 联系号码
        /// </summary>
        public string ContactNumber { get; set; }
        /// <summary>
        /// 精度
        /// </summary>
        public decimal Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal Latitude { get; set; }
        /// <summary>
        /// 医生名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        public string OfficeName { get; set; }
    }
}
