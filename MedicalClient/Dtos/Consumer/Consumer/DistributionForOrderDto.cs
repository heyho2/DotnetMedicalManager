using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 订单分销信息Dto
    /// </summary>
    public class DistributionForOrderDto:BaseDto
    {
        /// <summary>
        /// 订单guid
        /// </summary>
        public string OrderGuid { get; set; }

        /// <summary>
        /// 下单用户guid
        /// </summary>
        public string UserGuid { get; set; }

        /// <summary>
        /// 订单实付
        /// </summary>
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// 上级分销人
        /// </summary>
        public string OneLevelDistribution { get; set; }

        /// <summary>
        /// 上上级分销人
        /// </summary>
        public string TwoLevelDistribution { get; set; }

        /// <summary>
        /// 平台类型
        /// </summary>
        public string PlatformType { get; set; }

    }
}
