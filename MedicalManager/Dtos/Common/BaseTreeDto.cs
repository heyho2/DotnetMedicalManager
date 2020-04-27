using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Common
{
    /// <summary>
    /// 树基类
    /// </summary>
    public abstract class BaseTreeDto<T> : BaseDto where T : BaseTreeDto<T>
    {
        /// <summary>
        /// 子集
        /// </summary>
        public IList<T> Children { get; set; }
    }
}
