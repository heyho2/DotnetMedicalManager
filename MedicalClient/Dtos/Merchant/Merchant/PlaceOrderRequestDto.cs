using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 门店开单请求dto
    /// </summary>
    public class PlaceOrderRequestDto : BaseDto
    {
        /// <summary>
        /// 门店guid
        /// </summary>
        public string MerchantGuid { get; set; }

        /// <summary>
        /// 用户手机号
        /// </summary>
        public string UserPhone { get; set; }

        /// <summary>
        /// 商品列表
        /// </summary>
        public List<PlaceOrderProduct> Products { get; set; }

        /// <summary>
        /// 商品
        /// </summary>
        public class PlaceOrderProduct
        {
            /// <summary>
            /// 商品guid
            /// </summary>
            public string ProductGuid { get; set; }

            /// <summary>
            /// 商品数量
            /// </summary>
            public int ProductNum { get; set; }
        }
    }
}
