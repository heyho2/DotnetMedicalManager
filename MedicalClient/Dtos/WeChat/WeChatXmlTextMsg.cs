using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.WeChat
{
    /// <summary>
    /// 微信公众号回传消息xml数据包 text消息类
    /// </summary>
    public class WeChatXmlTextMsg : BaseWeChatXmlMsg
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public string MsgId { get; set; }
    }
}
