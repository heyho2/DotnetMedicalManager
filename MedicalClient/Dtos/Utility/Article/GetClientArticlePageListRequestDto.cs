using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Utility.Article
{
    /// <summary>
    /// 获取客户端综合文章(普通文章+健康管理文章)分页列表 请求dto
    /// </summary>
    public class GetClientArticlePageListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 文章类型guid
        /// </summary>
        public string ArticleTypeDic { get; set; }

        /// <summary>
        /// 作者guid
        /// </summary>
        public string AuthorGuid { get; set; }
    }


}
