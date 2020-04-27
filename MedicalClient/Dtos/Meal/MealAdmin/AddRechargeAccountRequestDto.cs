using GD.Models.Meal;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Meal.MealAdmin
{
    /// <summary>
    /// 个人充值账户请求
    /// </summary>
    public class AddRechargeAccountRequestDto
    {
        ///<summary>
        ///账户所属医院guid
        ///</summary>
        [Required(ErrorMessage = "医院参数必填")]
        public string HospitalGuid { get; set; }

        ///<summary>
        ///用户GUID
        ///</summary>
        public string UserGuid { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public MealAccountDetailTypeEnum Type { get; set; } = MealAccountDetailTypeEnum.Recharge;

        /// <summary>
        /// 操作金额
        /// </summary>
        public decimal Fee { get; set; }
    }
}
