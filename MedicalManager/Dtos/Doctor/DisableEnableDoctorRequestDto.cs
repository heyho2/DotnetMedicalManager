using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor
{
    /// <summary>
    /// 禁用医生
    /// </summary>
    public class RecommendDoctorRequestDto : BaseDto
    {
        /// <summary>
        /// 推荐
        /// </summary>
        public bool IsRecommend { get; set; }
        /// <summary>
        /// Guid
        /// </summary>
        public string Guid { get; set; }
    }
}
