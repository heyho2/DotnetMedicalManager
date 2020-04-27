using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Utility
{
    /// <summary>
    /// 访问量，点赞量统计
    /// </summary>
    [Table("t_utility_hot")]
    public class HotModel : BaseModel
    {
        ///<summary>
        ///主键(与目标对象主键guid值相同)
        ///</summary>
        [Column("owner_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "主键")]
        public string OwnerGuid { get; set; }

        ///<summary>
        ///点赞量
        ///</summary>
        [Column("like_count"), Required(ErrorMessage = "{0}必填"), Display(Name = "点赞量")]
        public int LikeCount { get; set; }

        ///<summary>
        ///访问量
        ///</summary>
        [Column("visit_count"), Required(ErrorMessage = "{0}必填"), Display(Name = "访问量")]
        public int VisitCount { get; set; }

        /// <summary>
        /// 收藏量
        /// </summary>
        [Column("collect_count"), Required(ErrorMessage = "{0}必填"), Display(Name = "收藏量")]

        public int CollectCount { get; set; }
    }
}