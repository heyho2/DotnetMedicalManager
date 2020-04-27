using GD.Common;
using GD.Common.Helper;
using GD.DataAccess;
using GD.Dtos.MallPay.ControllerApi;
using GD.Dtos.MallPay.FangDiInterface;
using GD.Mall;
using GD.Models.Mall;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GD.API.Controllers.MallPay
{
    /// <summary>
    /// 方迪-商城支付相关
    /// </summary>
    public class FangDiPayController : MallPayBaseController
    {
        /// <summary>
        /// 请求获取openid
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetOpenIDResponseDto>))]
        public async Task<IActionResult> GetOpenIDByCodeTest([FromBody]GetOpenIDRequestDto requestDto)
        {
            var responseDto = await new FangDiPayBiz().GetOpenID(requestDto);
            return Success(responseDto);
        }

        /// <summary>
        /// h5下单接口
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<OrdersPayResponseDto>))]
        public async Task<IActionResult> OrdersPay([FromBody]OrdersPayRequestDto requestDto)
        {
            var responseDto = await new FangDiPayBiz().OrdersPay(requestDto);
            return Success(responseDto);
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<DoRefundResponseDto>))]
        public async Task<IActionResult> OrderRefundAsync([FromBody]DoRefundRequestDto requestDto)
        {
            var response = await new FangDiPayBiz().RefundAsync(requestDto);
            return Success(response);
        }

        /// <summary>
        /// 退款--test
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<DoRefundResponseDto>))]
        public async Task<IActionResult> OrderListRefundAsync(bool isRun = true)
        {
            return Success();
            var modelList = await new TransactionFlowingBiz().GetModels();
            modelList = modelList.Where(a => a.CreationDate > Convert.ToDateTime("2019-10-10 00:00:00")).ToList();
            var count = DateTime.Now.Second;
            foreach (var item in modelList)
            {
                Logger.Warn($@"----{ JsonConvert.SerializeObject(item).ToString()}");
                count++;
                if (item.Amount < 1) item.Amount = item.Amount * 100;
                var requestDto = new DoRefundRequestDto
                {
                    Reason = "AsyncRefunding.......",
                    Refund_Fee = Convert.ToInt32(item.Amount).ToString(),
                    Refund_No = count.ToString(),
                    Trade_No = item.OutTradeNo
                };
                var response = await new FangDiPayBiz().RefundAsync(requestDto);
                if (response.ResultCode.Equals("0") && response.ResultMsg.ToLower().Equals("success"))
                {
                    Logger.Warn($@"OrderListRefundAsync--退款成功--{ JsonConvert.SerializeObject(response).ToString()}");
                    item.TransactionStatus = TransactionStatusEnum.RefundSuccess.ToString();
                    item.LastUpdatedDate = DateTime.Now;
                    item.OutRefundNo = response.Refund_No;
                    item.Update();
                }
                Logger.Warn($@"AsyncRefunding----{ JsonConvert.SerializeObject(response).ToString()}");
            }
            return Success();
        }

        /// <summary>
        /// 支付结果查询
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<QueryATradeResponseDto>))]
        public async Task<IActionResult> QueryTradeAsync([FromBody]QueryATradeRequestDto requestDto)
        {
            var responseDto = await new FangDiPayBiz().QueryTradeAsync(requestDto);
            return Success(responseDto);
        }

        /// <summary>
        /// 关闭交易
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<DoCloseTradeResponseDto>))]
        public async Task<IActionResult> CloseTradeAsync([FromBody]DoCloseTradeRequestDto requestDto)
        {
            var responseDto = await new FangDiPayBiz().CloseTradeAsync(requestDto);
            return Success(responseDto);
        }

        /// <summary>
        /// 第三方支付回调
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<PaymentPushResponse>)), AllowAnonymous]
        public IActionResult DoPaymentPush([FromBody]PaymentPushRequest requestDto)
        {
            Logger.Info($"DoPaymentPush=>requestDto:{JsonConvert.SerializeObject(requestDto)}");
            var response = new PaymentPushResponse
            {
                resultCode = "0",
                resultMsg = "SUCCESS"
            };
            return Ok(response);
        }

        /// <summary>
        /// 微信支付回调方法
        /// </summary>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public IActionResult WeiXinPaymentBackFunction()
        {
            string result = WeiXinPaymentBackFunction(HttpRequestToString());
            return Ok(result); ;
        }

        /// <summary>
        /// 微信退款回调方法
        /// </summary>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public IActionResult WeiXinRefundBackFunction()
        {
            string result = WeiXinRefundBackFunction(HttpRequestToString());
            return Ok(result); ;
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

        ///// <summary>
        ///// 第三方退款回调
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost, Produces(typeof(ResponseDto<string>))]
        //public string PaymentBackFunction()
        //{
        //    return GetReturnXml("SUCCESS", "OK");
        //}

        ///// <summary>
        ///// 扫码付--返回二维码(Base64)
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet, Produces(typeof(ResponseDto<GetScanCodeStringResponseDto>))]
        //public IActionResult DoScanCodePay(GetScanCodeStringRequestDto requestDto)
        //{
        //    var orderModel = new OrderBiz().Getmodel(requestDto.OrderGuid);
        //    if (orderModel == null)
        //    {
        //        return Failed(ErrorCode.FormatError, "订单编号查询数据有误，请检查！");
        //    }
        //    var result = new FangDiPayBiz().ScanCodePayAsync(requestDto);
        //    if (result == null)
        //    {
        //        return Failed(ErrorCode.FormatError, "请求失败，请联系IT");
        //    }
        //    var payBiz = new FinancePayBiz();
        //    var model = payBiz.GetModelAsyncByTradeNo(requestDto.Trade_No);
        //    var isSuccess = false;
        //    if (model.Result != null)
        //    {
        //        model.Result.ChannelID = requestDto.ChannelId;
        //        model.Result.PayWay = requestDto.Pay_Way;
        //        model.Result.PayMode = requestDto.Pay_Mode;
        //        model.Result.Subject = requestDto.Subject;
        //        model.Result.Amount = (requestDto.Amount * 100).ToString();
        //        model.Result.Qr_Code = result.Qr_Code;

        //        model.Result.Flag = result.Flag;
        //        model.Result.Qr_Code = result.Qr_Code;
        //        model.Result.ResultCode = result.ResultCode;
        //        model.Result.ResultMsg = result.ResultMsg;

        //        model.Result.LastUpdatedBy = UserID;
        //        model.Result.LastUpdatedDate = DateTime.Now;
        //        isSuccess = payBiz.AddAsync(model.Result).Result;
        //    }
        //    else
        //    {
        //        //新的支付请求
        //        var newModel = new FinancePayModel
        //        {
        //            //请求
        //            PayGuid = Guid.NewGuid().ToString("N"),
        //            OrderGuid = requestDto.OrderGuid,
        //            TradeNo = requestDto.Trade_No,
        //            ChannelID = requestDto.ChannelId,
        //            PayWay = requestDto.Pay_Way,
        //            PayMode = requestDto.Pay_Mode,
        //            Subject = requestDto.Subject,
        //            Amount = (requestDto.Amount * 100).ToString(),

        //            //响应
        //            Flag = result.Flag,
        //            Qr_Code = result.Qr_Code,
        //            ResultCode = result.ResultCode,
        //            ResultMsg = result.ResultMsg,

        //            CreatedBy = UserID,
        //            CreationDate = DateTime.Now,
        //            LastUpdatedBy = UserID,
        //            LastUpdatedDate = DateTime.Now
        //        };
        //        isSuccess = payBiz.AddAsync(newModel).Result;
        //    }
        //    if (isSuccess)
        //    {
        //        return Failed(ErrorCode.DataBaseError, "数据库更新失败，请检查！");
        //    }
        //    GetScanCodeStringResponseDto responseDto = new GetScanCodeStringResponseDto
        //    {
        //        ResultCode = result.ResultCode,
        //        ResultMsg = result.ResultMsg,
        //        Qr_Code = result.Qr_Code,
        //        Flag = result.Flag
        //    };
        //    return Success(responseDto);
        //}

        ///// <summary>
        ///// 支付结果查询
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet, Produces(typeof(ResponseDto<QueryATradeResponseDto>))]
        //public IActionResult DoQueryTrade(QueryATradeRequestDto requestDto)
        //{
        //    var orderModel = new OrderBiz().Getmodel(requestDto.Trade_No);
        //    if (orderModel == null)
        //    {
        //        return Failed(ErrorCode.FormatError, "订单编号查询数据有误，请检查！");
        //    }
        //    var result = new FangDiPayBiz().QueryTradeAsync(requestDto);
        //    if (result == null)
        //    {
        //        return Failed(ErrorCode.FormatError, "请求失败，请联系IT");
        //    }

        //    var payBiz = new FinancePayBiz();
        //    var model = payBiz.GetModelAsyncByTradeNo(requestDto.Trade_No);
        //    var isSuccess = false;
        //    if (model.Result != null)
        //    {
        //        model.Result.OutTradeNo = result.Out_Trade_No;
        //        model.Result.PayTime = result.Pay_Time;
        //        model.Result.Status = result.Status;
        //        model.Result.PayWay = result.Pay_Way;
        //        model.Result.ResultCode = result.ResultCode;
        //        model.Result.ResultMsg = result.ResultMsg;

        //        model.Result.LastUpdatedBy = UserID;
        //        model.Result.LastUpdatedDate = DateTime.Now;
        //        isSuccess = payBiz.AddAsync(model.Result).Result;
        //    }
        //    if (isSuccess)
        //    {
        //        return Failed(ErrorCode.DataBaseError, "数据库更新失败，请检查！");
        //    }
        //    //变更订单状态

        //    var responseDto = new QueryATradeResponseDto
        //    {
        //        Out_Trade_No = result.Out_Trade_No,
        //        Pay_Time = result.Pay_Time,
        //        Status = result.Status,
        //        Pay_Way = result.Pay_Way,
        //        ResultCode = result.ResultCode,
        //        ResultMsg = result.ResultMsg
        //    };
        //    return Success(responseDto);
        //}

        ///// <summary>
        ///// 退款
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet, Produces(typeof(ResponseDto<DoRefundResponseDto>))]
        //public IActionResult DoRefund(DoRefundRequestDto requestDto)
        //{
        //    var orderModel = new OrderBiz().Getmodel(requestDto.Trade_No);
        //    if (orderModel == null)
        //    {
        //        return Failed(ErrorCode.FormatError, "订单编号查询数据有误，请检查！");
        //    }
        //    var result = new FangDiPayBiz().RefundAsync(requestDto);
        //    if (result == null)
        //    {
        //        return Failed(ErrorCode.FormatError, "请求失败，请联系IT");
        //    }

        //    var refundBiz = new FinanceRefundBiz();
        //    var refundModel = new FinanceRefundModel();
        //    //请求
        //    refundModel.RefundGuid = Guid.NewGuid().ToString("N");
        //    refundModel.TradeNo = requestDto.Trade_No;
        //    refundModel.Reason = requestDto.Reason;
        //    refundModel.RefundNo = refundModel.RefundGuid;
        //    refundModel.RefundFee = (orderModel.PaidAmount * 100).ToString();
        //    //响应
        //    refundModel.ResultCode = result.ResultCode;
        //    refundModel.ResultMsg = result.ResultMsg;

        //    refundModel.CreatedBy = UserID;
        //    refundModel.CreationDate = DateTime.Now;
        //    refundModel.LastUpdatedBy = UserID;
        //    refundModel.LastUpdatedDate = DateTime.Now;
        //    var isSuccess = refundBiz.AddAsync(refundModel);
        //    if (isSuccess.Result)
        //    {
        //        return Failed(ErrorCode.DataBaseError, "数据库更新失败，请检查！");
        //    }
        //    //变更订单状态

        //    var responseDto = new DoRefundResponseDto
        //    {
        //        ResultCode = result.ResultCode,
        //        ResultMsg = result.ResultMsg
        //    };
        //    return Success(responseDto);
        //}

        ///// <summary>
        ///// 关闭交易
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet, Produces(typeof(ResponseDto<DoCloseTradeResponseDto>))]
        //public IActionResult DoCloseTrade(DoCloseTradeRequestDto requestDto)
        //{
        //    var orderModel = new OrderBiz().Getmodel(requestDto.Trade_No);
        //    if (orderModel == null)
        //    {
        //        return Failed(ErrorCode.FormatError, "订单编号查询数据有误，请检查！");
        //    }
        //    if (orderModel.OrderStatus.ToString().Equals(OrderStatusEnum.Obligation.ToString()))
        //    {
        //        return Failed(ErrorCode.FormatError, "该订单不是未支付状态，不支持关闭！");
        //    }
        //    var result = new FangDiPayBiz().CloseTradeAsync(requestDto);
        //    if (result == null)
        //    {
        //        return Failed(ErrorCode.FormatError, "请求失败，请联系IT");
        //    }
        //    var payBiz = new FinancePayBiz();
        //    var refundModel = payBiz.GetModelAsyncByTradeNo(requestDto.Trade_No).Result;

        //    //响应
        //    refundModel.ResultCode = result.ResultCode;
        //    refundModel.ResultMsg = result.ResultMsg;

        //    refundModel.LastUpdatedBy = UserID;
        //    refundModel.LastUpdatedDate = DateTime.Now;
        //    var isSuccess = payBiz.UpdateAsync(refundModel);
        //    if (isSuccess.Result)
        //    {
        //        return Failed(ErrorCode.DataBaseError, "数据库更新失败，请检查！");
        //    }
        //    //变更订单状态

        //    var responseDto = new DoCloseTradeResponseDto
        //    {
        //        ResultCode = result.ResultCode,
        //        ResultMsg = result.ResultMsg
        //    };
        //    return Success(responseDto);
        //}
        ///// <summary>
        ///// 微信支付回调方法
        ///// </summary>
        ///// <returns></returns>
        //public async Task<string> WeiXinPaymentBackFunction(string xml)
        //{
        //    if (!string.IsNullOrWhiteSpace(xml))
        //    {
        //        XmlDocument xmlDocument = new XmlDocument();
        //        xmlDocument.LoadXml(xml);

        //        //检查支付结果中transaction_id是否存在
        //        string transactionId = GetXmlValue(xmlDocument, "transaction_id");
        //        if (string.IsNullOrWhiteSpace(transactionId))
        //        {
        //            //若transaction_id不存在，则立即返回结果给微信支付后台
        //            return GetReturnXml("FAIL", "支付结果中微信订单号不存在");
        //        }
        //        //验证签名
        //        if (!CheckSign(xmlDocument))
        //        {
        //            return GetReturnXml("FAIL", "签名不合法");
        //        }
        //        //验证订单数据
        //        string outTradeNo = GetXmlValue(xmlDocument, "out_trade_no");
        //        //查询订单数据
        //        OrderQueryResult queryResult = await OrderQuery(transactionId, outTradeNo);
        //        //查询订单，判断订单真实性
        //        if (queryResult.result_code != "SUCCESS" || queryResult.return_code != "SUCCESS")
        //        {
        //            //若订单查询失败，则立即返回结果给微信支付后台
        //            return GetReturnXml("FAIL", "订单查询失败");
        //        }
        //        //根据查询结果更新交易流水
        //        if (await UpdateTransactionByWeiXinQuery(queryResult))
        //        {
        //            return GetReturnXml("SUCCESS", "OK");
        //        }
        //        return GetReturnXml("FAIL", "更新数据失败");
        //    }
        //    return GetReturnXml("FAIL", "未返回XML数据");
        //}
        /// <summary>
        /// 检测签名是否正确
        /// </summary>
        /// <param name="xmlDocument">xml文档</param>
        /// <returns>正确返回true，错误抛异常</returns>

        /// <summary>
        /// 生成MD5本地签名
        /// </summary>
        /// <returns></returns>
        private string MakeSign(XmlDocument xmlDocument)
        {
            string url = $"{ToUrl(xmlDocument)}&key={"MerchantSecret"}";
            var md5 = System.Security.Cryptography.MD5.Create();
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
        private string ToUrl(XmlDocument xmlDocument)
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

        /// <summary>
        /// 微信退款回调业务
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private string WeiXinRefundBackFunction(string xml)
        {
            Logger.Info($"退款xml：{Environment.NewLine}{xml}");

            #region 具体操作

            //XElement xdoc = XElement.Parse(xml);
            //var returnCode = xdoc.Element("return_code")?.Value;
            //var returnMsg = xdoc.Element("return_msg")?.Value;
            //if (returnCode != "SUCCESS")
            //{
            //    Logger.Error($"退款回调失败：{returnMsg} at {nameof(WeiXinPayBiz)}.{nameof(WeiXinRefundBackFunction)} {Environment.NewLine}{xml}");
            //    return GetReturnXml("FAIL", "退款结果失败");
            //}
            //var reqInfo = xdoc.Element("req_info").Value;
            //var merchantKey = MerchantSecret;
            //var base64Data = reqInfo;
            //var md5Key = Senparc.CO2NET.Helpers.EncryptHelper.GetLowerMD5(merchantKey, Encoding.UTF8);
            //var result = Senparc.CO2NET.Helpers.EncryptHelper.AESDecrypt(base64Data, md5Key);

            //if (string.IsNullOrEmpty(result))
            //{
            //    Logger.Error($"退款回调失败：未解析到退款加密数据 at {nameof(WeiXinPayBiz)}.{nameof(WeiXinRefundBackFunction)} {Environment.NewLine}{xml}");
            //    return GetReturnXml("FAIL", "退款结果失败");

            //}
            //Logger.Info($"退款加密数据：{Environment.NewLine}{result}");
            //XElement reqInfoXml = XElement.Parse(result);
            //var out_trade_no = reqInfoXml.Element("out_trade_no")?.Value;
            //var refund_status = reqInfoXml.Element("refund_status")?.Value;
            //if (string.IsNullOrWhiteSpace(out_trade_no))
            //{
            //    Logger.Error($"退款回调失败：退款结果中微信外部订单号不存在 at {nameof(WeiXinPayBiz)}.{nameof(WeiXinRefundBackFunction)} {Environment.NewLine}{xml}");
            //    return GetReturnXml("FAIL", "退款结果中微信外部订单号不存在");
            //}
            //try
            //{
            //    var tfBiz = new TransactionFlowingBiz();
            //    TransactionFlowingModel model = await tfBiz.GetModelByOutTradeNo(out_trade_no);
            //    //已退款成功，无需重复处理
            //    if (model.TransactionStatus == TransactionStatus.RefundSuccess.ToString())
            //    {
            //        return GetReturnXml("SUCCESS", "OK");
            //    }
            //    model.TransactionStatus = refund_status == "SUCCESS" ? TransactionStatus.RefundSuccess.ToString() : TransactionStatus.RefundFailure.ToString();
            //    model.LastUpdatedBy = "system";
            //    model.LastUpdatedDate = DateTime.Now;

            //    var mfModels = await new MerchantFlowingBiz().GetModelByTransactionFlowingGuid(model.TransactionFlowingGuid);
            //    mfModels.ForEach(a =>
            //    {
            //        a.FlowStatus = refund_status == "SUCCESS" ? FlowStatus.RefundSuccess.ToString() : FlowStatus.RefundFailure.ToString();
            //        a.LastUpdatedBy = "system";
            //        a.LastUpdatedDate = DateTime.Now;
            //    });

            //    var resultUpdate = MySqlHelper.TransactionAsync(async (conn, tran) =>
            //    {
            //        await conn.UpdateAsync(model);
            //        foreach (var item in mfModels)
            //        {
            //            await conn.UpdateAsync(item);
            //        }
            //        return true;
            //    });
            //    Logger.Info($"退款回调成功:{resultUpdate.ToString()}");
            //}
            //catch (Exception ex)
            //{
            //    Logger.Error($"退款回调更新数据失败：{ex.Message} at {nameof(WeiXinPayBiz)}.{nameof(WeiXinRefundBackFunction)} {Environment.NewLine}{xml}");
            //}

            #endregion 具体操作

            return GetReturnXmlCallBack("SUCCESS", "OK");
        }

        /// <summary>
        /// 微信支付回调方法
        /// </summary>
        /// <returns></returns>
        private string WeiXinPaymentBackFunction(string xml)
        {
            if (!string.IsNullOrWhiteSpace(xml))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);

                //检查支付结果中transaction_id是否存在
                string transactionId = GetXmlValue(xmlDocument, "transaction_id");
                if (string.IsNullOrWhiteSpace(transactionId))
                {
                    //若transaction_id不存在，则立即返回结果给微信支付后台
                    return GetReturnXmlCallBack("FAIL", "支付结果中微信订单号不存在");
                }
                Common.Helper.Logger.Info($"WeiXinPaymentBackFunction=>requestDto:{JsonConvert.SerializeObject(xmlDocument)}");

                #region 具体操作

                ////验证签名
                //if (!CheckSign(xmlDocument))
                //{
                //    return GetReturnXml("FAIL", "签名不合法");
                //}
                ////验证订单数据
                //string outTradeNo = GetXmlValue(xmlDocument, "out_trade_no");
                ////查询订单数据
                //OrderQueryResult queryResult = await OrderQuery(transactionId, outTradeNo);
                ////查询订单，判断订单真实性
                //if (queryResult.result_code != "SUCCESS" || queryResult.return_code != "SUCCESS")
                //{
                //    //若订单查询失败，则立即返回结果给微信支付后台
                //    return GetReturnXml("FAIL", "订单查询失败");
                //}
                ////根据查询结果更新交易流水
                //if (await UpdateTransactionByWeiXinQuery(queryResult))
                //{
                //    return GetReturnXml("SUCCESS", "OK");
                //}
                //return GetReturnXml("FAIL", "更新数据失败");

                #endregion 具体操作

                return GetReturnXmlCallBack("SUCCESS", "OK");
            }
            return GetReturnXmlCallBack("FAIL", "未返回XML数据");
        }

        #region XML 处理

        /// <summary>
        /// 获取XML值
        /// </summary>
        /// <param name="strXml">XML字符串</param>
        /// <param name="strData">字段值</param>
        /// <returns></returns>
        public static string GetXmlValue(XmlDocument xmlDocument, string strData)
        {
            string xmlValue = string.Empty;
            var selectSingleNode = xmlDocument.DocumentElement.SelectSingleNode(strData);
            if (selectSingleNode != null)
            {
                xmlValue = selectSingleNode.InnerText;
            }
            return xmlValue;
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
        /// 返回通知 XML
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="returnMsg"></param>
        /// <returns></returns>
        public static string GetReturnXmlCallBack(string returnCode, string returnMsg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.Append("<return_code><![CDATA[" + returnCode + "]]></return_code>");
            sb.Append("<return_msg><![CDATA[" + returnMsg + "]]></return_msg>");
            sb.Append("</xml>");
            return sb.ToString();
        }

        #endregion XML 处理
    }
}