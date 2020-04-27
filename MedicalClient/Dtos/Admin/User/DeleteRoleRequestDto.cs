using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Admin.User
{
    /// <summary>
    /// 删除角色 请求
    /// </summary>
    public class DeleteRoleRequestDto : BaseDto
    {
        /// <summary>
        /// guid
        /// </summary>
        public string OrgGuid { get; set; }
    }
}
