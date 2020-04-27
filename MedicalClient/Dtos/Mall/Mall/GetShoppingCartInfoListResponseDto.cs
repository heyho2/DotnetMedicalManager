using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Mall.Mall
{
    /// <inheritdoc />
    /// <summary>
    /// 购物车产品列表--按商户分组
    /// </summary>
    public class GetShoppingCartInfoListResponseDto:BaseDto
    {
        
        /// <summary>
        /// 商户Guid
        /// </summary>
        public string MerchantGuid { get; set; }
        /// <summary>
        /// 商户名称
        /// </summary>
        public string MerchantName { get; set; }
        /// <summary>
        /// 该商户的购物车产品列表
        /// </summary>
        public List<ProductListGroupByMerchant> ProductList { get; set; }
        /// <summary>
        /// 该商户的购物车产品列表
        /// </summary>
        public class ProductListGroupByMerchant
        {
            /// <summary>
            /// 购物车记录主键id
            /// </summary>
            public string ItemGuid { get; set; }

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
            /// 产品规格
            /// </summary>
            public string Standerd { get; set; }
            /// <summary>
            /// 产品简介内容
            /// </summary>
            public string Content { get; set; }
            /// <summary>
            /// 产品价格
            /// </summary>
            public decimal Price { get; set; }
            /// <summary>
            /// 购买数量
            /// </summary>
            public string Count { get; set; }

            /// <summary>
            /// 商品运费
            /// </summary>
            public decimal Freight { get; set; }

            /// <summary>
            /// 购物车商品库存是否足够，不够则无效
            /// </summary>
            public bool IsValid { get; set; }
        }
    }

}
