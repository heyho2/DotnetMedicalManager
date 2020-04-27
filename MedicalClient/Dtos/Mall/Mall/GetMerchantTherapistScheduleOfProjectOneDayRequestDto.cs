using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 获取店铺某天某项目的美疗师排班列表请求dto
    /// </summary>
    public class GetMerchantTherapistScheduleOfProjectOneDayRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 店铺guid
        /// </summary>
        public string MerchantId { get; set; }

        /// <summary>
        /// 项目guid
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// 排班日期
        /// </summary>
        public DateTime ScheduleDate { get; set; }
    }
}
