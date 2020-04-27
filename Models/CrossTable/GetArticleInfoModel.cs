using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using GD.Common.Base;

namespace GD.Models.CrossTable
{
    /// <summary>
    /// 获取文章信息
    /// </summary>
    public class GetArticleInfoModel : BaseModel
    {
        ///<summary>
        ///文章GUID
        ///</summary>
        [Column("article_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "文章ID")]
        public string ArticleGuid { get; set; }

        ///<summary>
        ///医生姓名
        ///</summary>
        [Column("UserName"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "医生姓名")]
        public string UserName { get; set; }

        ///<summary>
        ///基础路径
        ///</summary>
        [Column("article_pic"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "文章logo全路径")]
        public string ArticlePic { get; set; }

        /// <summary>
        /// 文章来源
        /// </summary>
        [Column("source_type"), Display(Name = "文章来源")]
        public string SourceType { get; set; }

        ///<summary>
        ///文章标题
        ///</summary>
        [Column("title"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "文章标题")]
        public string Title { get; set; }

        ///<summary>
        ///字典ID
        ///</summary>
        [Column("dic_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "字典ID")]
        public string DicGuid { get; set; }

        ///<summary>
        ///配置名称
        ///</summary>
        [Column("config_name"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "配置名称")]
        public string ConfigName { get; set; }


        ///<summary>
        ///发布日期（创建日期）
        ///</summary>
        [Column("creation_date"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "发布日期")]
        public new string CreationDate { get; set; }

        ///<summary>
        ///浏览次数
        ///</summary>
        [Column("view_num"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "浏览次数")]
        public int ViewNum { get; set; } = 0;

        ///<summary>
        ///点赞次数
        ///</summary>
        [Column("thumbup_num"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "点赞次数")]
        public int ThumbUpNum { get; set; } = 0;
    }
}
