using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.FAQs.FAQsClient
{
    /// <summary>
    /// 微信支付提问
    /// </summary>
    public class PostQuestionsByWeChatPayAsyncRequest
    {
        ///// <summary>
        ///// 支付流水号
        ///// </summary>
        //[Required(ErrorMessage = "支付流水号必填")]
        //public string FaqsTransferFlowing { get; set; }

        /// <summary>
        /// 问题内容
        /// </summary>
        [Required(ErrorMessage = "问题内容必填")]
        [StringLength(500, ErrorMessage = "内容长度不能超过500")]
        public string Content { get; set; }

        /// <summary>
        /// 附件图片地址
        /// </summary>
        public List<string> AttachedPictures { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "支付金额")]
        public decimal FeeNum { get; set; }
    }

    /// <summary>
    /// response
    /// </summary>
    public class PostQuestionsByWeChatPayAsyncResponse
    {


    }

}
