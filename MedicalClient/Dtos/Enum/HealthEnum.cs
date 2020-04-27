using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Enum
{
    /// <summary>
    /// 健康指标预警状态枚举
    /// </summary>
    public enum IndicatorWarningStatusEnum
    {
        /// <summary>
        /// 待处理
        /// </summary>
        Pending = 1,
        /// <summary>
        /// 已过期
        /// </summary>
        Expired,
        /// <summary>
        /// 已关闭
        /// </summary>
        Closed
    }
}
