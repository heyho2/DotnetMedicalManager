using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.MallPay.FangDiInterface
{
    /// <summary>
    /// 获取支付二维码--返回(Base64) 请求Dto
    /// </summary>
    public class GetScanCodeStringRequestDto:BaseDto
    {
        /// <summary>
        /// 渠道ID
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "渠道ID")]
        public string ChannelId { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "订单编号")]
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
        public string Pay_Mode { get; set; }
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

    }

    /// <summary>
    /// 响应Dto
    /// </summary>
    public class GetScanCodeStringResponseDto
    {
        /// <summary>
        /// 二维码Base64
        /// </summary>
        public string Qr_Code { get; set; }
        /// <summary>
        /// 标志
        /// </summary>
        public string Flag { get; set; }

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
