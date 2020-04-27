using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Mall.Mall
{
    /// <inheritdoc />
    /// <summary>
    /// 获取用户购物车记录总数
    /// </summary>
    public class GetMyShoppingCarTotalResponseDto:BaseModel
    {
        /// <summary>
        /// 购物车数量
        /// </summary>
        public int ShoppingCarNum { get; set; }
    }
}
