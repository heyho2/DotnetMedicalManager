using GD.Common.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Article
{
    /// <summary>
    /// 添加文章
    /// </summary>
    public class AddArticleRequestDto : BaseDto
    {

        ///<summary>
        ///文章富文本内容
        ///</summary>
        [Required(ErrorMessage = "文本内容必填")]
        public string Content { get; set; }

        ///<summary>
        ///标题
        ///</summary>
        [Required(ErrorMessage = "标题必填")]
        public string Title { get; set; }

        ///<summary>
        ///简介
        ///</summary>
        [Required(ErrorMessage = "简介必填")]
        public string Abstract { get; set; }

        ///<summary>
        ///图片GUID
        ///</summary>
        public string PictureGuid { get; set; }

        ///<summary>
        ///文章类型CODE
        ///</summary>
        [Required(ErrorMessage = "文章类型必填")]
        public string ArticleTypeDic { get; set; }

        ///<summary>
        ///是否显示
        ///</summary>
        public bool Visible { get; set; }
        /// <summary>
        /// 发布状态
        /// </summary>
        [Required(ErrorMessage = "发布状态必填")]
        public ReleaseStatus ActcleReleaseStatus { get; set; }
        /// <summary>
        /// 来源类型(医生文章、平台文章)
        /// </summary>
        [Required(ErrorMessage = "来源类型必填")]
        public ArticleSourceTypeEnum SourceType { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        [Required]
        public string[] Keyword { get; set; }
        /// <summary>
        /// 外部链接
        /// </summary>
        public string ExternalLink { get; set; }
    }
}
