using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.MallPay
{
    /// <summary>
    /// 支付相关枚举
    /// </summary>
    public enum MallPayEnum
    {
        /// <summary>
        /// pay_way
        /// </summary>
        wechat,

        /// <summary>
        /// pay_mode
        /// </summary>
        wechatpublic,
    }

    /// <summary>
    /// 支付结果查询状态枚举
    /// </summary>
    public enum QueryTradeStatus
    {
        /// <summary>
        /// 支付中
        /// </summary>
        Paying,

        /// <summary>
        /// 支付成功
        /// </summary>
        Pay_Ok,

        /// <summary>
        /// 已退款(包含部分退款)
        /// </summary>
        Refunded,

        /// <summary>
        /// 订单已经关闭
        /// </summary>
        Closed,


    }
}
