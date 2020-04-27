using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Merchant
{
    /// <summary>
    /// 禁用医生
    /// </summary>
    public class DisableEnableMerchantRequestDto : BaseDto
    {
        /// <summary>
        /// Enable
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// Guid
        /// </summary>
        public string Guid { get; set; }
    }
}
