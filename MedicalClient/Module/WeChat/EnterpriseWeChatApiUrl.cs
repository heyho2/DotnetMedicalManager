using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Module.WeChat
{
    public class EnterpriseWeChatApiUrl
    {
        /// <summary>
        /// 发送应用消息
        /// Post请求
        /// </summary>
        public const string SEND_MESSAGE = "https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}";
    }
}
