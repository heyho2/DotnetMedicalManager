using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Mall.Mall
{
    /// <inheritdoc />
    /// <summary>
    /// 提交订单结果返回
    /// </summary>
    public class ResultOfSubmitOrderResponseDto:BaseDto
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderGuid { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public string StatusCode { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get; set; }
    }
}
