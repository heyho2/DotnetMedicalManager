using GD.Common.Base;
using GD.Dtos.Merchant.Merchant;
using System.Collections.Generic;

namespace GD.Dtos.Merchant.Therapist
{
    /// <summary>
    /// 获取商户某天某项目的服务人员和排班详情响应Dto
    /// </summary>
    public class GetTherapistsScheduleByProjectIdOneDayResponseDto : BaseDto
    {
        /// <summary>
        /// 服务人员guid
        /// </summary>
        public string TherapistGuid { get; set; }

        /// <summary>
        /// 服务人员姓名
        /// </summary>
        public string TherapistName { get; set; }

        /// <summary>
        /// 服务人员图片
        /// </summary>
        public string PortraitUrl { get; set; }

        /// <summary>
        /// 排班guid
        /// </summary>
        public string ScheduleGuid { get; set; }

        /// <summary>
        /// 排班详情（时间刻度列表）
        /// </summary>
        public List<ScheduleTimeDetailDto> ScheduleDetails { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TherapistsScheduleByProjectIdOneDayResponseDto : BaseDto
    {
        /// <summary>
        /// 服务人员列表
        /// </summary>
        public List<TherapistsByProjectIdOneDayDto> Therapists { get; set; }

        /// <summary>
        ///服务人员排班列表
        /// </summary>
        public List<TherapistsScheduleByProjectIdOneDayDto> ScheduleDetails { get; set; }

    }

    /// <summary>
    /// 获取商户某天某项目的服务人员和排班详情——服务人员列表
    /// </summary>
    public class TherapistsByProjectIdOneDayDto : BaseDto
    {
        /// <summary>
        /// 服务人员guid
        /// </summary>
        public string TherapistGuid { get; set; }

        /// <summary>
        /// 服务人员姓名
        /// </summary>
        public string TherapistName { get; set; }

        /// <summary>
        /// 服务人员图片
        /// </summary>
        public string PortraitUrl { get; set; }
    }
    /// <summary>
    /// 获取商户某天某项目的服务人员和排班详情——服务人员排班明细
    /// </summary>
    public class TherapistsScheduleByProjectIdOneDayDto : BaseDto
    {
        /// <summary>
        /// 服务人员guid
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
        /// 排班明细消费guid
        /// </summary>
        public string ConsumptionGuid { get; set; }

        /// <summary>
        /// 排班明细-开始时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 排班明细-结束时间
        /// </summary>
        public string EndTime { get; set; }
    }
}
