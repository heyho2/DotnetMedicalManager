using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.WeChat
{
    /// <summary>
    /// 微信jsapi_ticket 响应Dto
    /// </summary>
    public class WechatJsAPITicketResponse : WeChatResponseDto
    {
        /// <summary>
        /// jsapi_ticket
        /// </summary>
        [JsonProperty("ticket")]
        public string Ticket { get; set; }

        /// <summary>
        /// 超时时间，单位（秒）
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }



    }
}
