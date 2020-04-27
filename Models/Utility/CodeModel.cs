using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Utility
{
    /// <summary>
    /// 最大自增id存储
    /// </summary>
    [Table("t_utility_code")]
    public class CodeModel
    {
        /// <summary>
        /// id
        /// </summary>
        [Column("id"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "id")]
        public int Id { get; set; }
        /// <summary>
        /// 优惠券，自增最大id
        /// </summary>
        [Column("meal_order_max_id"), Required(ErrorMessage = "{0}必填"), Display(Name = "点餐订单，自增最大id")]
        public int MealOrderMaxId { get; set; }
        /// <summary>
        /// 字典，自增最大id
        /// </summary>
        [Column("dictionary_max_id"), Required(ErrorMessage = "{0}必填"), Display(Name = "字典，自增最大id")]
        public int DictionaryMaxId { get; set; }
    }
}
