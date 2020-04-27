using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GD.Models.CommonEnum
{
    /// <summary>
    /// 收藏状态
    /// </summary>
    public enum CollectionStateEnum
    {
        /// <summary>
        /// 收藏
        /// </summary>
        [Description("收藏")]
        Establish = 1,
        /// <summary>
        /// 取消
        /// </summary>
        [Description("取消收藏")]
        Cancel = 2
    }
}
