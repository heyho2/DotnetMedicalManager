using GD.AppSettings;
using GD.Dtos.MallPay.FangDiInterface;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GD.Mall
{
    /// <summary>
    /// 公共类封装
    /// </summary>
    public class FangDiPayBiz
    {
        #region 基础数据

        /// <summary>
        /// 配置文件
        /// </summary>
        private static readonly Settings settings = Factory.GetSettings("host.json");

        /// <summary>
        /// 渠道ID
        /// </summary>
        private static readonly string ChannelID = settings["FangDi:Client:ChannelID"];

        /// <summary>
        /// OrgID
        /// </summary>
        private static readonly string OrgID = settings["FangDi:Client:OrgID"];

        /// <summary>
        /// PayWay
        /// </summary>
        private static readonly string PayWay = settings["FangDi:Client:PayWay"];

        /// <summary>
        /// PayModel
        /// </summary>
        private static readonly string PayModel = settings["FangDi:Client:PayModel"];

        /// <summary>
        /// 链接地址
        /// </summary>
        private static readonly string Url = settings["FangDi:Client:URL"] + "{0}";  //  URL

        /// <summary>
        ///
        /// </summary>
        private static readonly string RedisKey = "FangDi:Client:WeiXinPayBiz:Appid";

        /// <summary>
        /// 支付前数据
        /// </summary>
        private static readonly string RedisPaymentBeforeKey = "FangDi:Client:WeiXinPayBiz:PaymentBefore";

        #endregion 基础数据

        /// <summary>
        /// 请求获取openid
        /// </summary>
        /// <returns></returns>
        public async Task<GetOpenIDResponseDto> GetOpenID(GetOpenIDRequestDto requestDto)
        {
            string paraJson = JsonConvert.SerializeObject(requestDto);
            string url = string.Format(Url, "GetOpenId");
            var responseDto = await HttpPostAsync<GetOpenIDResponseDto>(url, paraJson);
            return responseDto;
        }

        /// <summary>
        /// h5下单接口
        /// </summary>
        /// <returns></returns>
        public async Task<OrdersPayResponseDto> OrdersPay(OrdersPayRequestDto requestDto)
        {
            requestDto.ChannelId = ChannelID;
            requestDto.OrgId = OrgID;
            requestDto.Pay_Way = PayWay;
            requestDto.Pay_Mode = PayModel;
            requestDto.Subject = System.Web.HttpUtility.UrlEncode(requestDto.Subject);
            string paraJson = JsonConvert.SerializeObject(requestDto);
            string url = string.Format(Url, "ScanCodePay");
            var responseDto = await HttpPostAsync<OrdersPayResponseDto>(url, paraJson);
            return responseDto;
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <returns></returns>
        public async Task<DoRefundResponseDto> RefundAsync(DoRefundRequestDto requestDto)
        {
            requestDto.Reason = System.Web.HttpUtility.UrlEncode(requestDto.Reason);
            string paraJson = JsonConvert.SerializeObject(requestDto);
            string url = string.Format(Url, "Refund");
            var responseDto = await HttpPostAsync<DoRefundResponseDto>(url, paraJson);
            return responseDto;
        }

        /// <summary>
        /// 支付结果查询
        /// </summary>
        /// <returns></returns>
        public async Task<QueryATradeResponseDto> QueryTradeAsync(QueryATradeRequestDto requestDto)
        {
            string paraJson = JsonConvert.SerializeObject(requestDto);
            string url = string.Format(Url, "QueryTrade");
            var responseDto = await HttpPostAsync<QueryATradeResponseDto>(url, paraJson);
            return responseDto;
        }

        /// <summary>
        /// 关闭交易
        /// </summary>
        /// <returns></returns>
        public async Task<DoCloseTradeResponseDto> CloseTradeAsync(DoCloseTradeRequestDto requestDto)
        {
            requestDto.Pay_Way = PayWay;
            string paraJson = JsonConvert.SerializeObject(requestDto);
            string url = string.Format(Url, "CloseTrade");
            var responseDto = await HttpPostAsync<DoCloseTradeResponseDto>(url, paraJson);
            return responseDto;
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
    }
}