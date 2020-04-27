using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Health.HealthManager
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateOrUpdateHealthReportRequestDto
    {
        /// <summary>
        /// 上传和编辑时用户Guid必传
        /// </summary>
        public string UserGuid { get; set; }
        /// <summary>
        /// 编辑报告时，报告Guid必传
        /// </summary>
        public string ReportGuid { get; set; }
        /// <summary>
        /// 报告名称
        /// </summary>
        [Required(ErrorMessage = "报告名称未填写"), MaxLength(100, ErrorMessage = "超过报告最大长度限制")]
        public string ReportName { get; set; }
        /// <summary>
        /// 报告建议
        /// </summary>
        public string Suggestion { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<UploadReportAttachment> Attachments { get; set; }
    }

    /// <summary>
    /// 报告附件
    /// </summary>
    public class UploadReportAttachment
    {
        /// <summary>
        /// 报告详情Guid
        /// </summary>
        public string ReportDetailGuid { get; set; }
        /// <summary>
        /// 附件名称
        /// </summary>
        public string Name { get; set; }
    }
}

