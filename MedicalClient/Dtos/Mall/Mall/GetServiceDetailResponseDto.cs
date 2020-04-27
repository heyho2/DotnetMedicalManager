using GD.Dtos.CommonEnum;
using System;
using System.Collections.Generic;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 
    /// </summary>
    public class GetServiceDetailResponseDto
    {
        /// <summary>
        /// 服务单号
        /// </summary>
        public string ServiceNo { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderStatus { get; set; }
        /// <summary>
        /// 用户手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplicationTime { get; set; }
        /// <summary>
        /// 退款状态（1：申请中、2：已拒绝、3：已完成）
        /// </summary>
        public AfterSaleServiceStatusEnum AfterSaleStatus { get; set; }
        /// <summary>
        /// 售后类型
        /// </summary>
        public AfterSaleServiceTypeEnum AfterSaleType { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal? RefundFee { get; set; }
        /// <summary>
        /// 退款时间
        /// </summary>
        public DateTime? RefundTime { get; set; }
        /// <summary>
        /// 退款原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 拒绝原因
        /// </summary>
        public string RefuseReason { get; set; }
        /// <summary>
        /// 商品列表
        /// </summary>
        public List<ProductItem> Items { get; set; }
    }

    /// <summary>
    /// 产品详情
    /// </summary>
    public class ProductItem
    {
        /// <summary>
        /// 图片地址
        /// </summary>
        public string Pic { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 价格（单位：分）
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// 退款数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal RefundFee { get; set; }
    }
}
