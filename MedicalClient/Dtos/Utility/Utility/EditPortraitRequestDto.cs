using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Utility.Utility
{
    /// <summary>
    /// 修改头像 请求
    /// </summary>
    public class EditPortraitRequestDto : BaseDto
    {
        ///<summary>
        ///新头像Guid
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "新头像Guid")]
        public string PortraitGuid
        {
            get;
            set;
        }
    }
}
