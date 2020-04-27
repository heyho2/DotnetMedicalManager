using System;

namespace GD.Dtos.Meal.MealAdmin
{
    /// <summary>
    /// 食堂操作员列表
    /// </summary>
    public class GetMealOperatorListResponseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string OperatorGuid { get; set; }

        ///<summary>
        ///用户名（手机号码）
        ///</summary>
        public string UserName { get; set; }

        ///<summary>
        ///密码
        ///</summary>
        public string Password { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        ///最后一次登录时间
        /// </summary>
        public DateTime LastUpdatedDate { get; set; }
    }
}
