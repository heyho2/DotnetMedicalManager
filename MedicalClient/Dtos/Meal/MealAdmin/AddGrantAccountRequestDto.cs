using GD.Common.Base;
using GD.Models.Meal;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Meal.MealAdmin
{
    /// <summary>
    /// 指定医院所有赠款账户请求
    /// </summary>
    public class AddAllGrantAccountRequestDto : BaseDto
    {
        ///<summary>
        ///账户所属医院guid
        ///</summary>
        [Required(ErrorMessage = "医院参数必填")]
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 操作类型 "recharge"：充值，"deduction":扣减
        /// </summary>
        public MealAccountDetailTypeEnum Type { get; set; } = MealAccountDetailTypeEnum.Recharge;

        /// <summary>
        ///操作金额
        /// </summary>
        public decimal Fee { get; set; }
    }

    /// <summary>
    /// 指定医院指定赠款账户请求
    /// </summary>
    public class AddGrantAccountRequestDto : AddAllGrantAccountRequestDto
    {
        ///<summary>
        ///用户GUID
        ///</summary>
        [Required(ErrorMessage = "需选择赠款账户")]
        public string UserGuid { get; set; }
    }
}
