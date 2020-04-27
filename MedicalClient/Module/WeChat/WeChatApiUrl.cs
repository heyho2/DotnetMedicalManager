using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Module.WeChat
{
    public class WeChatApiUrl
    {
        /// <summary>
        /// 获取微信公众号TOKEN URL (7200s过期)
        /// HttpGet 请求
        /// </summary>
        public const string GET_ACCESS_TOKEN = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";

        /// <summary>
        /// 获取小程序码，适用于需要的码数量较少的业务场景。通过该接口生成的小程序码，永久有效，有数量限制
        /// </summary>
        public const string GET_WXACODE = "https://api.weixin.qq.com/wxa/getwxacode?access_token={0}";

        /// <summary>
        /// 微信JS接口的临时票据(7200s过期)
        /// </summary>
        public const string GET_JSAPI_TICKET = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";

        /// <summary>
        /// 发送模板消息
        /// </summary>
        public const string SEND_TEMPLATE_MSG = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";

        /// <summary>
        /// 获取微信公众号发送客服消息
        /// HttpPost 请求
        /// </summary>
        public const string SEND_CUSTOM_MSG = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";

        /// <summary>
        /// 通过code换取网页授权access_token和OpenId
        /// HttpGet 请求
        /// </summary>
        public const string OAUTH2_ACCESS_TOKEN = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";

        /// <summary>
        /// 创建公众号二维码
        /// HttpPost 请求
        /// </summary>
        public const string CREATE_QRCODE = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
        /// <summary>
        /// 企业付款
        /// </summary>
        public static readonly string ENTERPRISE_PAYMENT = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers";

        /// <summary>
        /// 获取用户信息
        /// HttpGet 请求
        /// </summary>
        public const string GET_USER_INFO = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN";
    }
}
