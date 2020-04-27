using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Mall.Groupbuy
{
    /// <inheritdoc />
    /// <summary>
    ///生美-获取商品适用门店 请求Dto
    /// </summary>
    public class GetProductGroupBuyInfoRequestDto: BasePageRequestDto
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
    public class GetProductGroupBuyInfoResponseDto : BasePageResponseDto<GetProductGroupBuyInfoItem>
    {

    }
    /// <inheritdoc />
    /// <summary>
    /// 子项
    /// </summary>
    public class GetProductGroupBuyInfoItem : BaseDto
    {
        /// <summary>
        /// 团购名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 该团购总数量(团购限定人数)
        /// </summary>
        public string Qty { get; set; }
        /// <summary>
        /// 一个账号最低购买数量最低
        /// </summary>
        public string BuyQty { get; set; }
        /// <summary>
        /// 团购价
        /// </summary>
        public string Price { get; set; }
      
    }
}
