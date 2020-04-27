using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.MallPay.ControllerApi
{
    /// <summary>
    /// 获取OpenId 
    /// </summary>
    public class GetOpenIDByCodeRequestDto
    {
        /// <summary>
        /// 静默授权标识
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "静默授权标识")]
        public string Code { get; set; }

    }

    /// <summary>
    /// 获取OpenId 
    /// </summary>
    public class GetOpenIDByCodeResponseDto
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public string ResultCode { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string ResultMsg { get; set; }

        /// <summary>
        /// 微信openid
        /// </summary>
        public string Open_Id { get; set; }
    }

}
