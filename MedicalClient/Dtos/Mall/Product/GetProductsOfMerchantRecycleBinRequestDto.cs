using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.Product
{
    /// <summary>
    /// 获取商铺回收站中的商品列表（智慧云医）
    /// </summary>
    public class GetProductsOfMerchantRecycleBinRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 商铺guid
        /// </summary>
        [Required(ErrorMessage ="商铺guid必填")]
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 商品分类guid
        /// </summary>
        public string CategoryGuid { get; set; }

        /// <summary>
        /// 商品品牌guid
        /// </summary>
        public string ProductBrandGuid { get; set; }

        /// <summary>
        /// 添加至回收站日期（起始）
        /// </summary>
        public DateTime? StartRecycleDate { get; set; }

        /// <summary>
        /// 添加至回收站日期（结束）
        /// </summary>
        public DateTime? EndRecycleDate { get; set; }

    }
}
