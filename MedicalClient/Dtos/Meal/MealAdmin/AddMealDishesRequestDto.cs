using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Meal.MealAdmin
{
    /// <summary>
    /// 创建菜品请求
    /// </summary>
    public class AddMealDishesRequestDto : BaseDto
    {
        ///<summary>
        ///GUID
        ///</summary>
        public string DishesGuid { get; set; }

        ///<summary>
        ///医院GUID
        ///</summary>
        [Required(ErrorMessage = "医院参数需提供"),]
        public string HospitalGuid { get; set; }

        ///<summary>
        ///菜品名称
        ///</summary>
        [Required(ErrorMessage = "菜品名称必填")]
        public string DishesName { get; set; }

        ///<summary>
        ///菜品图片
        ///</summary>
        [Required(ErrorMessage = "菜品图片需提供")]
        public string DishesImg { get; set; }

        ///<summary>
        ///菜品内部价
        ///</summary>
        public decimal DishesInternalPrice { get; set; }

        ///<summary>
        ///菜品外部价
        ///</summary>
        public decimal DishesExternalPrice { get; set; }

        ///<summary>
        ///菜品介绍
        ///</summary>
        public string DishesDescription { get; set; }
    }
}
