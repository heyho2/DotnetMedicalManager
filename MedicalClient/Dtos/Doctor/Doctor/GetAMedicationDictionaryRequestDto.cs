using GD.Common.Base;
using GD.Dtos.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 药品使用字典Dto
    /// </summary>
    public class GetMedicationDictionaryRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 搜索名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用药类型:Usage:用法，Dosage:用量，Frequency:频率
        /// </summary>
        public PrescriptionEnum PrescriptionEnum { get; set; }
    }
    public class GetMedicationDictionaryPageListResponseDto : BasePageResponseDto<GetMedicationDictionaryItemDto>
    {

    }
    public class GetMedicationDictionaryItemDto : BaseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public string MedicationGuid { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}
