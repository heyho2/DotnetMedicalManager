using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Utility.Richtext
{
    /// <summary>
    /// 修改富文本内容
    /// </summary>
    public class ModifyRichtextRequestDto : BaseDto
    {
        /// <summary>
        /// 富文本guid
        /// </summary>
        [Required(ErrorMessage ="富文本guid必填")]
        public string TextGuid { get; set; }

        /// <summary>
        /// 富文本内容
        /// </summary>
        [Required(ErrorMessage = "富文本内容必填")]
        public string Content { get; set; }

    }
}
