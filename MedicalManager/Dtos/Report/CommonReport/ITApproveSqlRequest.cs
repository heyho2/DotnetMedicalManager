using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Report.CommonReport
{
    /// <summary>
    /// 
    /// </summary>
    public class ITApproveSqlRequest
    {
        /// <summary>
        /// 审批记录Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "审批记录Guid")]
        public string ApproveGuid { get; set; }

        /// <summary>
        ///  根据需求写SQL语句，SQL语句返回固定UserName,Phone等字段
        ///  举例： select username, phone，age,sex from table
        /// </summary>
        [Display(Name = "SQL语句")]
        public string ThemeSqlStr { get; set; }

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
