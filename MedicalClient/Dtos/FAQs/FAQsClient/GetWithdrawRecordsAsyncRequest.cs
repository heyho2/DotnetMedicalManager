using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.FAQs.FAQsClient
{
    /// <summary>
    /// 提现明细
    /// </summary>
    public class GetWithdrawRecordsAsyncRequest : BasePageRequestDto
    {

    }

    /// <summary>
    /// response
    /// </summary>
    public class GetWithdrawRecordsAsyncResponse : BaseDto
    {
        /// <summary>
        /// 申请主键
        /// </summary>
        public string ApplyGuid { get; set; }

        /// <summary>
        /// 申请编号(转账编号)
        /// </summary>
        public string ApplyCode { get; set; }

        /// <summary>
        /// 申请医生Guid
        /// </summary>
        public string DoctorGuid { get; set; }

        /// <summary>
        ///提现金额(分)
        /// </summary>
        public decimal Withdraw { get; set; } = 0;

        /// <summary>
        /// 申请状态（Apply申请中，Transfering已批准转账中，Refuse已拒绝，Complete已转账）
        /// </summary>
        public string Status { get; set; }
       
        ///// <summary>
        ///// 转账流水
        ///// </summary>
        //public string TransactionFlowingGuid { get; set; }

        ///// <summary>
        ///// 审核人Guid
        ///// </summary>
        //public string ApproverGuid { get; set; }

        ///// <summary>
        /////审批原因
        ///// </summary>
        //public string Reason { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime? CreationDate { get; set; }

    }
}
