using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Merchant.MerchantFlowing
{
    /// <summary>
    /// 商户流水返回DTO
    /// </summary>
    public class MerchantFlowingReportResponseDto
    {
        /// <summary>
        /// 商户GUID
        /// </summary>
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 产品数量
        /// </summary>
        public long ProductCount { get; set; }

        /// <summary>
        /// 销售额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public long OrderCount { get; set; }

        /// <summary>
        /// 预约完成数量
        /// </summary>
        public long ConsumptionCount { get; set; }

        ///// <summary>
        ///// 项目数量
        ///// </summary>
        //public long ProjectCount { get; set; }
        
    }
}