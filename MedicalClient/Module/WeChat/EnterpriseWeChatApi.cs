using GD.Common.Helper;
using GD.DataAccess;
using GD.Dtos.Doctor.Hospital;
using GD.Dtos.WeChat;
using GD.Module.CommonUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GD.Module.WeChat
{
    /// <summary>
    /// 企业微信接口
    /// </summary>
    public class EnterpriseWeChatApi
    {
        /// <summary>
        /// 获取企业的微信TOKEN URL (7200s过期)
        /// HttpGet 请求
        /// </summary>
        public const string GET_ACCESS_TOKEN = "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpId={0}&corpSecret={1}";
        /// <summary>
        /// 企业微信基础token(客户端) Redis key前缀
        /// </summary>
        public const string WechatAccessTokenPrefix = "CloudDoctor:EnterpriseWechatAccessToken:";
        public static async Task<WeChatLoginResponse> GetEnterpriseAccessToken(string corpId, string corpSecret)
        {
            var key = WechatAccessTokenPrefix + corpId + corpSecret;
            var has = await RedisHelper.Database.KeyExistsAsync(key);
            var response = new WeChatLoginResponse();
            if (!has)
            {
                var url = string.Format(GET_ACCESS_TOKEN, corpId, corpSecret);
                response = await HttpClientHelper.HttpGetAsync<WeChatLoginResponse>(url);
                if (response.Errcode != 0)
                {
                    //取得当前方法类全名 包括命名空间    
                    string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
                    //取得当前方法名    
                    string methedName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    Logger.Error($"获取企业微信TOKEN报错 {url}：{response.Errmsg} {Environment.NewLine} at {className}.{methedName}:");
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
        /// 发送企业微信应用消息
        /// </summary>
        /// <param name="pram"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<QyMessageResponse> SendQyMessageAsync(QyMessageRequestBase pram, string token)
        {
            var sendUrl = string.Format(EnterpriseWeChatApiUrl.SEND_MESSAGE, token);
            var p = JsonConvert.SerializeObject(pram, Formatting.Indented,
                new JsonSerializerSettings { ContractResolver = new LowercaseContractResolver() });
            var response = await HttpClientHelper.HttpPostAsync<QyMessageResponse>(sendUrl, p);
            if (response.Errcode != 0)
            {
                //取得当前方法类全名 包括命名空间    
                string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
                //取得当前方法名    
                string methedName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                Common.Helper.Logger.Error($"发送企业微信应用消息报错 {sendUrl}：{response.Errmsg} {Environment.NewLine} at {className}.{methedName}:{Environment.NewLine} {p}");
            }
            return response;
        }

        /// <summary>
        /// 通过code获取企业微信用户信息
        /// </summary>
        /// <param name="code">前端传入的企业微信重定向code参数</param>
        /// <param name="token">企业微信token</param>
        /// <returns></returns>
        public static async Task<UserDetail> GetEnterpriseWeChatUserInfo(string code, string token)
        {
            UserDetail enterpriseWeChatUserInfo = null;
            //根据code查找用户信息数据
            EnterpriseWeChatApi bill = new EnterpriseWeChatApi();
            string getUserurl = $"https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={token}&code={code}";
            var userModel = await bill.Send<UserModel>(getUserurl);
            if (userModel == null || string.IsNullOrEmpty(userModel.UserId))
            {
                return enterpriseWeChatUserInfo;
            }
            //根据用户Id查询用户信息
            string getUserDetailurl = $"https://qyapi.weixin.qq.com/cgi-bin/user/get?access_token={token}&userid={userModel.UserId}";
            var userDetail = await bill.Send<UserDetail>(getUserDetailurl);
            if (userModel == null)
            {
                return enterpriseWeChatUserInfo;
            }
            enterpriseWeChatUserInfo = userDetail;
            return enterpriseWeChatUserInfo;
        }


        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<T> Send<T>(string url, Dictionary<string, string> values = null)
        {
            HttpClient client = new HttpClient();
            //发送Get请求
            string responseString = string.Empty;
            try
            {
                if (values == null)
                {
                    responseString = await client.GetStringAsync(url);
                    if (!string.IsNullOrEmpty(responseString))
                    {
                        return JsonConvert.DeserializeObject<T>(responseString);
                    }
                }
                else
                {
                    //发送Post请求
                    var content = new FormUrlEncodedContent(values);
                    var response = await client.PostAsync(url, content);
                    responseString = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(responseString))
                    {
                        return JsonConvert.DeserializeObject<T>(responseString);
                    }
                }
            }
            catch (Exception ex)
            {

                Logger.Error($"获取企业微信TOKEN报错 {url}：{ex.Message}");
            }
            return default(T);
        }
    }
}
