using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 修改排序
    /// </summary>
    public class UpdateProductSortRequestDto
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        [Required(ErrorMessage = "商品Id必填")]
        public string ProductGuid { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
