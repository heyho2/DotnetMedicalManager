using Newtonsoft.Json;

namespace GD.Dtos.WeChat
{
    /// <summary>
    /// 获取公众号API凭证响应Dto
    /// </summary>
    public class WeChatLoginResponse : WeChatResponseDto
    {
        /// <summary>
        /// 获取到的凭证
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
