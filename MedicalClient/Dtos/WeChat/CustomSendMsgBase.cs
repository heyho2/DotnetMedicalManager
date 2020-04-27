using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.WeChat
{
    /// <summary>
    /// 客服消息基类
    /// </summary>
    public class CustomSendMsgBase
    {
        /// <summary>
        /// 普通用户openid
        /// </summary>
        public string Touser { get; set; }

        /// <summary>
        /// 消息类型：
        /// 文本为text，
        /// 图片为image，
        /// 语音为voice，
        /// 视频消息为video，
        /// 音乐消息为music，
        /// 图文消息（点击跳转到外链）为news，
        /// 图文消息（点击跳转到图文消息页面）为mpnews，
        /// 卡券为wxcard，
        /// 小程序为miniprogrampage
        /// </summary>
        public string MsgType { get; set; }
    }
}
