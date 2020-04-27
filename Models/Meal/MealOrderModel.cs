using GD.Common.Base;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Meal
{
    ///<summary>
    /// 点餐订单表实体
    ///</summary>
    [Table("t_meal_order")]
    public class MealOrderModel : BaseModel
    {

        ///<summary>
        ///订单GUID
        ///</summary>
        [Column("order_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "订单GUID")]
        public string OrderGuid { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [Column("order_no")]
        public string OrderNo { get; set; }

        ///<summary>
        ///餐别GUID
        ///</summary>
        [Column("category_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "餐别GUID")]
        public string CategoryGuid { get; set; }

        ///<summary>
        ///餐别名称
        ///</summary>
        [Column("category_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "餐别名称")]
        public string CategoryName { get; set; }

        ///<summary>
        ///用餐日期
        ///</summary>
        [Column("meal_date"), Required(ErrorMessage = "{0}必填"), Display(Name = "用餐日期")]
        public DateTime MealDate { get; set; }


        ///<summary>
        ///用餐开始时间
        ///</summary>
        [Column("meal_start_time")]
        public DateTime MealStartTime { get; set; }

        ///<summary>
        ///用餐结束时间
        ///</summary>
        [Column("meal_end_time")]
        public DateTime MealEndTime { get; set; }

        /// <summary>
        /// 就餐医院guid
        /// </summary>
        [Column("hospital_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "就餐医院guid")]
        public string HospitalGuid { get; set; }

        ///<summary>
        ///用户GUID
        ///</summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户GUID")]
        public string UserGuid { get; set; }

        ///<summary>
        ///已付款/已取餐/已取消/已过期/已转让
        ///</summary>
        [Column("order_status"), Required(ErrorMessage = "{0}必填"), Display(Name = "已付款/已取餐/已取消/已过期/已转让")]
        public string OrderStatus { get; set; }

        ///<summary>
        ///菜品数量
        ///</summary>
        [Column("quantity")]
        public int Quantity { get; set; }

        ///<summary>
        ///总价
        ///</summary>
        [Column("total_price")]
        public decimal TotalPrice { get; set; }

        ///<summary>
        ///已转让订单的来源订单GUID
        ///</summary>
        [Column("transferred_from")]
        public string TransferredFrom { get; set; }

        ///<summary>
        ///已转让订单的去向订单GUID
        ///</summary>
        [Column("transferred_to")]
        public string TransferredTo { get; set; }
    }
    /// <summary>
    /// 点餐订单状态
    /// </summary>
    public enum MealOrderStatusEnum
    {
        /// <summary>
        /// 已付款
        /// </summary>
        [Description("已付款")]
        Paided,

        /// <summary>
        /// 已取餐
        /// </summary>
        [Description("已取餐")]
        Completed,

        /// <summary>
        /// 已取消
        /// </summary>
        [Description("已取消")]
        Canceled,

        /// <summary>
        /// 已过期
        /// </summary>
        [Description("已过期")]
        Expired,

        /// <summary>
        /// 已转让
        /// </summary>
        [Description("已转让")]
        Transferred
    }
}



