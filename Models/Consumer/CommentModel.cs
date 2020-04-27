using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Consumer
{
    ///<summary>
    ///评论表模型
    ///</summary>
    [Table("t_consumer_comment")]
    public class CommentModel : BaseModel
    {
        ///<summary>
        ///评论GUID
        ///</summary>
        [Column("comment_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "评论GUID")]
        public string CommentGuid
        {
            get;
            set;
        }

        ///<summary>
        ///目标GUID
        ///</summary>
        [Column("target_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "目标GUID")]
        public string TargetGuid
        {
            get;
            set;
        }

        ///<summary>
        ///内容
        ///</summary>
        [Column("content"), Required(ErrorMessage = "{0}必填"), Display(Name = "内容")]
        public string Content
        {
            get;
            set;
        }

        ///<summary>
        ///评论打分
        ///</summary>
        [Column("score"), Required(ErrorMessage = "{0}必填"), Display(Name = "评论打分")]
        public int Score
        {
            get;
            set;
        }
        ///<summary>
        ///评论打分
        ///</summary>
        [Column("anonymous"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否匿名")]
        public bool Anonymous
        {
            get;
            set;
        }

        /// <summary>
        /// 排序
        /// </summary>
        [Column("sort"), Display(Name = "排序")]
        public int Sort { get; set; } = 1;
    }
}