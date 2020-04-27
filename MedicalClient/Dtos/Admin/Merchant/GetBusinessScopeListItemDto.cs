using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Admin.Merchant
{
    /// <summary>
    /// 获取经范围
    /// </summary>
    public class GetBusinessScopeListItemDto : BaseDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// guid
        /// </summary>
        public string Guid { get; set; }
    }
}
