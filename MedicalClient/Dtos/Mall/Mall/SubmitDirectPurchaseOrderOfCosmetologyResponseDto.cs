using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 双美- 一键购提交生成订单(包括预付款和全款) 响应Dto
    /// </summary>
    public class SubmitDirectPurchaseOrderOfCosmetologyResponseDto : BaseDto
    {
        /// <summary>
        /// 订单guid
        /// </summary>
        public string OrderGuid { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单价格
        /// </summary>
        public decimal Price { get; set; }
    }
}
