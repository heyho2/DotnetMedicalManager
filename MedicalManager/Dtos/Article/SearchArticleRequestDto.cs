using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Article
{
    /// <summary>
    /// 搜索文章 请求
    /// </summary>
    public class SearchArticleRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
    }
    /// <summary>
    /// 搜索文章 响应
    /// </summary>
    public class SearchArticleResponseDto : BasePageResponseDto<SearchArticleItemDto>
    {

    }
    /// <summary>
    /// 搜索文章  项
    /// </summary>
    public class SearchArticleItemDto : BaseDto
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
        ///图片url
        ///</summary>
        public string PictureUrl { get; set; }

        ///<summary>
        ///文章类型CODE
        ///</summary>
        public string ArticleTypeDic { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        public int Sort { get; set; }
    }
}
