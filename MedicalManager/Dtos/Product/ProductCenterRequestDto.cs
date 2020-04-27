using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 商品中心
    /// </summary>
    public class ProductCenterRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 商家Id
        /// </summary>
        public string MerchantGuid { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 商品状态
        /// </summary>
        public bool? ProductStatus { get; set; }
    }
    /// <summary>
    /// 商品中心响应
    /// </summary>
    public class ProductCenterResponseDto : BasePageResponseDto<ProductCenterItemDto>
    {

    }
    /// <summary>
    /// 商品项
    /// </summary>
    public class ProductCenterItemDto : BaseDto
    {
        ///<summary>
        ///产品GUID
        ///</summary>
        public string ProductGuid { get; set; }
        ///<summary>
        ///商户名称
        ///</summary>
        public string MerchantName { get; set; }
        /// <summary>
        /// 产品类型
        /// </summary>
        public string ProductForm { get; set; }
        ///<summary>
        ///商品编码
        ///</summary>
        public string ProductCode { get; set; }
        ///<summary>
        ///所属分类名称
        ///</summary>
        public string CategoryName { get; set; }
        ///<summary>
        ///产品名称
        ///</summary>
        public string ProductName { get; set; }
        ///<summary>
        ///价格
        ///</summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        public decimal Inventory { get; set; }
        /// <summary>
        /// 销量
        /// </summary>
        public int SaleCount { get; set; }
        /// <summary>
        /// 商品状态
        /// </summary>
        public bool OnSale { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public int EffectiveDays { get; set; }
        ///<summary>
        ///是否热门
        ///</summary>
        public bool Recommend { get; set; }
        ///<summary>
        ///排序
        ///</summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否上架
        /// </summary>
        public bool PlatformOnSale { get; set; }
    }
}
