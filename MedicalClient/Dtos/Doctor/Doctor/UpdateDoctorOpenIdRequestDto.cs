using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 更新医生openId
    /// </summary>
    public class UpdateDoctorOpenIdRequestDto : BaseDto
    {
        /// <summary>
        /// 网页授权code
        /// </summary>
        [Required(ErrorMessage = "网页授权code必填")]
        public string Code { get; set; }
    }
}
