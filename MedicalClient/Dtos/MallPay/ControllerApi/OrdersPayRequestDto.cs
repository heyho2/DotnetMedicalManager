using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.MallPay.ControllerApi
{
    /// <summary>
    /// H5下单
    /// </summary>
    public class OrdersSubmitPayRequestDto
    {
        ///// <summary>
        ///// 渠道ID
        ///// </summary>
        //[Required(ErrorMessage = "{0}必填"), Display(Name = "渠道ID")]
        //public string ChannelId { get; set; }

        /// <summary>
        /// 订单编号（Trade_No）
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "订单编号")]
        public string OrderKey { get; set; }
        /// <summary>
        /// Open_Id
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "Open_Id")]
        public string OpenId { get; set; }
        ///// <summary>
        ///// 支付方式
        ///// </summary>
        //[Display(Name = "支付方式")]
        //public string Pay_Way { get; set; } = MallPayEnum.wechat.ToString();

        ///// <summary>
        ///// 支付类型
        ///// </summary>
        //[Display(Name = "支付类型")]
        //public string Pay_Mode { get; set; } = MallPayEnum.wechatpublic.ToString();
        /// <summary>
        /// 支付内容描述(不可空)
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "支付内容描述")]
        public string Subject { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "支付金额")]
        public string Amount { get; set; }
        ///// <summary>
        ///// 医院简称/机构ID
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
    /// Response
    /// </summary>
    public class OrdersSubmitPayResponseDto
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public string ResultCode { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string ResultMsg { get; set; }
        /// <summary>
        /// 返回消息
        /// </summary>
        public string Qr_Code { get; set; }
        /// <summary>
        /// 返回消息
        /// </summary>
        public string Flag { get; set; }
        /// <summary>
        /// 微信返回的数据组
        /// </summary>
        public PaySign Paysign { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public class PaySign
        {
            /// <summary>
            /// AppId
            /// </summary>
            public string AppId { get; set; }
            /// <summary>
            /// 时间戳
            /// </summary>
            public string TimeStamp { get; set; }
            /// <summary>
            /// NonceStr
            /// </summary>
            public string NonceStr { get; set; }
            /// <summary>
            /// Package
            /// </summary>
            public string Package { get; set; }
            ///// <summary>
            ///// Prepay_Id
            ///// </summary>
            //public string Prepay_Id { get; set; }
            /// <summary>
            /// SignType - MD5
            /// </summary>
            public string SignType { get; set; }
            /// <summary>
            /// PaySign
            /// </summary>
            public string paySign { get; set; }
        }
    }

}
