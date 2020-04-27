using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 线下支付
    /// </summary>
    public class OffLinePayRequest
    {
        /// <summary>
        /// 订单统一下单KEY
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "订单统一下单KEY")]
        public string OrderKey { get; set; }

        ///// <summary>
        ///// 静默授权标识
        ///// </summary>
        //[Required(ErrorMessage = "{0}必填"), Display(Name = "静默授权标识")]
        //public string Code { get; set; }
        ///// <summary>
        ///// 支付方式（wechat/offlinepay）
        ///// </summary>
        //[Required(ErrorMessage = "{0}必填"), Display(Name = "支付方式")]
        //public string PayType { get; set; }

    }
}
