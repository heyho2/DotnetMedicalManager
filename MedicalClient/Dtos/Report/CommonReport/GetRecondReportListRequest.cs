using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Report.CommonReport
{
    /// <summary>
    /// 审核列表
    /// </summary>
    public class GetRecondReportListRequest
    {
        /// <summary>
        /// 审批记录Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "审批记录Guid")]
        public string ApproveGuid { get; set; }
    }
}
