using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Member
{
    /// <summary>
    /// 订单信息（详细信息）
    /// </summary>
    public class GetOrderInfoResponseDto : BaseDto
    {
        ///<summary>
        ///数量
        ///</summary>
        public int ProductCount { get; set; }
        
        /// <summary>
        /// 用户
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 订单日期
        /// </summary>
        public DateTime OrderDate { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string OrderPhone { get; set; }
        /// <summary>
        /// 物流
        /// </summary>
        public string LogisticsNo { get; set; }
        /// <summary>
        /// 物流公司
        /// </summary>
        public string LogisticsName { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal DiscountAmout { get; set; }
        /// <summary>
        /// 积分金额
        /// </summary>
        public decimal PointAmount { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal PayablesAmount { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        public decimal Freight { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 订单地址
        /// </summary>
        public string OrderAddress { get; set; }
        /// <summary>
        /// 收件人
        /// </summary>
        public string OrderReceiver { get; set; }
    }
}
