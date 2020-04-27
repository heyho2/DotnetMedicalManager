using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Report.CommonReport
{
    /// <summary>
    /// 取消我的申请
    /// </summary>
    public class CancelMyApproveRequest
    {
        /// <summary>
        /// 申请Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "申请Guid")]
        public string ApproveGuid { get; set; }
    }
}
