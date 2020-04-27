using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.WeChat
{
    /// <summary>
    /// 微信模板消息类
    /// </summary>
    public class WeChatTemplateMsg
    {
        /// <summary>
        /// 普通用户openid
        /// </summary>
        public string Touser { get; set; }

        /// <summary>
        /// 消息模板Id
        /// </summary>
        public string Template_Id { get; set; }

        /// <summary>
        /// 模板消息链接（非必须）
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 跳小程序所需数据，不需跳小程序可不用传该数据
        /// </summary>
        public MiniProgramData MiniProgram { get; set; }

        /// <summary>
        /// 消息正文，value为消息内容文本（200字以内），没有固定格式，可用\n换行，color为整段消息内容的字体颜色（目前仅支持整段消息为一种颜色）
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 跳小程序所需数据
        /// </summary>
        public class MiniProgramData
        {
            /// <summary>
            /// 所需跳转到的小程序appid（该小程序appid必须与发模板消息的公众号是绑定关联关系，
            /// 暂不支持小游戏）
            /// </summary>
            public string AppId { get; set; }

            /// <summary>
            /// 所需跳转到小程序的具体页面路径，支持带参数,（示例index?foo=bar），
            /// 要求该小程序已发布，暂不支持小游戏
            /// </summary>
            public string PagePath { get; set; }
        }

        
    }
}
