using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Common
{
    /// <summary>
    /// 枚举类型
    /// </summary>
    public class EnumItemDto : BaseDto
    {
        /// <summary>
        /// code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// name
        /// </summary>
        public string Name { get; set; }
    }
}
