using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 门店端获取订单列表筛选条件订单状态列表数据响应
    /// </summary>
    public class GetMerchantOrderStatusConditionListResponseDto:BaseDto
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public string OrderStatusCode { get; set; }

        /// <summary>
        /// 状态名
        /// </summary>
        public string OrderStatusName { get; set; }
    }
}
