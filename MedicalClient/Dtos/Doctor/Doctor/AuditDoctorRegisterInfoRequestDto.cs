using GD.Common.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 审核医生请求Dto
    /// </summary>
    public class AuditDoctorRegisterInfoRequestDto : BaseDto
    {
        /// <summary>
        /// 医生Id
        /// </summary>
        [Required]
        public string DoctorGuid { get; set; }

        /// <summary>
        /// 审核状态 驳回0；同意1；提交2；草稿3
        /// </summary>
        [Required]
        public StatusEnum Status { get; set; }
    }

    /// <summary>
    /// 审核状态枚举  驳回0；同意1；提交2；草稿3
    /// </summary>
    public enum StatusEnum
    {
        /// <summary>
        /// 驳回
        /// </summary>
        [Description("驳回")]
        Reject,

        /// <summary>
        /// 同意
        /// </summary>
        [Description("同意")]
        Approved,

        /// <summary>
        /// 提交
        /// </summary>
        [Description("提交")]
        Submit,

        /// <summary>
        /// 草稿
        /// </summary>
        [Description("草稿")]
        Draft
    }
}
