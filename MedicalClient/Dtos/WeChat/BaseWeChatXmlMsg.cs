using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.WeChat
{
    /// <summary>
    /// 微信消息xml数据包类实体
    /// </summary>
    public class BaseWeChatXmlMsg
    {
        /// <summary>
        /// 发送者标识
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public WeChartMsgType MsgType { get; set; }

        /// <summary>
        /// 消息表示。普通消息时，为msgid，事件消息时，为事件的创建时间
        /// </summary>
        public string MsgFlag { get; set; }

        /// <summary>
        /// 添加到队列的时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
    }

    /// <summary>
    /// 事件类型枚举
    /// </summary>
    public enum WeChartEvent
    {
        /// <summary>
        /// 非事件类型
        /// </summary>
        NOEVENT,
        /// <summary>
        /// 订阅
        /// </summary>
        SUBSCRIBE,
        /// <summary>
        /// 取消订阅
        /// </summary>
        UNSUBSCRIBE,
        /// <summary>
        /// 扫描带参数的二维码
        /// </summary>
        SCAN,
        /// <summary>
        /// 地理位置
        /// </summary>
        LOCATION,
        /// <summary>
        /// 单击按钮
        /// </summary>
        CLICK,
        /// <summary>
        /// 链接按钮
        /// </summary>
        VIEW,
        /// <summary>
        /// 扫码推事件
        /// </summary>
        SCANCODE_PUSH,
        /// <summary>
        /// 扫码推事件且弹出“消息接收中”提示框
        /// </summary>
        SCANCODE_WAITMSG,
        /// <summary>
        /// 弹出系统拍照发图
        /// </summary>
        PIC_SYSPHOTO,
        /// <summary>
        /// 弹出拍照或者相册发图
        /// </summary>
        PIC_PHOTO_OR_ALBUM,
        /// <summary>
        /// 弹出微信相册发图器
        /// </summary>
        PIC_WEIXIN,
        /// <summary>
        /// 弹出地理位置选择器
        /// </summary>
        LOCATION_SELECT,
        /// <summary>
        /// 模板消息推送
        /// </summary>
        TEMPLATESENDJOBFINISH
    }

    /// <summary>
    /// 消息类型枚举
    /// </summary>
    public enum WeChartMsgType
    {
        /// <summary>
        ///文本类型
        /// </summary>
        TEXT,
        /// <summary>
        /// 图片类型
        /// </summary>
        IMAGE,
        /// <summary>
        /// 语音类型
        /// </summary>
        VOICE,
        /// <summary>
        /// 视频类型
        /// </summary>
        VIDEO,
        /// <summary>
        /// 地理位置类型
        /// </summary>
        LOCATION,
        /// <summary>
        /// 链接类型
        /// </summary>
        LINK,
        /// <summary>
        /// 事件类型
        /// </summary>
        EVENT
    }
}
