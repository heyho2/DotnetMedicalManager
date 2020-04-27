using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GD.Dtos.WeChat
{
    public class QyMarkdownMessageRequest : QyMessageRequestBase
    {
        public QyMarkdownMessageRequest()
        {
            MsgType = "markdown";
        }

        /// <summary>
        /// markdown内容
        /// </summary>
        [JsonProperty("markdown")]
        public MarkdownContent Markdown { get; set; }

        public class MarkdownContent
        {
            /// <summary>
            /// 内容
            /// </summary>
            [JsonProperty("content")]
            public string Content { get; set; }
        }
    }
}
