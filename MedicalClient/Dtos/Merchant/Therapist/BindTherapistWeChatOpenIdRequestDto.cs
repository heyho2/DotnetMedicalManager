using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Merchant.Therapist
{
    /// <summary>
    /// 服务人员绑定微信OpenId请求Dto
    /// </summary>
    public class BindTherapistWeChatOpenIdRequestDto : BaseDto
    {
        ///<summary>
        ///手机账号
        ///</summary>
        [Required(ErrorMessage = "服务人员手机号必填")]
        public string TherapistPhone { get; set; }

        ///<summary>
        ///密码
        ///</summary>
        [Required(ErrorMessage = "服务人员密码必填")]
        public string TherapistPassword { get; set; }

        /// <summary>
        /// 微信公众号OpenId
        /// </summary>
        [Required(ErrorMessage = "微信公众号OpenId")]
        public string WeChatOpenId { get; set; }
    }
}
