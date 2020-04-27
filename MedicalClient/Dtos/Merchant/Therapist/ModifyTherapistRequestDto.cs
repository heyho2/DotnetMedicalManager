using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Merchant.Therapist
{
    /// <summary>
    /// 编辑服务人员资料 - 请求Dto
    /// </summary>
    public class ModifyTherapistRequestDto
    {
        ///<summary>
        ///服务人员GUID
        ///</summary>
        [Required(ErrorMessage = "{0}需提供"), Display(Name = "服务人员标识")]
        public string TherapistGuid { get; set; }

        ///<summary>
        ///服务人员姓名
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "服务人员姓名")]
        public string TherapistName { get; set; }

        /// <summary>
        /// 所属大类
        /// </summary>
        public string[] ClassifyGuids { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "职称")]
        [MaxLength(100, ErrorMessage = "超过职称最大长度限制")]
        public string JobTitle { get; set; }

        ///<summary>
        ///头像Guid
        ///</summary>
        [Display(Name = "头像Guid")]
        public string PortraitGuid { get; set; }
        ///<summary>
        ///手机账号
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "手机账号")]
        public string TherapistPhone { get; set; }

        ///<summary>
        ///擅长的标签
        ///</summary>
        public string[] Tag { get; set; }

        ///<summary>
        ///个人介绍
        ///</summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 项目Guid列表
        ///</summary>
        [Display(Name = "项目Guid列表")]
        public string[] MerchantProjectGuidList { get; set; }

    }

    /// <summary>
    /// 响应Dto
    /// </summary>
    public class ModifyTherapistResponseDto
    {
    }
}
