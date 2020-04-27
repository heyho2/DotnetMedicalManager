using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 再次购买
    /// </summary>
    public class BuyProductListAgainRequest
    {
        /// <summary>
        /// 订单guid
        /// </summary>
        [Required(ErrorMessage = "订单Guid")]
        public string OrderGuid { get; set; }
       
    }
}
