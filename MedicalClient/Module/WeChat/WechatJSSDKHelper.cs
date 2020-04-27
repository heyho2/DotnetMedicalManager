using GD.Dtos.WeChat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GD.Module.WeChat
{
    /// <summary>
    /// 微信JS-SKD帮助类
    /// </summary>
    public class WechatJSSDKHelper
    {
        /// <summary>
        /// 获取JS-SDK权限验证的签名Signature
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="url">当前网页的URL，不包含#及其后面部分</param>
        /// <returns></returns>
        public static async Task<WeChatJsSDKSignatureResponse> GetSignatureAsync(string appId, string appSecret, string url)
        {
            var ticketResponse = await WeChartApi.GetJsAPITicket(appId, appSecret);
            string ticket = ticketResponse.Ticket;
            string noncestr = WeChatUtils.GetNoncestr();
            string timestamp = WeChatUtils.GetTimestamp();
            Hashtable val = new Hashtable();
            val.Add("jsapi_ticket", ticket);
            val.Add("noncestr", noncestr);
            val.Add("timestamp", timestamp);
            val.Add("url", url);
            return new WeChatJsSDKSignatureResponse
            {
                AppId = appId,
                NonceStr = noncestr,
                Timestamp = timestamp,
                Signature = CreateSha1(val)
            };
        }

        /// <summary>
		/// 签名算法
		/// </summary>
		/// <returns></returns>
		private static string CreateSha1(Hashtable parameters)
        {
            StringBuilder val = new StringBuilder();
            ArrayList val2 = new ArrayList(parameters.Keys);
            val2.Sort(ASCIISort.Create());
            IEnumerator enumerator = val2.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    if (parameters[current] != null)
                    {
                        string text = (string)parameters[current];
                        if (val.Length == 0)
                        {
                            val.Append(string.Concat(current, "=", text));
                        }
                        else
                        {
                            val.Append(string.Concat(new object[4]
                            {
                                "&",
                                current,
                                "=",
                                text
                            }));
                        }
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                disposable?.Dispose();
            }
            return val.ToString().Hash().ToLower();
        }
    }

    /// <summary>
    /// ASCII字典排序
    /// </summary>
    public class ASCIISort : IComparer
    {
        /// <summary>
        /// 创建新的ASCIISort实例
        /// </summary>
        /// <returns></returns>
        public static ASCIISort Create()
        {
            return new ASCIISort();
        }


        public int Compare(object x, object y)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(x.ToString());
            byte[] bytes2 = Encoding.ASCII.GetBytes(y.ToString());
            int num = bytes.Length;
            int num2 = bytes2.Length;
            int num3 = Math.Min(num, num2);
            for (int i = 0; i < num3; i++)
            {
                byte b = bytes[i];
                byte b2 = bytes2[i];
                if (b > b2)
                {
                    return 1;
                }
                if (b < b2)
                {
                    return -1;
                }
            }
            if (num == num2)
            {
                return 0;
            }
            if (num > num2)
            {
                return 1;
            }
            return -1;
        }


    }
}
