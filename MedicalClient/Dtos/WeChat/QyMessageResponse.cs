using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.WeChat
{
    /// <summary>
    /// 企业微信发送应用消息响应
    /// 如果部分接收人无权限或不存在，发送仍然执行，但会返回无效的部分（即invaliduser或invalidparty或invalidtag），常见的原因是接收人不在应用的可见范围内。
    /// 如果全部接收人无权限或不存在，则本次调用返回失败，errcode为81013。
    /// 返回包中的userid，不区分大小写，统一转为小写
    /// </summary>
    public class QyMessageResponse : WeChatResponseDto
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("invaliduser")]
        public string InvalidUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("invalidparty")]
        public string InvalidParty { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("invalidtag")]
        public string InvalidTag { get; set; }
    }
}
