using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 
    /// </summary>
    public class GetMerchantOrderStatisticsDataSomeDaysResponseDto:BaseDto
    {
        /// <summary>
        /// 订单日期
        /// </summary>
        public string OrderDate { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int ProductCount { get; set; }

        /// <summary>
        /// 销售额
        /// </summary>
        public decimal PaidAmount { get; set; }
    }
}
