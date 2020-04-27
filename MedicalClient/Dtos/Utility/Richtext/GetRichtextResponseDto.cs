using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Utility.Richtext
{
    /// <summary>
    /// 获取富文本响应dto
    /// </summary>
    public class GetRichtextResponseDto : BaseDto
    {
        /// <summary>
        /// 富文本guid
        /// </summary>
        public string TextGuid { get; set; }

        /// <summary>
        /// 富文本内容
        /// </summary>
        public string Content { get; set; }


    }
}
