using Newtonsoft.Json;
using System.Collections.Generic;

namespace GD.Dtos.Doctor.Prescription
{
    public class GetPrescriptionResponseDto : BasePatientInfo
    {
        /// <summary>
        /// 处方前记（用户信息Id）
        /// </summary>
        public string InformationGuid { get; set; }
        /// <summary>
        /// 处方数据
        /// </summary>
        public List<GetPrescrtionDetail> Prescriptions { get; set; } =
            new List<GetPrescrtionDetail>();
    }

    public class GetPrescrtionDetail : BasePrescription
    {
        /// <summary>
        /// 处方Guid
        /// </summary>
        public string PrescriptionGuid { get; set; }
        /// <summary>
        /// 作废原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 处方状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        ///处方下药品数据
        /// </summary>
        public List<GetPrescriptionReception> Receptions { get; set; }
    }

    public class GetPrescriptionReception : BasePrescriptionReception
    {
        /// <summary>
        /// 药品Guid
        /// </summary>
        public string RecipeGuid { get; set; }
        [JsonIgnore]
        public string PrescriptionGuid { get; set; }
    }
}
