using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Admin.Recommend
{
    /// <summary> 
    /// 新增推荐详细 请求
    /// </summary>
    public class AddRecommendDetailRequestDto
    {
        /// <summary>
        /// 推荐guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "推荐GUID")]
        public string RecommendGuid { get; set; }
        /// <summary>
        /// 推荐归属id
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "推荐归属id")]
        public string[] OwnerGuids { get; set; }
    }
}
