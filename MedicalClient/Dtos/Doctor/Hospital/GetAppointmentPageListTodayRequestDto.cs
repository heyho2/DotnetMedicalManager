using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;
using GD.Dtos.Enum.DoctorAppointment;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Doctor.Hospital
{
    /// <summary>
    /// 获取医院今日挂号预约列表请求
    /// </summary>
    public class GetAppointmentPageListTodayRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 挂号预约状态
        /// </summary>
        public AppointmentStatusEnum? AppointmentStatus { get; set; }

        /// <summary>
        /// 医生guid
        /// </summary>
        public string DoctorGuid { get; set; }

        /// <summary>
        /// 患者姓名/挂号号码/手机号
        /// </summary>
        public string Keyword { get; set; }
    }

    /// <summary>
    /// 获取医院今日挂号预约列表响应
    /// </summary>
    public class GetAppointmentPageListTodayResponseDto : BasePageResponseDto<GetAppointmentPageListTodayItemDto>
    {

    }

    /// <summary>
    /// 获取医院今日挂号预约列表响应详情
    /// </summary>
    public class GetAppointmentPageListTodayItemDto : BaseDto
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
        /// 挂号账号guid
        /// </summary>
        public string UserGuid { get; set; }

        /// <summary>
        /// 患者年龄
        /// </summary>
        public int? PatientAge { get; set; }

        /// <summary>
        /// 预约就诊时间
        /// </summary>
        public DateTime AppointmentTime { get; set; }

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
        /// <summary>
        /// 付款状态（0：确认收款，1：已收款）【注意：若为空将收款状态按钮隐藏】
        /// </summary>
        public bool? PaidStatus { get; set; }

        /// <summary>
        /// 待付款金额
        /// </summary>
        public decimal ObligationAmount { get; set; }
    }
}
