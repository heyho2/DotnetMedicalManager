using GD.Common.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 
    /// </summary>
    public class GetConsumerHealthReportPageListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "指定会员标识未提供")]
        public string UserGuid { get; set; }
        /// <summary>
        /// 报告名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetConsumerHealthReportPageListResponseDto : BasePageResponseDto<GetConsumerHealthReportItem>
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class GetConsumerHealthReportItem : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string ReportGuid { get; set; }
        /// <summary>
        /// 报告名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UploadedDate { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdatedDate { get; set; }
    }
}
