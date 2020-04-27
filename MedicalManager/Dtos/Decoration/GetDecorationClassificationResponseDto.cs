using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Decoration
{
    /// <summary>
    /// 获取装修记录分类列表
    /// </summary>
    public class GetDecorationClassificationResponseDto : BaseDto
    {
        /// <summary>
        /// 分类guid
        /// </summary>
        public string ClassificationGuid { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string ClassificationName { get; set; }
    }
}
