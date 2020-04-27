using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;
using GD.Models.CommonEnum;
using GD.Models.Mall;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 双美-获取订单列表(全部订单、待付款订单)请求Dto
    /// </summary>
    public class GetOrderListOfCosmetologyRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatusEnum OrderStatus { get; set; } = OrderStatusEnum.All;

        /// <summary>
        /// 用户id(选填，默认为登录用户)
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 筛选关键字（可支持订单号、产品名称）
        /// </summary>
        public string Keyword { get; set; }
    }
}
