using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Score
{
    /// <summary>
    /// 查询医生所有积分请求Dto
    /// </summary>
    public class GetDoctorScoreAllInfoRequestDto
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? startTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? endTime { get; set; }
    }
}
