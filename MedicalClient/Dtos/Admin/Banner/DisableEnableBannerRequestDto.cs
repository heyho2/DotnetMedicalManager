using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Admin.Hospital
{
    /// <summary>
    /// 修改角色 请求
    /// </summary>
    public class DisableEnableHospitalRequestDto : BaseDto
    {
        /// <summary>
        /// guid
        /// </summary>
        [Required]
        public string Guid { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Enable { get; set; }
    }
}
