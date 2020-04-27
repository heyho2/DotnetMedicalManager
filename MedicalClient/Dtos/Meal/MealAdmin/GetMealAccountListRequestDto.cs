using GD.Common.Base;
using GD.Models.Meal;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Meal.MealAdmin
{
    /// <summary>
    /// 用户钱包账户列表
    /// </summary>
    public class GetMealAccountListRequestDto : BasePageRequestDto
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
    }



    /// <summary>
    ///账户明细响应
    /// </summary>
    public class GetMealAccountListResponseDto<T> where T : class
    {
        /// <summary>
		/// 当前页数据
		/// </summary>
		public IEnumerable<T> CurrentPage
        {
            get;
            set;
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total
        {
            get;
            set;
        }

    }

    /// <summary>
    /// 账户明细具体信息
    /// </summary>
    public class MealAccountItem
    {
        /// <summary>
        ///用户GUID
        /// </summary>
        public string UserGuid { get; set; }

        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 用户类型（1：内部人员，2：外部人员）
        /// </summary>
        public MealUserTypeEnum UserType { get; set; } = MealUserTypeEnum.External;

        /// <summary>
        /// 账户类型
        /// </summary>
        [JsonIgnore]
        public MealAccountTypeEnum AccountType { get; set; } = MealAccountTypeEnum.Recharge;

        /// <summary>
        /// 是否禁用，0：禁用，1：启用
        /// </summary>
        public bool Enable { get; set; } = true;

        /// <summary>
        /// 账户余额
        /// </summary>
        [JsonIgnore]
        public decimal Accountbalance { get; set; }

        /// <summary>
        /// 个人充值金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? RechargeBalance { get; set; }

        /// <summary>
        /// 赠款账户赠款金额
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? GrantBalance { get; set; }
    }
}
