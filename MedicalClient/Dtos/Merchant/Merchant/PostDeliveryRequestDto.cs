using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 提交发货
    /// </summary>
    public class PostDeliveryRequestDto : BaseDto
    {
        /// <summary>
        /// 订单guid
        /// </summary>
        [Required(ErrorMessage = "订单guid必填")]
        public string OrderGuid { get; set; }

        ///<summary>
        ///快递公司
        ///</summary>
        [Required(ErrorMessage = "快递公司必填")]
        public string ExpressCompany { get; set; }

        ///<summary>
        ///快递单号
        ///</summary>
        [Required(ErrorMessage = "快递单号必填")]
        public string ExpressNo { get; set; }
    }
}
