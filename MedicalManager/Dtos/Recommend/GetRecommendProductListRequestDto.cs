using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Recommend
{
    /// <summary>
    /// 获取推荐文章列表 请求
    /// </summary>
    public class GetRecommendProductListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 推荐Guid
        /// </summary>
        public string RecommendGuid { get; set; }
    }

    /// <summary>
    /// 获取推荐文章列表 响应
    /// </summary>
    public class GetRecommendProductListResponseDto : BasePageResponseDto<GetRecommendProductListItemDto>
    {
    }
    /// <summary>
    /// 获取推荐文章列表 项
    /// </summary>
    public class GetRecommendProductListItemDto : BaseDto
    {
        /// <summary>
        /// 推荐id
        /// </summary>
        public string RecommendGuid { get; set; }
        ///<summary>
        ///文章GUID
        ///</summary>
        public string ProductGuid { get; set; }
        /// <summary>
        /// 商户Guid
        /// </summary>
        public string MerchantGuid { get; set; }
        /// <summary>
        /// 图片guid
        /// </summary>
        public string PictureGuid { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string PictureUrl { get; set; }
        ///<summary>
        ///产品名称
        ///</summary>
        public string ProductName { get; set; }
        ///<summary>
        ///产品标签
        ///</summary>
        public string ProductLabel { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        public string Inventory { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public string CategoryName { get; set; }
    }
}
