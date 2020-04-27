using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Mall.Mall
{
    /// <inheritdoc />
    /// <summary>
    /// 取消订单 请求Dto
    /// </summary>
    public class CancelOrderRequestDto:BaseDto
    {
        /// <summary>
        /// 订单Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "订单Guid")]
        public string OrderGuid { get; set; }
    }



}
