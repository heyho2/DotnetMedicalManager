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
    public class DisableEnableOrganizationRequestDto : BaseDto
    {
        /// <summary>
        /// 角色id
        /// </summary>
        [Required]
        public string OrgGuid { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Enable { get; set; }
    }
}
