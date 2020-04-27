using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Meal.MealAdmin
{
    /// <summary>
    /// 修改操作员密码请求实体
    /// </summary>
    public class ModifyMealOperatorPasswordRequestDto : BaseDto
    {
        /// <summary>
        /// 医院Id
        /// </summary>
        [Required(ErrorMessage = "医院Id需提供")]
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 操作员标识
        /// </summary>
        [Required(ErrorMessage = "操作员标识需提供")]
        public string OperatorGuid { get; set; }

        ///<summary>
        ///密码
        ///</summary>
        [Required(ErrorMessage = "密码需提供")]
        public string Password { get; set; }
    }
}
