using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos
{
    /// <summary>
    /// 排序
    /// </summary>
    public interface IBaseOrderBy
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        string SortField { get; set; }
        /// <summary>
        /// 是否升序
        /// </summary>
        bool IsAscending { get; set; }
    }
}
