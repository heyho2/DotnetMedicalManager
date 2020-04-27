using GD.Common.Base;
using System;

namespace GD.Dtos.Doctor.Score
{
    /// <summary>
    /// 获取医生积分请求DTO
    /// </summary>
    public class GetDoctorScoreRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 用户GUID
        /// </summary>
        public string userId { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public string userType { get; set; }

        /// <summary>
        /// 积分变化原因
        /// </summary>
        public string UserActionGuid { get; set; }

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
