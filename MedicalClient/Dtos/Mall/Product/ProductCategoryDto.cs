using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.Product
{
    /// <summary>
    /// 商品分类Dto
    /// </summary>
    public class ProductCategoryDto : BaseDto
    {
        /// <summary>
        /// 分类guid
        /// </summary>
        public string CategoryGuid { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }
    }
}
