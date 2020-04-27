using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 双美- 一键购提交生成订单(包括预付款和全款)
    /// </summary>
    public class SubmitDirectPurchaseOrderOfCosmetologyRequestDto : BaseDto
    {
        /// <summary>
        /// 商品Guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 是否预付款
        /// </summary>
        public bool WhetherPayDeposit { get; set; } = false;
    }
}
