using GD.Common;
using GD.Common.Helper;
using GD.Consumer;
using GD.Dtos.CommonEnum;
using GD.Dtos.Mall.Mall;
using GD.Dtos.MallPay.FangDiInterface;
using GD.Mall;
using GD.Mall.AfterSaleInterfaces;
using GD.Models.CommonEnum;
using GD.Models.Mall;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using static GD.Models.Mall.OrderModel;

namespace GD.API.Controllers.Mall
{
    /// <summary>
    /// 售后控制器
    /// </summary>
    public class AfterSaleController : MallBaseController
    {
        /// <summary>
        /// 退款日期限制为15天以内
        /// </summary>
        const int REFUND_LIMIT = 15;

        /// <summary>
        /// 获取退款列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetAfterSaleServiceListResponseDto>))]
        public async Task<IActionResult> GetAfterSaleServicesPageList([FromQuery] GetAfterSaleServiceListRequestDto request)
        {
            var afterSaleServiceBiz = new AfterSaleServiceBiz();

            request.MerchantGuid = UserID;

            return Success(await afterSaleServiceBiz.GetServicePageListAsync(request));
        }

        /// <summary>
        /// 获取指定服务单详情
        /// </summary>
        /// <param name="serviceGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetServiceDetailResponseDto>))]
        public async Task<IActionResult> GetAfterSaleServiceDetail(string serviceGuid)
        {
            if (string.IsNullOrEmpty(serviceGuid))
            {
                return Failed(ErrorCode.Empty, "参数不正确");
            }

            var afterSaleServiceBiz = new AfterSaleServiceBiz();

            return Success(await afterSaleServiceBiz.GetServiceDetail(UserID, serviceGuid));
        }

        /// <summary>
        /// 处理服务单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ProcessAfterSaleService([FromBody] ProcessAfterSaleServiceRequestDto request)
        {
            if (!request.Check())
            {
                return Failed(ErrorCode.Empty, "拒绝原因未填写，请检查");
            }

            var context = new ProcessAfterSaleServiceContext(request);

            var afterSaleServiceBiz = new AfterSaleServiceBiz();

            var service = await afterSaleServiceBiz.GetAsync(request.ServiceGuid);

            if (service is null)
            {
                return Failed(ErrorCode.Empty, "服务单不存在，请检查");
            }

            if (!request.IsPass && service.Status.Equals(
                AfterSaleServiceStatusEnum.Reject.ToString()))
            {
                return Failed(ErrorCode.Empty, "服务单已拒绝一次，请勿重复提交");
            }

            if (service.Status.Equals(AfterSaleServiceStatusEnum.Completed.ToString()))
            {
                return Failed(ErrorCode.Empty, "服务单已完成，请勿重复提交");
            }

            if (request.IsPass)
            {
                if (service.Type != AfterSaleServiceTypeEnum.Exchange.ToString())
                {
                    if (request.RefundFee <= 0)
                    {
                        return Failed(ErrorCode.Empty, "退款金额必须大于0，请检查");
                    }

                    if (service.RefundFee < Convert.ToInt32(request.RefundFee * 100))
                    {
                        return Failed(ErrorCode.Empty, "退款金额必须小于或等于总退款金额");
                    }
                }
            }
            context.AfterSaleService = service;

            var orderBiz = new OrderBiz();
            var order = await orderBiz.GetAsync(service.OrderGuid);

            if (order is null)
            {
                return Failed(ErrorCode.Empty, "订单不存在，请联系客服处理");
            }

            if (order.OrderCategory == OrderCategoryEnum.Physical.ToString())
            {
                return Failed(ErrorCode.Empty, "暂不支持实物类退款等操作");
            }

            if (!order.OrderStatus.Equals(OrderStatusEnum.Completed.ToString()))
            {
                return Failed(ErrorCode.Empty, "订单尚未支付成功，不支持退款");
            }

            DateTime? payTime = order.PaymentDate;
            if (!payTime.HasValue)
            {
                return Failed(ErrorCode.Empty, "订单支付时间为空，请联系客服处理");
            }

            if (payTime.Value.AddDays(REFUND_LIMIT) < DateTime.Now)
            {
                return Failed(ErrorCode.Empty, $"订单超过{REFUND_LIMIT}天可申请售后限制");
            }

            if (order.PayType == PayTypeEnum.OffLinePay.ToString())
            {
                return Failed(ErrorCode.Empty, "订单为线下支付，请用户前往线下门店退款");
            }
            context.Order = order;

            context.AfterSaleDetails = await afterSaleServiceBiz.GetAfterSaleDetailModels(request.ServiceGuid);

            if (context.AfterSaleDetails is null || context.AfterSaleDetails.Count <= 0)
            {
                return Failed(ErrorCode.Empty, "服务订单数据出现异常，请联系客服处理");
            }

            context.SameOrderServiceCompletedCount = await afterSaleServiceBiz.GetAfterSaleOrderServicesCount(service.OrderGuid);

            await ProcessAfterSaleServiceContext(context);

            //Enum.TryParse(service.Type, out AfterSaleServiceTypeEnum serviceType);

            //var saleService = AfterSaleFactory.GetSaleService(serviceType);

            //await saleService.ProcessAfterSaleServiceContextAsync(context);

            var result = await afterSaleServiceBiz.ProcessAfterSaleService(context);

            if (result)
            {
                if (request.IsPass)
                {
                    return (context.AfterSaleRefund.Status == AfterSaleRefundStatusEnum.Success.ToString()) ? Success() : Failed(ErrorCode.DataBaseError, "处理服务单失败");
                }
                return Success();
            }
            return Failed(ErrorCode.DataBaseError, "处理服务单失败");
        }

        /// <summary>
        /// 获取服务单订单详情
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetAfterSaleServiceOrderDetailResponseDto>))]
        public async Task<IActionResult> GetAfterSaleServiceOrderDetail(string orderNo)
        {
            if (string.IsNullOrEmpty(orderNo))
            {
                return Failed(ErrorCode.Empty, "订单编号不正确");
            }

            var afterSaleServiceBiz = new AfterSaleServiceBiz();

            var detail = await afterSaleServiceBiz.GetAfterSaleServiceOrderDetail(orderNo);

            return Success(detail);
        }

        /// <summary>
        /// 发起方迪支付退款
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        async Task LaunchWechatRefund(ProcessAfterSaleServiceContext context)
        {
            var afterSaleOrderDetails = context.AfterSaleDetails;

            var fanDiPayBiz = new FangDiPayBiz();

            var refundRequest = new DoRefundRequestDto()
            {
                Refund_Fee = Convert.ToInt32(context.Request.RefundFee * 100).ToString(),
                Trade_No = context.TransactionFlowing.OutTradeNo,
                Refund_No = $"SHTKSN_{GetRandomString(10)}{DateTime.Now.ToString("yyyyMMddHHmmss")}",
                Reason = "售后退款"
            };

            Logger.Info($"(LaunchWechatRefund)-方迪支付退款请求：{JsonConvert.SerializeObject(refundRequest)}");

            var result = await fanDiPayBiz.RefundAsync(refundRequest);

            Logger.Info($"(LaunchWechatRefund)-方迪支付退款响应：{JsonConvert.SerializeObject(result)}");

            context.AfterSaleRefund.OutRefundNo = refundRequest.Refund_No;

            if (result?.ResultCode == "-1")
            {
                context.AfterSaleRefund.Status = AfterSaleRefundStatusEnum.Failed.ToString();
            }
            else
            {
                context.AfterSaleRefund.Status = AfterSaleRefundStatusEnum.Success.ToString();
            }
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
        private string GetRandomString(int length, bool useNum = false, bool useLow = false, bool useUpp = true, bool useSpe = false, string custom = "")
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
        /// 处理服务单上下文
        /// </summary>
        /// <param name="context"></param>
        async Task ProcessAfterSaleServiceContext(ProcessAfterSaleServiceContext context)
        {
            var request = context.Request;
            var afterSaleService = context.AfterSaleService;
            var afterSaleOrderDetails = context.AfterSaleDetails;
            var order = context.Order;
            var count = 0;

            if (request.IsPass)
            {
                var orderDetailBiz = new OrderDetailBiz();
                context.OrderDetails = await orderDetailBiz.GetModelsByOrderIdAsync(order.OrderGuid);

                //【1】更新服务单状态
                afterSaleService.Status = AfterSaleServiceStatusEnum.Completed.ToString();

                //【2】
                //（1）若批量退款，则更新主订单状态为交易关闭
                //（2）若分批退款，则更新主订单状态为交易关闭
                if (((count = context.OrderDetails.Count) == afterSaleOrderDetails.Count)
                    || (count == (context.SameOrderServiceCompletedCount + 1)))
                {
                    order.OrderStatus = OrderStatusEnum.Closed.ToString();
                    order.LastUpdatedBy = UserID;
                    order.LastUpdatedDate = DateTime.Now;
                }

                //【3】记录退款流水
                var flowBiz = new TransactionFlowingBiz();

                context.TransactionFlowing = await flowBiz.GetModelsById(order.TransactionFlowingGuid);

                context.AfterSaleRefund = new AfterSaleRefundModel()
                {
                    RefundGuid = Guid.NewGuid().ToString("N"),
                    ServiceGuid = afterSaleService.ServiceGuid,
                    OrderGuid = afterSaleService.OrderGuid,
                    FlowingGuid = order.TransactionFlowingGuid,
                    RefundFee = Convert.ToInt32(request.RefundFee * 100),
                    Status = AfterSaleRefundStatusEnum.Refunding.ToString(),
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID
                };

                context.Goods = await new GoodsBiz().GetModelsByOrderDetailIdAsync(context.AfterSaleDetails.Select(a => a.OrderDetailGuid).Distinct(), true);

                var commentBiz = new OrderProductCommentBiz();

                context.Comments = await commentBiz.GetModelsByOrderDetailGuidsAsync(context.AfterSaleDetails.Select(d => d.OrderDetailGuid).Distinct().ToArray(), true);

                if (context.Goods != null && context.Goods.Count > 0)
                {
                    context.Goods.All(d =>
                    {
                        d.Enable = false;
                        d.LastUpdatedBy = UserID;
                        d.LastUpdatedDate = DateTime.Now;

                        return true;
                    });
                }

                if (context.Comments != null && context.Comments.Count > 0)
                {
                    context.Comments.All(d =>
                    {
                        d.LastUpdatedBy = UserID;
                        d.LastUpdatedDate = DateTime.Now;
                        d.Enable = false;

                        return true;
                    });
                }

                await LaunchWechatRefund(context);
            }
            else
            {
                afterSaleService.RefuseReason = request.Reason;
                afterSaleService.Status = AfterSaleServiceStatusEnum.Reject.ToString();

                context.Goods = await new GoodsBiz().GetModelsByOrderDetailIdAsync(context.AfterSaleDetails.Select(a => a.OrderDetailGuid).Distinct(), false);

                var commentBiz = new OrderProductCommentBiz();

                context.Comments = await commentBiz.GetModelsByOrderDetailGuidsAsync(context.AfterSaleDetails.Select(d => d.OrderDetailGuid).Distinct().ToArray(), false);

                if (context.Goods != null && context.Goods.Count > 0)
                {
                    context.Goods.All(d =>
                    {
                        d.Enable = true;
                        d.LastUpdatedBy = UserID;
                        d.LastUpdatedDate = DateTime.Now;
                        return true;
                    });
                }

                if (context.Comments != null && context.Comments.Count > 0)
                {
                    context.Comments.All(d =>
                    {
                        d.LastUpdatedBy = UserID;
                        d.LastUpdatedDate = DateTime.Now;
                        d.Enable = true;

                        return true;
                    });
                }
            }

            //【3】添加协商历史记录
            foreach (var afterDetail in context.AfterSaleDetails)
            {
                context.AfterSaleConsultations.Add(new AfterSaleConsultationModel()
                {
                    ConsultationGuid = Guid.NewGuid().ToString("N"),
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    DetailGuid = afterDetail.DetailGuid,
                    Title = (request.IsPass ? "商家已同意申请并退款" : "商家已拒绝申请"),
                    Content = request.Reason,
                    RoleType = AfterSaleConsultationRoleEnum.Seller.ToString()
                });
            }
        }
    }
}
