using GD.Common.Base;
using GD.Dtos.Merchant.Merchant;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.Mall
{
    /// <summary>
    /// 获取店铺某天某项目的美疗师排班列表 响应Dto
    /// </summary>
    public class GetMerchantTherapistScheduleOfProjectOneDayResponseDto : MerchantScheduleTherapistDto
    {
        /// <summary>
        /// 预约排班时间详情
        /// </summary>
        public List<MerchantTherapistTimeDetailsDto> ScheduleDetails { get; set; }

    }
    /// <summary>
    /// 排班美疗师Dto
    /// </summary>
    public class MerchantScheduleTherapistDto : BaseDto
    {
        /// <summary>
        /// 美疗师guid
        /// </summary>
        public string TherapistGuid { get; set; }

        /// <summary>
        /// 美疗师姓名
        /// </summary>
        public string TherapistName { get; set; }

        /// <summary>
        /// 美疗师图片url
        /// </summary>
        public string PortraitUrl { get; set; }
    }
    /// <summary>
    /// 美疗师排班时间相关Dto
    /// </summary>
    public class MerchantTherapistTimeDetailsDto : TimeDto
    {
        /// <summary>
        /// 美疗师guid
        /// </summary>
        public string TherapistGuid { get; set; }

        /// <summary>
        /// 排班guid
        /// </summary>
        public string ScheduleGuid { get; set; }

        /// <summary>
        /// 排班明细guid
        /// </summary>
        public string ScheduleDetailGuid { get; set; }

        /// <summary>
        /// 消费guid
        /// </summary>
        public string ConsumptionGuid { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class GetMerchantTherapistScheduleOfProjectOneDayDataTransfer
    {
        /// <summary>
        /// 项目排班美疗师
        /// </summary>
        public List<MerchantScheduleTherapistDto> Therapists { get; set; }

        /// <summary>
        /// 项目详细的排班计划
        /// </summary>
        public List<MerchantTherapistTimeDetailsDto> ScheduleDetails { get; set; }
    }

}
