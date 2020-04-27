using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using static GD.Models.Utility.ArticleModel;

namespace GD.Dtos.Utility.Article
{
    /// <summary>
    /// 获取用户文章列表请求Dto
    /// </summary>
    public class GetUserArticleRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 作者Guid
        /// </summary>
        [Required(ErrorMessage ="作者guid必填")]
        public string AuthorGuid { get; set; }

        /// <summary>
        /// 文章来源
        /// </summary>
        public ArticleSourceTypeEnum? SourceType { get; set; } = ArticleSourceTypeEnum.Doctor;
    }
}
