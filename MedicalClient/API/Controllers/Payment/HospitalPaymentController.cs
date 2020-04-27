using GD.Common;
using GD.Common.Helper;
using GD.Doctor;
using GD.Dtos.Payment.HospitalPayment;
using GD.Dtos.WeChat;
using GD.Models.Doctor;
using GD.Models.Payment;
using GD.Module.WeChat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace GD.API.Controllers.Payment
{
    /// <summary>
    /// 医院支付控制器
    /// </summary>
    public class HospitalPaymentController : PaymentBaseController
    {
        /// <summary>
        /// 智慧云医公众号code换openid
        /// </summary>
        /// <param name="code"></param>
        /// <returns>直接返回openid字符串</returns>
        [HttpGet, AllowAnonymous]
        [Produces(typeof(ResponseDto<string>))]
        public IActionResult GetOpenIdByOauth2AccessToken(string code)
        {
            var paymentBiz = new SenparcPayBiz();
            var openId = paymentBiz.GetOpenIdByOauth2AccessToken(code);
            if (string.IsNullOrWhiteSpace(openId))
            {
                Failed(Common.ErrorCode.SystemException, "获取openid失败");
            }
            return Success<string>(openId);
        }
        /// <summary>
        /// 医院公众号二维码扫码支付下单
        /// </summary>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        [Produces(typeof(ResponseDto<GetH5PaymentBeforeResponseDto>))]
        public async Task<IActionResult> HospitalUnifiedOrderAsync([FromBody]HospitalUnifiedOrderRequestDto requestDto)
        {
            var hospitalModel = await new HospitalBiz().GetAsync(requestDto.HospitalGuid);
            var hospitalPaymentConfig = await new HospitalPaymentConfigBiz().GetAsync(hospitalModel.HospitalGuid);
            if (hospitalPaymentConfig == null)
            {
                Logger.Error($"未配置商户id(merchantId)对应的医院支付配置数据 at HospitalPaymentController.HospitalUnifiedOrderAsync {Environment.NewLine}{JsonConvert.SerializeObject(requestDto)}");
                return Failed(ErrorCode.UserData, $"未配置{hospitalModel.HosName}的医院支付配置数据");
            }
            var outTradeNo = $"GDSMDD_{WeChatUtils.GetRandomString(10, false, false, true, false, "")}{DateTime.Now.ToString("yyyyMMddHHmmss")}";
            var paymentBiz = new SenparcPayBiz();
            var payDto = new GetH5PaymentBeforeResquestDto
            {
                OutTradeNo = outTradeNo,
                MerchantId = hospitalPaymentConfig.MerchantId,
                MerchantSecret = hospitalPaymentConfig.MerchantSecret,
                OpenId = requestDto.OpenId,
                TotalFee = requestDto.TotalFee,
                Body = $"{hospitalModel.HosName}扫码支付",
                NotifyUrl = $"{PlatformSettings.APIDomain}payment/HospitalPayment/PaymentBackAsync"
            };
            (var result, var response) = await paymentBiz.GetH5PaymentBeforeAsync(payDto);
            if (!result)
            {
                return Failed(ErrorCode.SystemException, "微信支付失败");
            }
            var hospitalPayment = new HospitalPaymentModel
            {
                PaymentGuid = Guid.NewGuid().ToString("N"),
                HospitalGuid = requestDto.HospitalGuid,
                OutTradeNo = payDto.OutTradeNo,
                MerchantNo = payDto.MerchantId,
                PayAccount = payDto.OpenId,
                Amount = payDto.TotalFee,
                Status = PayStatusEnum.WaitForPayment.ToString(),
                CreatedBy = "HospitalUnifiedOrderAsync",
                LastUpdatedBy = "HospitalUnifiedOrderAsync",
                OrgGuid = string.Empty
            };
            var res = await new HospitalPaymentBiz().InsertAsync(hospitalPayment);
            if (!res)
            {
                var errMsg = new StringBuilder();
                errMsg.Append($"医院微信扫码支付记录业务数据失败,hospitalPaymentModel:{JsonConvert.SerializeObject(hospitalPayment)}");
                errMsg.Append($"{Environment.NewLine}at HospitalPaymentController.HospitalUnifiedOrderAsync({JsonConvert.SerializeObject(requestDto)})");
                Logger.Error(errMsg.ToString());
            }
            return Success(response);
        }

        /// <summary>
        /// 医院扫码支付回调
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> PaymentBackAsync()
        {
            var xml = HttpRequestToString();
            Logger.Info($"医院扫码支付回调-HospitalPaymentController.PaymentBackAsync-xml：{Environment.NewLine}{xml}");
            if (string.IsNullOrWhiteSpace(xml))
            {
                return Ok(WeChatUtils.GetReturnXml("FAIL", "未返回XML数据"));
            }


            XElement xdoc = XElement.Parse(xml);
            var returnCode = xdoc.Element("return_code")?.Value;
            var returnMsg = xdoc.Element("return_msg")?.Value;
            if (returnCode != "SUCCESS")
            {
                Logger.Error($"医院扫码支付回调通讯失败：{returnMsg} at HospitalPaymentController.PaymentBackAsync {Environment.NewLine}{xml}");
                return Ok(WeChatUtils.GetReturnXml("FAIL", "支付结果失败"));
            }
            var outTradeNo = xdoc.Element("out_trade_no")?.Value;
            var paymentModel = await new HospitalPaymentBiz().GetModelByOutTradeNoAsync(outTradeNo);
            if (paymentModel == null || paymentModel?.Status != PayStatusEnum.WaitForPayment.ToString())
            {
                return Ok(WeChatUtils.GetReturnXml("SUCCESS", "OK"));
            }

            var merchantId = xdoc.Element("mch_id")?.Value;
            //通过商户id获取商户秘钥
            var merchantInfos = await new HospitalPaymentConfigBiz().GetModelsByMerchantIdAsync(merchantId);
            var merchantInfo = merchantInfos.FirstOrDefault(a => a.HospitalGuid == paymentModel.HospitalGuid);
            if (!merchantInfos.Any() || merchantInfo == null)
            {
                Logger.Error($"未配置商户id({merchantId})对应医院({paymentModel.HospitalGuid})的支付配置数据 at HospitalPaymentController.PaymentBackAsync {Environment.NewLine}{xml}");
                return Ok(WeChatUtils.GetReturnXml("FAIL", "缺乏商户配置数据"));
            }
            //验证回调签名
            var checkSignRes = CheckSign(xml, merchantInfo.MerchantSecret);
            if (!checkSignRes)
            {
                Logger.Error($"医院扫码支付回调签名不合法 at HospitalPaymentController.PaymentBackAsync {Environment.NewLine}{xml}");
                return Ok(WeChatUtils.GetReturnXml("FAIL", "签名不合法"));
            }
            var resultCode = xdoc.Element("result_code")?.Value;
            var isSuccess = resultCode == "SUCCESS";
            //若支付不成功，则需要记录不成功原因
            if (!isSuccess)
            {
                var erroCode = xdoc.Element("err_code")?.Value;
                var erroCodeDes = xdoc.Element("err_code_des")?.Value;
                Logger.Error($"医院扫码支付回调结果通知支付失败：err_code-{erroCode} err-code_des-{erroCodeDes} at HospitalPaymentController.PaymentBackAsync {Environment.NewLine}{xml}");
                return Ok(WeChatUtils.GetReturnXml("FAIL", "支付结果失败"));
            }


            paymentModel.Status = isSuccess ? PayStatusEnum.Success.ToString() : PayStatusEnum.Failure.ToString();
            paymentModel.TransactionId = xdoc.Element("transaction_id")?.Value;
            paymentModel.LastUpdatedBy = "PaymentBackAsync";
            paymentModel.LastUpdatedDate = DateTime.Now;
            var result = await new HospitalPaymentBiz().UpdateAsync(paymentModel);
            DoAfterPaymentBackAsync(paymentModel);
            return Ok(WeChatUtils.GetReturnXml("SUCCESS", "OK"));
        }



        private async Task DoAfterPaymentBackAsync(HospitalPaymentModel paymentModel)
        {
            //1.写入评价数据
            var evaluationGuid = await CreateHospitalEvaluationAsync();
            //2.发送模板消息通知用户支付成功
            var hospitalModel = await new HospitalBiz().GetAsync(paymentModel.HospitalGuid);
            var notifyDto = new PaymentSuccessMsgNotifyDto
            {
                EvaluationId = evaluationGuid,
                OpenId = paymentModel.PayAccount,
                TotalFee = paymentModel.Amount / 100M,
                HospitalName = hospitalModel?.HosName,
                HospitalGuid = hospitalModel?.HospitalGuid,
                TransactionTime = paymentModel.CreationDate,
                TransactionNo = paymentModel.TransactionId
            };
            PaymentSuccessMsgNotify(notifyDto);
            //3.通知数据看板更新数据
            RealtimeNotification(new HospitalDataBoardNotificationMsg
            {
                NotificationType = 1,
                HospitalGuid = paymentModel.HospitalGuid,
                Amount = notifyDto.TotalFee
            });
        }

        /// <summary>
        /// 创建支付评价记录
        /// </summary>
        /// <returns></returns>
        private async Task<string> CreateHospitalEvaluationAsync()
        {
            try
            {
                var evaluationModel = new HospitalEvaluationModel
                {
                    EvaluationGuid = Guid.NewGuid().ToString("N"),
                    UserGuid = string.Empty,
                    HospitalGuid = string.Empty,
                    OfficeGuid = string.Empty,
                    EvaluationTag = string.Empty,
                    Score = 0,
                    ConditionDetail = string.Empty,
                    Anonymous = false,
                    CreatedBy = string.Empty,
                    LastUpdatedBy = string.Empty,
                    OrgGuid = string.Empty

                };
                var result = await new HospitalEvaluationBiz().InsertAsync(evaluationModel);
                return result ? evaluationModel.EvaluationGuid : null;
            }
            catch (Exception ex)
            {
                Logger.Error($"医院扫码支付成功后创建医院调查数据失败：{ex.Message}{Environment.NewLine} at HospitalPaymentController.CreateHospitalEvaluationAsync");
                return null;
            }

        }

        /// <summary>
        /// 支付成功模板消息通知
        /// </summary>
        /// <param name="dto"></param>
        private void PaymentSuccessMsgNotify(PaymentSuccessMsgNotifyDto dto)
        {
            try
            {
                var resToken = WeChartApi.GetAccessToken(PlatformSettings.CDClientAppId, PlatformSettings.CDClientAppSecret).Result;
                Logger.Debug($"PaymentSuccessMsgNotify获取token-{JsonConvert.SerializeObject(resToken)}");
                if (string.IsNullOrWhiteSpace(resToken.AccessToken))
                {
                    var err = new StringBuilder();
                    err.Append($"GD.API.Controllers.Payment.{nameof(HospitalPaymentController)}.{nameof(PaymentSuccessMsgNotify)}{Environment.NewLine}{JsonConvert.SerializeObject(dto)} {Environment.NewLine} ");
                    err.Append($"error: 支付成功模板消息通知获取token失败。{ resToken.Errmsg}");
                    Logger.Error(err.ToString());
                    return;
                }
                var urlHosName = System.Web.HttpUtility.UrlEncode(dto.HospitalName, System.Text.Encoding.UTF8);
                var clientMsg = new WeChatTemplateMsg
                {
                    Touser = dto.OpenId,
                    Template_Id = PlatformSettings.HospitalSuccessPayTemplateMsgId,
                    Url = $"{PlatformSettings.HospitalEvaluationUrl}/{dto.HospitalGuid}/{dto.EvaluationId}/{urlHosName}",
                    Data = new
                    {
                        //副标题
                        First = new { Value = "您已缴费成功，祝您早日康复！" },
                        //交易商户
                        Keyword1 = new { Value = dto.HospitalName },
                        //交易单号
                        Keyword2 = new { Value = dto.TransactionNo },
                        //缴费金额
                        Keyword3 = new { Value = dto.TotalFee.ToString("F2") },
                        //交易时间
                        Keyword4 = new { Value = dto.TransactionTime.ToString("yyyy-MM-dd HH:mm:ss") },
                        //备注
                        Remark = new { Value = "诚邀您对本次就诊进行评价，后续您将获得线上各种专属定制福利，如健康随访，线上免费咨询等", Color = "#4eb2ff" },
                    }
                };
                var clientTempMsgRes = WeChartApi.SendTemplateMsg(clientMsg, resToken.AccessToken);
                Logger.Debug($"PaymentSuccessMsgNotify发送模板消息-{JsonConvert.SerializeObject(clientTempMsgRes)}");
            }
            catch (Exception ex)
            {
                Logger.Error($"医院扫码支付成功模板消息通知失败：{ex.Message}{Environment.NewLine} at HospitalPaymentController.PaymentSuccessMsgNotify {Environment.NewLine}{JsonConvert.SerializeObject(dto)}");
            }

        }

        /// <summary>
        /// 检测支付回调签名是否正确
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="merchantSecret"></param>
        /// <returns></returns>
        private bool CheckSign(string xml, string merchantSecret)
        {
            XElement xdoc = XElement.Parse(xml);
            //获取签名
            string sign = xdoc.Element("sign")?.Value;
            //如果没有设置签名，则跳过检测
            if (string.IsNullOrWhiteSpace(sign))
            {
                Logger.Error($"{nameof(CheckSign)} => 微信回调签名不存在!");
                return false;
            }
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            //在本地计算新的签名
            string cal_sign = WeChatUtils.MakeSign(xmlDocument, merchantSecret);
            if (cal_sign != sign)
            {
                Logger.Error($"{nameof(CheckSign)} => 微信回调签名不合法!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// HTTPBody转成字符串
        /// </summary>
        /// <returns></returns>
        private string HttpRequestToString()
        {
            Stream s = Request.Body;
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            return builder.ToString();
        }


        /// <summary>
        /// 实时通知微信收款看板数据更新
        /// </summary>
        private void RealtimeNotification(HospitalDataBoardNotificationMsg msg)
        {
            try
            {
                var bus = Communication.MQ.Client.CreateConnection();
                var advancedBus = bus.Advanced;
                if (advancedBus.IsConnected)
                {
                    var exchange = advancedBus.ExchangeDeclare("HospitalDataBoardExchange", "fanout");
                    var queue = advancedBus.QueueDeclare("HospitalDataBoardQueue");
                    advancedBus.Bind(exchange, queue, "hospitalDataBoard");
                    advancedBus.Publish(exchange, "", false, new EasyNetQ.Message<HospitalDataBoardNotificationMsg>(msg));
                }
                bus.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Error($"医院扫码支付成功后实时通知微信收款看板数据更新失败：{ex.Message}{Environment.NewLine} at HospitalPaymentController.RealtimeNotification{Environment.NewLine}{JsonConvert.SerializeObject(msg)}");
            }
        }

        /// <summary>
        /// MQ通知
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public IActionResult MQNotify()
        {
            RealtimeNotification(new HospitalDataBoardNotificationMsg());
            return Success();
        }

    }


}
