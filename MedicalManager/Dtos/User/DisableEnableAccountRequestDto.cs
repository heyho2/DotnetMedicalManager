using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.User
{
    /// <summary>
    /// 修改角色 请求
    /// </summary>
    public class DisableEnableAccountRequestDto : BaseDto
    {
        /// <summary>
        /// guid
        /// </summary>
        [Required]
        public string UserGuid { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Enable { get; set; }
    }
}
