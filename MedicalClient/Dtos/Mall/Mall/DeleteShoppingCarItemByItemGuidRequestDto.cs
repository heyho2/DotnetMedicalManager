using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Mall.Mall
{
    /// <inheritdoc />
    /// <summary>
    /// 获取指定购物车列表请求Dto
    /// </summary>
    public class DeleteShoppingCarItemByItemGuidRequestDto : BaseDto
    {
        /// <summary>
        /// 购物车记录Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "订单地址")]
        public string[] ItemGuidListStr { get; set; }

    }
}
