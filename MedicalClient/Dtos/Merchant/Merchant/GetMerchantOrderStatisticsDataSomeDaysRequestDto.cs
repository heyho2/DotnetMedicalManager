using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 
    /// </summary>
    public class GetMerchantOrderStatisticsDataSomeDaysRequestDto : BaseDto
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 终止日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 商户guid
        /// </summary>
        public string MerchantGuid { get; set; }


    }
}
