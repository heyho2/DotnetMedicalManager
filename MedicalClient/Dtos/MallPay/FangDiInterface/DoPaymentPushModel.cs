using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.MallPayDto.FangDiInterface
{
    /// <summary>
    /// 交易推送信息
    /// </summary>
    public class DoPaymentPushModel : BaseDto
    {
        /// <summary>
        /// 第三方平台ID
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }

        /// <summary>
        /// 子商户
        /// </summary>
        public string sub_mch_id { get; set; }

        /// <summary>
        /// 子商户平台ID
        /// </summary>
        public string sub_appid { get; set; }

        /// <summary>
        /// 第三方支付支付生成的医疗订单号
        /// </summary>
        public string med_trans_id { get; set; }

        /// <summary>
        /// 医院订单号,若为自费支付(非移动医疗平台下单),则本字段传商户订单号
        /// </summary>
        public string hosp_out_trade_no { get; set; }

        /// <summary>
        /// 业务状态| pay_ok: 支付成功,paying: 支付中,refunded: 已退款,closed: 订单已经关闭,canceled: 订单已经撤销
        /// </summary>
        public string trade_statu { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string err_code { get; set; }

        /// <summary>
        /// 错误描述
        /// </summary>
        public string err_msg { get; set; }

        /// <summary>
        /// 订单支付时间,格式为yyyyMMddHHmmss,如20091225091010
        /// </summary>
        public string time_end { get; set; }

        /// <summary>
        /// 支付类型 1:现金 2:医保 3:现金+医保
        /// </summary>
        public string pay_type { get; set; }

        /// <summary>
        /// 总共需要支付的金额,以分为单位
        /// </summary>
        public string total_fee { get; set; }

        /// <summary>
        /// 现金支付金额,以分为单位
        /// </summary>
        public string cash_fee { get; set; }

        /// <summary>
        /// 医保支付金额,以分为单位
        /// </summary>
        public string insurance_fee { get; set; }

        /// <summary>
        /// 诊疗证编号
        /// </summary>
        public string medical_card_id { get; set; }

        /// <summary>
        /// 医保单据号
        /// </summary>
        public string bill_no { get; set; }

        /// <summary>
        /// 医保流水号
        /// </summary>
        public string serial_no { get; set; }

        /// <summary>
        /// 医保子单号
        /// </summary>
        public string insurance_order_id { get; set; }

        /// <summary>
        /// 支付子单号
        /// </summary>
        public string cash_order_id { get; set; }
    }
}