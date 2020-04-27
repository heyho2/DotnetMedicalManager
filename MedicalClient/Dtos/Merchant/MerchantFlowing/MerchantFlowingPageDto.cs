using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Merchant
{
    /// <summary>
    /// 商户流水请求Dto
    /// </summary>
    public class MerchantFlowingPageRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 商户账号
        /// </summary>
        
        public string MerchantAccount { get; set; }

        /// <summary>
        /// 流水状态
        /// </summary>
        public string FlowStatus { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "开始时间")]

        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "结束时间")]
        public DateTime EndTime { get; set; }
    }

    /// <summary>
    /// 商户流水返回Dto
    /// </summary>
    public class MerchantFlowingPageResponseDto : BasePageResponseDto<MerchantFlowingPageItemDto>
    {

    }

    /// <summary>
    /// 商户流水返回项
    /// </summary>
    public class MerchantFlowingPageItemDto : BaseDto
    {
        /// <summary>
        /// 商户流水GUID
        /// </summary>
        public string MerchantFlowingGuid { get; set; }

        /// <summary>
        /// 用户支付流水GUID
        /// </summary>
        public string TransactionFlowingGuid { get; set; }

        /// <summary>
        /// 订单GUID
        /// </summary>
        public string OrderGuid { get; set; }

        /// <summary>
        /// 商户账号
        /// </summary>
        public string MerchantAccount { get; set; }

        /// <summary>
        /// 流水状态
        /// </summary>
        public string FlowStatus { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreationDate { get; set; }
    }
}