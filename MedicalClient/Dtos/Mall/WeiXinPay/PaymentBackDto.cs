using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.WeiXinPay
{
    /// <summary>
    /// 支付回调Dto
    /// </summary>
    public class PaymentBackDto
    {
        /// <summary>
        /// 统一下单KEY
        /// </summary>
        public string OrderKey { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
    }
}
