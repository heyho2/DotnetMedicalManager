using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Meal.MealClient
{
    /// <summary>
    /// 钱包余额信息响应Dto
    /// </summary>
    public class GetMealWalletBalanceResponseDto : BaseDto
    {
        /// <summary>
        /// 账户总额
        /// </summary>
        public decimal TotalBalance { get; set; }

        /// <summary>
        /// 充值款余额
        /// </summary>
        public decimal RechargeBalance { get; set; }

        /// <summary>
        /// 赠款余额
        /// </summary>
        public decimal GrantBalance { get; set; }

        /// <summary>
        /// 是否是内部用户
        /// </summary>
        public bool IsInternal { get; set; } = false;
    }
}
