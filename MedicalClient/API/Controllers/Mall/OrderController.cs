using GD.Mall;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using GD.Dtos.Mall.Order;
using GD.Common;
using GD.Dtos.CommonEnum;
using GD.Models.Mall;
using GD.Models.CommonEnum;
using static GD.Models.Mall.OrderModel;
using GD.Consumer;
using GD.Utility;

namespace GD.API.Controllers.Mall
{
    /// <summary>
    /// 订单相关控制器
    /// </summary>
    public class OrderController : MallBaseController
    {
        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="orderGuid">订单guid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetOrderDetailsInfoResponseDto>))]
        public async Task<IActionResult> GetOrderDetailsInfoAsync(string orderGuid)
        {
            var model = await new OrderBiz().GetAsync(orderGuid);
            if (model == null)
            {
                return Failed(ErrorCode.UserData, "当前订单数据不存在");
            }
            var result = await new OrderBiz().GetOrderDetailsInfoAsync(orderGuid);
            var groupData = result.GroupBy(a =>
                new
                {
                    a.MerchantGuid,
                    a.MerchantName,
                    a.ProductTotal,
                    a.OrderStatus,
                    a.OrderNo,
                    a.OrderGuid,
                    a.PayablesAmount,
                    a.DiscountAmout,
                    a.PaidAmount,
                    a.CreationDate,
                    a.PayType,
                    a.OrderKey,
                    a.PaymentDate
                });
            var response = groupData.Select(a => new GetOrderDetailsInfoResponseDto
            {
                MerchantGuid = a.Key.MerchantGuid,
                MerchantName = a.Key.MerchantName,
                ProductTotal = a.Key.ProductTotal,
                OrderStatus = a.Key.OrderStatus,
                OrderNo = a.Key.OrderNo,
                OrderGuid = a.Key.OrderGuid,
                PayablesAmount = a.Key.PayablesAmount,
                DiscountAmout = a.Key.DiscountAmout,
                PaidAmount = a.Key.PaidAmount,
                CreationDate = a.Key.CreationDate,
                PayType = a.Key.PayType,
                OrderKey = a.Key.OrderKey,
                PaymentDate = a.Key.PaymentDate,
                Products = a.OrderBy(p => p.ProductName).Select(p => new GetOrderDetailsInfoItemDto
                {
                    ProductPicture = p.ProductPicture,
                    ProductName = p.ProductName,
                    ProductCount = p.ProductCount,
                    ServiceGuid = p.ServiceGuid,
                    ServiceDetailGuid = p.ServiceDetailGuid,
                    OrderDetailGuid = p.OrderDetailGuid,
                    ServiceStatus = p.ServiceStatus,
                    ServiceType = p.ServiceType,
                    ProductPrice = p.ProductPrice,
                    ServiceStatusDisplay = GetServiceStatusDisplay(model, p.ServiceStatus, p.ServiceType)
                }).ToList()
            });
            var res = response.FirstOrDefault();

            /* 对于服务类商品，售后前提是，商品卡未曾使用
             * 一、参数解析：
             * CanApplyAfterSale 能否显示售后状态按钮
             * ServiceDetailGuid 售后记录Id
             * 二、参数组合情况说明：
             * 1.CanApplyAfterSale = true && ServiceDetailGuid != null 表示售后状态按钮已显示，且能查看售后记录
             * 2.CanApplyAfterSale = true && ServiceDetailGuid == null 表示售后状态按钮已显示，能申请售后
             * 3.CanApplyAfterSale=false 表示无法申请售后记录
             */

            //只有服务类商品可以申请售后
            if (model.OrderCategory == OrderCategoryEnum.Service.ToString())
            {
                //订单必须是已完成的、线上支付的、订单完成时间不超过15天的
                if ((model.OrderStatus == OrderStatusEnum.Completed.ToString() || model.OrderStatus == OrderStatusEnum.Closed.ToString()) && model.PayType == PayTypeEnum.Wechat.ToString())
                {
                    res.Products.ForEach(async a =>
                    {
                        var goodsModels = await new GoodsBiz().GetModelsByOrderDetailIdAsync(new List<string> { a.OrderDetailGuid });
                        //检测是否有商品卡过期
                        var overdue = goodsModels.FirstOrDefault(g => g.EffectiveEndDate != null && g.EffectiveEndDate.Value.Date < DateTime.Now.Date) != null;
                        //商品下的商品卡没用过，才能申请售后
                        var goodsItemModels = await new GoodsItemBiz().GetByOrderDetailIdsAsync(new List<string> { a.OrderDetailGuid });
                        if (goodsItemModels.FirstOrDefault(b => b.Used > 0) == null)//商品被用过一定无售后记录，也无法申请售后
                        {
                            /*
                             *1.未申请售后的15天内的订单明细可以显示售后状态按钮（待申请售后）
                             *2.存在申请售后记录的订单明细，均需要显示售后状态按钮（查看售后状态）
                             */
                            if (((DateTime.Now - model.PaymentDate.Value).TotalDays <= 15 && !overdue)
                            || !string.IsNullOrWhiteSpace(a.ServiceDetailGuid))
                            {
                                a.CanApplyAfterSale = true;
                            }

                        }
                    });
                    //订单明细中至少有一个可以申请售后，才能允许批量售后操作
                    if (res.Products.FirstOrDefault(a => a.CanApplyAfterSale && a.ServiceDetailGuid == null) != null)
                    {
                        res.CanApplyAfterSale = true;
                    }
                }
            }
            return Success(res);
        }

        /// <summary>
        /// 提交售后单
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> SubmitAfterSaleServiceAsync([FromBody]SubmitAfterSaleServiceRequestDto requestDto)
        {
            var context = new SubmitAfterSaleServiceContext(requestDto);
            //1.检测售后单提交数据正确性
            (bool checkRes, ResponseDto response) = await CheckSubmitAfterSaleServiceAsync(context);
            if (!checkRes)
            {
                return response;
            }
            //2.生成售后单数据：售后单、售后单明细、协商记录
            CreateAfterServiceData(context);

            //3.若售后订单为服务类订单，则在申请退款时，需先将对应订单明细的商品卡设置为不可用
            await SetGoodsDisabledAsync(context);

            //4.订单在申请退款时，需将对应的商品待评记录设置为不可用
            await SetOrderProductCommentDisabledAsync(context);

            //5.提交数据库数据
            var result = await new AfterSaleServiceBiz().SubmitAfterSaleServiceAsync(context);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "提交售后单失败");

        }

        /// <summary>
        /// 获取售后服务单详情
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetServiceDetailInfoResponseDto>))]
        public async Task<IActionResult> GetServiceDetailInfoAsync(string serviceDetailGuid)
        {
            var result = await new AftersaleDetailBiz().GetServiceDetailInfoAsync(serviceDetailGuid);
            var groupData = result.GroupBy(a => new
            {
                a.ServiceNo,
                a.ServiceStatus,
                a.ServiceType,
                a.ProductGuid,
                a.ProductName,
                a.ProductPrice,
                a.ProductCount,
                a.ProductPicture,
                a.RefundFee,
                a.RefundDate
            });
            var response = groupData.Select(a => new GetServiceDetailInfoResponseDto
            {
                ServiceNo = a.Key.ServiceNo,
                ServiceStatus = a.Key.ServiceStatus,
                ServiceType = a.Key.ServiceType,
                ProductGuid = a.Key.ProductGuid,
                ProductName = a.Key.ProductName,
                ProductPrice = a.Key.ProductPrice,
                ProductCount = a.Key.ProductCount,
                ProductPicture = a.Key.ProductPicture,
                RefundFee = a.Key.RefundFee / 100M,
                RefundDate = a.Key.RefundDate,
                ServiceStatusDisplay = $"{ a.Key.ServiceType.GetDescription()}{ a.Key.ServiceStatus.GetDescription()}",
                Consultations = a.Select(b => new GetServiceDetailConsultationDto
                {
                    ConsultationTitle = b.ConsultationTitle,
                    ConsultationContent = b.ConsultationContent,
                    ConsultationDate = b.ConsultationDate,
                    RoleType = b.RoleType
                }).OrderBy(b => b.ConsultationDate).ToList()
            });
            return Success(response.FirstOrDefault());
        }

        /// <summary>
        /// 检测售后单提交数据正确性
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task<(bool, ResponseDto)> CheckSubmitAfterSaleServiceAsync(SubmitAfterSaleServiceContext context)
        {
            if (context.RequestDto.Detials == null || !context.RequestDto.Detials.Any())
            {
                return (false, Failed(ErrorCode.UserData, "请至少选择一种商品"));
            }
            var orderDetails = await new OrderDetailBiz().GetModelsByPrimaryKeyIdsAsync(context.RequestDto.Detials.Select(a => a.OrderDetailGuid), true);
            context.OrderDetails = orderDetails;
            if (!orderDetails.Any())
            {
                return (false, Failed(ErrorCode.UserData, "未找到订单明细数据"));
            }
            if (orderDetails.Select(a => a.OrderGuid).Distinct().Count() != 1)
            {
                return (false, Failed(ErrorCode.UserData, "当前售后订单明细不是属于同一个订单，数据错误"));
            }
            var orderModel = await new OrderBiz().GetAsync(orderDetails.FirstOrDefault().OrderGuid);
            context.OrderModel = orderModel;
            if (orderModel == null)
            {
                return (false, Failed(ErrorCode.UserData, "未找到指定订单数据"));
            }
            if (orderModel.PayType != PayTypeEnum.Wechat.ToString())
            {
                return (false, Failed(ErrorCode.UserData, "目前只支持线上微信支付的订单进行售后操作，请联系客服"));
            }
            //暂时只支持服务类订单的售后服务操作
            if (orderModel.OrderCategory != OrderCategoryEnum.Service.ToString())
            {
                return (false, Failed(ErrorCode.UserData, "暂时只支持服务类订单的售后服务操作，请联系客服"));
            }
            if (orderModel.OrderStatus != OrderStatusEnum.Completed.ToString())
            {
                var orderStatusEnum = (OrderStatusEnum)Enum.Parse(typeof(OrderStatusEnum), orderModel.OrderStatus);
                return (false, Failed(ErrorCode.UserData, $"订单状态为[{orderStatusEnum.GetDescription()}]，不支持售后操作"));
            }
            //检查传入的订单详情是否存在售后记录，避免重复提交
            var afterSaleServiceDetails = await new AftersaleDetailBiz().GetByOrderDetialIdsAsync(context.RequestDto.Detials.Select(a => a.OrderDetailGuid));
            if (afterSaleServiceDetails.Any())
            {
                return (false, Failed(ErrorCode.UserData, "已存在售后记录,不可重复提交售后单"));
            }
            //若为服务类订单
            if (context.OrderModel.OrderCategory == OrderCategoryEnum.Service.ToString())
            {
                if ((DateTime.Now - context.OrderModel.PaymentDate.Value).TotalDays > 15)
                {
                    return (false, Failed(ErrorCode.UserData, $"订单完成超过15天，不支持售后操作，请联系客服"));
                }
                var goodsModels = await new GoodsBiz().GetModelsByOrderDetailIdAsync(context.OrderDetails.Select(a => a.DetailGuid).Distinct());
                var overdueGoods = goodsModels.Where(g => g.EffectiveEndDate != null && g.EffectiveEndDate.Value.Date < DateTime.Now.Date);
                if (overdueGoods.Any())
                {
                    return (false, Failed(ErrorCode.UserData, $"所选商品[{string.Join(",", overdueGoods.Select(a => a.ProductName))}]中存在已过期的商品卡，不可退款"));
                }

                #region 针对服务类商品，若已满足售后要求，还需要检测是否有商品卡已使用过
                var goodsItemModels = await new GoodsItemBiz().GetByOrderDetailIdsAsync(context.RequestDto.Detials.Select(a => a.OrderDetailGuid));
                var usedGoodsItems = goodsItemModels.Where(a => a.Used > 0);
                if (usedGoodsItems.Count() > 0)
                {
                    var usedGoodsModelNames = goodsModels.Join(usedGoodsItems, p => p.GoodsGuid, u => u.GoodsGuid, (p, u) => p.ProductName);
                    return (false, Failed(ErrorCode.UserData, $"所选商品[{string.Join(",", usedGoodsModelNames)}]中存在已用过商品卡的情况，不可退款"));
                }
                #endregion
                context.RequestDto.AfterSaleServiceType = AfterSaleServiceTypeEnum.RefundWhitoutReturn;//服务类订单售后类型只能为【退款不退货】类型
            }
            return (true, null);
        }

        /// <summary>
        /// 生成售后服务单数据
        /// </summary>
        /// <param name="context"></param>
        private void CreateAfterServiceData(SubmitAfterSaleServiceContext context)
        {
            var serviceModel = new AfterSaleServiceModel
            {
                ServiceGuid = Guid.NewGuid().ToString("N"),
                ServiceNo = new ServiceNoBiz().GetServiceNo().Result.PadLeft(9, '0'),
                MerchantGuid = context.OrderModel.MerchantGuid,
                OrderGuid = context.OrderModel.OrderGuid,
                UserGuid = context.OrderModel.UserGuid,
                Status = AfterSaleServiceStatusEnum.Applying.ToString(),
                Type = context.RequestDto.AfterSaleServiceType.ToString(),
                Reason = context.RequestDto.Reason,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty
            };
            var serviceDetails = context.OrderDetails.Select(a => new AfterSaleDetailModel
            {
                DetailGuid = Guid.NewGuid().ToString("N"),
                ServiceGuid = serviceModel.ServiceGuid,
                OrderDetailGuid = a.DetailGuid,
                ProductGuid = a.ProductGuid,
                UnitPrice = (int)(a.ProductPrice * 100),//订单明细商品的单价为元，需要转换为分
                                                        //售后商品数量，可能要小于实际订单明细的商品数量，例如买了3个苹果，可能2个走售后流程，剩余1个是没有问题的苹果，前端传null表示全部走售后流程
                ProductCount = context.RequestDto.Detials.FirstOrDefault(d => d.OrderDetailGuid == a.DetailGuid).ProductCount ?? a.ProductCount,
#warning 若后续版本加入优惠活动，那么此处的退款价格应该是分摊到订单明细之后的折后价
                RefundFee = (int)(a.ProductPrice * 100) * (context.RequestDto.Detials.FirstOrDefault(d => d.OrderDetailGuid == a.DetailGuid).ProductCount ?? a.ProductCount),
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty
            }).ToList();
            if (context.RequestDto.AfterSaleServiceType != AfterSaleServiceTypeEnum.RefundWhithReturn && context.RequestDto.AfterSaleServiceType != AfterSaleServiceTypeEnum.RefundWhitoutReturn)
            {
                serviceDetails.ForEach(a =>
                {
                    a.RefundFee = 0;
                });
            }
            serviceModel.RefundFee = serviceDetails.Sum(a => a.RefundFee);//服务单的退款金额=明细退款金额之和，若明细无退款，则无退款金额

            var consultationModels = serviceDetails.Select(a => new AfterSaleConsultationModel
            {
                ConsultationGuid = Guid.NewGuid().ToString("N"),
                DetailGuid = a.DetailGuid,
                Title = $"用户发起{context.RequestDto.AfterSaleServiceType.GetDescription()}申请",
                //Content = $"{requestDto.AfterSaleServiceType.GetDescription()}原因：{requestDto.Reason}",
                RoleType = AfterSaleConsultationRoleEnum.Buyer.ToString(),
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty
            }).ToList();

            context.ServiceModel = serviceModel;
            context.ServiceDetailModels = serviceDetails;
            context.ConsultationModels = consultationModels;
        }

        /// <summary>
        /// 若售后订单为服务类订单，则在申请退款时，需先将对应订单明细的商品卡设置为禁用状态
        /// </summary>
        private async Task SetGoodsDisabledAsync(SubmitAfterSaleServiceContext context)
        {
            if (context.OrderModel.OrderCategory != OrderCategoryEnum.Service.ToString())
            {
                return;
            }
            var goodsModels = await new GoodsBiz().GetModelsByOrderDetailIdAsync(context.RequestDto.Detials.Select(a => a.OrderDetailGuid).Distinct());
            goodsModels.ForEach(a =>
            {
                a.Enable = false;
                a.LastUpdatedBy = UserID;
                a.LastUpdatedDate = DateTime.Now;
            });
            context.GoodsModels = goodsModels;
        }

        /// <summary>
        /// 订单在申请退款时，需将对应的商品待评记录设置为不可用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task SetOrderProductCommentDisabledAsync(SubmitAfterSaleServiceContext context)
        {
            var models = await new OrderProductCommentBiz().GetModelsByOrderDetailGuidsAsync(context.OrderDetails.Select(a => a.DetailGuid).Distinct().ToArray());
            models.ForEach(a =>
            {
                a.Enable = false;
                a.LastUpdatedBy = UserID;
                a.LastUpdatedDate = DateTime.Now;
            });
            context.CommentModels = models;
        }

        /// <summary>
        /// 查询售后按钮的显示文字
        /// </summary>
        /// <param name="order">订单实例</param>
        /// <param name="serviceStatus">售后服务状态</param>
        /// <param name="serviceType">售后服务类型</param>
        /// <returns></returns>
        private string GetServiceStatusDisplay(OrderModel order, string serviceStatus, string serviceType)
        {
            //订单是待付款和已取消时，无售后状态;目前不支持实体类的售后操作
            if (order.OrderStatus == OrderStatusEnum.Obligation.ToString()
                || order.OrderStatus == OrderStatusEnum.Canceled.ToString()
                || order.OrderCategory == OrderCategoryEnum.Physical.ToString())
            {
                return null;
            }

            //超过15天的已完成订单不能进行售后操作
            if ((DateTime.Now.Date - order.PaymentDate.Value.Date).TotalDays > 15 && string.IsNullOrWhiteSpace(serviceStatus))
            {
                return null;
            }


            /* 
             * 1.服务类：
             * a.无售后单，则显示“退款”
             * b.审核中
             * c.退款被拒绝
             * d.已退款
             */
            var display = "审核中";
            if (order.OrderCategory == OrderCategoryEnum.Service.ToString())
            {
                if (string.IsNullOrWhiteSpace(serviceStatus))
                {
                    display = "退款";
                }
                else if (serviceStatus == AfterSaleServiceStatusEnum.Reject.ToString())
                {
                    display = "退款被拒绝";
                }
                else if (serviceStatus == AfterSaleServiceStatusEnum.Completed.ToString())
                {
                    display = "已退款";
                }
            }
            return display;
        }
    }
}
