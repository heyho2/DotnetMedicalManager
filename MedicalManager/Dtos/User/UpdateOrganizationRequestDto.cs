using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.User
{
    /// <summary>
    /// 修改组织架构 请求
    /// </summary>
    public class UpdateOrganizationRequestDto : BaseDto
    {
        /// <summary>
        /// guid
        /// </summary>
        public string OrgGuid { get; set; }
        /// <summary>
        /// 上级guid
        /// </summary>
        public string ParentGuid { get; set; }

        ///<summary>
        ///组织名称
        ///</summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public string Sort { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
    }
}
