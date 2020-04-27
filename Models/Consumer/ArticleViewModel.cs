using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GD.Models.Consumer
{
    /// <summary>
    /// 文章浏览记录
    ///</summary>
    [Table("t_consumer_article_view")]
    public class ArticleViewModel : BaseModel
    {
        ///<summary>
        ///文章浏览记录主键
        ///</summary>
        [Column("view_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "文章浏览记录主键")]
        public string ViewGuid
        {
            get;
            set;
        }

        ///<summary>
        ///文章guid
        ///</summary>
        [Column("target_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "文章guid")]
        public string TargetGuid
        {
            get;
            set;
        }
    }
}
