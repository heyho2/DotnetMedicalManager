using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.Product
{
    /// <summary>
    /// 验证商品编号是否被占用
    /// </summary>
    public class CheckProductCodeRequestDto : BaseDto
    {
        /// <summary>
        /// 商铺guid
        /// </summary> 
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        [Required(ErrorMessage = "商品编号必填")]
        public string ProductCode { get; set; }

        /// <summary>
        /// 商品guid
        /// </summary>
        public string ProductGuid { get; set; }
    }
}
