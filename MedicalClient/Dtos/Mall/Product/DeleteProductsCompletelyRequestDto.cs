using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.Product
{
    /// <summary>
    /// 彻底删除商品请求Dto
    /// </summary>
    public class DeleteProductsCompletelyRequestDto : BaseDto
    {
        /// <summary>
        /// 商铺guid
        /// </summary>
        [Required(ErrorMessage = "商铺guid必填")]
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 商品Ids
        /// </summary>
        [Required(ErrorMessage = "商品Ids必填")]
        public List<string> ProductIds { get; set; }
    }
}
