using GD.Common.Base;
using System;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 获取排班周期数据响应Dto
    /// </summary>
    public class GetScheduleTemplateListResponseDto : BasePageResponseDto<GetScheduleTemplateListItemDto>
    {
    }

    /// <summary>
    /// 获取排班周期数据ItemDto
    /// </summary>
    public class GetScheduleTemplateListItemDto : BaseDto
    {
        /// <summary>
        /// 排班周期guid
        /// </summary>
        public string ScheduleTemplateGuid { get; set; }

        /// <summary>
        /// 排班周期开始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 排班周期开始日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 排班周期创建日期
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 班次模板guid
        /// </summary>
        public string TemplateGuid { get; set; }

        /// <summary>
        /// 班次模板名称
        /// </summary>
        public string TemplateName { get; set; }

    }
}
