using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Report.CommonReport
{
    /// <summary>
    /// 运营审核数据报表
    /// </summary>
    public class ApproveReportListRequest
    {
        /// <summary>
        /// 审批记录Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "审批记录Guid")]
        public string ApproveGuid { get; set; }

        /// <summary>
        ///  审批原因
        /// </summary>
        [Display(Name = "原因")]
        public string ApprovedReason { get; set; }

        /// <summary>
        ///  审批状态（驳回/通过）
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "审批状态（驳回/通过）")]
        public string ApproveStatus { get; set; }
    }


}
