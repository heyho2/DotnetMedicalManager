using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Doctor.Prescription
{
    public class CancellPrescriptionRequestDto
    {
        /// <summary>
        /// 处方Guid
        /// </summary>
        [Required(ErrorMessage = "指定处方参数需提供")]
        public string PrescriptionGuid { get; set; }
        /// <summary>
        /// 作废原因
        /// </summary>
        [Required(ErrorMessage = "作废原因必填"), MaxLength(1000, ErrorMessage = "作废原因超出最大长度限制")]
        public string Reason { get; set; }
    }
}
