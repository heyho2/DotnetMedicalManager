using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Admin.Article
{
    /// <summary>
    /// 获取文章
    /// </summary>
    public class GetArticleInfoRequestDto : BaseDto
    {
        ///<summary>
        ///文章GUID
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "文章GUID")]
        public string ArticleGuid { get; set; }


    }
    /// <summary>
    /// 获取文章 响应
    /// </summary>
    public class GetArticleInfoResponseDto : BaseDto
    {
        /// <summary>
        /// 文章GUID
        /// </summary>
        public string ArticleGuid { get; set; }

        ///<summary>
        ///文章富文本内容
        ///</summary>
        public string Content { get; set; }

        ///<summary>
        ///标题
        ///</summary>
        public string Title { get; set; }

        ///<summary>
        ///简介
        ///</summary>
        public string Abstract { get; set; }

        ///<summary>
        ///图片GUID
        ///</summary>
        public string PictureGuid { get; set; }

        ///<summary>
        ///文章类型CODE
        ///</summary>
        public string ArticleTypeDic { get; set; }

        ///<summary>
        ///是否显示
        ///</summary>
        public bool Visible { get; set; }
        /// <summary>
        /// 图片Url
        /// </summary>
        public string PictureUrl { get; set; }

        /// <summary>
        /// 文章发布状态
        /// </summary>
        public string ActcleReleaseStatus { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationDate { get; set; }

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
    }
}
