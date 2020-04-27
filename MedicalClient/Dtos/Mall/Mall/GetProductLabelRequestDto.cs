using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Mall.Mall
{
    /// <inheritdoc />
    /// <summary>
    ///生美-获取商品购买标签 请求Dto
    /// </summary>
    public class GetProductLabelRequestDto : BasePageRequestDto
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
    public class GetProductLabelResponseDto : BasePageResponseDto<GetProductLabelItem>
    {
        /// <summary>
        /// 产品Guid
        /// </summary>
        public string ProductGuid { get; set; }
    }
    /// <summary>
    /// 子项
    /// </summary>
    public class GetProductLabelItem : BaseDto
    {

        /// <summary>
        /// 标签Guid
        /// </summary>
        public string TagGuid { get; set; }

        /// <summary>
        /// 标签名
        /// </summary>
        public string Tags { get; set; }

    }

}