using GD.Common.Base;
using GD.Models.Meal;
using System;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Meal.MealAdmin
{
    /// <summary>
    /// 账户明细请求
    /// </summary>
    public class GetMealAccountDetailListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 医院GUID
        /// </summary>
        [Required(ErrorMessage = "医院参数不正确")]
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }

    /// <summary>
    ///账户明细响应
    /// </summary>
    public class GetMealAccountDetailListResponseDto : BasePageResponseDto<MealAccountDetailItem>
    {

    }


    /// <summary>
    /// 账户明细具体信息
    /// </summary>
    public class MealAccountDetailItem : BaseDto
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 用户类型（1：内部人员，2：外部人员）
        /// </summary>
        public MealUserTypeEnum UserType { get; set; }
        /// <summary>
        /// 账户类型（1：充值账户，2：赠款账户）
        /// </summary>
        public MealAccountTypeEnum AccountType { get; set; }

        /// <summary>
        /// 操作金额
        /// </summary>
        public decimal Fee { get; set; }

        /// <summary>
        /// 消费类型(0：收入，1：支出)
        /// </summary>
        public bool InComeType { get; set; }

        /// <summary>
        /// 操作类型（"consume"：消费，"recharge"：充值，"deduction":扣减，"refund": 退款）
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 账户总额
        /// </summary>
        public decimal TotalFee { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}
