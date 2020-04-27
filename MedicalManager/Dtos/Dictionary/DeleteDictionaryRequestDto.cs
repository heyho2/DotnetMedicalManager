using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Dictionary
{
    /// <summary>
    /// 删除字典
    /// </summary>
    public class DeleteDictionaryRequestDto : BaseDto
    {
        /// <summary>
        /// Guid
        /// </summary>
        public string Guid { get; set; }
    }
}
