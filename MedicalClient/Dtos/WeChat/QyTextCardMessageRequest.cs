using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.WeChat
{
    /// <summary>
    /// 企业微信文本卡片消息
    /// 卡片消息的展现形式非常灵活，支持使用br标签或者空格来进行换行处理，也支持使用div标签来使用不同的字体颜色，
    /// 目前内置了3种文字颜色：灰色(gray)、高亮(highlight)、默认黑色(normal)，
    /// 将其作为div标签的class属性即可，具体用法请参考示例。
    /// </summary>
    public class QyTextCardMessageRequest : QyMessageRequestBase
    {
        public QyTextCardMessageRequest()
        {
            MsgType = "textcard";
        }

        /// <summary>
        /// 消息内容
        /// </summary>
        [JsonProperty("textcard")]
        public Content TextCard { get; set; }

        /// <summary>
        /// 表示是否开启id转译，0表示否，1表示是，默认0
        /// </summary>
        public int EnableIdTrans { get; set; } = 0;

        public class Content
        {
            /// <summary>
            /// 标题，不超过128个字节，超过会自动截断（支持id转译）
            /// </summary>
            [JsonProperty("title")]
            public string Title { get; set; }


            /// <summary>
            /// 描述，不超过512个字节，超过会自动截断（支持id转译）
            /// 示例： <div class=\"gray\">2016年9月26日</div> <div class=\"normal\">恭喜你抽中iPhone 7一台，领奖码：xxxx</div><div class=\"highlight\">请于2016年10月10日前联系行政同事领取</div>
            /// </summary>
            [JsonProperty("description")]
            public string Description { get; set; }

            /// <summary>
            /// 点击后跳转的链接。
            /// </summary>
            [JsonProperty("url")]
            public string Url { get; set; } = "#";

            /// <summary>
            /// 按钮文字。 默认为“详情”， 不超过4个文字，超过自动截断。
            /// </summary>
            [JsonProperty("btntxt")]
            public string BtnTxt { get; set; } = "更多";



        }
    }
}
