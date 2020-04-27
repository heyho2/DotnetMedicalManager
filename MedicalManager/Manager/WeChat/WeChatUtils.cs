using GD.Dtos.WeChat;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace GD.Manager.WeChat
{
    public class WeChatUtils
    {
        /// <summary>
        /// 微信回调，验证微信签名
        /// </summary>
        /// * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        /// <returns></returns>
        public static bool CheckSignature(string token, Dictionary<string, string> queryString)
        {
            string signature = queryString["signature"];
            string timestamp = queryString["timestamp"];
            string nonce = queryString["nonce"];
            string[] ArrTmp = { token, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = tmpStr.Hash().ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
		/// 获取随机字符串
		/// </summary>
		/// <returns></returns>
		public static string GetNoncestr()
        {
            Guid val = Guid.NewGuid();
            var bytes = Encoding.GetEncoding("utf-8").GetBytes(val.ToString());
            return BitConverter.ToString(MD5.Create().ComputeHash(bytes)).Replace("-", "").ToUpper();
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimestamp()
        {
            TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
            return ts.TotalMilliseconds.ToString();
        }

        public static T ConvertXmlToObj<T>(string xmlstr)
        {
            XElement xdoc = XElement.Parse(xmlstr);
            var type = typeof(T);
            var t = Activator.CreateInstance<T>();
            foreach (XElement element in xdoc.Elements())
            {
                var pr = type.GetProperty(element.Name.ToString());
                if (pr == null)
                {
                    continue;
                }
                if (element.HasElements)
                {//这里主要是兼容微信新添加的菜单类型。nnd，竟然有子属性，所以这里就做了个子属性的处理
                    foreach (var ele in element.Elements())
                    {
                        pr = type.GetProperty(ele.Name.ToString());
                        pr.SetValue(t, Convert.ChangeType(ele.Value, pr.PropertyType), null);
                    }
                    continue;
                }
                if (pr.PropertyType.Name == "WeChartMsgType")//获取消息模型
                {
                    pr.SetValue(t, (WeChartMsgType)Enum.Parse(typeof(WeChartMsgType), element.Value.ToUpper()), null);
                    continue;
                }
                if (pr.PropertyType.Name == "WeChartEvent")//获取事件类型。
                {
                    pr.SetValue(t, (WeChartEvent)Enum.Parse(typeof(WeChartEvent), element.Value.ToUpper()), null);
                    continue;
                }
                pr.SetValue(t, Convert.ChangeType(element.Value, pr.PropertyType), null);
            }
            return t;
        }





    }
}
