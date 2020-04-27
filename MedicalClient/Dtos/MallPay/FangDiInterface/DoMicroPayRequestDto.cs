using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.MallPay.FangDiInterface
{
    /// <summary>
    /// 扫码枪支付 请求Dto
    /// </summary>
    public class DoMicroPayRequestDto:BaseDto
    {
        /// <summary>
        /// 渠道ID
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "渠道ID")]
        public string ChannelId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "订单号")]
        public string Trade_No { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "支付方式")]
        public string Pay_Way { get; set; }
        /// <summary>
        /// 支付类型
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "支付类型")]
        public string Pay_Type { get; set; }
        /// <summary>
        /// 支付标题
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "支付标题")]
        public string Subject { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "支付金额")]
        public string Amount { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "机构ID")]
        public string OrgId { get; set; }
        /// <summary>
        /// 用户Guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "用户Guid")]
        public string User_ID { get; set; }
        /// <summary>
        /// 付款码
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "付款码")]
        public string AuthCode { get; set; }
       


    }

    /// <summary>
    /// 响应Dto
    /// </summary>
    public class MicroPayResponseDto:BaseDto
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
    }
}
