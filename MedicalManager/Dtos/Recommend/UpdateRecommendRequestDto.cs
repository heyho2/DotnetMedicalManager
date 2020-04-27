using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Recommend
{
    /// <summary>
    /// 修改推荐 请求
    /// </summary>
    public class UpdateRecommendRequestDto : BaseDto
    {
        ///<summary>
        ///推荐GUID
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "推荐GUID")]
        public string RecommendGuid { get; set; }

        ///<summary>
        ///名称
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "名称")]
        public string Name { get; set; }

        ///<summary>
        ///点击响应目标
        ///</summary>
        public string Target { get; set; }

        ///<summary>
        ///图片GUID
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "图片GUID")]
        public string PictureGuid { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        public int Sort { get; set; }
        ///<summary>
        ///类型 
        ///</summary>
        public string Type { get; set; }
        ///<summary>
        ///备注
        ///</summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
    }
}
