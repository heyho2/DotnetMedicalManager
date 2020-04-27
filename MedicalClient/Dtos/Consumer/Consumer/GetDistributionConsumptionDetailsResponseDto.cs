using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 获取分销订单详情响应Dto
    /// </summary>
    public class GetDistributionConsumptionDetailsResponseDto : BaseDto
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 付款日期
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int ProductCount { get; set; }

        /// <summary>
        /// 订单总价
        /// </summary>
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// 积分比例
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// 可得积分
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 核实客服电话
        /// </summary>
        public string ServiceTel { get { return "400-520-5200"; } }
    }
}
