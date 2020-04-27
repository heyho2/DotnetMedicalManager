using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Decoration
{
    /// <summary>
    /// 获取装修记录信息
    /// </summary>
    public class GetDecorationResponseDto : BaseDto
    {
        /// <summary>
        /// 专题标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 装修记录行内容
        /// </summary>
        public List<GetDecorationContentResponseDto> Contents { get; set; }
    }
}
