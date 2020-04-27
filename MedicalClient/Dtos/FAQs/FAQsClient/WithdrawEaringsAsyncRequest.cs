using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.FAQs.FAQsClient
{
    /// <summary>
    /// 提现
    /// </summary>
    public class WithdrawEaringsAsyncRequest
    {
        /// <summary>
        /// 提现金额（元）
        /// </summary>
        [Required(ErrorMessage = "提现金额必填")]
        public decimal WithdrawNum { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 提现
    /// </summary>
    public class WithdrawEaringsAsyncResponse
    {
    }
}
