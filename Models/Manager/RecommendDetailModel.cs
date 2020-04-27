using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Manager
{
    /// <summary>
    /// 推荐详细
    /// </summary>
    [Table("t_manager_recommend_detail")]
    public class RecommendDetailModel : BaseModel
    {
        ///<summary>
        ///推荐详细GUID
        ///</summary>
        [Column("detail_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "推荐详细GUID")]
        public string DetailGuid { get; set; }

        ///<summary>
        ///推荐归属GUID
        ///</summary>
        [Column("owner_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "推荐GUID")]
        public string OwnerGuid { get; set; }

        ///<summary>
        ///推荐GUID
        ///</summary>
        [Column("recommend_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "推荐GUID")]
        public string RecommendGuid { get; set; }

    }
}
