using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Mall.Mall
{
    /// <inheritdoc />
    /// <summary>
    /// 获得商户指定分类的产品列表
    /// </summary>
    public class GetProductListInMerchantResponseDto : BaseDto
    {
        /// <summary>
        /// 商户Guid
        /// </summary>
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 分类Guid（字典dicGuid）
        /// </summary>
        public string ClassifyGuid { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string ClassifyName { get; set; }
        /// <summary>
        /// 商品列表信息
        /// </summary>
        public List<ProductListInfo> ProListInfo { get; set; }

        /// <summary>
        /// 商品列表信息
        /// </summary>
        public class ProductListInfo
        {
            /// <summary>
            /// 商品Guid
            /// </summary>
            public string ProductGuid { get; set; }
            /// <summary>
            /// 商品Logo
            /// </summary>
            public string ProductPicUrl { get; set; }
            /// <summary>
            /// 商品名称
            /// </summary>
            public string ProductName { get; set; }
            /// <summary>
            /// 商品形态(服务，实体)'Service','Physical'
            /// </summary>
            public string ProductForm { get; set; }
            /// <summary>
            /// 商品价格
            /// </summary>
            public string Price { get; set; }
            /// <summary>
            /// 销售量
            /// </summary>
            public int SaleNum { get; set; } = 0;

        }

    }
}
