using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.WeChat
{
    /// <summary>
    /// 获取JS-SDK权限验证的签名Signature响应
    /// </summary>
    public class WeChatJsSDKSignatureResponse
    {
        /// <summary>
        /// 微信公众号AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 随即字符串
        /// </summary>
        public string NonceStr { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// JsSDK签名
        /// </summary>
        public string Signature { get; set; }
    }
}
