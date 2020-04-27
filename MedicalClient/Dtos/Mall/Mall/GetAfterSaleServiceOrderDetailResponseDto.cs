using System;
using System.Collections.Generic;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAfterSaleServiceOrderDetailResponseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string OrderGuid { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime OrderTime { get; set; }
        /// <summary>
        /// 付款时间
        /// </summary>
        public DateTime PayTime { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal DiscountAmount { get; set; }
        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal PaidAmount { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal PayablesAmount { get; set; }
        /// <summary>
        /// 商品列表
        /// </summary>
        public List<ProductDetailItem> Items { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ProductDetailItem
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 所属大类
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice { get; set; }
    }
}
