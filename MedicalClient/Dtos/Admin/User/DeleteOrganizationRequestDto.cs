using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Admin.User
{
    /// <summary>
    /// 删除组织架构 请求
    /// </summary>
    public class DeleteOrganizationRequestDto : BaseDto
    {
        /// <summary>
        /// guid
        /// </summary>
        public string OrgGuid { get; set; }
    }
}
