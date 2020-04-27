using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Dictionary
{
    /// <summary>
    /// 元数据
    /// </summary>
    public class GetDictionaryMatedataItemDto : BaseDto
    {
        /// <summary>
        /// name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// guid
        /// </summary>
        public string Guid { get; set; }
    }
}
