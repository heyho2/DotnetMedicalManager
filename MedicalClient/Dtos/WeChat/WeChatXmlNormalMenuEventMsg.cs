using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.WeChat
{
    /// <summary>
    /// 普通菜单事件，包括click和view
    /// </summary>
    public class WeChatXmlNormalMenuEventMsg: BaseWeChatEventXmlMsg
    {
        /// <summary>
        /// 事件KEY值/设置的跳转URL
        /// </summary>
        public string EventKey { get; set; }
    }
}
