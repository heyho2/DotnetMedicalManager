using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Meal
{
    ///<summary>
    ///点餐用户钱包账户明细
    ///</summary>
    [Table("t_meal_account_detail")]
    public class MealAccountDetailModel : BaseModel
    {

        ///<summary>
        ///GUID
        ///</summary>
        [Column("account_detail_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "GUID")]
        public string AccountDetailGuid { get; set; }

        ///<summary>
        ///钱包账户GUID
        ///</summary>
        [Column("account_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "钱包账户GUID")]
        public string AccountGuid { get; set; }

        ///<summary>
        ///操作类型，"consume"：消费，"recharge"：充值，"deduction":扣减， "refund": 退款
        ///注意：取消订单、转让订单属于退款，加减通过收支类型来判断
        ///注意：红冲属于扣减
        ///</summary>
        [Column("account_detail_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "操作类型")]
        public string AccountDetailType { get; set; }

        ///<summary>
        ///收支类型，0：收入，1：支出
        ///</summary>
        [Column("account_detail_income_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "收支类型，0：收入，1：支出")]
        public sbyte AccountDetailIncomeType { get; set; }

        ///<summary>
        ///操作之前金额
        ///</summary>
        [Column("account_detail_before_fee"), Required(ErrorMessage = "{0}必填"), Display(Name = "操作之前金额")]
        public decimal AccountDetailBeforeFee { get; set; }

        ///<summary>
        ///操作金额
        ///</summary>
        [Column("account_detail_fee"), Required(ErrorMessage = "{0}必填"), Display(Name = "操作金额")]
        public decimal AccountDetailFee { get; set; }

        ///<summary>
        ///操作后金额
        ///</summary>
        [Column("account_detail_after_fee"), Required(ErrorMessage = "{0}必填"), Display(Name = "操作后金额")]
        public decimal AccountDetailAfterFee { get; set; }

        ///<summary>
        ///操作描述
        ///</summary>
        [Column("account_detail_description")]
        public string AccountDetailDescription { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("remark")]
        public string Remark { get; set; }

    }

    /// <summary>
    /// 点餐用户钱包账户明细流水类型枚举
    /// </summary>
    public enum MealAccountDetailTypeEnum
    {
        /// <summary>
        /// 消费相关：用餐消费、取消订单、转让订单属于消费
        /// </summary>
        Consume,

        /// <summary>
        /// 充值
        /// </summary>
        Recharge,

        /// <summary>
        /// 扣减：红冲属于扣减
        /// </summary>
        Deduction,

        /// <summary>
        /// 退款
        /// </summary>
        Refund

    }

    /// <summary>
    /// 点餐用户钱包账户明细流水- 收支类型，0：收入，1：支出
    /// </summary>
    public enum MealAccountDetailIncomeTypeEnum
    {
        /// <summary>
        /// 收入
        /// </summary>
        Income = 0,

        /// <summary>
        /// 支出
        /// </summary>
        Expenditure = 1
    }
}



