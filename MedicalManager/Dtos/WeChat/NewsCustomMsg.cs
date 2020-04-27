using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.WeChat
{
    /// <summary>
    /// 客服图文消息
    /// </summary>
    public class NewsCustomMsg : CustomSendMsgBase
    {
        /// <summary>
        /// 构造
        /// </summary>
        public NewsCustomMsg()
        {
            MsgType = "news";
        }

        /// <summary>
        /// 消息列表
        /// </summary>
        public NewsMsg News { get; set; }

        /// <summary>
        /// 客服图文消息列表
        /// </summary>
        public class NewsMsg
        {
            /// <summary>
            /// 详情集合
            /// </summary>
            public List<NewsArticle> Articles { get; set; }
        }
        /// <summary>
        /// 客服图文消息详情
        /// </summary>
        public class NewsArticle
        {
            /// <summary>
            /// 标题
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// 描述
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// 消息外链
            /// </summary>
            public string Url { get; set; }

            /// <summary>
            /// 消息图片url
            /// </summary>
            public string PicUrl { get; set; }
        }
    }
}
