using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Recommend
{
    /// <summary>
    /// 删除推荐详细 请求
    /// </summary>
    public class DeleteRecommendDetail2RequestDto : BaseDto
    {
        ///<summary>
        ///推荐归属GUID
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "推荐GUID")]
        public string OwnerGuid { get; set; }

        ///<summary>
        ///推荐GUID
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "推荐GUID")]
        public string RecommendGuid { get; set; }
    }
}
