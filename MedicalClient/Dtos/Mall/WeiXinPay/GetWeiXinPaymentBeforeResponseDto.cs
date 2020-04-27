using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.WeiXinPay
{
    /// <summary>
    /// 获取微信支付前准备返回Dto
    /// </summary>
    public class GetWeiXinPaymentBeforeResponseDto
    {
        /// <summary>
        /// 公众账号ID
        /// </summary>
        public string appId { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public int timeStamp { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonceStr { get; set; }

        /// <summary>
        /// 统一下单对应id
        /// </summary>
        public string package { get; set; }

        /// <summary>
        /// 签名类型
        /// </summary>
        public string signType { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// 是否需要唤醒微信支付
        /// 若金额为0的情况，则不需要唤醒微信支付
        /// </summary>
        public bool NeedPay { get; set; } = true;
    }
}
