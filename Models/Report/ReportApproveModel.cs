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
    [Table("t_report_approve")]
    public class ReportApproveModel : BaseModel
    {

        ///<summary>
        ///主键
        ///</summary>
        [Column("approve_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "主键")]
        public string ApproveGuid { get; set; }

        ///<summary>
        ///需求主键
        ///</summary>
        [Column("theme_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "需求主键")]
        public string ThemeGuid { get; set; }

        ///<summary>
        ///需求申请人
        ///</summary>
        [Column("apply_user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "需求申请人")]
        public string ApplyUserGuid { get; set; }

        ///<summary>
        ///编写sql的人
        ///</summary>
        [Column("sql_writer_guid"), Display(Name = "编写sql的人")]
        public string SQLWriterGuid { get; set; }

        ///<summary>
        ///sql审批人
        ///</summary>
        [Column("sql_approver_guid"), Display(Name = "sql审批人")]
        public string SQLApproverGuid { get; set; }

        ///<summary>
        ///数据列表审核人
        ///</summary>
        [Column("list_approver_guid"), Display(Name = "数据列表审核人")]
        public string ListApproverGuid { get; set; }

        ///<summary>
        ///当前审批理由
        ///</summary>
        [Column("approved_reason"), Display(Name = "当前审批理由")]
        public string ApprovedReason { get; set; }

        ///<summary>
        ///当前审批时间
        ///</summary>
        [Column("approved_datetime"), Display(Name = "当前审批时间")]
        public DateTime ApprovedDatetime { get; set; }

        ///<summary>
        ///审批进度枚举
        ///</summary>
        [Column("approve_schedule_enum"), Required(ErrorMessage = "{0}必填"), Display(Name = "审批进度枚举")]
        public string ApproveScheduleEnum { get; set; }


        ///<summary>
        ///审批状态(待审批/驳回/通过/取消)
        ///</summary>
        [Column("approve_status"), Display(Name = "审批状态(待审批/驳回/通过/取消)")]
        public string ApproveStatus { get; set; }


    }
}
