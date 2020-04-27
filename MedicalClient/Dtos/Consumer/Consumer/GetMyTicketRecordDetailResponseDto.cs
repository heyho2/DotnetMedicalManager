using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 获取我的卡券列表卡券详情响应Dto
    /// </summary>
    public class GetMyTicketRecordDetailResponseDto : BaseDto
    {
        /// <summary>
        /// 卡券明细guid
        /// </summary>
        public string TicketRecordDetailGuid { get; set; }

        /// <summary>
        /// 项目总数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 项目guid
        /// </summary>
        public string ProjectGuid { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public int Remain { get; set; }

        /// <summary>
        /// 适用门店名称，以逗号分隔
        /// </summary>
        public string MerchantNames { get; set; }
    }
}
