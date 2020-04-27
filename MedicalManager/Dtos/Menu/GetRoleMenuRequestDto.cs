using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Menu
{
    /// <summary>
    /// 获取权限 请求
    /// </summary>
    public class GetRoleMenuRequestDto
    {
        /// <summary>
        /// 角色id
        /// </summary>
        [Required]
        public string RoleGuid { get; set; }
    }
}
