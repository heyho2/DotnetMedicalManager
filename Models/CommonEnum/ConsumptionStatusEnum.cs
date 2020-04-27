using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GD.Models.CommonEnum
{
    /// <summary>
    /// 消费记录状态枚举
    /// </summary>
    public enum ConsumptionStatusEnum
    {
        /// <summary>
        /// 已预约
        /// </summary>
        [Description("已预约")]
        Booked = 1,

        /// <summary>
        /// 已到店
        /// </summary>
        [Description("已到店")]
        Arrive,

        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Completed,

        /// <summary>
        /// 已取消
        /// </summary>
        [Description("已取消")]
        Canceled,

        /// <summary>
        /// 已错过
        /// </summary>
        [Description("已错过")]
        Miss
    }
}
