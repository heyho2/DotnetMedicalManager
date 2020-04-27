using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GD.Models.FAQs
{
    /// <summary>
    /// 提现申请
    /// </summary>
    [Table("t_doctor_withdraw_apply")]
    public class DoctorWithDrawApplyModel : BaseModel
    {
        /// <summary>
        /// 申请主键
        /// </summary>
        [Column("apply_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "申请主键")]
        public string ApplyGuid { get; set; }

        /// <summary>
        /// 申请编号(转账编号)
        /// </summary>
        [Column("apply_code"), Required(ErrorMessage = "{0}必填"), Display(Name = "申请编号(转账编号)")]
        public string ApplyCode { get; set; }

        /// <summary>
        /// 申请医生Guid
        /// </summary>
        [Column("doctor_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "申请医生Guid")]
        public string DoctorGuid { get; set; }

        /// <summary>
        ///提现金额(分)
        /// </summary>
        [Column("withdraw"), Required(ErrorMessage = "{0}必填"), Display(Name = "提现金额(分)")]
        public int Withdraw { get; set; }

        /// <summary>
        /// 申请状态（1申请中，2已批准转账中，3已拒绝，4已转账）
        /// </summary>
        [Column("status"), Required(ErrorMessage = "{0}必填"), Display(Name = "申请状态")]
        public string Status { get; set; } = FaqsApplyStatusEnum.Apply.ToString();

        /// <summary>
        /// 转账流水
        /// </summary>
        [Column("transaction_flowing_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "转账流水")]
        public string TransactionFlowingGuid { get; set; }

        /// <summary>
        /// 审核人Guid
        /// </summary>
        [Column("approver_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "审核人Guid")]
        public string ApproverGuid { get; set; }

        /// <summary>
        ///审批原因
        /// </summary>
        [Column("reason"), Required(ErrorMessage = "{0}必填"), Display(Name = "审批原因")]
        public string Reason { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("remark"), Required(ErrorMessage = "{0}必填"), Display(Name = "备注")]
        public string Remark { get; set; }

        /// <summary>
        /// 申请状态枚举
        /// </summary>
        public enum FaqsApplyStatusEnum
        {
            /// <summary>
            /// 申请中
            /// </summary>
            [Description("申请中")]
            Apply = 1,
            /// <summary>
            /// 转账中
            /// </summary>
            [Description("转账中")]
            Transfering = 1,
            /// <summary>
            /// 已拒绝
            /// </summary>
            [Description("已拒绝")]
            Refuse,
            /// <summary>
            /// 已完成
            /// </summary>
            [Description("已完成")]
            Complete,

        }
    }
}
