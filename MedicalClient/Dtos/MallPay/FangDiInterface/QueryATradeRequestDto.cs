﻿using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.MallPay.FangDiInterface
{
    /// <summary>
    /// 支付结果查询 请求Dto
    /// </summary>
    public class QueryATradeRequestDto : BaseDto
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "订单编号")]
        public string Trade_No { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [Display(Name = "类型")]
        public string Flag { get; set; }
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
    public class QueryATradeResponseDto
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
        /// 第三方订单号
        /// </summary>
        public string Trade_No { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public string Pay_Time { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string Out_Trade_No { get; set; }
        /// <summary>
        /// 支付金额(当退款状态时，显示实际退款金额)
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        public string Status { get; set; }
       
        ///// <summary>
        ///// 支付方式
        ///// </summary>
        //public string Pay_Way { get; set; }

    }

}
