using GD.Common.Base;
using GD.Dtos.Doctor.Doctor;
using GD.Dtos.Enum;
using GD.Dtos.Enum.DoctorAppointment;
using System;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Doctor.Prescription
{
    public class GetHistoryPrescriptionRecordsRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 预约guid
        /// </summary>
        [Required(ErrorMessage = "预约参数必填")]
        public string AppointmentGuid { get; set; }

        /// <summary>
        /// 用户guid（不用传）
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
        /// 就诊人/诊断/药品/医生/处方编号
        /// </summary>
        public string Keyword { get; set; }
    }

    public class GetHistoryPrescriptionRecordsResponseDto : BasePageResponseDto<GetHistoryPrescriptionRecordsItemDto>
    {

    }

    public class GetHistoryPrescriptionRecordsItemDto : BaseDto
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
        /// 临床诊断
        /// </summary>
        public string ClinicalDiagnosis { get; set; }

        /// <summary>
        /// 就诊类型
        /// </summary>
        public ReceptionTypeEnum ReceptionType { get; set; }
        /// <summary>
        /// 支付状态 无效处方时候禁用调用
        /// </summary>
        public PrescriptionInformationPaidStatus PaidStatus { get; set; }

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
        /// 开方医生
        /// </summary>
        public string DoctorName { get; set; }
    }
}
