using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 健康档案查询Dto
    /// </summary>
    public class GetHealthInformationArchivesPageListRequestDto : BasePageRequestDto
    {
    }
    /// <summary>
    /// 健康档案查询响应Dto
    /// </summary>
    public class GetHealthInformationArchivesPageListResponseDto : BasePageResponseDto<GetHealthInformationArchivesPageListItemDto>
    {

    }
    /// <summary>
    /// 详情
    /// </summary>
    public class GetHealthInformationArchivesPageListItemDto : BaseDto
    {
        /// <summary>
        /// id
        /// </summary>
        public string ReportGuid { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string ReportName { get; set; }
        /// <summary>
        /// 建议
        /// </summary>
        public string Suggestion { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
    /// <summary>
    /// 健康档案明细数据
    /// </summary>
    public class ConsumerHealthReportDetailResponse
    {
        /// <summary>
        /// 附件名称
        /// </summary>
        public string AccessoryName { get; set; }
        /// <summary>
        /// 附件路径
        /// </summary>
        public string PicturePath { get; set; }
        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string Extension { get; set; }
    }

}
