using GD.Common.Base;
using System;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 获取我的卡券列表响应Dto
    /// </summary>
    public class GetTicketRecordsOfCosmetologyResponseDto : BaseDto
    {
        /// <summary>
        /// 卡券记录Guid
        /// </summary>
        public string TicketRecordGuid { get; set; }

        /// <summary>
        /// 卡券名称(商品名称)
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 卡券有效期起始日期
        /// </summary>
        public DateTime? EffectiveStartDate { get; set; }

        /// <summary>
        /// 卡券有效期结束日期
        /// </summary>
        public DateTime? EffectiveEndDate { get; set; }

        /// <summary>
        /// 卡券已领取人数
        /// </summary>
        public int ReceiverCount { get; set; }
    }
}
