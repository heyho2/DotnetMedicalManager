using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.WeChat
{
    /// <summary>
    /// 微信公众号回传消息xml数据包 推事件基类
    /// </summary>
    public class BaseWeChatEventXmlMsg : BaseWeChatXmlMsg
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public WeChartEvent Event { get; set; }

    }
}
