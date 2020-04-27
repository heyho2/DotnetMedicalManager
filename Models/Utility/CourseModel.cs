using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Utility
{
    /// <summary>
    /// 课堂表
    /// </summary>
    [Table("t_utility_course")]
    public class CourseModel : BaseModel
    {
        ///<summary>
        ///主键
        ///</summary>
        [Column("course_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "主键")]
        public string CourseGuid { get; set; }

        ///<summary>
        ///标题
        ///</summary>
        [Column("title"), Required(ErrorMessage = "{0}必填"), Display(Name = "标题")]
        public string Title { get; set; }

        ///<summary>
        ///简介
        ///</summary>
        [Column("summary"), Required(ErrorMessage = "{0}必填"), Display(Name = "简介")]
        public string Summary { get; set; }

        ///<summary>
        ///否显示
        ///</summary>
        [Column("visible"), Required(ErrorMessage = "{0}必填"), Display(Name = "否显示")]
        public bool Visible { get; set; }

        ///<summary>
        ///文章富文本内容GUID
        ///</summary>
        [Column("content_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "文章富文本内容GUID")]
        public string ContentGuid { get; set; }

        ///<summary>
        ///图片guid
        ///</summary>
        [Column("logo_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "图片guid")]
        public string LogoGuid { get; set; }

        ///<summary>
        ///课堂视频资源
        ///</summary>
        [Column("video_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "课堂视频资源")]
        public string VideoGuid { get; set; }
        ///<summary>
        ///作者GUID
        ///</summary>
        [Column("author_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "作者GUID")]
        public string AuthorGuid { get; set; }
    }
}
