using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.WeiXinPay
{
    /// <summary>
    /// 微信配置返参Dto
    /// </summary>
    public class WeiXinConfigResponseDto
    {
        /// <summary>
        /// appid
        /// </summary>
        public string AppID { get; set; }
        /// <summary>
        /// 是否需要获取微信Code
        /// </summary>
        public bool IsNeedToGetCode { get; set; } = false;

        /// <summary>
        /// 用户OpenID
        /// </summary>
        public string UserOpenID { get; set; }
    }
}