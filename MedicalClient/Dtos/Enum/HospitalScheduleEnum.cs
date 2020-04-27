using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GD.Dtos.Enum.HospitalScheduleEnum
{
    public enum WorkshiftTypeEnum
    {
        /// <summary>
        /// 上午班别
        /// </summary>
        [Description("上午")]
        AM = 1,
        /// <summary>
        /// 下午班别
        /// </summary>
        [Description("下午")]
        PM
    }
}
