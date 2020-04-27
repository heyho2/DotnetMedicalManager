using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Common
{
    /// <summary>
    /// 枚举类型
    /// </summary>
    public class EnumItem2Dto : BaseDto
    {
        /// <summary>
        /// code
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// name
        /// </summary>
        public string Name { get; set; }
    }
}
