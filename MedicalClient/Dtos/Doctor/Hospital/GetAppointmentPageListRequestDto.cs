using GD.Common.Base;
using GD.Dtos.Enum.DoctorAppointment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Doctor.Hospital
{


    /// <summary>
    /// 获取医院今日挂号预约列表请求
    /// </summary>
    public class GetAppointmentPageListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 医院guid
        /// </summary>
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 开始时间 产品经理说查询时间段非必填 于 2019-12-32 11:30:00
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束时间 产品经理说查询时间段非必填 于 2019-12-32 11:30:00
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 挂号预约状态
        /// </summary>
        public AppointmentStatusEnum? AppointmentStatus { get; set; }

        /// <summary>
        /// 科室guid
        /// </summary>
        public string OfficeGuid { get; set; }

        /// <summary>
        /// 医生guid
        /// </summary>
        public string DoctorGuid { get; set; }

        /// <summary>
        /// 患者姓名/挂号号码/手机号
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 是否为导出查询
        /// </summary>
        [Required]
        public bool IsExport { get; set; } = false;
    }

    /// <summary>
    /// 获取医院今日挂号预约列表响应
    /// </summary>
    public class GetAppointmentPageListResponseDto : BasePageResponseDto<GetAppointmentPageListItemDto>
    {

    }

    /// <summary>
    /// 获取医院今日挂号预约列表响应详情
    /// </summary>
    public class GetAppointmentPageListItemDto : BaseDto
    {
        /// <summary>
        /// 预约guid
        /// </summary>
        public string AppointmentGuid { get; set; }

        /// <summary>
        /// 预约编号
        /// </summary>
        public string AppointmentNo { get; set; }

        /// <summary>
        /// 患者姓名
        /// </summary>
        public string PatientName { get; set; }

        /// <summary>
        /// 患者性别
        /// </summary>
        public string PatientGender { get; set; }

        /// <summary>
        /// 患者年龄
        /// </summary>
        public int? PatientAge { get; set; }

        /// <summary>
        /// 预约就诊时间
        /// </summary>
        public DateTime AppointmentTime { get; set; }

        /// <summary>
        /// 预约就诊截止时间
        /// </summary>
        public DateTime AppointmentDeadline { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        public string OfficeName { get; set; }

        /// <summary>
        /// 医生名称
        /// </summary>
        public string DoctorName { get; set; }

        /// <summary>
        /// 患者手机号
        /// </summary>
        public string PatientPhone { get; set; }

        /// <summary>
        /// 预约状态
        /// </summary>
        public AppointmentStatusEnum AppointmentStatus { get; set; }
    }
}
