using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Dtos.Manager.Recommend
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
        [Column("recommend_guid")]
        public string RecommendGuid { get; set; }

        ///<summary>
        ///文章GUID
        ///</summary>
        [Column("article_guid")]
        public string ArticleGuid { get; set; }

        /// <summary>
        /// 作者guid
        /// </summary>
        public string AuthorGuid { get; set; }

        /// <summary>
        /// 作者昵称
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// 浏览量
        /// </summary>
        public int PageView { get; set; }

        /// <summary>
        /// 文章来源
        /// </summary>
        public string SourceType { get; set; }

        /// <summary>
        /// 文章创建日期
        /// </summary>
        public DateTime ArticleDate { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Column("title")]
        public string Title { get; set; }
        /// <summary>
        /// login
        /// </summary>
        [Column("picture_guid")]
        public string PictureGuid { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string PictureUrl { get; set; }

        ///<summary>
        ///简介
        ///</summary>
        [Column("abstract")]
        public string Abstract { get; set; }

        ///<summary>
        ///文章类型
        ///</summary>
        [Column("article_type_dic")]
        public string ArticleTypeDic { get; set; }
        ///<summary>
        ///文章类型
        ///</summary>
        public string ArticleType { get; set; }
    }
}

