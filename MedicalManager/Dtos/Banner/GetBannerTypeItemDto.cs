using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Banner
{
    /// <summary>
    /// 获取banner类型
    /// </summary>
    public class GetBannerTypeItemDto : BaseDto
    {
        /// <summary>
        /// 类型Guid
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
