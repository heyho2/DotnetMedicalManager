using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Admin.Article
{
    /// <summary>
    /// 添加文章
    /// </summary>
    public class UpdateArticleRequestDto : BaseDto
    {
        ///<summary>
        ///文章GUID
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "文章GUID")]
        public string ArticleGuid { get; set; }
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
        [Required(ErrorMessage = "文章类型必填")]
        public string ActcleReleaseStatus { get; set; }

    }
}
