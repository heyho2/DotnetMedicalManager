using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Hospital
{
    /// <summary>
    /// 今日接诊人次统计响应
    /// </summary>
    public class GetReceptionPersonTimeTodayStatisticsResponseDto : BaseDto
    {
        /// <summary>
        /// 接诊总人数
        /// </summary>
        public int ReceptionTotal { get; set; }

        /// <summary>
        /// 较昨日上涨百分比,例如上涨为25.5%，则此处的值为25.5
        /// </summary>
        public decimal Increase { get; set; }
    }
}
