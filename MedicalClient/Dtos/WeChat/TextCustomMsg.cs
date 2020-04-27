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

    public class NewCustomMsg : CustomSendMsgBase
    {
        /// <summary>
        /// 图文消息内容
        /// </summary>
        public NewContent News { get; set; }
        /// <summary>
        /// 图文消息内容类
        /// </summary>
        public class NewContent
        {
            /// <summary>
            /// 
            /// </summary>
            public List<Article> Articles { get; set; }
        }

        public class Article
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Url { get; set; }
            public string PicUrl { get; set; }
        }
    }
}
