using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.DtoIn
{
    /// <summary>
    /// 经营范围传入Dto
    /// </summary>
    public class ScopeDto : BaseDto
    {
        /// <summary>
        /// 范围字典Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "范围字典Guid")]
        public string ScopeDicGuid { get; set; }

        /// <summary>
        /// 经营范围图片guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "经营范围图片guid")]
        public string AccessoryGuid { get; set; }
    }
}
