using GD.Common.Base;
using System;

namespace GD.Dtos.Admin.Article
{
    /// <summary>
    /// 获取文章列表 请求
    /// </summary>
    public class GetArticleListRequestDto : BasePageRequestDto, IBaseOrderBy
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 文章发布状态
        /// </summary>
        public string ActcleReleaseStatus { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        /// 是否升序
        /// </summary>
        public bool IsAscending { get; set; }
    }

    /// <summary>
    /// 获取文章列表 响应
    /// </summary>
    public class GetArticleListResponseDto : BasePageResponseDto<GetArticleListItemDto>
    {

    }

    /// <summary>
    /// 获取文章列表 项
    /// </summary>
    public class GetArticleListItemDto : BaseDto
    {
        ///<summary>
        ///文章GUID
        ///</summary>
        public string ArticleGuid { get; set; }

        ///<summary>
        ///作者GUID
        ///</summary>
        public string AuthorGuid { get; set; }

        ///<summary>
        ///标题
        ///</summary>
        public string Title { get; set; }

        ///<summary>
        ///简介
        ///</summary>
        public string Abstract { get; set; }

        ///<summary>
        ///文章类型CODE
        ///</summary>
        public string ArticleTypeDic { get; set; }

        /// <summary>
        /// 文章发布状态
        /// </summary>
        public string ActcleReleaseStatus { get; set; }

        ///<summary>
        ///是否显示
        ///</summary>
        public bool Visible { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        public int Sort { get; set; }

        /// <summary>
        /// 点赞
        /// </summary>
        public int LikeCount { get; set; }

        /// <summary>
        /// 访问
        /// </summary>
        public int VisitCount { get; set; }

        /// <summary>
        /// 收藏
        /// </summary>
        public int Collection { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}