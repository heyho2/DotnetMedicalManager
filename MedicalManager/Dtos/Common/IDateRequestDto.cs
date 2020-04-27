using System;

namespace GD.Dtos.Common
{
    /// <summary>
    /// 时间搜索
    /// </summary>
    public interface IDateRequestDto
    {
        /// <summary>
        /// 时间
        /// </summary>
        DateTime? BeginDate { get; set; }
        /// <summary>
        /// 时间 至
        /// </summary>
        DateTime? EndDate { get; set; }
    }
}
