using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Decoration
{
    /// <summary>
    /// 设置装修记录的启用状态
    /// </summary>
    public class DisableEnableDecorationRequestDto : BaseDto
    {
        /// <summary>
        /// 装修记录guid
        /// </summary>
        public string DecorationGuid { get; set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        public bool Status { get; set; }
    }
}
