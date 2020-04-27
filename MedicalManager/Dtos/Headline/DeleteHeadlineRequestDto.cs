using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Headline
{
    /// <summary>
    /// 删除头条 请求
    /// </summary>
    public class DeleteHeadlineRequestDto : BaseDto
    {
        /// <summary>
        /// guid
        /// </summary>
        public string Guid { get; set; }
    }
}
