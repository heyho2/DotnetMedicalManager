using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.WeiXinPay
{
    /// <summary>
    /// 微信支付回调方法Dto
    /// </summary>
    public class WeiXinPaymentBackFunctionRequestDto
    {
        /// <summary>
        /// 应用ID|微信开放平台审核通过的应用APPID|是
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 商户号|微信支付分配的商户号|是
        /// </summary>
        public string mch_id { get; set; }

        /// <summary>
        /// 设备号|微信支付分配的终端设备号|否
        /// </summary>
        public string device_info { get; set; }

        /// <summary>
        /// 随机字符串|随机字符串，不长于32位|是
        /// </summary>
        public string nonce_str { get; set; }

        /// <summary>
        /// 签名|签名，详见签名算法|是
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// 业务结果|SUCCESS/FAIL|是
        /// </summary>
        public string result_code { get; set; }

        /// <summary>
        /// 错误代码|错误返回的信息描述|否
        /// </summary>
        public string err_code { get; set; }

        /// <summary>
        /// 错误代码描述|错误返回的信息描述|否
        /// </summary>
        public string err_code_des { get; set; }

        /// <summary>
        /// 用户标识|用户在商户appid下的唯一标识|是
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 是否关注公众账号|用户是否关注公众账号，Y-关注，N-未关注|是
        /// </summary>
        public string is_subscribe { get; set; }

        /// <summary>
        /// 交易类型|APP|是
        /// </summary>
        public string trade_type { get; set; }

        /// <summary>
        /// 付款银行|银行类型，采用字符串类型的银行标识，银行类型见银行列表|是
        /// </summary>
        public string bank_type { get; set; }

        /// <summary>
        /// 总金额|订单总金额，单位为分|是
        /// </summary>
        public int total_fee { get; set; }

        /// <summary>
        /// 货币种类|货币类型，符合ISO4217标准的三位字母代码，默认人民币：CNY，其他值列表详见货币类型|否
        /// </summary>
        public string fee_type { get; set; }

        /// <summary>
        /// 现金支付金额|现金支付金额订单现金支付金额，详见支付金额|是
        /// </summary>
        public int cash_fee { get; set; }

        /// <summary>
        /// 现金支付货币类型|货币类型，符合ISO4217标准的三位字母代码，默认人民币：CNY，其他值列表详见货币类型|否
        /// </summary>
        public string cash_fee_type { get; set; }

        /// <summary>
        /// 代金券金额|代金券或立减优惠金额小于或等于订单总金额,订单总金额-代金券或立减优惠金额=现金支付金额,详见支付金额|否
        /// </summary>
        public int coupon_fee { get; set; }

        /// <summary>
        /// 代金券使用数量|代金券或立减优惠使用数量|否
        /// </summary>
        public int coupon_count { get; set; }

        /// <summary>
        /// 代金券ID|代金券或立减优惠ID,$n为下标，从0开始编号|否
        /// </summary>
        public string coupon_id_n { get; set; }

        /// <summary>
        /// 单个代金券支付金额|单个代金券或立减优惠支付金额,$n为下标，从0开始编号|否
        /// </summary>
        public int coupon_fee_n { get; set; }

        /// <summary>
        /// 微信支付订单号|微信支付订单号|是
        /// </summary>
        public string transaction_id { get; set; }

        /// <summary>
        /// 商户订单号|商户系统内部订单号，要求32个字符内，只能是数字、大小写字母_-|*@ ，且在同一个商户号下唯一。|是
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 商家数据包|商家数据包，原样返回|否
        /// </summary>
        public string attach { get; set; }

        /// <summary>
        /// 支付完成时间|支付完成时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010。|是
        /// </summary>
        public string time_end { get; set; }
    }
}