using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Merchant.Therapist
{
    /// <summary>
    /// 更新服务人员OpenId
    /// </summary>
    public class UpdateTherapistOpenIdRequestDto : BaseDto
    {
        /// <summary>
        /// 网页授权code
        /// </summary>
        [Required(ErrorMessage = "网页授权code必填")]
        public string Code { get; set; }
    }
}
