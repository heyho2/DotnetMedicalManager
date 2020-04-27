using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.WeChat
{
    /// <summary>
    /// 获取小程序码参数
    /// </summary>
    public class WXACodeParam
    {
        /// <summary>
        /// 小程序页面url，可带参数
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 二维码的宽度，单位 px。最小 280px，最大 1280px
        /// </summary>
        public int Width { get; set; } = 430;
    }
}
