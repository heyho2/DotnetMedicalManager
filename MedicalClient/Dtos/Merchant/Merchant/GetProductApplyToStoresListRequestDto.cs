using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Merchant.Merchant
{
    /// <inheritdoc />
    /// <summary>
    ///生美-获取商品适用门店 请求Dto
    /// </summary>
    public class GetProductApplyToStoresListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 产品Guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable { get; set; } = true;
    }
    /// <inheritdoc />
    /// <summary>
    /// 响应Dto
    /// </summary>
    public class GetProductApplyToStoresListResponseDto : BasePageResponseDto<GetProductApplyToStoresItem>
    {

    }
    /// <inheritdoc />
    /// <summary>
    /// 子项
    /// </summary>
    public class GetProductApplyToStoresItem : BaseDto
    {
        /// <summary>
        /// 主键Guid
        /// </summary>
        public string ProjectMerchantGuid { get; set; }
        /// <summary>
        /// 商户guid
        /// </summary>
        public string MerchantGuid { get; set; }
        /// <summary>
        /// 商户名
        /// </summary>
        public string MerchantName { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string MerchantAddress { get; set; }


    }
}