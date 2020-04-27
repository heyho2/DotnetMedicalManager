using GD.Common.Base;

namespace GD.Dtos.Mall.Mall
{
    /// <inheritdoc />
    /// <summary>
    /// 获得商户指定分类的产品列表
    /// </summary>
    public class GetProductListInMerchantRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 商户Guid
        /// </summary>
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 分类Guid（字典dicGuid）
        /// </summary>
        public string ClassifyGuid { get; set; }

    }
}
