using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GD.Dtos.Article
{
    /// <summary>
    /// 发布状态
    /// </summary>
    public enum ReleaseStatus
    {
        /// <summary>
        /// 发布
        /// </summary>
        [Description("发布")]
        Release = 1,

        /// <summary>
        /// 未发布
        /// </summary>
        [Description("未发布")]
        NoRelease,

        /// <summary>
        /// 驳回
        /// </summary>
        [Description("驳回")]
        Reject,

        /// <summary>
        /// 审核通过
        /// </summary>
        [Description("审核通过")]
        ReviewPass
    }
}
