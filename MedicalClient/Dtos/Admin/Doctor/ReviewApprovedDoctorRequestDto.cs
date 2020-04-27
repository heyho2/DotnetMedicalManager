using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Admin.Doctor
{
    /// <summary>
    /// 审核驳回
    /// </summary>
    public class ReviewApprovedDoctorRequestDto : BaseDto
    {
        /// <summary>
        /// 归属GUID
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "归属GUID")]
        public string OwnerGuid { get; set; }
    }
}
