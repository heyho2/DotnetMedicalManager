using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.WeiXinPay
{
    /// <summary>
    /// 获取微信支付前准备请求Dto
    /// </summary>
    public class GetWeiXinPaymentBeforeRequestDto : BaseDto
    {
        /// <summary>
        /// openId
        /// </summary>
        public string openId { get; set; }

        /// <summary>
        /// 订单统一下单KEY
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "订单统一下单KEY")]
        public string orderKey { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public int totalFee;

        /// <summary>
        /// 商品内容
        /// </summary>
        public string body;

        /// <summary>
        /// 商品列表
        /// </summary>
        public string detail;

        /// <summary>
        /// 附加数据
        /// </summary>
        public string attach;
    }
}