using GD.Common.Helper;
using GD.DataAccess;
using GD.Dtos.WeChat;
using GD.Module.CommonUtility;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GD.Module.WeChat
{


    /// <summary>
    /// 微信API
    /// </summary>
    public class WeChartApi
    {
        /// <summary>
        /// 微信基础token(客户端) Redis key前缀
        /// </summary>
        public const string WechatAccessTokenPrefix = "CloudDoctor:WechatAccessToken:";


        /// <summary>
        /// 公众号用于调用微信JS接口的临时票据 Redis key 前缀
        /// </summary>
        private static string WechatJsAPITicketPrefix = "CloudDoctor:WechatJsAPITicket:";

        public static async Task<WeChatLoginResponse> GetAccessToken(string appId, string secret)
        {
            var key = WechatAccessTokenPrefix + appId;
            var has = await RedisHelper.Database.KeyExistsAsync(key);
            var response = new WeChatLoginResponse();
            if (!has)
            {
                var url = string.Format(WeChatApiUrl.GET_ACCESS_TOKEN, appId, secret);
                response = await HttpClientHelper.HttpGetAsync<WeChatLoginResponse>(url);
                if (response.Errcode != 0)
                {
                    //取得当前方法类全名 包括命名空间    
                    string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
                    //取得当前方法名    
                    string methedName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    Logger.Error($"获取微信公众号TOKEN报错 {url}：{response.Errmsg} {Environment.NewLine} at {className}.{methedName}:");
                    return new WeChatLoginResponse
                    {
                        Errcode = response.Errcode,
                        Errmsg = response.Errmsg
                    };
                }
                else
                {
                    await RedisHelper.Database.StringSetAsync(key, JsonConvert.SerializeObject(response), new TimeSpan((response.ExpiresIn - 60 * 5) * (long)10000000));
                }
            }
            response = JsonConvert.DeserializeObject<WeChatLoginResponse>(await RedisHelper.Database.StringGetAsync(key));
            return response;
        }

        /// <summary>
        /// 获取微信小程序码
        /// </summary>
        /// <param name="token"></param>
        public static async Task<string> GetWXACode(WXACodeParam param, string token)
        {
            string result = null;
            var p = JsonConvert.SerializeObject(param, Formatting.Indented,
                    new JsonSerializerSettings { ContractResolver = new LowercaseContractResolver() });
            var sendUrl = string.Format(WeChatApiUrl.GET_WXACODE, token);
            try
            {
                var response = await HttpClientHelper.HttpPostStreamAsync(sendUrl, p);
                var bytesArr = StreamHelper.StreamToBytes(response);
                var b64 = Convert.ToBase64String(bytesArr);
                var imgB64 = $"data:image/jpeg;base64,{b64}";
                result = imgB64;
            }
            catch (Exception ex)
            {
                result = null;
                //取得当前方法类全名 包括命名空间    
                string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
                //取得当前方法名    
                string methedName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Common.Helper.Logger.Error($"获取微信小程序码失败 {sendUrl}：{ex.Message} {Environment.NewLine} at {className}.{methedName}:{Environment.NewLine} {p}");
            }
            return result;
        }

        /// <summary>
        /// 微信JS接口的临时票据(7200s过期)
        /// </summary>
        /// <param name="token">此token为微信基础token，并非网页授权token，有效期为7200s</param>
        public static async Task<WechatJsAPITicketResponse> GetJsAPITicket(string appId, string secret)
        {
            var key = WechatJsAPITicketPrefix + appId;
            var has = await RedisHelper.Database.KeyExistsAsync(key);
            var response = new WechatJsAPITicketResponse();
            if (!has)
            {
                var token = await GetAccessToken(appId, secret);
                var url = string.Format(WeChatApiUrl.GET_JSAPI_TICKET, token.AccessToken);
                response = await HttpClientHelper.HttpGetAsync<WechatJsAPITicketResponse>(url);
                if (response.Errcode != 0)
                {
                    //取得当前方法类全名 包括命名空间    
                    string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
                    //取得当前方法名    
                    string methedName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    Common.Helper.Logger.Error($"获取微信JS接口的临时票据出错 {url}：{response.Errmsg} {Environment.NewLine} at {className}.{methedName}:");
                }
                else
                {
                    await RedisHelper.Database.StringSetAsync(key, JsonConvert.SerializeObject(response), new TimeSpan((response.ExpiresIn - 60 * 5) * (long)10000000));
                }
            }
            response = JsonConvert.DeserializeObject<WechatJsAPITicketResponse>(await RedisHelper.Database.StringGetAsync(key));
            return response;
        }

        /// <summary>
        /// 发送微信模板消息
        /// </summary>
        /// <param name="param"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<WeChatResponseDto> SendTemplateMsg(WeChatTemplateMsg param, string token)
        {
            var sendUrl = string.Format(WeChatApiUrl.SEND_TEMPLATE_MSG, token);
            //var tmplateMsg = new WeChatTemplateMsg
            //{
            //    Touser = "of1ho6Ktb59TGP2Nd4473cyhssLI",
            //    Template_Id = "yy6RiFVRRy2kVx064_A9-OSKKN8o8PwDV7k1j5whYRc",
            //    Data = new
            //    {
            //        //First = new { Value = "first", Color = "#173177" },
            //        ConsumerName = new { Value = "张美丽", Color = "#173177" },
            //        AppointmentDate = new { Value = "2019-05-01 09:00:00", Color = "#173177" },
            //        TherapistName = new { Value = "Amy", Color = "#173177" },
            //        ProjectName = new { Value = "玻尿酸", Color = "#173177" },
            //        Remark = new { Value = "请临近预约时间时主动联系客户", Color = "#173177" },
            //        Remark1 = new { Value = "11111", Color = "#173177" }
            //    }
            //};
            var p = JsonConvert.SerializeObject(param, Formatting.Indented,
                new JsonSerializerSettings { ContractResolver = new LowercaseContractResolver() });
            var response = await HttpClientHelper.HttpPostAsync<WeChatResponseDto>(sendUrl, p);
            return response;
        }

        /// <summary>
        /// 发送客服消息
        /// </summary>
        /// <param name="pram">参数</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        public static async Task<WeChatResponseDto> SendCustomMsg(CustomSendMsgBase pram, string token)
        {
            var sendUrl = string.Format(WeChatApiUrl.SEND_CUSTOM_MSG, token);
            var p = JsonConvert.SerializeObject(pram, Formatting.Indented,
                new JsonSerializerSettings { ContractResolver = new LowercaseContractResolver() });
            var response = await HttpClientHelper.HttpPostAsync<WeChatResponseDto>(sendUrl, p);
            if (response.Errcode != 0)
            {
                //取得当前方法类全名 包括命名空间    
                string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
                //取得当前方法名    
                string methedName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Common.Helper.Logger.Error($"发送客服消息报错 {sendUrl}：{response.Errmsg} {Environment.NewLine} at {className}.{methedName}:{Environment.NewLine} {p}");
            }
            return response;
        }

        /// <summary>
        /// 通过code换取网页授权access_token和OpenId
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="secret"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static async Task<Oauth2AccessTokenResponseDto> Oauth2AccessTokenAsync(string appId, string secret, string code)
        {
            var url = string.Format(WeChatApiUrl.OAUTH2_ACCESS_TOKEN, appId, secret, code);
            var response = await HttpClientHelper.HttpGetAsync<Oauth2AccessTokenResponseDto>(url);
            if (response.Errcode != 0)
            {
                //取得当前方法类全名 包括命名空间    
                string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
                //取得当前方法名    
                string methedName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Logger.Error($"获取网页授权token和openid失败 {url}：{response.Errmsg} {Environment.NewLine} at {className}.{methedName}:");
                return new Oauth2AccessTokenResponseDto
                {
                    Errcode = response.Errcode,
                    Errmsg = response.Errmsg
                };
            }
            return response;
        }

        /// <summary>
        /// 获取公众号二维码
        /// </summary>
        public static async Task<CreateQRCodeResponseDto> CreateTemporaryQRCodeAsync(CreateQRCodeRequestDto pram, string token)
        {
            var sendUrl = string.Format(WeChatApiUrl.CREATE_QRCODE, token);
            var p = JsonConvert.SerializeObject(pram, Formatting.Indented);
            var response = await HttpClientHelper.HttpPostAsync<CreateQRCodeResponseDto>(sendUrl, p);
            if (response.Errcode != 0)
            {
                //取得当前方法类全名 包括命名空间    
                string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
                //取得当前方法名    
                string methedName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Common.Helper.Logger.Error($"获取公众号二维码报错 {sendUrl}：{response.Errmsg} {Environment.NewLine} at {className}.{methedName}:{Environment.NewLine} {p}");
            }
            return response;
        }

        /// <summary>
        /// 获取公众号用户信息
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="secret"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static async Task<GetUserInfoResponseDto> GetUserInfoAsync(string openId, string token)
        {
            var url = string.Format(WeChatApiUrl.GET_USER_INFO, token, openId);
            var response = await HttpClientHelper.HttpGetAsync<GetUserInfoResponseDto>(url);
            if (response.Errcode != 0)
            {
                //取得当前方法类全名 包括命名空间    
                string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
                //取得当前方法名    
                string methedName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Logger.Error($"获取公众号用户信息失败 {url}：{response.Errmsg} {Environment.NewLine} at {className}.{methedName}:");
                return new GetUserInfoResponseDto
                {
                    Errcode = response.Errcode,
                    Errmsg = response.Errmsg
                };
            }
            return response;
        }

    }
    /// <summary>
    /// json序列化设置：key值全小写
    /// </summary>
    public class LowercaseContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
}
