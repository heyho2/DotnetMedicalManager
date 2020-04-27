using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Recommend
{
    /// <summary>
    /// 获取推荐文章列表 请求
    /// </summary>
    public class GetRecommendArticleListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 推荐Guid
        /// </summary>
        public string RecommendGuid { get; set; }
    }

    /// <summary>
    /// 获取推荐文章列表 响应
    /// </summary>
    public class GetRecommendArticleListResponseDto : BasePageResponseDto<GetRecommendArticleListItemDto>
    {
    }
    /// <summary>
    /// 获取推荐文章列表 项
    /// </summary>
    public class GetRecommendArticleListItemDto : BaseDto
    {
        /// <summary>
        /// 推荐id
        /// </summary>
        public string RecommendGuid { get; set; }

        ///<summary>
        ///文章GUID
        ///</summary>
        public string ArticleGuid { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// login
        /// </summary>
        public string PictureGuid { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string PictureUrl { get; set; }

        ///<summary>
        ///简介
        ///</summary>
        public string Abstract { get; set; }

        ///<summary>
        ///文章类型
        ///</summary>
        public string ArticleTypeDic { get; set; }
        ///<summary>
        ///文章类型
        ///</summary>
        public string ArticleType { get; set; }
    }
}
