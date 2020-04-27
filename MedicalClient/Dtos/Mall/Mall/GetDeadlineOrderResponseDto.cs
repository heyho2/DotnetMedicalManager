using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 获取已截止付款的待付款订单响应Dto
    /// </summary>
    public class GetDeadlineOrderResponseDto : BaseDto
    {
        /// <summary>
        /// 订单key
        /// </summary>
        public string OrderKey { get; set; }
        /// <summary>
        /// 交易流水guid
        /// </summary>
        public string TransactionFlowingGuid { get; set; }
    }
}
