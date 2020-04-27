using GD.Common.Base;
using GD.Dtos.Doctor.Doctor;
using GD.Dtos.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Prescription
{
    /// <summary>
    /// 获取处方单据数据(打印处方单时的预览数据)
    /// </summary>
    public class GetPrescriptionBillsResponseDto : BaseDto
    {
        /// <summary>
        /// 处方编号
        /// </summary>
        public string PrescriptionNo { get; set; }

        /// <summary>
        /// 处方名称
        /// </summary>
        public string PrescriptionName { get; set; }

        /// <summary>
        /// 处方状态 Obligation-1 未付款 ；Paied-2 已付款 ；Cancellation-3 已作废
        /// </summary>
        public PrescriptionStatusEnum? PrescriptionStatus { get; set; }

        /// <summary>
        /// 开单日期
        /// </summary>
        public DateTime BillingDate { get; set; }

        /// <summary>
        /// 患者姓名
        /// </summary>
        public string PatientName { get; set; }

        /// <summary>
        /// 患者性别
        /// </summary>
        public GenderEnum PatientGender { get; set; }

        /// <summary>
        /// 患者年龄（开单时的年龄）
        /// </summary>
        public int? PatientAge { get; set; }

        /// <summary>
        /// 患者手机号
        /// </summary>
        public string PatientPhone { get; set; }

        /// <summary>
        /// 门诊号码
        /// </summary>
        public string ReceptionNo { get; set; }

        /// <summary>
        /// 医院名称
        /// </summary>
        public string HosName { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        public string OfficeName { get; set; }

        /// <summary>
        /// 临床诊断
        /// </summary>
        public string ClinicalDiagnosis { get; set; }

        /// <summary>
        /// 患者症状
        /// </summary>
        public string PatientSymptoms { get; set; }

        /// <summary>
        /// 患者地址
        /// </summary>
        public string PatientAddress { get; set; }

        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }

        /// <summary>
        /// 处方金额
        /// </summary>
        public decimal? TotalCost { get; set; }

        /// <summary>
        /// 处方项列表
        /// </summary>
        public List<RecipeItemDto> RecipeItems { get; set; }

        /// <summary>
        /// 处方项
        /// </summary>
        public class RecipeItemDto
        {
            /// <summary>
            /// 处方项名称
            /// </summary>
            public string ItemName { get; set; }

            /// <summary>
            /// 处方项规格
            /// </summary>
            public string ItemSpecification { get; set; }

            /// <summary>
            /// 药品用法
            /// </summary>
            public string DrugUsage { get; set; }

            /// <summary>
            /// 药品用量
            /// </summary>
            public string DrugDosage { get; set; }

            /// <summary>
            /// 用药频度数量：例如 “每6小时1次”中的“6”
            /// </summary>
            public decimal? DrugFrequencyQuantity { get; set; }

            /// <summary>
            /// 用药频度单位：例如 “每6小时1次”中的“小时”
            /// </summary>
            public string DrugFrequencyUnit { get; set; }

            /// <summary>
            /// 用药频度次数：例如 “每6小时1次”中的“1”
            /// </summary>
            public decimal? DrugFrequencyTimes { get; set; }

            /// <summary>
            /// 处方项数量
            /// </summary>
            public int? ItemQuantity { get; set; }
        }
    }

    public class GetPrescriptionBillsItemDto : BaseDto
    {
        public string PrescriptionName { get; set; }
        /// <summary>
        /// 处方编号
        /// </summary>
        public string PrescriptionNo { get; set; }

        /// <summary>
        /// 处方状态 Obligation-1 未付款 ；Paied-2 已付款 ；Cancellation-3 已作废
        /// </summary>
        public PrescriptionStatusEnum PrescriptionStatus { get; set; }

        /// <summary>
        /// 开单日期
        /// </summary>
        public DateTime BillingDate { get; set; }

        /// <summary>
        /// 患者姓名
        /// </summary>
        public string PatientName { get; set; }

        /// <summary>
        /// 患者性别
        /// </summary>
        public GenderEnum PatientGender { get; set; }

        /// <summary>
        /// 患者年龄（开单时的年龄）
        /// </summary>
        public int? PatientAge { get; set; }

        /// <summary>
        /// 患者手机号
        /// </summary>
        public string PatientPhone { get; set; }

        /// <summary>
        /// 门诊号码
        /// </summary>
        public string ReceptionNo { get; set; }

        /// <summary>
        /// 医院名称
        /// </summary>
        public string HosName { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        public string OfficeName { get; set; }

        /// <summary>
        /// 临床诊断
        /// </summary>
        public string ClinicalDiagnosis { get; set; }

        /// <summary>
        /// 患者症状
        /// </summary>
        public string PatientSymptoms { get; set; }

        /// <summary>
        /// 患者地址
        /// </summary>
        public string PatientAddress { get; set; }

        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }

        /// <summary>
        /// 处方金额
        /// </summary>
        public decimal? TotalCost { get; set; }

        /// <summary>
        /// 处方项名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 处方项规格
        /// </summary>
        public string ItemSpecification { get; set; }

        /// <summary>
        /// 药品用法
        /// </summary>
        public string DrugUsage { get; set; }

        /// <summary>
        /// 药品用量
        /// </summary>
        public string DrugDosage { get; set; }

        /// <summary>
        /// 用药频度数量：例如 “每6小时1次”中的“6”
        /// </summary>
        public decimal? DrugFrequencyQuantity { get; set; }

        /// <summary>
        /// 用药频度单位：例如 “每6小时1次”中的“小时”
        /// </summary>
        public string DrugFrequencyUnit { get; set; }

        /// <summary>
        /// 用药频度次数：例如 “每6小时1次”中的“1”
        /// </summary>
        public decimal? DrugFrequencyTimes { get; set; }

        /// <summary>
        /// 处方项数量
        /// </summary>
        public int? ItemQuantity { get; set; }
    }
}
