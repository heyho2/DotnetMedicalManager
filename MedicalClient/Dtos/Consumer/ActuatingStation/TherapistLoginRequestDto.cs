using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Consumer.ActuatingStation
{
    /// <summary>
    /// 美疗师执行端登录请求Dto
    /// </summary>
    public class TherapistLoginRequestDto : BaseDto
    {
        /// <summary>
        /// 美疗师账号手机号
        /// </summary>
        [Required(ErrorMessage ="手机号必填")]
        public string TherapistPhone { get; set; }

        /// <summary>
        /// 美疗师账号密码
        /// </summary>
        [Required(ErrorMessage = "密码必填")]
        public string TherapistPassword { get; set; }


    }
}
