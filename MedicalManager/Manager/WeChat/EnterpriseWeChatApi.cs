using GD.Common.Helper;
using GD.DataAccess;
using GD.Dtos.Doctor.Hospital;
using GD.Dtos.WeChat;
using GD.Manager.CommonUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GD.Manager.WeChat
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
    }
}
