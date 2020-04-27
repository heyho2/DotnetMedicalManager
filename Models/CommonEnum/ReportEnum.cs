using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GD.Models.CommonEnum
{
    /// <summary>
    /// 报表枚举--审批状态
    /// </summary>
    public enum ReportApproveStatusEnum
    {
        /// <summary>
        /// 待审批
        /// </summary>
        [Description("待审批")]
        Pending,
        /// <summary>
        /// 驳回
        /// </summary>
        [Description("驳回")]
        Reject,
        /// <summary>
        /// 通过
        /// </summary>
        [Description("通过")]
        Adopt,
        /// <summary>
        /// 取消
        /// </summary>
        [Description("取消")]
        Cancel,
    }

    /// <summary>
    ///审批时枚举
    /// </summary>
    public enum ReportSimpleApproveEnum
    {
        /// <summary>
        /// 驳回
        /// </summary>
        [Description("驳回")]
        Reject,
        /// <summary>
        /// 通过
        /// </summary>
        [Description("通过")]
        Adopt
    }


    /// <summary>
    /// 审批进度枚举
    /// </summary>
    public enum ReportApproveScheduleEnums
    {
        /// <summary>
        /// 运营申请状态
        /// </summary>
        [Description("申请中")]
        Apply,

        /// <summary>
        /// IT确认需求并转为SQL
        /// </summary>
        [Description("IT确认需求并转为SQL")]
        SqlWrite,
        /// <summary>
        /// 运营审批发送列表
        /// </summary>
        [Description("SQL审批")]
        SqlApprove,
        /// <summary>
        /// 运营审批发送列表
        /// </summary>
        [Description("确认人员列表")]
        Approve,

        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Complete
    }



}
