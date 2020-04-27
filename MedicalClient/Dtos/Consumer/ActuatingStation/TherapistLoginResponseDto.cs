using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.ActuatingStation
{
    /// <summary>
    /// 美疗师登录响应Dto
    /// </summary>
    public class TherapistLoginResponseDto : BaseDto
    {
        /// <summary>
        /// 美疗师guid
        /// </summary>
        public string TherapistGuid { get; set; }

        /// <summary>
        /// 美疗师姓名
        /// </summary>
        public string TherapistName { get; set; }

        /// <summary>
        /// 美疗师guid
        /// </summary>
        public string Token { get; set; }

    }
}
