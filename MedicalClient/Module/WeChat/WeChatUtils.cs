using GD.Dtos.WeChat;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace GD.Module.WeChat
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
		/// 获取微信随机字符串
		/// </summary>
		/// <returns></returns>
		public static string GetNoncestr()
        {
            Guid val = Guid.NewGuid();
            var bytes = Encoding.GetEncoding("utf-8").GetBytes(val.ToString());
            return BitConverter.ToString(MD5.Create().ComputeHash(bytes)).Replace("-", "").ToUpper();
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
        public static string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
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

        /// <summary>
        /// 微信支付签名算法
        /// </summary>
        /// <param name="xmlDocument">xml文档</param>
        /// <param name="signType">签名类型</param>
        /// <returns></returns>
        public static string MakeSign(XmlDocument xmlDocument,string merchantSecret)
        {
            string url = $"{ToUrl(xmlDocument)}&key={merchantSecret}";
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(url));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            return sb.ToString().ToUpper();
        }

        /// <summary>
        /// 转换成url路径
        /// </summary>
        /// <param name="xmlDocument">xml文档</param>
        /// <returns></returns>
        private static string ToUrl(XmlDocument xmlDocument)
        {
            StringBuilder sb = new StringBuilder();
            XmlNode xmlNode = xmlDocument.FirstChild;
            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                if (xe.Name != "sign" && !string.IsNullOrWhiteSpace(xe.InnerText))
                {
                    sb.Append($"{xe.Name}={xe.InnerText}&");
                }
            }
            return sb.ToString().Trim("&");
        }

       

    }
}
