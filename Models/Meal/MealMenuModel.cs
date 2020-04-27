
using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Meal
{
    ///<summary>
    ///点餐菜单
    ///</summary>
    [Table("t_meal_menu")]
    public class MealMenuModel : BaseModel
    {

        ///<summary>
        ///GUID
        ///</summary>
        [Column("menu_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "GUID")]
        public string MenuGuid { get; set; }

        /// <summary>
        /// hsopital_guid
        /// </summary>
        [Column("hospital_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "医院guid")]
        public string HospitalGuid { get; set; }

        ///<summary>
        ///餐别GUID
        ///</summary>
        [Column("category_guid")]
        public string CategoryGuid { get; set; }

        ///<summary>
        ///菜品GUID
        ///</summary>
        [Column("dishes_guid")]
        public string DishesGuid { get; set; }

        ///<summary>
        ///菜单日期
        ///</summary>
        [Column("menu_date"), Required(ErrorMessage = "{0}必填"), Display(Name = "选餐日期")]
        public DateTime MenuDate { get; set; }

    }
}



