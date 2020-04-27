using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GD.Models.Report
{
    ///<summary>
    /// 问题模块-回答表实体
    ///</summary>
    [Table("t_report_approvedlog")]
    public class ReportApproveLogModel : BaseModel
    {
        ///<summary>
        ///审批日志主键
        ///</summary>
        [Column("approvedlog_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "审批日志主键")]
        public string ApprovedLogGuid { get; set; }

        ///<summary>
        ///审批人guid
        ///</summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "审批人guid")]
        public string UserGuid { get; set; }

        ///<summary>
        ///审批记录guid
        ///</summary>
        [Column("target_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "审批记录guid")]
        public string TargetGuid { get; set; }

        ///<summary>
        ///审批结果(驳回1/通过2/取消3)
        ///</summary>
        [Column("approved_result"), Required(ErrorMessage = "{0}必填"), Display(Name = "审批结果")]
        public string ApprovedResult { get; set; }

        ///<summary>
        ///审批原因
        ///</summary>
        [Column("approved_reason"), Display(Name = "审批原因")]
        public string ApprovedReason { get; set; }

        ///<summary>
        ///审批时间
        ///</summary>
        [Column("approved_datetime"), Required(ErrorMessage = "{0}必填"), Display(Name = "审批时间")]
        public string ApprovedDatetime { get; set; }

    }
}
