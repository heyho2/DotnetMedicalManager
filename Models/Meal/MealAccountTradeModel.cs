using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Meal
{
    ///<summary>
    ///
    ///</summary>
    [Table("t_meal_account_trade")]
    public class MealAccountTradeModel : BaseModel
    {

        ///<summary>
        ///GUID
        ///</summary>
        [Column("account_trade_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "GUID")]
        public string AccountTradeGuid { get; set; }

        ///<summary>
        ///
        ///</summary>
        [Column("order_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "")]
        public string OrderGuid { get; set; }

        ///<summary>
        ///交易类型。0：消费，1：退款
        ///</summary>
        [Column("account_trade_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "交易类型。0：消费，1：退款")]
        public sbyte AccountTradeType { get; set; }

        ///<summary>
        ///交易总金额
        ///</summary>
        [Column("account_trade_fee")]
        public decimal AccountTradeFee { get; set; }

        ///<summary>
        ///交易描述
        ///</summary>
        [Column("account_trade_description")]
        public string AccountTradeDescription { get; set; }

    }
    /// <summary>
    /// 点餐订单交易类型
    /// </summary>
    public enum MealAccountTradeTypeEnum
    {
        /// <summary>
        /// 消费
        /// </summary>
        Consumer=0,

        /// <summary>
        /// 退款
        /// </summary>
        Refund=1
    }
}



