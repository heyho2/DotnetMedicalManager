using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Merchant.Therapist
{
    /// <summary>
    /// 新增服务人员 - 请求Dto
    /// </summary>
    public class AddNewTherapistRequestDto
    {
        ///<summary>
        ///服务人员姓名
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "服务人员姓名")]
        [MaxLength(30, ErrorMessage = "超过服务人员姓名最大长度限制")]
        public string TherapistName { get; set; }

        ///<summary>
        ///头像Guid
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "头像Guid")]
        public string PortraitGuid { get; set; }

        /// <summary>
        /// 所属大类Guid
        /// </summary>
        public string[] ClassifyGuids { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "职称")]
        [MaxLength(100, ErrorMessage = "超过职称最大长度限制")]
        public string JobTitle { get; set; }

        ///<summary>
        ///手机账号
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "手机号")]
        public string TherapistPhone { get; set; }

        ///<summary>
        ///密码
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "密码")]
        public string TherapistPassword { get; set; }

        /// <summary>
        /// 项目Guid列表
        ///</summary>
        [Display(Name = "项目Guid列表")]
        public string[] MerchantProjectGuidList { get; set; }

        /// <summary>
        /// 擅长标签
        /// </summary>
        public string[] Tag { get; set; }

        /// <summary>
        /// 个人介绍
        /// </summary>
        public string Introduction { get; set; }

    }

    /// <summary>
    /// 响应Dto
    /// </summary>
    public class AddNewTherapistResponseDto
    {
    }
}
