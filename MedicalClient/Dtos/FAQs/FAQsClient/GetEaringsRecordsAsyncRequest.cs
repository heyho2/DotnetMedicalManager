using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.FAQs.FAQsClient
{
    /// <summary>
    /// 获取收益明细
    /// </summary>
    public class GetEaringsRecordsAsyncRequest : BasePageRequestDto
    {

    }

    /// <summary>
    /// response
    /// </summary>
    public class GetEaringsRecordsAsyncResponse : BaseDto
    {
        /// <summary>
        /// 记录主键
        /// </summary>
        public string DetailGuid { get; set; }

        /// <summary>
        /// 医生Guid
        /// </summary>
        public string DoctorGuid { get; set; }

        /// <summary>
        /// 回答Guid
        /// </summary>
        public string AnswerGuid { get; set; }

        /// <summary>
        /// 所获来源枚举（answer:回答问题）
        /// </summary>
        public string FeeFrom { get; set; }

        /// <summary>
        ///所获金额(分)
        /// </summary>
        public decimal ReceivedFee { get; set; }

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
