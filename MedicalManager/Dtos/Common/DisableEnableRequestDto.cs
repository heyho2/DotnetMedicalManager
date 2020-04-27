using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Common
{
    /// <summary>
    /// 禁用启用
    /// </summary>
    public class DisableEnableRequestDto : BaseDto
    {
        /// <summary>
        /// 是否禁用
        /// </summary>
        [Required]
        public bool Enable { get; set; }
        /// <summary>
        /// guid
        /// </summary>
        [Required]
        public string Guid { get; set; }
    }
}
