using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 获取下属科室 请求
    /// </summary>
    public class GetOfficesByParentRequestDto : BaseDto
    {
        /// <summary>
        /// 医院guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "医院Guid")]
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 上级科室Guid
        /// </summary>
        public string ParentOfficeGuid { get; set; }
    }
}
