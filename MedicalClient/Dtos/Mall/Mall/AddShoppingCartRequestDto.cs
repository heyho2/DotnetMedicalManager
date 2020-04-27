using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 添加购物车
    /// </summary>
    public class AddShoppingCartRequestDto : BaseDto
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "产品Guid")]
        public string ProductGuid { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        [Display(Name = "产品名称")]
        public string ProductName { get; set; }
        /// <summary>
        /// 产品数量
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "产品数量")]
        public int ProductNum { get; set; }

    }
}
