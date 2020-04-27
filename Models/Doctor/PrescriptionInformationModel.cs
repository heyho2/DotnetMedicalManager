
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Doctor
{
    /// <summary>
    /// 处方前记表
    /// </summary>
    [Table("t_doctor_prescription_information")]
    public class PrescriptionInformationModel : BaseModel
    {
        /// <summary>
        /// 处方前记guid
        /// </summary>
        [Column("information_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "处方前记guid")]
        public string InformationGuid { get; set; }

        /// <summary>
        /// 挂号guid
        /// </summary>
        [Column("appointment_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "挂号guid")]
        public string AppointmentGuid { get; set; }

        /// <summary>
        /// 医院guid
        /// </summary>
        [Column("hospital_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "医院guid")]
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 就诊人姓名
        /// </summary>
        [Column("patient_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "就诊人姓名")]
        public string PatientName { get; set; }

        /// <summary>
        /// 就诊人电话
        /// </summary>
        [Column("patient_phone"), Required(ErrorMessage = "{0}必填"), Display(Name = "就诊人电话")]
        public string PatientPhone { get; set; }

        /// <summary>
        /// 就诊人性别（M/F），默认为M
        /// </summary>
        [Column("patient_gender")]
        public string PatientGender { get; set; }

        /// <summary>
        /// 就诊人年纪
        /// </summary>
        [Column("patient_age")]
        public int PatientAge { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        [Column("patient_province"), Required(ErrorMessage = "{0}必填"), Display(Name = "省")]
        public string PatientProvince { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        [Column("patient_city"), Required(ErrorMessage = "{0}必填"), Display(Name = "市")]
        public string PatientCity { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        [Column("patient_district"), Required(ErrorMessage = "{0}必填"), Display(Name = "区")]
        public string PatientDistrict { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Column("patient_address"), Required(ErrorMessage = "{0}必填"), Display(Name = "地址")]
        public string PatientAddress { get; set; }

        /// <summary>
        /// 就诊科室
        /// </summary>
        [Column("office_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "就诊科室")]
        public string OfficeGuid { get; set; }

        /// <summary>
        /// 就诊医生
        /// </summary>
        [Column("doctor_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "就诊医生")]
        public string DoctorGuid { get; set; }

        /// <summary>
        /// 预约就诊时间
        /// </summary>
        [Column("appointment_time"), Required(ErrorMessage = "{0}必填"), Display(Name = "预约就诊时间")]
        public DateTime AppointmentTime { get; set; }

        /// <summary>
        /// 临床诊断
        /// </summary>
        [Column("clinical_diagnosis"), Required(ErrorMessage = "{0}必填"), Display(Name = "临床诊断")]
        public string ClinicalDiagnosis { get; set; }

        /// <summary>
        /// 患者症状
        /// </summary>
        [Column("patient_symptoms")]
        public string PatientSymptoms { get; set; }

        /// <summary>
        /// 接诊类型 First:初诊，Repeat:复诊
        /// </summary>
        [Column("reception_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "接诊类型 First:初诊，Repeat:复诊")]
        public string ReceptionType { get; set; }

        /// <summary>
        /// 总费用
        /// </summary>
        [Column("total_cost"), Required(ErrorMessage = "{0}必填"), Display(Name = "总费用")]
        public decimal TotalCost { get; set; }

        /// <summary>
        /// 是否有过敏史
        /// </summary>
        [Column("has_allergy")]
        public bool HasAllergy { get; set; }

        /// <summary>
        /// 是否有慢性病
        /// </summary>
        [Column("has_chronic_disease")]
        public bool HasChronicDisease { get; set; }

        /// <summary>
        /// 门诊编号
        /// </summary>
        [Column("reception_no"), Required(ErrorMessage = "{0}必填"), Display(Name = "门诊编号")]
        public string ReceptionNo { get; set; }

        /// <summary>
        /// 收款状态
        /// </summary>
        [Column("paid_status")]
        public string PaidStatus { get; set; }
    }
}



