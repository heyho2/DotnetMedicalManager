using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.WeChat
{
    /// <summary>
    /// 微信公众号API操作响应Dto
    /// </summary>
    public class WeChatResponseDto
    {
        /// <summary>
        /// 错误码（0表示成功）
        /// </summary>
        [JsonProperty("errcode")]
        public int Errcode { get; set; } = 0;

        /// <summary>
        /// 错误信息
        /// </summary>
        [JsonProperty("errmsg")]
        public string Errmsg { get; set; }
    }
}
