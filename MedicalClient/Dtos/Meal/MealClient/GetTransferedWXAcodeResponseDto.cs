using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Meal.MealClient
{
    /// <summary>
    /// 获取转让码相关信息
    /// </summary>
    public class GetTransferedWXAcodeResponseDto:BaseDto
    {
        /// <summary>
        /// 转让码：base64格式字符串
        /// </summary>
        public string TransferedCodeImg { get; set; }

        /// <summary>
        /// 餐别名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 就餐开始时间
        /// </summary>
        public DateTime MealStartTime { get; set; }

        /// <summary>
        /// 就餐截止时间
        /// </summary>
        public DateTime MealEndTime { get; set; }
    }
}
