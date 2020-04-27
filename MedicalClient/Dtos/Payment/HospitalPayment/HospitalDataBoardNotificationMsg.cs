using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Payment.HospitalPayment
{
    /// <summary>
    /// 医院数据看板通知
    /// </summary>
    public class HospitalDataBoardNotificationMsg
    {
        /// <summary>
        /// 1表示收款通知；2表示业绩上报通知
        /// </summary>
        public int NotificationType { get; set; } = 1;

        /// <summary>
        /// 医院guid
        /// </summary>
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 金额(元)
        /// </summary>
        public decimal Amount { get; set; }
    }
}
