using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.Product
{
    /// <summary>
    /// 
    /// </summary>
    public class ModifyProductCategoryInfoRequestDto : BaseDto
    {
        /// <summary>
        /// 商品guid
        /// </summary>
        [Required(ErrorMessage = "商品guid必填")]
        public string ProductGuid { get; set; }

        /// <summary>
        /// 商品类别
        /// </summary>
        [Required(ErrorMessage = "商品类别必填")]
        public string CategoryGuid { get; set; }
    }
}
