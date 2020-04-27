using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Mall.Mall
{
    /// <inheritdoc />
    /// <summary>
    ///生美-获取商品组合详情 请求Dto
    /// </summary>
    public class GetComboProductDetailListRequestDto : BasePageRequestDto
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
    public class GetComboProductDetailListResponseDto : BasePageResponseDto<GetComboProductDetailItem>
    {

    }
    /// <inheritdoc />
    /// <summary>
    /// 子项
    /// </summary>
    public class GetComboProductDetailItem : BaseDto
    {
        /// <summary>
        /// guid
        /// </summary>
        public string Guid { get; set; }


    }
}