using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Manager.Recommend
{
    /// <summary>
    /// 获取推荐类型 项
    /// </summary>
    public class GetRecommendTypesItemDto
    {
        /// <summary>
        /// code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
