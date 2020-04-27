using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Doctor.Hospital
{
    /// <summary>
    /// 获取医生详细信息 请求
    /// </summary>
    public class GetHospitalInfoRequestDto : BaseDto
    {
        /// <summary>
        /// 医院guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "医院GUID")]
        public string HospitalGuid { get; set; }
    }
}
