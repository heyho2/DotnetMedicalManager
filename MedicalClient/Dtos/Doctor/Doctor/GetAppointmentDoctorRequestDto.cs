using GD.Common.Base;
using GD.Dtos.Enum.HospitalScheduleEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 获取指定门诊下医生列表Dto
    /// </summary>
    public class GetAppointmentDoctorRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 医院(诊所)id
        /// </summary>
        [Required(ErrorMessage = "医院Id为必填值")]
        public string HspitalGuid { get; set; }
        /// <summary>
        /// 科室Guid
        /// </summary>
        public string OfficeGuid { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        public DateTime? AppointmentDate { get; set; }
    }
    /// <summary>
    /// 响应Dto
    /// </summary>
    public class GetAppointmentDoctorPageListResponseDto : BasePageResponseDto<GetAppointmentDoctorItemDto>
    {

    }
    public class GetAppointmentDoctorItemDto : BaseDto
    {
        /// <summary>
        /// 医院名称
        /// </summary>
        public string HosName { get; set; }
        /// <summary>
        /// 医生Id
        /// </summary>
        public string DoctorGuid { get; set; }
        /// <summary>
        /// 医生名
        /// </summary>
        public string DoctorName { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string Picture { get; set; }
        /// <summary>
        /// 医生职称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 医生所属科室
        /// </summary>
        public string OfficeName { get; set; }
        /// <summary>
        /// 擅长
        /// </summary>
        public string AdeptTags { get; set; }
    }
    /// <summary>
    /// 医院科室Dto
    /// </summary>
    public class HospitalDepartmentsResponse
    {
        /// <summary>
        /// 科室id
        /// </summary>
        public string OfficeGuid { get; set; }
        /// <summary>
        /// 科室名
        /// </summary>
        public string OfficeName { get; set; }
        /// <summary>
        /// 是否去过
        /// </summary>
        public bool IsBeen { get; set; }
    }
    /// <summary>
    /// 最近一次预约
    /// </summary>
    public class LastAppointmentResponse
    {
        /// <summary>
        /// 医院名称
        /// </summary>
        public string HosName { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 医生Id
        /// </summary>
        public string DoctorGuid { get; set; }
        /// <summary>
        /// 医生名
        /// </summary>
        public string DoctorName { get; set; }
        /// <summary>
        /// 医生职称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 医生所属科室
        /// </summary>
        public string OfficeName { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal? Latitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal? Longitude { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        public DateTime AppointmentTime { get; set; }
    }
    /// <summary>
    /// 医院列表请求Dto
    /// </summary>
    public class HospitalPageListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 医院名称关键字搜索
        /// </summary>
        public string KeyWord { get; set; }
    }

    /// <summary>
    /// 医院列表Dto
    /// </summary>
    public class HospitalPageListResponseDto : BasePageResponseDto<HospitalItemDto>
    {

    }

    public class HospitalItemDto : BaseDto
    {
        /// <summary>
        /// 医院Id
        /// </summary>
        public string HospitalGuid { get; set; }
        /// <summary>
        /// 是否是医院 true:是
        /// </summary>
        public bool IsHospital { get; set; }
        /// <summary>
        /// 医院名称
        /// </summary>
        public string HosName { get; set; }
        ///<summary>
        ///医院logo url
        ///</summary>
        public string LogoUrl { get; set; }
        ///<summary>
        /// 医院标签
        ///</summary>
        public string HosTag { get; set; }
        /// <summary>
        /// 医院地址
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 联系号码
        /// </summary>
        public string ContactNumber { get; set; }
        /// <summary>
        /// 跳转URL
        /// </summary>
        public string ExternalLink { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal? Latitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal? Longitude { get; set; }
    }
    /// <summary>
    /// 获取医生预约状态
    /// </summary>
    public class GetAppointmentDoctorInfoListResponse
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime ScheduleDate { get; set; }
        /// <summary>
        /// 排班集合数据
        /// </summary>
        public List<GetAppointmentDoctorInfo> List { get; set; }
    }
    public class GetAppointmentDoctorInfo
    {
        /// <summary>
        /// 类型 AM:上午 PM:下午
        /// </summary>
        public string WorkshiftType { get; set; }
        /// <summary>
        /// 可预约数量 等于0相当于约满
        /// </summary>
        public int? AppointmentCount { get; set; }
    }
    public class GetAppointmentDoctorInfoResponse
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime ScheduleDate { get; set; }
        /// <summary>
        /// 类型 AM:上午 PM:下午
        /// </summary>
        public string WorkshiftType { get; set; }
        /// <summary>
        /// 可预约数量 等于0相当于约满
        /// </summary>
        public int AppointmentCount { get; set; }
    }
    /// <summary>
    /// 获取详情Dto
    /// </summary>
    public class GetAppointmentDoctorScheduleDetailRequest
    {
        /// <summary>
        /// 医生Guid
        /// </summary>
        [Required(ErrorMessage = "医生Id为必填值")]
        public string DoctorGuid { get; set; }
        /// <summary>
        /// 排班时间
        /// </summary>
        [Required(ErrorMessage = "排班时间不能为空")]
        public DateTime ScheduleDate { get; set; }
        /// <summary>
        /// 排版类型
        /// </summary>
        [Required(ErrorMessage = "排班类型不能为空")]
        public WorkshiftTypeEnum WorkshiftType { get; set; }
    }
    /// <summary>
    /// 医生排班Dto
    /// </summary>
    public class GetAppointmentDoctorScheduleDetailResponse
    {
        /// <summary>
        /// 医生排班Id
        /// </summary>
        public string ScheduleGuid { get; set; }
        /// <summary>
        /// 排班详情Id
        /// </summary>
        public string WorkshiftDetailGuid { get; set; }
        /// <summary>
        /// 排班日期
        /// </summary>
        public DateTime ScheduleDate { get; set; }
        /// <summary>
        /// 放出的挂号最大数量
        /// </summary>
        public int AppointmentLimit { get; set; }
        /// <summary>
        /// 当前已挂号数量
        /// </summary>
        public int AppointmentQuantity { get; set; }
        /// <summary>
        /// 班次类别 AM：上午 PM：下午
        /// </summary>
        public string WorkshiftType { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 是否超时 true:超时
        /// </summary>
        public bool IsOvertime { get; set; }
    }
    /// <summary>
    /// 医生咨询推荐
    /// </summary>
    public class DoctorRecommendResponseDto
    {
        /// <summary>
        /// 医院名称
        /// </summary>
        public string HosName { get; set; }
        /// <summary>
        /// 医生Guid
        /// </summary>
        public string DoctorGuid { get; set; }
        /// <summary>
        /// 医生名
        /// </summary>
        public string DoctorName { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string Picture { get; set; }
        /// <summary>
        /// 医生职称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 医生所属科室
        /// </summary>
        public string OfficeName { get; set; }
        /// <summary>
        /// 擅长
        /// </summary>
        public string AdeptTags { get; set; }
    }
    /// <summary>
    /// 用户新增预约Dto
    /// </summary>
    public class AddAppointmentRequestDto
    {
        /// <summary>
        /// 医生guid
        /// </summary>
        [Required(ErrorMessage = "医生guid必填")]
        public string DoctorGuid { get; set; }

        /// <summary>
        /// 医生排班guid
        /// </summary>
        [Required(ErrorMessage = "医生排班guid必填")]
        public string ScheduleGuid { get; set; }
        /// <summary>
        /// 就诊人Id
        /// </summary>
        [Required(ErrorMessage = "就诊人guid必填")]
        public string PatientGuid { get; set; }
        /// <summary>
        /// 挂号手机号
        /// </summary>
        [Required(ErrorMessage = "挂号手机号必填")]
        public string Phone { get; set; }
    }
    /// <summary>
    /// 预约成功挂号返回Dto
    /// </summary>
    public class AppointmentResponseDto
    {
        /// <summary>
        /// 预约号
        /// </summary>
        public string AppointmentNo { get; set; }
        /// <summary>
        /// 医院名称
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 医生名称
        /// </summary>
        public string DoctorName { get; set; }
        /// <summary>
        /// 科室
        /// </summary>
        public string OfficeName { get; set; }
        /// <summary>
        /// 开始预约时间
        /// </summary>
        public DateTime AppointmentTime { get; set; }
        /// <summary>
        /// 预约结束时间
        /// </summary>
        public DateTime AppointmentDeadline { get; set; }
    }
}
