using System.Collections.Generic;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 
    /// </summary>
    public class GetConsumerHealthReportResponseDto
    {
        /// <summary>
        /// 会员昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 会员手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 报告名称
        /// </summary>
        public string ReportName { get; set; }
        /// <summary>
        /// 报告建议
        /// </summary>
        public string Suggestion { get; set; }

        /// <summary>
        /// 附件列表
        /// </summary>
        public List<ReportAttachment> Attachments { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ReportAttachment
    {
        /// <summary>
        /// 附件唯一标识
        /// </summary>
        public string ReportDetailGuid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AccessoryGuid { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 附件名称
        /// </summary>
        public string Name { get; set; }
    }
}
