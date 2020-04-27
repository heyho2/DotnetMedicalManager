using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 商家端获取商品列表响应Dto
    /// </summary>
    public class GetProductListForMerchantManagementResponseDto:BasePageResponseDto<GetProductListForMerchantManagementItemDto>
    {
    }

    /// <summary>
    /// 商家端获取商品列表响应项Dto
    /// </summary>
    public class GetProductListForMerchantManagementItemDto:BaseDto
    {
        /// <summary>
        /// 商品guid
        /// </summary>
        public string ProductGuid { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 是否上架
        /// </summary>
        public bool OnSale { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        public string Inventory { get; set; }
        /// <summary>
        /// 销量
        /// </summary>
        public int SalesVolume { get; set; }
        /// <summary>
        /// 商品形态(Service:服务，实体:Physical)
        /// </summary>
        public string ProductForm { get; set; }
        /// <summary>
        ///分类名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 自购买日期多少天有效（0表示永久有效）
        /// </summary>
        public int EffectiveDays { get; set; }
        /// <summary>
        /// 是否上架
        /// </summary>
        public bool PlatformOnSale { get; set; }
    }
}
