using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.WeChat
{
    public class GetH5PaymentBeforeResponseDto : BaseDto
    {
        /// <summary>
        /// 公众账号ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string NonceStr { get; set; }

        /// <summary>
        /// 统一下单对应id
        /// </summary>
        public string Package { get; set; }

        /// <summary>
        /// 签名类型
        /// </summary>
        public string SignType { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }
    }

    public class GetH5PaymentBeforeResquestDto : BaseDto
    {
        
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string MerchantId { get; set; }

        /// <summary>
        /// 秘钥
        /// </summary>
        public string MerchantSecret { get; set; }
        /// <summary>
        /// 付款用户OpenId
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 支付金额，单位分
        /// </summary>
        public int TotalFee { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 支付回调地址
        /// </summary>
        public string NotifyUrl { get; set; }

    }
}
