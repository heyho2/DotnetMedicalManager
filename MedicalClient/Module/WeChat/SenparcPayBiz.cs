using GD.Common.Helper;
using GD.Dtos.WeChat;
using Newtonsoft.Json;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.TenPay.V3;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GD.Module.WeChat
{
    public class SenparcPayBiz
    {
        /// <summary>
        /// 微信公众号静默授权Code换openid和token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetOpenIdByOauth2AccessToken(string code)
        {
            var res = OAuthApi.GetAccessToken(PlatformSettings.CDClientAppId, PlatformSettings.CDClientAppSecret, code);
            if (res.errcode == ReturnCode.请求成功)
            {
                return res.openid;

            }
            Logger.Error($"code换openid失败-appid({PlatformSettings.CDClientAppId}):{res.errmsg}{Environment.NewLine} at {nameof(SenparcPayBiz)}.{nameof(GetOpenIdByOauth2AccessToken)}({code})");
            return null;
        }

        public async Task<(bool, GetH5PaymentBeforeResponseDto)> GetH5PaymentBeforeAsync(GetH5PaymentBeforeResquestDto requstDto)
        {
            //微信下单
            
            //随机字符串参数
            string nonceStr = WeChatUtils.GetRandomString(16, false, true, true, false, string.Empty);
            var unifiedOrderRequestDto = new UnifiedOrderRequestDto
            {
                AppId = PlatformSettings.CDClientAppId,
                MerchantId = requstDto.MerchantId,
                Body = requstDto.Body,
                OutTradeNo = requstDto.OutTradeNo,
                TotalFee = requstDto.TotalFee,//支付金额，单位分
                NotifyUrl = requstDto.NotifyUrl,//支付通知回调
                OpenId = requstDto.OpenId,
                MerchantSecret = requstDto.MerchantSecret,
                NonceStr = nonceStr
            };
            var unifiedRes = await UnifiedOrderAsync(unifiedOrderRequestDto);
            if (!unifiedRes.IsReturnCodeSuccess() || !unifiedRes.IsResultCodeSuccess())
            {
                var errMsg = new StringBuilder();
                errMsg.Append($"微信支付下单失败：return_msg-{unifiedRes.return_msg}|err_code-{unifiedRes.err_code}|err_code_des-{unifiedRes.err_code_des}");
                errMsg.Append($"{Environment.NewLine}at SenparcPayBiz.GetH5PaymentBeforeAsync({JsonConvert.SerializeObject(requstDto)})");
                Logger.Error(errMsg.ToString());
                return (false, null);
            }
            var result = new GetH5PaymentBeforeResponseDto
            {
                AppId = unifiedOrderRequestDto.AppId,
                TimeStamp = WeChatUtils.GetTimestamp(),
                NonceStr = unifiedOrderRequestDto.NonceStr,
                Package = $"prepay_id={unifiedRes.prepay_id}",
                SignType="MD5"
            };
            result.Sign = TenPayV3.GetJsPaySign(
                unifiedOrderRequestDto.AppId,
                result.TimeStamp, result.NonceStr,
                result.Package,
                unifiedOrderRequestDto.MerchantSecret,
                "MD5");
            return (true, result);
        }

        /// <summary>
        /// 微信支付统一下单,订单失效时间是1小时10分钟
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<UnifiedorderResult> UnifiedOrderAsync(UnifiedOrderRequestDto request)
        {
            TenPayV3UnifiedorderRequestData data = new TenPayV3UnifiedorderRequestData(request.AppId, request.MerchantId, request.Body, request.OutTradeNo, request.TotalFee, "", request.NotifyUrl, Senparc.Weixin.TenPay.TenPayV3Type.JSAPI, request.OpenId, request.MerchantSecret, request.NonceStr, "WEB", DateTime.Now, DateTime.Now.AddHours(1).AddMinutes(10));
            data.SignType = "MD5";
            return await TenPayV3.UnifiedorderAsync(data);
        }
    }
}
