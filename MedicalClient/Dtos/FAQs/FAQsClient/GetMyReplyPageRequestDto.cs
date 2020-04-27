using GD.Common.Base;
using Newtonsoft.Json;
using System;

namespace GD.Dtos.FAQs.FAQsClient
{
    /// <summary>
    /// 搜索问题 请求
    /// </summary>
    public class GetMyReplyPageRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
