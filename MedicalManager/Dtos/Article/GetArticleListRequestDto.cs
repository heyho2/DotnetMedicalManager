using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Article
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
        /// 文章类型
        /// </summary>
        public string ArticleType { get; set; }

        /// <summary>
        /// 文章来源（医生，后台管理）
        /// </summary>
        [Required]
        public ArticleSourceTypeEnum SourceType { get; set; }
        /// <summary>
        /// 发布状态
        /// </summary>
        public ReleaseStatus? ReleaseStatus { get; set; }
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
        /// <summary>
        /// 作者名称
        /// </summary>
        public string AuthorName { get; set; }

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

        ///<summary>
        ///发布状态
        ///</summary>
        public ReleaseStatus ActcleReleaseStatus { get; set; }

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

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 外部链接
        /// </summary>
        public string ExternalLink { get; set; }
    }
}
