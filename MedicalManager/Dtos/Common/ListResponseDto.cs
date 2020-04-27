using GD.Common.Base;
using System.Collections.Generic;

namespace GD.Dtos.Common
{
    /// <summary>
    /// 列表基类
    /// </summary>
    public abstract class BaseListResponseDto<T> where T : BaseDto
    {
        /// <summary>
        /// 列表
        /// </summary>
        public IEnumerable<T> Items { get; set; }
    }
}
