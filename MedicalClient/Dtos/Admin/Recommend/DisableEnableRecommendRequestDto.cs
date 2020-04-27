using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Admin.Recommend
{
    /// <summary>
    /// 启用禁用 请求
    /// </summary>
    public class DisableEnableRecommendRequestDto : BaseDto
    {
        /// <summary>
        /// id
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// 启用禁用
        /// </summary>
        public bool Enable { get; set; }
    }
}
