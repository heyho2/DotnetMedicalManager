using GD.Common.Base;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 商家端获取商品列表请求Dto
    /// </summary>
    public class GetProductListForMerchantManagementRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 商户guid
        /// </summary>
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 是否上架在售
        /// </summary>
        public bool? OnSale { get; set; }

        /// <summary>
        /// 商品名称筛选条件
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
        /// 是否筛选达到警戒库存的商品
        /// </summary>
        public bool WarningInventory { get; set; } = false;
    }
}
