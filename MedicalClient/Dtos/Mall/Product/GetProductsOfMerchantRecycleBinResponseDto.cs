using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.Product
{
    /// <summary>
    /// 获取商铺回收站中的商品列表（智慧云医）响应Dto
    /// </summary>
    public class GetProductsOfMerchantRecycleBinResponseDto : BasePageResponseDto<GetProductsOfMerchantRecycleBinItemDto>
    {
    }

    /// <summary>
    /// 获取商铺回收站中的商品列表（智慧云医）响应ItemDto
    /// </summary>
    public class GetProductsOfMerchantRecycleBinItemDto : BaseDto
    {
        /// <summary>
        /// 商品guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public int Inventory { get; set; }
    }
}
