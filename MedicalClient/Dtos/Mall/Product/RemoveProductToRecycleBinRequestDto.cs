using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.Product
{
    /// <summary>
    /// 移除商品到回收站请求Dto
    /// </summary>
    public class RemoveProductToRecycleBinRequestDto : BaseDto
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
