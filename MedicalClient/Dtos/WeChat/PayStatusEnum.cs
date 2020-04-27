using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.WeChat
{
    public enum PayStatusEnum
    {
        /// <summary>
        /// 等待支付
        /// </summary>
        WaitForPayment = 1,

        /// <summary>
        /// Success
        /// </summary>
        Success,
        /// <summary>
        /// 失败
        /// </summary>
        Failure,
        /// <summary>
        /// 退款申请
        /// </summary>
        RequestRefund,
        /// <summary>
        /// 退款成功
        /// </summary>
        RefundSuccess,

        /// <summary>
        /// 退款失败
        /// </summary>
        RefundFailure
    }
}
