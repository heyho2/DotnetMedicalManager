using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.FAQs.FAQsClient
{
    /// <summary>
    /// 获取问答支付类型
    /// </summary>
    public class GetAskPayTypeAsyncResponse
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 枚举code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public decimal Minimum { get; set; }
    }
}
