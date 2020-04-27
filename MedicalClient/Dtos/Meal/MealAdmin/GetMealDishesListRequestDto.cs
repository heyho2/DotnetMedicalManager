using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Meal.MealAdmin
{
    /// <summary>
    /// 获取菜品分页列表
    /// </summary>
    public class GetMealDishesListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 医院GUID
        /// </summary>
        [Required(ErrorMessage = "医院参数需提供")]
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 菜品名称
        /// </summary>
        public string DishesName { get; set; }
    }


    /// <summary>
    /// 菜品响应信息
    /// </summary>
    public class GetMealDishesListResponseDto : BasePageResponseDto<GetMealDishesItem>
    {

    }

    /// <summary>
    /// 每个菜品信息
    /// </summary>
    public class GetMealDishesItem : BaseDto
    {
        /// <summary>
        /// 菜品GUID
        /// </summary>
        public string DishesGuid { get; set; }

        /// <summary>
        /// 菜品图片路径
        /// </summary>
        public string DishesImgPath { get; set; }

        /// <summary>
        /// 菜品图片GUID
        /// </summary>
        public string DishesImg { get; set; }

        /// <summary>
        /// 菜品名称
        /// </summary>
        public string DishesName { get; set; }

        /// <summary>
        /// 内部价
        /// </summary>
        public decimal DishesInternalPrice { get; set; }

        ///<summary>
        ///菜品外部价
        ///</summary>
        public decimal DishesExternalPrice { get; set; }


        ///<summary>
        ///菜品介绍
        ///</summary>
        public string DishesDescription { get; set; }

        ///<summary>
        ///是否在售
        ///</summary>
        public sbyte DishesOnsale { get; set; }
    }
}
