using System;
using System.Collections.Generic;
using System.Text;
using GD.Dtos.CommonEnum;

namespace GD.Dtos.Mall.Order
{
    /// <summary>
    /// 订单详情售后信息
    /// </summary>
    public class OrderDetailAfterServiceInfoDto
    {
        /// <summary>
        /// 订单详情guid
        /// </summary>
        public string OrderDetailGuid { get; set; }

        /// <summary>
        /// 售后单类型
        /// </summary>
        public AfterSaleServiceTypeEnum? ServiceType { get; set; }

        /// <summary>
        /// 售后状态
        /// </summary>
        public AfterSaleServiceStatusEnum? ServiceStatus { get; set; }
    }
}
