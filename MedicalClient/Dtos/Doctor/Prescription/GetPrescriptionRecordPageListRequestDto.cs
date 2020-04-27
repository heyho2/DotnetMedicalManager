using GD.Common.Base;
using GD.Dtos.Doctor.Doctor;
using GD.Dtos.Enum;
using GD.Dtos.Enum.DoctorAppointment;
using System;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Doctor.Prescription
{
    /// <summary>
    /// 获取处方记录分页列表请求dto
    /// </summary>
    public class GetPrescriptionRecordPageListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 医院guid
        /// </summary>
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 科室guid
        /// </summary>
        public string OfficeGuid { get; set; }

        /// <summary>
        /// 医生guid
        /// </summary>
        public string DoctorGuid { get; set; }

        /// <summary>
        /// 用户guid
        /// </summary>
        public string UserGuid { get; set; }

        /// <summary>
        /// 起始日期
        /// </summary>
        [Required(ErrorMessage = "起始日期必填")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        [Required(ErrorMessage = "结束日期必填")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 就诊人/手机号/诊断/药品/处方编号
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 收款状态：null 表示全部；1 未收款； 2 部分未收款； 3 已收款； 4 无有效处方
        /// </summary>
        public PrescriptionInformationPaidStatus? PaidStatus { get; set; }

        /// <summary>
        /// 接诊类型： null 表示全部； 1或First 初诊； 2或Repeat 复诊  ——传数字或字符串均可
        /// </summary>
        public ReceptionTypeEnum? ReceptionType { get; set; }

        /// <summary>
        /// 是否是导出，默认为否
        /// </summary>
        public bool IsExport { get; set; } = false;
    }

    public class GetPrescriptionRecordPageListResponseDto : BasePageResponseDto<GetPrescriptionRecordPageListItemDto>
    {

    }

    public class GetPrescriptionRecordPageListItemDto : BaseDto
    {
        /// <summary>
        /// 处方记录guid
        /// </summary>
        public string InformationGuid { get; set; }

        /// <summary>
        /// 预约挂号guid
        /// </summary>
        public string AppointmentGuid { get; set; }

        /// <summary>
        /// 问诊人与当前用户关系 Own:自己，Relatives:亲属，Friend:朋友,Other:其它
        /// </summary>
        public InquiryRelationshipEnum? PatientRelationship { get; set; }

        /// <summary>
        /// 患者姓名
        /// </summary>
        public string PatientName { get; set; }

        /// <summary>
        /// 患者性别
        /// </summary>
        public GenderEnum PatientGender { get; set; }

        /// <summary>
        /// 患者手机号
        /// </summary>
        public string PatientPhone { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        public string OfficeName { get; set; }

        /// <summary>
        /// 临床诊断
        /// </summary>
        public string ClinicalDiagnosis { get; set; }

        /// <summary>
        /// 就诊类型
        /// </summary>
        public ReceptionTypeEnum ReceptionType { get; set; }

        /// <summary>
        /// 就诊时间
        /// </summary>
        public DateTime ReceptionTime { get; set; }

        /// <summary>
        /// 处方编号集合
        /// </summary>
        public string PrescriptionNos { get; set; }

        /// <summary>
        /// 费用合计
        /// </summary>
        public decimal? TotalCost { get; set; }

        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 待付款金额
        /// </summary>
        public decimal ObligationAmount { get; set; }
    }
}
