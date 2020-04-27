using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;
using GD.Models.CommonEnum;

namespace GD.Models.Utility
{
    ///<summary>
    ///文章表模型
    ///</summary>
    [Table("t_utility_article")]
    public class ArticleModel : BaseModel
    {
        ///<summary>
        ///文章GUID
        ///</summary>
        [Column("article_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "文章GUID")]
        public string ArticleGuid { get; set; }

        ///<summary>
        ///作者GUID
        ///</summary>
        [Column("author_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "作者GUID")]
        public string AuthorGuid { get; set; }

        ///<summary>
        ///来源类型(医生文章、平台文章)
        ///</summary>
        [Column("source_type"), Display(Name = "来源类型")]
        public string SourceType { get; set; } = ArticleSourceTypeEnum.Doctor.ToString();

        /// <summary>
        /// 关键字
        /// </summary>
        [Column("keyword"), Display(Name = "关键字")]
        public string Keyword { get; set; }

        ///<summary>
        ///文章富文本内容GUID
        ///</summary>
        [Column("content_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "文章富文本内容GUID")]
        public string ContentGuid { get; set; }

        ///<summary>
        ///标题
        ///</summary>
        [Column("title"), Required(ErrorMessage = "{0}必填"), Display(Name = "标题")]
        public string Title { get; set; }

        ///<summary>
        ///简介
        ///</summary>
        [Column("abstract"), Required(ErrorMessage = "{0}必填"), Display(Name = "简介")]
        public string Abstract { get; set; }

        ///<summary>
        ///图片GUID
        ///</summary>
        [Column("picture_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "图片GUID")]
        public string PictureGuid { get; set; }

        ///<summary>
        ///文章类型CODE
        ///</summary>
        [Column("article_type_dic"), Required(ErrorMessage = "{0}必填"), Display(Name = "文章类型字典Guid")]
        public string ArticleTypeDic { get; set; }

        ///<summary>
        ///是否显示
        ///</summary>
        [Column("visible"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否显示")]
        public bool Visible { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        [Column("sort"), Required(ErrorMessage = "{0}必填"), Display(Name = "排序")]
        public int Sort { get; set; }

        ///<summary>
        ///平台类型
        ///</summary>
        [Column("platform_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "平台类型")]
        public string PlatformType { get; set; } = GD.Common.EnumDefine.PlatformType.CloudDoctor.ToString();

        /// <summary>
        /// 发布状态
        /// </summary>
        [Column("actcle_release_status"), Required(ErrorMessage = "{0}必填"), Display(Name = "发布状态")]
        public ReleaseStatus ActcleReleaseStatus { get; set; }
        /// <summary>
        /// 外部链接
        /// </summary>
        [Column("external_link"), Required(ErrorMessage = "{0}必填"), Display(Name = "外部链接")]
        public string ExternalLink { get; set; }
        
        /// <summary>
        /// 文章来源类型
        /// </summary>
        public enum ArticleSourceTypeEnum
        {
            /// <summary>
            /// 医生文章
            /// </summary>
            [Description("医生文章")]
            Doctor = 1,

            /// <summary>
            /// 平台文章
            /// </summary>
            [Description("平台文章")]
            Manager
        }
    }

    /// <summary>
    /// 发布状态
    /// </summary>
    public enum ReleaseStatus
    {
        /// <summary>
        /// 发布
        /// </summary>
        [Description("发布")]
        Release = 1,

        /// <summary>
        /// 未发布
        /// </summary>
        [Description("未发布")]
        NoRelease,

        /// <summary>
        /// 驳回
        /// </summary>
        [Description("驳回")]
        Reject,

        /// <summary>
        /// 审核通过
        /// </summary>
        [Description("审核通过")]
        ReviewPass
    }
}