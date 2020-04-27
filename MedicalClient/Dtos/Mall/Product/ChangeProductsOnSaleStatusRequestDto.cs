using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.Product
{
    /// <summary>
    /// 修改商品上下架状态 请求Dto
    /// </summary>
    public class ChangeProductsOnSaleStatusRequestDto : BaseDto
    {
        /// <summary>
        /// 商铺guid
        /// </summary>
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 上下架状态：true表示上架；false表示下架
        /// </summary>
        [Required(ErrorMessage ="上下架状态必填")]
        public bool OnSale { get; set; }

        /// <summary>
        /// 商品Ids
        /// </summary>
        [Required(ErrorMessage = "商品Ids必填")]
        public List<string> ProductIds { get; set; }
    }
}
