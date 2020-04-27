using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Payment.HospitalPayment
{
    public class PaymentSuccessMsgNotifyDto
    {
        /// <summary>
        /// 评价记录guid
        /// </summary>
        public string EvaluationId { get; set; }

        /// <summary>
        /// openid
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 支付金额（单位元）
        /// </summary>
        public decimal TotalFee { get; set; }

        /// <summary>
        /// 医院名称
        /// </summary>
        public string HospitalName { get; set; }

        /// <summary>
        /// 医院guid
        /// </summary>
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 交易时间
        /// </summary>
        public DateTime TransactionTime { get; set; }

        /// <summary>
        /// 交易单号
        /// </summary>
        public string TransactionNo { get; set; }
    }
}
