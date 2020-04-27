using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.WeChat
{
    public class UnifiedOrderRequestDto
    {
        /// <summary>
        /// 微信公众号AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string MerchantId { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 支付金额，单位分
        /// </summary>
        public int TotalFee { get; set; }

        /// <summary>
        /// 异步接收微信支付结果通知的回调地址，通知url必须为外网可访问的url，不能携带参数。
        /// </summary>
        public string NotifyUrl { get; set; }

        /// <summary>
        /// 支付用户openid
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 商户号秘钥
        /// </summary>
        public string MerchantSecret { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string NonceStr { get; set; }
    }
}
