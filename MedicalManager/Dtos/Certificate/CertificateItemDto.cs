using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Certificate
{
    /// <summary>
    /// 证书传入Dto
    /// </summary>
    public class CertificateItemDto : BaseDto
    {
        /// <summary>
        /// 证书配置项字典Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "证书配置项字典Guid")]
        public string DicGuid { get; set; }

        /// <summary>
        /// 附件Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "附件")]
        public string AccessoryGuid { get; set; }
    }
}
