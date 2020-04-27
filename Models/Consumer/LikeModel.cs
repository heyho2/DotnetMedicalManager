using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Consumer
{
    ///<summary>
    ///点赞表模型
    ///</summary>
    [Table("t_consumer_like")]
    public class LikeModel : BaseModel
    {
        ///<summary>
        ///点赞GUID
        ///</summary>
        [Column("like_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "点赞GUID")]
        public string LikeGuid
        {
            get;
            set;
        }

        ///<summary>
        ///被点赞主体GUID
        ///</summary>
        [Column("target_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "被点赞主体GUID")]
        public string TargetGuid
        {
            get;
            set;
        }
    }
}