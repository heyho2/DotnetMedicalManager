using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.FAQs.FAQsClient
{
    /// <summary>
    /// 付费问答-微信支付
    /// </summary>
    public class FAQsWechatPayRequest
    {
        /// <summary>
        /// 问题Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "问题Guid")]

        public string QuestionGuid { get; set; }
        /// <summary>
        /// 静默授权标识
        /// </summary>
        [Display(Name = "静默授权标识")]
        public string Code { get; set; }

        ///// <summary>
        ///// 支付金额
        ///// </summary>
        //[Required(ErrorMessage = "{0}必填"), Display(Name = "支付金额")]
        //public decimal FeeNum { get; set; }
    }

    /// <summary>
    /// response
    /// </summary>
    public class FAQsWechatPayResponse
    {
        /// <summary>
        /// 静默授权标识
        /// </summary>
        [Display(Name = "静默授权标识")]
        public string Code { get; set; }
    }
}
