using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Meal
{
    ///<summary>
    ///点餐菜品
    ///</summary>
    [Table("t_meal_dishes")]
    public class MealDishesModel : BaseModel
    {

        ///<summary>
        ///GUID
        ///</summary>
        [Column("dishes_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "GUID")]
        public string DishesGuid { get; set; }

        ///<summary>
        ///医院GUID
        ///</summary>
        [Column("hospital_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "医院GUID")]
        public string HospitalGuid { get; set; }

        ///<summary>
        ///菜品名称
        ///</summary>
        [Column("dishes_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "菜品名称")]
        public string DishesName { get; set; }

        ///<summary>
        ///菜品图片
        ///</summary>
        [Column("dishes_img")]
        public string DishesImg { get; set; }

        ///<summary>
        ///菜品内部价
        ///</summary>
        [Column("dishes_internal_price"), Required(ErrorMessage = "{0}必填"), Display(Name = "菜品内部价")]
        public decimal DishesInternalPrice { get; set; }

        ///<summary>
        ///菜品外部价
        ///</summary>
        [Column("dishes_external_price"), Required(ErrorMessage = "{0}必填"), Display(Name = "菜品外部价")]
        public decimal DishesExternalPrice { get; set; }

        ///<summary>
        ///菜品介绍
        ///</summary>
        [Column("dishes_description")]
        public string DishesDescription { get; set; }

        ///<summary>
        ///是否在售
        ///</summary>
        [Column("dishes_onsale")]
        public sbyte DishesOnsale { get; set; }

    }
}



