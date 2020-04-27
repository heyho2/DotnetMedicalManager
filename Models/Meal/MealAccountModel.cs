using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Meal
{
    ///<summary>
    ///点餐用户钱包账户
    ///</summary>
    [Table("t_meal_account")]
    public class MealAccountModel : BaseModel
    {

        ///<summary>
        ///GUID
        ///</summary>
        [Column("account_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "GUID")]
        public string AccountGuid { get; set; }

        ///<summary>
        ///账户所属医院guid
        ///</summary>
        [Column("hospital_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "账户所属医院guid")]
        public string HospitalGuid { get; set; }

        ///<summary>
        ///用户GUID
        ///</summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户GUID")]
        public string UserGuid { get; set; }


        ///<summary>
        ///用户类型，内部职工，外部人员
        ///</summary>
        [Column("user_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户类型，0：内部职工，1：外部人员")]
        public string UserType { get; set; }

        ///<summary>
        ///赠款，充值
        ///</summary>

        [Column("account_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "赠款，充值")]
        public string AccountType { get; set; }

        ///<summary>
        ///账户余额
        ///</summary>
        [Column("account_balance")]
        public decimal AccountBalance { get; set; }

    }

    /// <summary>
    /// 点餐用户类型
    /// </summary>
    public enum MealUserTypeEnum
    {
        /// <summary>
        /// 内部人员
        /// </summary>
        Internal = 1,

        /// <summary>
        /// 外部人员
        /// </summary>
        External
    }

    /// <summary>
    /// 点餐钱包账户类型
    /// </summary>
    public enum MealAccountTypeEnum
    {

        /// <summary>
        /// 个人充值款账户
        /// </summary>
        Recharge = 1,

        /// <summary>
        /// 赠款账户
        /// </summary>
        Grant
    }
}



