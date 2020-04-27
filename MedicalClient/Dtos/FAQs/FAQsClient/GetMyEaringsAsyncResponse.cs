using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.FAQs.FAQsClient
{
    /// <summary>
    /// 医生获取我的收益
    /// </summary>
    public class GetMyEaringsAsyncResponse : BaseDto
    {
        ///<summary>
        ///余额账户主键(即UserGuid)
        ///</summary>
        public string BalanceGuid { get; set; }

        ///<summary>
        ///总收益(分)
        ///</summary>
        public decimal TotalEarnings { get; set; } = 0;

        ///<summary>
        ///收益余额(分)
        ///</summary>
        public decimal AccBalance { get; set; } = 0;

        ///<summary>
        ///总提现(分)
        ///</summary>
        public decimal TotalWithdraw { get; set; } = 0;

        ///<summary>
        ///账户状态（frozen冻结，normal启动）
        ///</summary>
        public string Status { get; set; }
    }
}
