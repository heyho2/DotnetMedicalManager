using Dapper;
using GD.AppSettings;
using GD.DataAccess;
using GD.Dtos.Mall.WeiXinPay;
using GD.Models.Mall;
using Newtonsoft.Json;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.TenPay.V3;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GD.Mall
{
    /// <summary>
    /// 微信支付逻辑处理类
    /// </summary>
    public class WeiXinPayBiz
    {
        #region 基础数据

        /// <summary>
        /// token
        /// </summary>
        private readonly string RedisKey = "GD:Mall:WeiXinPayBiz:Appid";

        /// <summary>
        /// 流水号签名
        /// </summary>
        private readonly string RedisSignKey = "GD:Mall:WeiXinPayBiz:Sign";

        /// <summary>
        /// AppSecret
        /// 28ad4a33ad5f47156ee97b300f107e1c
        /// 03584b2a09e82ef7806c883a74677685
        /// </summary>
        private readonly string AppSecret = "03584b2a09e82ef7806c883a74677685";

        /// <summary>
        /// 商户号
        /// </summary>
        private readonly string MerchantNumber = "1502484441";

        /// <summary>
        /// 密钥
        /// </summary>
        private readonly string MerchantSecret = "087567639e6346b0a6c6af41d676ce88";

        /// <summary>
        /// SHA加盟key
        /// </summary>
        private readonly string SHAKey = "6257D693A078E1C3E4BB6BA4DC30B2";

        /// <summary>
        /// 回调域
        /// </summary>
        private readonly string Domin = "http://api-test.ghysjt.com";
        /// <summary>
        /// 配置文件
        /// </summary>
        private static Settings settings = Factory.GetSettings("host.json");
        /// <summary>
        /// AppID
        /// </summary>
        private readonly string AppID = settings["WeChat:Client:AppId"];// "wx805e49616e3c0451";
        /// <summary>
        /// 支付证书路径
        /// </summary>
        private readonly string CertPath = settings["WeChat:Client:CertPath"];

        /// <summary>
        /// 支付证书秘钥,一般默认为商户号
        /// </summary>
        private readonly string CertSecret = settings["WeChat:Client:CertSecret"];

        #endregion 基础数据

        /// <summary>
        /// 获取微信配置信息
        /// </summary>
        /// <returns></returns>
        public async Task<WeiXinConfigResponseDto> GetWeiXinConfig()
        {
            return new WeiXinConfigResponseDto() { AppID = AppID };
        }
        /// <summary>
        /// 企业付款（分销）
        /// </summary>
        /// <returns></returns>
        public async Task<TransfersResult> EnterpasePayAsync(string TransactionId, string OpenID, string receipterName, int amount, string createIP)
        {
            var PackageRequestHandler = new RequestHandler(null);
            PackageRequestHandler.Init();
            PackageRequestHandler.SetParameter("mch_appid", AppID);//商户账号appid
            PackageRequestHandler.SetParameter("mchid", MerchantNumber);//商户号
            PackageRequestHandler.SetParameterWhenNotNull("device_info", "");//设备号	
            PackageRequestHandler.SetParameter("nonce_str", GetRandomString(16, false, true, true, false, string.Empty));//随机字符串
            PackageRequestHandler.SetParameterWhenNotNull("partner_trade_no", TransactionId);//商户订单号
            PackageRequestHandler.SetParameterWhenNotNull("openid", OpenID);//用户openid
            PackageRequestHandler.SetParameterWhenNotNull("check_name", "NO_CHECK");//校验用户姓名选项
            PackageRequestHandler.SetParameterWhenNotNull("re_user_name", receipterName);//收款用户姓名	
            PackageRequestHandler.SetParameterWhenNotNull("amount", amount.ToString());//金额 （分）
            PackageRequestHandler.SetParameterWhenNotNull("desc", "分销返佣等");//企业付款备注
            PackageRequestHandler.SetParameterWhenNotNull("spbill_create_ip", createIP);//Ip地址	

            var Sign = PackageRequestHandler.CreateMd5Sign("key", MerchantSecret, Senparc.Weixin.TenPay.WorkPaySignType.None);
            PackageRequestHandler.SetParameter("sign", Sign);//签名

            var res = await Module.CommonUtility.HttpClientHelper.HttpPostAsync(Module.WeChat.WeChatApiUrl.ENTERPRISE_PAYMENT, PackageRequestHandler.ParseXML(), CertPath, CertSecret);
            return new TransfersResult(res);
        }
        /// <summary>
        /// 通过code换取网页授权access_token和openid的返回数据
        /// </summary>
        /// <param name="code">code</param>
        public async Task<WeiXinToken> GetOpenidAndAccessTokenFromCodeAsync(string userid, string code)
        {
            WeiXinToken token = null;
            string redisKey = $"{RedisKey}:userid:{userid}";
            string tokenString = RedisHelper.Database.StringGet(redisKey);
            if (!string.IsNullOrWhiteSpace(tokenString))
            {
                var wexintoken = JsonConvert.DeserializeObject<WeiXinToken>(tokenString);
                //失效时间大于当前时间
                if (DateTime.Now >= token.expires_date)
                {
                    return token;
                }
            }
            OAuthAccessTokenResult accessToken = OAuthApi.GetAccessToken(AppID, AppSecret, code);
            if (accessToken.errcode == ReturnCode.请求成功)
            {
                token = new WeiXinToken();
                token.access_token = accessToken.access_token;
                token.create_date = DateTime.Now;
                token.expires_in = accessToken.expires_in;
                token.expires_date = token.create_date.AddSeconds(accessToken.expires_in);
                token.openid = accessToken.openid;
                token.refresh_token = accessToken.refresh_token;
                token.scope = accessToken.scope;
                token.unionid = accessToken.unionid;
                RedisHelper.Database.StringSet(redisKey, token.access_token, new TimeSpan(1, 50, 0));
            }
            return token;
        }

        /// <summary>
        /// 订单查询
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<OrderQueryResult> OrderQuery(GetWeiXinPaymentBeforeRequestDto requestDto)
        {
            TenPayV3OrderQueryRequestData data = new TenPayV3OrderQueryRequestData(AppID, MerchantNumber, "wx0818171559664608e39c01920461325676", GetRandomString(16, false, true, true, false, string.Empty), "GDSPDD_IKJRMNLCCJ20190408181709", MerchantSecret);
            return TenPayV3.OrderQuery(data);
        }

        /// <summary>
        /// 统一下单
        /// </summary>
        /// <param name="order_no">order_no</param>
        /// <returns></returns>
        public async Task<UnifiedorderResult> UnifiedorderAsync(string outTradeNo, string nonceStr, GetWeiXinPaymentBeforeRequestDto requestDto)
        {
            TenPayV3UnifiedorderRequestData data = new TenPayV3UnifiedorderRequestData(AppID, MerchantNumber, "医疗用品", outTradeNo, 1, "", "http://api-test.ghysjt.com/api/v1/mallpay/MallWeiXinPay/PaymentBackFunction", Senparc.Weixin.TenPay.TenPayV3Type.JSAPI, requestDto.openId, MerchantSecret, nonceStr, "WEB", DateTime.Now, DateTime.Now.AddHours(1), "商品列表", "国丹医疗", "CNY", null, null, false);
            data.SignType = "MD5";
            return await TenPayV3.UnifiedorderAsync(data);
        }

        /// <summary>
        /// 微信支付回调方法
        /// </summary>
        /// <returns></returns>
        public async Task<string> PaymentBackFunction()
        {
            return GetReturnXml("SUCCESS", "OK");
        }

        /// <summary>
        /// 支付前调用方法
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetWeiXinPaymentBeforeResponseDto> GetPaymentBeforeAsync(string userId, GetWeiXinPaymentBeforeRequestDto requestDto)
        {
            //商品订单
            string outTradeNo = $"GDSPDD_{GetRandomString(10, false, false, true, false, "")}{DateTime.Now.ToString("yyyyMMddHHmmss")}";
            //交易流水
            string TransactionNumber = $"GDJYLS_{GetRandomString(10, false, false, true, false, "")}{DateTime.Now.ToString("yyyyMMddHHmmss")}";
            //随机字符串
            string nonceStr = GetRandomString(16, false, true, true, false, string.Empty);

            TransactionFlowingModel model = new TransactionFlowingModel();
            model.Amount = 1;
            model.OrgGuid = string.Empty;
            model.CreatedBy = userId;
            model.LastUpdatedBy = userId;
            model.TransactionFlowingGuid = Guid.NewGuid().ToString("N");
            model.OutTradeNo = outTradeNo;
            model.TransactionNumber = TransactionNumber;
            model.TransactionStatus = "WaitForPayment";
            model.Channel = "微信支付";

            UnifiedorderResult unifie = await UnifiedorderAsync(outTradeNo, nonceStr, requestDto);

            if (unifie.IsReturnCodeSuccess())
            {
                model.ChannelNumber = unifie.prepay_id;
                model.Insert();
                GetWeiXinPaymentBeforeResponseDto result = new GetWeiXinPaymentBeforeResponseDto
                {
                    appId = this.AppID,
                    nonceStr = nonceStr,
                    timeStamp = Convert.ToInt32(GetTimeSpan().TotalSeconds),
                    package = $"prepay_id={unifie.prepay_id}"
                };
                string sign = TenPayV3.GetJsPaySign(result.appId, result.timeStamp.ToString(), result.nonceStr, result.package, MerchantSecret, "MD5");
                result.sign = sign;
                result.signType = "MD5";
                return result;
            }
            else
            {
            }
            return null;
        }

        #region XML 处理

        /// <summary>
        /// 获取XML值
        /// </summary>
        /// <param name="strXml">XML字符串</param>
        /// <param name="strData">字段值</param>
        /// <returns></returns>
        public static string GetXmlValue(string strXml, string strData)
        {
            string xmlValue = string.Empty;
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(strXml);
            var selectSingleNode = xmlDocument.DocumentElement.SelectSingleNode(strData);
            if (selectSingleNode != null)
            {
                xmlValue = selectSingleNode.InnerText;
            }
            return xmlValue;
        }

        /// <summary>
        /// 返回通知 XML
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="returnMsg"></param>
        /// <returns></returns>
        public static string GetReturnXml(string returnCode, string returnMsg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.Append("<return_code><![CDATA[" + returnCode + "]]></return_code>");
            sb.Append("<return_msg><![CDATA[" + returnMsg + "]]></return_msg>");
            sb.Append("</xml>");
            return sb.ToString();
        }

        #endregion XML 处理

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        private TimeSpan GetTimeSpan()
        {
            return DateTime.Now - new DateTime(1970, 1, 1);
        }

        ///<summary>
        ///生成随机字符串
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<param name="useNum">是否包含数字，1=包含，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        ///<param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        ///<param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        ///<returns>指定长度的随机字符串</returns>
        private string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }
    }
}