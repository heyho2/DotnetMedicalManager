using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.User
{
    /// <summary>
    /// 组织架构列表 请求
    /// </summary>
    public class AddOrganizationRequestDto : BaseDto
    {
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
