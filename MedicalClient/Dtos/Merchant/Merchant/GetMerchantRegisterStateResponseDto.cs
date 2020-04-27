using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 查看当前用户是否注册商户,若注册提供注册状态 响应Dto
    /// </summary>
    public class GetMerchantRegisterStateResponseDto:BaseDto
    {
        /// <summary>
        /// 是否注册
        /// </summary>
        public bool WhetherRegister { get; set; }

        /// <summary>
        /// 注册状态： 'reject' 驳回,'approved' 通过审核,'submit' 审核中,'draft' 草稿
        /// </summary>
        public string RegisterState { get; set; }

        /// <summary>
        /// 审批备注
        /// </summary>
        public string ApprovalMessage { get; set; }
    }
}
