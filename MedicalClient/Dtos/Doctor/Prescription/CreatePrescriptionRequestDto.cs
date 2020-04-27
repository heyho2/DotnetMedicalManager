using GD.Dtos.Doctor.Doctor;
using GD.Dtos.Enum;
using GD.Models.Consumer;
using GD.Models.Doctor;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Doctor.Prescription
{
    public class BasePatientInfo
    {
        /// <summary>
        /// 患者姓名
        /// </summary>
        public string PatientName { get; set; }
        /// <summary>
        /// 患者性别（1：男，2：女）
        /// </summary>
        public GenderEnum PatientGender { get; set; }
        /// <summary>
        /// 患者年纪
        /// </summary>
        public int PatientAge { get; set; }
        /// <summary>
        /// 患者手机号码
        /// </summary>
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required(ErrorMessage = "电话号码必填"), Phone(ErrorMessage = "请输入正确的电话号码")]
        public string PatientPhone { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string PatientProvince { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string PatientCity { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        public string PatientDistrict { get; set; }
        /// <summary>
        /// 患者地址
        /// </summary>
        public string PatientAddress { get; set; }
        /// <summary>
        /// 是否有过敏史
        /// </summary>
        public bool HasAllergy { get; set; }
        /// <summary>
        /// 是否有慢性病
        /// </summary>
        public bool HasChronicDisease { get; set; }
        /// <summary>
        /// 临床诊断
        /// </summary>
        [Required(ErrorMessage = "临床诊断需填写"), MaxLength(500, ErrorMessage = "临床诊断长度超过最大长度限制")]
        public string ClinicalDiagnosis { get; set; }
        /// <summary>
        /// 患者症状
        /// </summary>
        public string PatientSymptoms { get; set; }
        /// <summary>
        /// 1:初诊，2：复诊
        /// </summary>
        public ReceptionTypeEnum ReceptionType { get; set; }
    }

    public class BasePrescription
    {
        /// <summary>
        /// 处方名称
        /// </summary>
        public string PrescriptionName { get; set; }
    }

    public class BasePrescriptionReception
    {
        /// <summary>
        /// 药品类型（1：药品收费，2：治疗收费）
        /// </summary>
        public ReceptionRecipeTypeEnum ItemType { get; set; }
        /// <summary>
        /// 收费项目名称
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 收费项目规格
        /// </summary>
        public string ItemSpecification { get; set; }
        /// <summary>
        /// 收费项目单价
        /// </summary>
        public decimal ItemPrice { get; set; }
        /// <summary>
        /// 收费项目数量
        /// </summary>
        public int ItemQuantity { get; set; }
        /// <summary>
        /// 药品用法
        /// </summary>
        public string DrugUsage { get; set; }
        /// <summary>
        /// 药品用量
        /// </summary>
        public string DrugDosage { get; set; }
        /// <summary>
        /// 用药频度
        /// </summary>
        public string DrugFrequency { get; set; }
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
    }

    public class BasePrecriptionRequestDto : BasePatientInfo
    {
        public List<CreatePrescritionDetail> BasePrescriptions { get; set; } = new List<CreatePrescritionDetail>();
    }

    public class CreatePrescritionDetail : BasePrescription
    {
        public List<BasePrescriptionReception> Receptions { get; set; } = new List<BasePrescriptionReception>();
    }

    public class ProcessPrescriptionRequestDto : BasePrecriptionRequestDto
    {
        /// <summary>
        /// 预约Id
        /// </summary>
        [Required(ErrorMessage = "预约参数需提供")]
        public string AppointmentGuid { get; set; }

        /// <summary>
        /// （注意：更新时需提供此参数）
        /// </summary>
        public string InformationGuid { get; set; }
    }

    public class PrescriptionContext
    {
        public readonly ProcessPrescriptionRequestDto Request;
        public PrescriptionContext(ProcessPrescriptionRequestDto request)
        {
            Request = request;
        }
        public DoctorAppointmentModel AppointmentModel { get; set; }
        public PrescriptionInformationModel InformationModel { get; set; } = new PrescriptionInformationModel();
        public List<PrescriptionModel> PrescriptionModels { get; set; } =
            new List<PrescriptionModel>();
        public List<PrescriptionRecipeModel> RecipeModels { get; set; } = new List<PrescriptionRecipeModel>();


        public List<PrescriptionModel> dbPrescriptionModels { get; set; } =
         new List<PrescriptionModel>();
    }
}
