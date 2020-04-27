using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Meal
{
    ///<summary>
    ///
    ///</summary>
    [Table("t_meal_account_trade_detail")]
    public class MealAccountTradeDetailModel : BaseModel
    {

        ///<summary>
        ///GUID
        ///</summary>
        [Column("account_trade_detail_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "GUID")]
        public string AccountTradeDetailGuid { get; set; }

        ///<summary>
        ///账户GUID
        ///</summary>
        [Column("account_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "账户GUID")]
        public string AccountGuid { get; set; }

        ///<summary>
        ///交易流水GUID
        ///</summary>
        [Column("account_trade_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "交易流水GUID")]
        public string AccountTradeGuid { get; set; }

        ///<summary>
        ///指定账户交易金额
        ///</summary>
        [Column("account_trade_fee")]
        public decimal AccountTradeFee { get; set; }

    }
}



