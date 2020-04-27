using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Hospital
{
    /// <summary>
    /// 获取排班周期列表：默认获取上月和上月之后的排班周期数据
    /// </summary>
    public class GetCycleListResponseDto : BaseDto
    {
        /// <summary>
        /// 周期guid
        /// </summary>
        public string CycleGuid { get; set; }

        /// <summary>
        /// 周期显示名
        /// </summary>
        public string CycleDisplay { get; set; }

        /// <summary>
        /// 是否是当前月
        /// </summary>
        public bool IsCurrentMonth { get; set; }
    }
}
