using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.WeChat
{
    /// <summary>
    /// 客服文本消息
    /// </summary>
    public class TextCustomMsg : CustomSendMsgBase
    {
        /// <summary>
        /// 文本消息内容
        /// </summary>
        public TextContent Text { get; set; }

        /// <summary>
        /// 文本消息内容类
        /// </summary>
        public class TextContent
        {
            /// <summary>
            /// 文本消息内容
            /// </summary>
            public string Content { get; set; }
        }
    }


}
