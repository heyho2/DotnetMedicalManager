using System.Collections.Generic;

namespace GD.Dtos.Doctor.Prescription
{
    public class SubmitPrescriptionSuccessResponseDto
    {
        /// <summary>
        /// 处方记录Id
        /// </summary>
        public string InformationGuid { get; set; }
        /// <summary>
        /// 处方列表
        /// /// </summary>
        public IEnumerable<SubmitPrescriptionSuccessItemDto> PrescriptionSuccess { get; set; }
    }
    /// <summary>
    /// 创建处方
    /// </summary>
    public class SubmitPrescriptionSuccessItemDto
    {
        /// <summary>
        /// 处方Id
        /// </summary>
        public string PrescriptionGuid { get; set; }
        /// <summary>
        /// 处方名称
        /// </summary>
        public string PrescriptionName { get; set; }
    }
}
