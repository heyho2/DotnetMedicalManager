using GD.Common.Base;
using GD.Dtos.Doctor.Doctor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 审核商户请求Dto
    /// </summary>
    public class AuditMerchantRegisterInfoRequestDto:BaseDto
    {
        /// <summary>
        /// 商户Id
        /// </summary>
        [Required]
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 审核状态 驳回0；同意1；提交2；草稿3
        /// </summary>
        [Required]
        public StatusEnum Status { get; set; }
    }
}
