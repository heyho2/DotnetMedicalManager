using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Admin.Dictionary
{
    /// <summary>
    /// 禁用Dictionary
    /// </summary>
    public class DisableEnableDictionaryRequestDto : BaseDto
    {
        /// <summary>
        /// id
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// Enable
        /// /// </summary>
        public bool Enable { get; set; }
    }
}
