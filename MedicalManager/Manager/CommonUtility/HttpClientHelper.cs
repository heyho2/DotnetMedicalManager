using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GD.Manager.CommonUtility
{
    public class HttpClientHelper
    {
        /// <summary>
        /// HttpGet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<T> HttpGetAsync<T>(string url)
        {
            using (HttpClient myHttpClient = new HttpClient())
            {
                //GET提交 返回string
                HttpResponseMessage response = myHttpClient.GetAsync(url).Result;
                var result = "";
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                    var dto = JsonConvert.DeserializeObject<T>(result);
                    return dto;
                }
                return default(T);
            }
        }

        /// <summary>
        /// HttpPost
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="bodyParas"></param>
        /// <returns></returns>
        public static async Task<T> HttpPostAsync<T>(string url, string bodyParas)
        {
            //后台client方式Post提交
            using (HttpClient myHttpClient = new HttpClient())
            {
                HttpContent content = new StringContent(bodyParas, Encoding.UTF8);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = myHttpClient.PostAsync(url, content).Result;
                var result = "";
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                    var dto = JsonConvert.DeserializeObject<T>(result);
                    return dto;
                }
                return default(T);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="xml"></param>
        /// <param name="CertPath"></param>
        /// <param name="CAPassword"></param>
        /// <returns></returns>
        public static Task<string> HttpPostAsync(string url, string xml, string CertPath, string CAPassword)
        {
            try
            {
                HttpClientHandler handler = new HttpClientHandler();
                if (!string.IsNullOrEmpty(CertPath))
                {
                    handler = CertHandler(CertPath, CAPassword);
                }
                //后台client方式Post提交
                using (HttpClient myHttpClient = new HttpClient(handler))
                {
                    MemoryStream val = new MemoryStream(Encoding.UTF8.GetBytes(xml));
                    HttpContent content = new StreamContent(val);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/xml");
                    HttpResponseMessage response = myHttpClient.PostAsync(url, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync();
                        return result;
                    }
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// 请求中带入证书参数
        /// </summary>
        /// <param name="CertPath">证书路径</param>
        /// <param name="CAPassword">证书秘钥</param>
        /// <returns></returns>
        public static HttpClientHandler CertHandler(string CertPath, string CAPassword)
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.SslProtocols = SslProtocols.Tls12;
            //获取证书路径
            //商户私钥证书，用于对请求报文进行签名
            try
            {
                handler.ClientCertificates.Add(new X509Certificate2(CertPath, CAPassword, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet));
            }
            catch (Exception e)
            {

            }
            handler.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls;
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            return handler;
        }
        /// <summary>
        /// httppost读取流
        /// </summary>
        /// <param name="url"></param>
        /// <param name="bodyParas"></param>
        /// <returns></returns>
        public static async Task<Stream> HttpPostStreamAsync(string url, string bodyParas)
        {
            //后台client方式Post提交
            using (HttpClient myHttpClient = new HttpClient())
            {
                HttpContent content = new StringContent(bodyParas, Encoding.UTF8);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = myHttpClient.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    return stream;
                }
                return default(Stream);
            }
        }
    }
}
