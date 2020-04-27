using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Meal
{
    ///<summary>
    /// 点餐订单明细表实体
    ///</summary>
    [Table("t_meal_order_detail")]
    public class MealOrderDetailModel : BaseModel
    {
        ///<summary>
        ///订单明细GUID
        ///</summary>
        [Column("order_detail_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "订单明细GUID")]
        public string OrderDetailGuid { get; set; }

        ///<summary>
        ///订单GUID
        ///</summary>
        [Column("order_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "订单GUID")]
        public string OrderGuid { get; set; }

        ///<summary>
        ///菜品GUID
        ///</summary>
        [Column("dishes_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "菜品GUID")]
        public string DishesGuid { get; set; }

        ///<summary>
        ///菜品名称
        ///</summary>
        [Column("dishes_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "菜品名称")]
        public string DishesName { get; set; }

        ///<summary>
        ///数量
        ///</summary>
        [Column("quantity"), Required(ErrorMessage = "{0}必填"), Display(Name = "数量")]
        public int Quantity { get; set; }

        ///<summary>
        ///单价
        ///</summary>
        [Column("unit_price"), Required(ErrorMessage = "{0}必填"), Display(Name = "单价")]
        public decimal UnitPrice { get; set; }
    }
}



