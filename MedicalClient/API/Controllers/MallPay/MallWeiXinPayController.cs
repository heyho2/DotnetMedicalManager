using GD.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using GD.Dtos.MallPayDto;
using GD.Mall;
using GD.Models.Mall;
using GD.Models.CommonEnum;
using Newtonsoft.Json;
using GD.Dtos.Mall.WeiXinPay;
using System.Threading.Tasks;

namespace GD.API.Controllers.MallPay
{
    /// <summary>
    /// 微信支付相关
    /// </summary>
    public class MallWeiXinPayController : MallPayBaseController
    {
        /// <summary>
        /// 获取微信OpenApi和Token授权
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<WeiXinToken>))]
        public async Task<IActionResult> GetOpenidAndAccessTokenFromCode(WeiXinTokenRequestDto requestDto)
        {
            return Success<WeiXinToken>(await new WeiXinPayBiz().GetOpenidAndAccessTokenFromCodeAsync(this.UserID, requestDto.code));
        }

        /// <summary>
        /// 微信支付前准备
        /// </summary>
        /// <param name="requestDto">requestDto</param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetWeiXinPaymentBeforeRequestDto>))]
        public async Task<IActionResult> GetPaymentBefore(GetWeiXinPaymentBeforeRequestDto requestDto)
        {
            return Success<GetWeiXinPaymentBeforeResponseDto>(await new WeiXinPayBiz().GetPaymentBeforeAsync(this.UserID, requestDto));
        }

        /// <summary>
        /// 支付回调方法
        /// </summary>
        /// <param name="requestDto">requestDto</param>
        /// <returns></returns>
        [HttpPost]
        public string PaymentBackFunction([FromBody]WeiXinPaymentBackFunctionRequestDto requestDto)
        {
            Common.Helper.Logger.Info($"DoPaymentPush=>requestDto:{JsonConvert.SerializeObject(requestDto)}");
            return "{\"resultMsg\":\"SUCCESS\"}";
        }

    }
}