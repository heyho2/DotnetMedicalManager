using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.Product
{
    /// <summary>
    /// 修改商品销售属性
    /// </summary>
    public class ModifyProductSalePropertyOfDoctorCloudRequestDto : BaseDto
    {
        /// <summary>
        /// 商品guid
        /// </summary>
        [Required(ErrorMessage = "商品guid必填")]
        public string ProductGuid { get; set; }

        /// <summary>
        /// 是否上架在售（销售中、已下架）
        /// </summary>
        [Required(ErrorMessage = "是否上架在售")]
        public bool OnSale { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal? Freight { get; set; } = 0M;

        /// <summary>
        /// 市场价
        /// </summary>
        public decimal? MarketPrice { get; set; }

        /// <summary>
        /// 成本价
        /// </summary>
        public decimal? CostPrice { get; set; }

        /// <summary>
        /// 售卖价格
        /// </summary>
        [Required(ErrorMessage = "商品售卖价格必填")]
        public decimal Price { get; set; }
    }
}
