using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 检测用户是否已关注智慧云医公众号响应
    /// </summary>
    public class IsConsumerSubscribeResponseDto:BaseDto
    {
        /// <summary>
        /// 是否关注公众号
        /// </summary>
        public bool Subscribe { get; set; }

        /// <summary>
        /// 微信公众号数字id base64加密后的结果，用于跳转到关注公众号页面的参数
        /// </summary>
        public string BizId { get; set; }
    }
}
