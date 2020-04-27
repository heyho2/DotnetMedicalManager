using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.MallPay.FangDiInterface
{
    /// <summary>
    /// 退款Dto
    /// </summary>
    public class DoRefundRequestDto : BaseDto
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "订单号")]
        public string Trade_No { get; set; }

        /// <summary>
        /// 退款原因
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "退款原因")]
        public string Reason { get; set; }

        /// <summary>
        /// 退款订单号
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "退款订单号")]
        public string Refund_No { get; set; }

        /// <summary>
        /// 退款金额(可不填，以获取订单信息金额为准)
        /// </summary>
        [ Display(Name = "退款金额（以分为单位）")]
        public string Refund_Fee { get; set; }
        ///// <summary>
        ///// 渠道ID
        ///// </summary>
        //[Required(ErrorMessage = "{0}必填"), Display(Name = "渠道ID")]
        //public string ChannelId { get; set; }
        ///// <summary>
        ///// 机构ID
        ///// </summary>
        //[Required(ErrorMessage = "{0}必填"), Display(Name = "机构ID")]
        //public string OrgId { get; set; }
        ///// <summary>
        ///// 用户Guid
        ///// </summary>
        //[Required(ErrorMessage = "{0}必填"), Display(Name = "用户Guid")]
        //public string User_ID { get; set; }
    }

    /// <summary>
    /// 响应Dto
    /// </summary>
    public class DoRefundResponseDto
    {
        /// <summary>
        /// 返回码
        /// -1：失败 0：成功 1：成功但没有查询到数据
        /// </summary>
        public string ResultCode { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string ResultMsg { get; set; }

        /// <summary>
        /// 退款订单号
        /// </summary>
        public string Refund_No { get; set; }

        /// <summary>
        /// 退款金额(分)
        /// </summary>
        public string Refund_Fee { get; set; }

    }

}
