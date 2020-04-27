using GD.Dtos.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 药品使用字典
    /// </summary>
    public class CreateMedicationDictionaryRequestDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称必填")]
        public string Name { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [Required(ErrorMessage = "类型必填")]
        public PrescriptionEnum PrescriptionEnum { get; set; }
    }
    /// <summary>
    /// 删除
    /// </summary>
    public class DeleteMedicationDictionaryRequestDto
    {
        /// <summary>
        /// id
        /// </summary>
        public List<string> MedicationGuids { get; set; } = new List<string>();
    }
    /// <summary>
    /// 编辑
    /// </summary>
    public class UpdateMedicationDictionaryRequestDto
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "Id必填")]
        public string MedicationGuid { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [Required(ErrorMessage = "类型必填")]
        public PrescriptionEnum PrescriptionEnum { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称必填")]
        public string Name { get; set; }
    }
}
