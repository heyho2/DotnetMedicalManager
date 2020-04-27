using GD.Common.Base;
using System;

namespace GD.Dtos.Utility.Article
{
    /// <summary>
    /// 获取客户端首页推荐文章分页列表
    /// </summary>
    public class GetClientRecommandArticleListRequestDto : BasePageRequestDto
    {

    }

    /// <summary>
    /// 获取客户端首页推荐文章分页列表 响应dto
    /// </summary>
    public class GetClientRecommandArticleListResponseDto : BasePageResponseDto<GetClientRecommandArticleListItemDto>
    {
    }

    /// <summary>
    /// 获取客户端首页推荐文章分页列表 响应Item dto
    /// </summary>
    public class GetClientRecommandArticleListItemDto : BaseDto
    {
        /// <summary>
        /// 文章Guid
        /// </summary>
        public string ArticleGuid { get; set; }

        /// <summary>
        /// 作者guid
        /// </summary>
        public string AuthorGuid { get; set; }

        /// <summary>
        /// 作者姓名
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 文章外链
        /// </summary>
        public string ExternalLink { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime LastUpdatedDate { get; set; }

        /// <summary>
        /// 浏览量
        /// </summary>
        public int PageView { get; set; }

        /// <summary>
        /// 点赞量
        /// </summary>
        public int LikeTotal { get; set; }

        /// <summary>
        /// 文章图片
        /// </summary>
        public string Picture { get; set; }

        /// <summary>
        /// 文章类型
        /// </summary>
        public string ArticleType { get; set; }

        /// <summary>
        /// 文章类型guid
        /// </summary>
        public string ArticleTypeDic { get; set; }

        /// <summary>
        /// 文章来源（Course平台文章；Article普通文章）
        /// </summary>
        public string ArticleSource { get; set; }
    }
}
