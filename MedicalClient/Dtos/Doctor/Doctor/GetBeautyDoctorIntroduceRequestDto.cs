using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Doctor.Doctor
{
    /// <inheritdoc />
    /// <summary>
    ///生美-获取美疗师介绍 请求Dto
    /// </summary>
    public class GetBeautyDoctorIntroduceRequestDto 
    {
        /// <summary>
        /// 美疗师Guid
        /// </summary>
        public string TherapistGuid { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable { get; set; } = true;
    }
    /// <inheritdoc />
    /// <summary>
    /// 响应Dto
    /// </summary>
    public class GetBeautyDoctorIntroduceResponseDto : BaseDto
    {
        /// <summary>
        /// guid
        /// </summary>
        public string ProjectGuid { get; set; }
    }
  
}
