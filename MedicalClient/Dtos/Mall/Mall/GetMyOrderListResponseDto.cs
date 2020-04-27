using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using GD.Common.Base;
using GD.Dtos.CommonEnum;

namespace GD.Dtos.Mall.Mall
{

    public class GetMyOrderListResponseItemDto : BaseDto
    {
        ///<summary>
        ///订单GUID
        ///</summary>
        public string OrderGuid { get; set; }
        ///<summary>
        ///订单状态
        ///</summary>
        public string OrderStatus { get; set; }
        /// <summary>
        /// 商户guid
        /// </summary>
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 商铺名称
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 订单金额（实付）
        /// </summary>
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// 商品总数量
        /// </summary>
        public int ProductCount { get; set; }

        /// <summary>
        /// 下单日期
        /// </summary>
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// 商品编号
        ///</summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 订单Key
        ///</summary>
        public string OrderKey { get; set; }
        /// <summary>
        /// 订单种类（Normal/Group）
        ///</summary>
        public string OrderType { get; set; }
        /// <summary>
        /// 订单分类（Service/Physical）
        ///</summary>
        public string OrderCategory { get; set; }
        /// <summary>
        /// 支付方式
        /// （支付方式：银行卡Card，现金Cash，苹果支付Apple，积分支付Score，微信支付Wechat，支付宝Alipay，线下支付OffLinePay）
        ///</summary>
        public string PayType { get; set; }
        /// <summary>
        /// 订单类型（Primary主订单、Secondary从订单）
        ///</summary>
        public string OrderMark { get; set; }
        /// <summary>
        /// 优惠金额
        ///</summary>
        public decimal DiscountAmout { get; set; }

        /// <summary>
        /// 商家是否存在
        ///</summary>
        public bool? MerchantEnable { get; set; }

        /// <summary>
        /// 订单售后情况描述
        /// </summary>
        public string AfterSeriveDescription { get; set; }

        /// <summary>
        /// 订单总运费
        /// </summary>
        public decimal Freight { get; set; }
        /// <summary>
        /// 订单详情
        /// </summary>
        public List<OrderDetail> OrderDetailList { get; set; }

        /// <summary>
        /// 订单详情
        /// </summary>
        public class OrderDetail
        {
            /// <summary>
            /// 购物车记录主键id
            /// </summary>
            public string DetailGuid { get; set; }

            /// <summary>
            /// 产品Guid
            /// </summary>
            public string ProductGuid { get; set; }
            /// <summary>
            /// 产品名称
            /// </summary>
            public string ProductName { get; set; }
            /// <summary>
            /// 产品图片
            /// </summary>
            public string ProductPicUrl { get; set; }

            /// <summary>
            /// 产品价格
            /// </summary>
            public decimal Price { get; set; }
            /// <summary>
            /// 购买数量
            /// </summary>
            public int Count { get; set; }

            ///// <summary>
            ///// 商品运费
            ///// </summary>
            //public decimal Freight { get; set; }

            /// <summary>
            /// 商品评价guid
            /// </summary>
            public string CommentGuid { get; set; }
            /// <summary>
            /// 商品是否在售
            ///</summary>
            public bool OnSale { get; set; }
        }
    }

    /// <summary>
    /// 获取我的订单列表Dto
    /// </summary>
    public class GetMyOrderListResponseDto : BasePageResponseDto<GetMyOrderListResponseItemDto>
    {




    }

}
