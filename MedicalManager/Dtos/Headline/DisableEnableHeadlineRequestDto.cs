using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Headline
{
    /// <summary>
    /// 修改角色 请求
    /// </summary>
    public class DisableEnableHeadlineRequestDto : BaseDto
    {
        /// <summary>
        /// id
        /// </summary>
        [Required]
        public string Guid { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Enable { get; set; }
    }
}
