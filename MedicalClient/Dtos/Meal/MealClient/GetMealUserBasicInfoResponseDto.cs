using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Meal.MealClient
{
    /// <summary>
    /// 获取点餐用户基础信息响应dto
    /// </summary>
    public class GetMealUserBasicInfoResponseDto : BaseDto
    {
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 点餐人员类型
        /// </summary>
        public string UserType { get; set; }

        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal BalanceTotal { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderTotal { get; set; }
    }
}
