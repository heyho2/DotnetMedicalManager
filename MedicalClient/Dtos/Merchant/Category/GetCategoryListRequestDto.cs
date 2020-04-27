using GD.Common.Base;

namespace GD.Dtos.Merchant.Category
{
    /// <summary>
    /// 获取类别列表请求类
    /// </summary>
    public class GetMerchantCategoryListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 商户Id
        /// </summary>
        public string MerchantGuid { get; set; }
        /// <summary>
        ///分类
        /// </summary>
        public string ClassifyGuid { get; set; }
    }


    /// <summary>
    ///获取类别列表响应类
    /// </summary>
    public class GetMerchantCategoryListResponseDto : BasePageResponseDto<CategoryItem>
    {

    }


    /// <summary>
    /// 类别项具体信息
    /// </summary>
    public class CategoryItem : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string CategoryGuid { get; set; }
        /// <summary>
        /// 服务类型一级分类名称
        /// </summary>
        public string ClassifyName { get; set; }
        /// <summary>
        /// 大类名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        ///联系电话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 是否上线，true表示上线，false表示下线
        /// </summary>
        public bool Enable { get; set; }
    }
}
