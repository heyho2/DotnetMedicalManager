using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using GD.API.Code;
using GD.Common;
using GD.Dtos.Meal.MealClient;
using System.Linq;
using GD.Meal;
using GD.Models.Meal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using GD.Utility;
using GD.Module;
using GD.Module.WeChat;
using GD.Dtos.WeChat;
using GD.Dtos.FAQs.FAQsClient;

namespace GD.API.Controllers.Meal
{
    /// <summary>
    /// 点餐客户端控制器
    /// </summary>
    public class MealClientController : MealBaseController
    {
        /// <summary>
        /// 提交点餐订单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> SubmitMealOrderAsync([FromBody]SubmitMealOrderRequestDto requestDto)
        {
            var dishesIds = requestDto.DishesDetails.Select(a => a.DishesGuid).Distinct().ToList();
            var dishesCategoryIds = requestDto.DishesDetails.Select(a => a.CategoryGuid).Distinct().ToList();

            var accountModels = await new MealAccountBiz().GetModelsByUserIdAsync(UserID, requestDto.HospitalGuid);
            if (!accountModels.Any())
            {
                return Failed(ErrorCode.Empty, "未检测到钱包账户，请先充值");
            }
            var isInternal = accountModels.FirstOrDefault(a => a.UserType == MealUserTypeEnum.Internal.ToString()) != null;

            var dishesModels = await new MealDishesBiz().GetModelsByIdsAsync(dishesIds);
            var checkDishesOnsale = dishesModels.FirstOrDefault(a => a.DishesOnsale == 0);
            if (checkDishesOnsale != null)
            {
                return Failed(ErrorCode.UserData, "订单中存在下架的菜品");
            }

            var codeBiz = new CodeBiz();
            var orderNo = codeBiz.GetMealOrderCode("M");
            //餐别基础数据
            var dishesCategoryModels = await new MealCategoryBiz().GetModelsByIdsAsync(dishesCategoryIds);
            var groupDishes = requestDto.DishesDetails.GroupBy(a => new { a.MealDate, a.CategoryGuid });
            List<MealOrderModel> orders = new List<MealOrderModel>();//点餐订单列表
            List<MealOrderDetailModel> orderDetails = new List<MealOrderDetailModel>();//点餐订单明细列表
            List<MealAccountDetailModel> accountDetails = new List<MealAccountDetailModel>();//点餐钱包流水列表
            List<MealAccountTradeModel> tradeModels = new List<MealAccountTradeModel>();//点餐交易流水列表
            List<MealAccountTradeDetailModel> tradeDetailsModels = new List<MealAccountTradeDetailModel>();//点餐交易流水明细列表
            var orderIndex = 1;
            foreach (var item in groupDishes)
            {
                var theDishesCategory = dishesCategoryModels.FirstOrDefault(a => a.CategoryGuid == item.Key.CategoryGuid);
                var order = new MealOrderModel
                {
                    OrderGuid = Guid.NewGuid().ToString("N"),
                    OrderNo = $"{orderNo}-{orderIndex++}",
                    CategoryGuid = item.Key.CategoryGuid,
                    CategoryName = theDishesCategory?.CategoryName,
                    MealDate = item.Key.MealDate,
                    MealStartTime = Convert.ToDateTime($"{item.Key.MealDate.ToString("yyyy-MM-dd")} {theDishesCategory.MealStartTime}"),
                    MealEndTime = Convert.ToDateTime($"{item.Key.MealDate.ToString("yyyy-MM-dd")} {theDishesCategory.MealEndTime}"),
                    UserGuid = UserID,
                    HospitalGuid = requestDto.HospitalGuid,
                    OrderStatus = MealOrderStatusEnum.Paided.ToString(),
                    Quantity = item.Sum(a => a.Quantity),
                    TotalPrice = 0,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = string.Empty
                };
                var totalPrice = 0M;
                foreach (var dish in item)
                {
                    var theDishes = dishesModels.FirstOrDefault(a => a.DishesGuid == dish.DishesGuid);
                    var internalPrice = theDishes.DishesInternalPrice;
                    var externalPrice = theDishes.DishesExternalPrice;
                    var orderDetail = new MealOrderDetailModel
                    {
                        OrderDetailGuid = Guid.NewGuid().ToString("N"),
                        OrderGuid = order.OrderGuid,
                        DishesGuid = dish.DishesGuid,
                        DishesName = theDishes.DishesName,
                        Quantity = dish.Quantity,
                        UnitPrice = isInternal ? internalPrice : externalPrice,
                        CreatedBy = UserID,
                        LastUpdatedBy = UserID,
                        OrgGuid = string.Empty
                    };
                    totalPrice += orderDetail.Quantity * orderDetail.UnitPrice;
                    orderDetails.Add(orderDetail);
                }
                order.TotalPrice = totalPrice;
                orders.Add(order);



                #region 交易流水记录
                tradeModels.Add(new MealAccountTradeModel
                {
                    AccountTradeGuid = Guid.NewGuid().ToString("N"),
                    OrderGuid = order.OrderGuid,
                    AccountTradeType = (sbyte)MealAccountTradeTypeEnum.Consumer,
                    AccountTradeFee = order.TotalPrice,
                    AccountTradeDescription = string.Empty,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = string.Empty
                });
                #endregion

            }
            var accountToal = accountModels.Sum(a => a.AccountBalance);
            var orderTotal = orders.Sum(a => a.TotalPrice);
            if (accountToal < orderTotal)
            {
                return Failed(ErrorCode.UserData, "余额不足，请先充值");
            }
            var rechargeAccount = accountModels.FirstOrDefault(a => a.AccountType == MealAccountTypeEnum.Recharge.ToString());
            var grantAccount = accountModels.FirstOrDefault(a => a.AccountType == MealAccountTypeEnum.Grant.ToString());
            var paidMoney = orderTotal;

            var orderDic = orders.ToDictionary(a => a.OrderGuid, a => a.TotalPrice);
            //创建钱包流水记录
            if (grantAccount != null && grantAccount.AccountBalance > 0)
            {
                var grantPaid = grantAccount.AccountBalance > paidMoney ? paidMoney : grantAccount.AccountBalance;
                accountDetails.Add(new MealAccountDetailModel
                {
                    AccountDetailGuid = Guid.NewGuid().ToString("N"),
                    AccountGuid = grantAccount.AccountGuid,
                    AccountDetailType = MealAccountDetailTypeEnum.Consume.ToString(),
                    AccountDetailIncomeType = (sbyte)MealAccountDetailIncomeTypeEnum.Expenditure,
                    AccountDetailBeforeFee = grantAccount.AccountBalance,
                    AccountDetailFee = grantPaid,
                    AccountDetailAfterFee = grantAccount.AccountBalance - grantPaid,
                    AccountDetailDescription = "订单消费",
                    Remark = orderNo,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = string.Empty
                });
                paidMoney -= grantPaid;

                for (int i = 0; i < orderDic.Count; i++)
                {
                    var item = orderDic.ElementAt(i);
                    if (item.Value > 0 && grantPaid > 0)
                    {
                        var orderPaid = grantPaid > item.Value ? item.Value : grantPaid;
                        tradeDetailsModels.Add(new MealAccountTradeDetailModel
                        {
                            AccountTradeDetailGuid = Guid.NewGuid().ToString("N"),
                            AccountGuid = grantAccount.AccountGuid,
                            AccountTradeGuid = tradeModels.FirstOrDefault(a => a.OrderGuid == item.Key)?.AccountTradeGuid,
                            AccountTradeFee = orderPaid,
                            CreatedBy = UserID,
                            LastUpdatedBy = UserID,
                            OrgGuid = string.Empty
                        });
                        orderDic[item.Key] -= orderPaid;
                        grantPaid -= orderPaid;
                    }
                }
            }
            if (paidMoney > 0 && rechargeAccount != null && rechargeAccount.AccountBalance > 0)
            {
                accountDetails.Add(new MealAccountDetailModel
                {
                    AccountDetailGuid = Guid.NewGuid().ToString("N"),
                    AccountGuid = rechargeAccount.AccountGuid,
                    AccountDetailType = MealAccountDetailTypeEnum.Consume.ToString(),
                    AccountDetailIncomeType = (sbyte)MealAccountDetailIncomeTypeEnum.Expenditure,
                    AccountDetailBeforeFee = rechargeAccount.AccountBalance,
                    AccountDetailFee = paidMoney,
                    AccountDetailAfterFee = rechargeAccount.AccountBalance - paidMoney,
                    AccountDetailDescription = "订单消费",
                    Remark = orderNo,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = string.Empty

                });
                for (int i = 0; i < orderDic.Count; i++)
                {
                    var item = orderDic.ElementAt(i);
                    if (item.Value > 0)
                    {
                        tradeDetailsModels.Add(new MealAccountTradeDetailModel
                        {
                            AccountTradeDetailGuid = Guid.NewGuid().ToString("N"),
                            AccountGuid = rechargeAccount.AccountGuid,
                            AccountTradeGuid = tradeModels.FirstOrDefault(a => a.OrderGuid == item.Key)?.AccountTradeGuid,
                            AccountTradeFee = item.Value,
                            CreatedBy = UserID,
                            LastUpdatedBy = UserID,
                            OrgGuid = string.Empty
                        });
                        orderDic[item.Key] -= item.Value;
                    }
                }
            }

            var res = await new MealOrderBiz().SubmitMealOrderAsync(orders, orderDetails, accountDetails, tradeModels, tradeDetailsModels);
            return res ? Success() : Failed(ErrorCode.DataBaseError);
        }

        /// <summary>
        /// 检测是否有菜品未在售
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<CheckDishesSaleStatusResponseDto>))]
        public async Task<IActionResult> CheckDishesSaleStatus([FromBody]CheckDishesSaleStatusRequestDto requestDto)
        {
            //菜品基础数据
            var dishesModels = await new MealDishesBiz().GetModelsByIdsAsync(requestDto.DishesIds.Distinct().ToList());
            var checkDishes = dishesModels.Where(a => a.DishesOnsale == 0).Select(a => new CheckDishesSaleStatusResponseDto.CheckDishes
            {
                DishesGuid = a.DishesGuid,
                DishesName = a.DishesName
            });
            var result = new CheckDishesSaleStatusResponseDto
            {
                CheckResult = !checkDishes.Any(),
                NotSaleDishes = checkDishes.ToList()
            };
            return Success(result);
        }

        /// <summary>
        /// 获取点餐用户基础数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetMealUserBasicInfoResponseDto>))]
        public async Task<IActionResult> GetMealUserBasicInfoAsync(string hospitalGuid)
        {
            var accountModels = await new MealAccountBiz().GetModelsByUserIdAsync(UserID, hospitalGuid);
            //是否是内部员工
            var isInternal = accountModels.FirstOrDefault(a => a.UserType == MealUserTypeEnum.Internal.ToString()) != null;
            //钱包账户总余额
            var accountBalanceTotal = accountModels.Sum(a => a.AccountBalance);
            var orderTotal = await new MealOrderBiz().GetUserOrderTotalAsync(UserID, hospitalGuid);
            var userModel = await new UserBiz().GetModelAsync(UserID);
            var response = new GetMealUserBasicInfoResponseDto
            {
                UserName = userModel.UserName,
                BalanceTotal = accountBalanceTotal,
                OrderTotal = orderTotal,
                UserType = isInternal ? "内部职工" : "普通会员"
            };
            return Success(response);
        }

        /// <summary>
        /// 获取食堂已安排菜单的日期
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<DateTime>>))]
        public async Task<IActionResult> GetMealMenuDateListAsync(string hospitalGuid)
        {
            var res = await new MealMenuBiz().GetLatestModelsByMenuDateAsync(hospitalGuid, DateTime.Now.Date);
            return Success(res);
        }

        /// <summary>
        /// 获取指定日期的菜单详情
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetMenuDetailOneDayResponseDto>>))]
        public async Task<IActionResult> GetMenuDetailOneDayAsync([FromBody]GetMenuDetailOneDayRequestDto requestDto)
        {
            var accountModels = await new MealAccountBiz().GetModelsByUserIdAsync(UserID, requestDto.HospitalGuid);
            //是否是内部员工
            var isInternal = accountModels.FirstOrDefault(a => a.UserType == MealUserTypeEnum.Internal.ToString()) != null;

            var menuDate = requestDto.MenuDate;
            var queryDetail = await new MealMenuBiz().GetMenuDetailOneDayQueryAsync(requestDto.HospitalGuid, requestDto.MenuDate);
            List<GetMenuDetailOneDayResponseDto> res = new List<GetMenuDetailOneDayResponseDto>();
            var groupDetail = queryDetail.GroupBy(a => new { a.CategoryGuid, a.CategoryName, a.CategoryAdvanceDay, a.CategoryScheduleTime, a.MealStartTime, a.MealEndTime }).OrderBy(a => a.Key.MealStartTime);
            foreach (var item in groupDetail)
            {
                var bookingDeadlineUtc = Convert.ToDateTime($"{menuDate.AddDays(item.Key.CategoryAdvanceDay * (-1)).ToString("yyyy-MM-dd")} {item.Key.CategoryScheduleTime}").AddHours(-8);
                res.Add(new GetMenuDetailOneDayResponseDto
                {
                    CategoryGuid = item.Key.CategoryGuid,
                    CategoryName = item.Key.CategoryName,
                    MealStartTime = item.Key.MealStartTime,
                    MealEndTime = item.Key.MealEndTime,
                    BookingDeadline = TimeZoneInfo.ConvertTimeFromUtc(bookingDeadlineUtc, TimeZoneInfo.Local),//东八区时间
                    MenuDishes = item.Select(a => new GetMenuDetailOneDayResponseDto.MenuDishesDto
                    {
                        DishesGuid = a.DishesGuid,
                        DishesName = a.DishesName,
                        DishesPrice = isInternal ? a.DishesInternalPrice : a.DishesExternalPrice,
                        DishesDescription = a.DishesDescription,
                        DishesImg = a.DishesImg

                    }).ToList()
                });
            }
            return Success(res);
        }

        /// <summary>
        /// 获取用户点餐订单分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetUserMealOrderPageListResponseDto>))]
        public async Task<IActionResult> GetUserMealOrderPageListAsync([FromBody]GetUserMealOrderPageListRequestDto requestDto)
        {
            if (string.IsNullOrWhiteSpace(requestDto.UserGuid))
            {
                requestDto.UserGuid = UserID;
            }
            var result = await new MealOrderBiz().GetUserMealOrderPageListAsync(requestDto);
            return Success(result);
        }

        /// <summary>
        /// 获取待转让订单详情信息
        /// </summary>
        /// <param name="orderGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetMealOrderDetailResponseDto>))]
        public async Task<IActionResult> GetMealOrderDetailForTransferAsync(string orderGuid)
        {
            var orderBiz = new MealOrderBiz();
            var orderDetailBiz = new MealOrderDetailBiz();
            var orderModel = await orderBiz.GetModelAsync(orderGuid);
            if (orderModel == null)
            {
                return Failed(ErrorCode.Empty, "无此订单数据");
            }
            if (orderModel.OrderStatus != MealOrderStatusEnum.Paided.ToString())
            {
                var enumStatus = (MealOrderStatusEnum)Enum.Parse(typeof(MealOrderStatusEnum), orderModel.OrderStatus);
                return Failed(ErrorCode.UserData, $"当店订单状态为[{enumStatus.GetDescription()}]不支持转让");
            }
            else if (orderModel.MealEndTime < DateTime.Now)
            {
                return Failed(ErrorCode.UserData, $"当店订单已过用餐时间，无法转让");
            }
            var orderDetails = await orderDetailBiz.GetModelsByOrderGuidAsync(orderGuid);
            var response = orderModel.ToDto<GetMealOrderDetailResponseDto>();
            response.Dishees = orderDetails?.Select(a => a.ToDto<GetMealOrderDetailResponseDto.MealOrderDetailDishesDto>()).ToList();
            return Success(response);
        }

        /// <summary>
        /// 取消点餐订单
        /// </summary>
        /// <param name="orderGuid">订单guid</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CancelMealOrderAsync(string orderGuid)
        {
            var orderBiz = new MealOrderBiz();
            var orderModel = await orderBiz.GetModelAsync(orderGuid);
            if (orderModel == null)
            {
                return Failed(ErrorCode.Empty, "无此订单数据");
            }
            var mealCategoryModel = await new MealCategoryBiz().GetModelAsync(orderModel.CategoryGuid);
            if (mealCategoryModel == null)
            {
                return Failed(ErrorCode.Empty, "餐别数据未找到");
            }
            if (orderModel.OrderStatus != MealOrderStatusEnum.Paided.ToString())
            {
                return Failed(ErrorCode.Empty, "此订单已完成取餐或已转让，无法取消");
            }
            var canCancel = orderModel.OrderStatus == MealOrderStatusEnum.Paided.ToString() && Convert.ToDateTime($"{orderModel.MealDate.AddDays(mealCategoryModel.CategoryAdvanceDay * (-1)).ToString("yyyy-MM-dd")} {mealCategoryModel.CategoryScheduleTime}") > DateTime.Now;
            if (!canCancel)
            {
                return Failed(ErrorCode.UserData, "此订单已过点餐截止时间，无法取消");
            }
            //修改订单状态为已取消
            orderModel.OrderStatus = MealOrderStatusEnum.Canceled.ToString();
            //获取应退款的数据
            var tradeDetals = await new MealAccountTradeDetailBiz().GetModelsByOrderGuidAsync(orderModel.OrderGuid, 0);

            //退款-订单交易记录
            var refundTradeModels = new MealAccountTradeModel
            {
                AccountTradeGuid = Guid.NewGuid().ToString("N"),
                OrderGuid = orderModel.OrderGuid,
                AccountTradeType = (sbyte)MealAccountTradeTypeEnum.Refund,
                AccountTradeFee = orderModel.TotalPrice,
                AccountTradeDescription = string.Empty,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty
            };
            //退款-订单交易明细
            var refundTradeDetailModels = new List<MealAccountTradeDetailModel>();
            foreach (var td in tradeDetals)
            {
                var item = td.Clone() as MealAccountTradeDetailModel;
                item.AccountTradeDetailGuid = Guid.NewGuid().ToString("N");
                item.AccountTradeGuid = refundTradeModels.AccountTradeGuid;
                item.CreatedBy = UserID;
                item.CreationDate = DateTime.Now;
                item.LastUpdatedBy = UserID;
                item.LastUpdatedDate = DateTime.Now;
                refundTradeDetailModels.Add(item);
            }
            //退款-钱包账户流水
            var accountModels = await new MealAccountBiz().GetModelsByUserIdAsync(orderModel.UserGuid, orderModel.HospitalGuid);
            if (!accountModels.Any())
            {
                return Failed(ErrorCode.UserData, "未检测到钱包账户，请先充值");
            }
            var refundAccountDetailModels = tradeDetals.Select(a => new MealAccountDetailModel
            {
                AccountDetailGuid = Guid.NewGuid().ToString("N"),
                AccountGuid = a.AccountGuid,
                AccountDetailType = MealAccountDetailTypeEnum.Refund.ToString(),
                AccountDetailIncomeType = (sbyte)MealAccountDetailIncomeTypeEnum.Income,
                AccountDetailBeforeFee = accountModels.FirstOrDefault(p => p.AccountGuid == a.AccountGuid)?.AccountBalance ?? 0M,
                AccountDetailFee = a.AccountTradeFee,
                AccountDetailAfterFee = (accountModels.FirstOrDefault(p => p.AccountGuid == a.AccountGuid)?.AccountBalance ?? 0M) + a.AccountTradeFee,
                AccountDetailDescription = $"订单退款：订单号[{orderModel.OrderNo}]",
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty

            }).ToList();

            var result = await orderBiz.CancelMealOrderAsync(orderModel, refundTradeModels, tradeDetals, refundAccountDetailModels);
            return result ? Success() : Failed(ErrorCode.DataBaseError);
        }

        /// <summary>
        /// 接收转让订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AcceptTransferedMealOrderAsync([FromBody]AcceptTransferedMealOrderRequestDto requestDto)
        {
            var orderGuid = requestDto.OrderGuid;
            var orderBiz = new MealOrderBiz();
            var orderDetailBiz = new MealOrderDetailBiz();

            #region 检测待转让订单是否能被转让
            var orderModelOld = await orderBiz.GetModelAsync(orderGuid);
            if (orderModelOld == null)
            {
                return Failed(ErrorCode.Empty, "无此订单数据");
            }
            if (orderModelOld.OrderStatus != MealOrderStatusEnum.Paided.ToString())
            {
                var enumStatus = (MealOrderStatusEnum)Enum.Parse(typeof(MealOrderStatusEnum), orderModelOld.OrderStatus);
                return Failed(ErrorCode.UserData, $"当店订单状态为[{enumStatus.GetDescription()}]不支持转让");
            }
            else if (orderModelOld.MealEndTime < DateTime.Now)
            {
                return Failed(ErrorCode.UserData, $"当店订单已过用餐时间，无法转让");
            }
            else if (orderModelOld.UserGuid == UserID)
            {
                return Failed(ErrorCode.UserData, $"不能接收来自自己的转让订单");
            }
            #endregion

            //获取转让订单去向用户的钱包账户
            var accountModels = await new MealAccountBiz().GetModelsByUserIdAsync(UserID, orderModelOld.HospitalGuid);
            if (!accountModels.Any())
            {
                return Failed(ErrorCode.UserData, "未检测到钱包账户，请先充值");
            }

            //获取待转让订单的订单明细数据
            var orderDetailsOld = await orderDetailBiz.GetModelsByOrderGuidAsync(orderGuid);

            MealOrderModel orderInsert = new MealOrderModel();//新增订单记录
            MealOrderModel orderUpdate = new MealOrderModel();//更新订单记录
            List<MealOrderDetailModel> orderDetails = new List<MealOrderDetailModel>();//新增点餐订单明细列表
            List<MealAccountDetailModel> accountDetails = new List<MealAccountDetailModel>();//新增点餐钱包流水列表
            var tradeModels = new List<MealAccountTradeModel>();//点餐交易流水列表
            List<MealAccountTradeDetailModel> tradeDetailsModels = new List<MealAccountTradeDetailModel>();//点餐订单交易流水明细列表

            //创建新订单
            var orderModelNew = orderModelOld.Clone() as MealOrderModel;
            orderModelNew.OrderGuid = Guid.NewGuid().ToString("N");
            orderModelNew.UserGuid = UserID;
            orderModelNew.CreatedBy = UserID;
            orderModelNew.CreationDate = DateTime.Now;
            orderModelNew.LastUpdatedBy = UserID;
            orderModelNew.LastUpdatedDate = DateTime.Now;
            orderModelNew.TransferredFrom = orderModelOld.OrderGuid;

            //修改旧订单
            orderModelOld.TransferredTo = orderModelNew.OrderGuid;
            orderModelOld.OrderStatus = MealOrderStatusEnum.Transferred.ToString();
            orderModelOld.LastUpdatedBy = UserID;
            orderModelOld.LastUpdatedDate = DateTime.Now;

            orderInsert = orderModelNew;
            orderUpdate = orderModelOld;

            //创建新的订单详情
            for (int i = 0; i < orderDetailsOld.Count; i++)
            {
                var od = orderDetailsOld[i];
                od.OrderDetailGuid = Guid.NewGuid().ToString("N");
                od.OrderGuid = orderModelNew.OrderGuid;
                od.CreatedBy = UserID;
                od.CreationDate = DateTime.Now;
                od.LastUpdatedBy = UserID;
                od.LastUpdatedDate = DateTime.Now;
            }
            orderDetails.AddRange(orderDetailsOld);

            #region 旧订单退款:创建钱包退款流水、订单退款交易流水

            //退款-订单交易记录
            var refundTradeModels = new MealAccountTradeModel
            {
                AccountTradeGuid = Guid.NewGuid().ToString("N"),
                OrderGuid = orderModelOld.OrderGuid,
                AccountTradeType = (sbyte)MealAccountTradeTypeEnum.Refund,
                AccountTradeFee = orderModelOld.TotalPrice,
                AccountTradeDescription = string.Empty,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty
            };
            tradeModels.Add(refundTradeModels);
            //退款-订单交易明细
            var tradeDetal_TransferFrom = await new MealAccountTradeDetailBiz().GetModelsByOrderGuidAsync(orderModelOld.OrderGuid, 0);
            foreach (var td in tradeDetal_TransferFrom)
            {
                var item = td.Clone() as MealAccountTradeDetailModel;
                item.AccountTradeDetailGuid = Guid.NewGuid().ToString("N");
                item.AccountTradeGuid = refundTradeModels.AccountTradeGuid;
                item.CreatedBy = UserID;
                item.CreationDate = DateTime.Now;
                item.LastUpdatedBy = UserID;
                item.LastUpdatedDate = DateTime.Now;
                tradeDetailsModels.Add(item);
            }
            //退款-钱包账户流水
            var sourceUserAccountModels = await new MealAccountBiz().GetModelsByUserIdAsync(orderModelOld.UserGuid, orderModelOld.HospitalGuid);
            if (!sourceUserAccountModels.Any())
            {
                return Failed(ErrorCode.UserData, "未检测到钱包账户");
            }
            accountDetails.AddRange(tradeDetal_TransferFrom.Select(a => new MealAccountDetailModel
            {
                AccountDetailGuid = Guid.NewGuid().ToString("N"),
                AccountGuid = a.AccountGuid,
                AccountDetailType = MealAccountDetailTypeEnum.Refund.ToString(),
                AccountDetailIncomeType = (sbyte)MealAccountDetailIncomeTypeEnum.Income,
                AccountDetailBeforeFee = (sourceUserAccountModels.FirstOrDefault(p => p.AccountGuid == a.AccountGuid)?.AccountBalance ?? 0M),
                AccountDetailFee = a.AccountTradeFee,
                AccountDetailAfterFee = (sourceUserAccountModels.FirstOrDefault(p => p.AccountGuid == a.AccountGuid)?.AccountBalance ?? 0M) + a.AccountTradeFee,
                AccountDetailDescription = $"订单转让退款：订单号[{orderModelOld.OrderNo}]",
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty
            }).ToList());

            #endregion

            #region 新订单扣款、创建钱包流水、创建订单交易流水
            var accountToal = accountModels.Sum(a => a.AccountBalance);
            var orderTotal = orderModelNew.TotalPrice;
            if (accountToal < orderTotal)
            {
                return Failed(ErrorCode.UserData, "余额不足，请先充值");
            }
            var rechargeAccount = accountModels.FirstOrDefault(a => a.AccountType == MealAccountTypeEnum.Recharge.ToString());
            var grantAccount = accountModels.FirstOrDefault(a => a.AccountType == MealAccountTypeEnum.Grant.ToString());

            var newOrderTradeModel = new MealAccountTradeModel
            {
                AccountTradeGuid = Guid.NewGuid().ToString("N"),
                OrderGuid = orderModelNew.OrderGuid,
                AccountTradeType = (sbyte)MealAccountTradeTypeEnum.Consumer,
                AccountTradeFee = orderModelNew.TotalPrice,
                AccountTradeDescription = string.Empty,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty
            };
            tradeModels.Add(newOrderTradeModel);

            var paidMoney = orderTotal;
            //创建钱包流水记录
            if (grantAccount != null && grantAccount.AccountBalance > 0)
            {
                var grantPaid = grantAccount.AccountBalance > paidMoney ? paidMoney : grantAccount.AccountBalance;
                accountDetails.Add(new MealAccountDetailModel
                {
                    AccountDetailGuid = Guid.NewGuid().ToString("N"),
                    AccountGuid = grantAccount.AccountGuid,
                    AccountDetailType = MealAccountDetailTypeEnum.Consume.ToString(),
                    AccountDetailIncomeType = (sbyte)MealAccountDetailIncomeTypeEnum.Expenditure,
                    AccountDetailBeforeFee = grantAccount.AccountBalance,
                    AccountDetailFee = grantPaid,
                    AccountDetailAfterFee = grantAccount.AccountBalance - grantPaid,
                    AccountDetailDescription = "订单消费",
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = string.Empty
                });
                var orderPaid = grantPaid > orderModelNew.TotalPrice ? orderModelNew.TotalPrice : grantPaid;
                tradeDetailsModels.Add(new MealAccountTradeDetailModel
                {
                    AccountTradeDetailGuid = Guid.NewGuid().ToString("N"),
                    AccountGuid = grantAccount.AccountGuid,
                    AccountTradeGuid = newOrderTradeModel.AccountTradeGuid,
                    AccountTradeFee = orderPaid,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = string.Empty
                });
                paidMoney -= grantPaid;
            }
            if (paidMoney > 0 && rechargeAccount != null && rechargeAccount.AccountBalance > 0)
            {
                accountDetails.Add(new MealAccountDetailModel
                {
                    AccountDetailGuid = Guid.NewGuid().ToString("N"),
                    AccountGuid = rechargeAccount.AccountGuid,
                    AccountDetailType = MealAccountDetailTypeEnum.Consume.ToString(),
                    AccountDetailIncomeType = (sbyte)MealAccountDetailIncomeTypeEnum.Expenditure,
                    AccountDetailBeforeFee = rechargeAccount.AccountBalance,
                    AccountDetailFee = paidMoney,
                    AccountDetailAfterFee = rechargeAccount.AccountBalance - paidMoney,
                    AccountDetailDescription = "订单消费",
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = string.Empty
                });
                tradeDetailsModels.Add(new MealAccountTradeDetailModel
                {
                    AccountTradeDetailGuid = Guid.NewGuid().ToString("N"),
                    AccountGuid = rechargeAccount.AccountGuid,
                    AccountTradeGuid = newOrderTradeModel.AccountTradeGuid,
                    AccountTradeFee = paidMoney,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = string.Empty
                });
            }
            #endregion

            var result = await orderBiz.AcceptTransferedMealOrderAsync(orderInsert, orderUpdate, orderDetails, accountDetails, tradeModels, tradeDetailsModels);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "订单转让操作失败，请重试");
        }

        /// <summary>
        /// 获取钱包余额
        /// </summary>
        /// <param name="hospitalGuid">账户所属医院guid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetMealWalletBalanceResponseDto>))]
        public async Task<IActionResult> GetMealWalletBalanceAsync(string hospitalGuid)
        {
            if (string.IsNullOrWhiteSpace(hospitalGuid))
            {
                return Failed(ErrorCode.UserData, "账户所属医院guid必填");
            }
            var accountModels = await new MealAccountBiz().GetModelsByUserIdAsync(UserID, hospitalGuid);
            //if (!accountModels.Any())
            //{
            //    return Failed(ErrorCode.UserData, "未检测到钱包账户，请先充值");
            //}
            var isInternal = accountModels.FirstOrDefault(a => a.UserType == MealUserTypeEnum.Internal.ToString()) != null;
            GetMealWalletBalanceResponseDto response = new GetMealWalletBalanceResponseDto
            {
                TotalBalance = accountModels.Sum(a => a.AccountBalance),
                GrantBalance = accountModels.FirstOrDefault(a => a.AccountType == MealAccountTypeEnum.Grant.ToString())?.AccountBalance ?? 0M,
                RechargeBalance = accountModels.FirstOrDefault(a => a.AccountType == MealAccountTypeEnum.Recharge.ToString())?.AccountBalance ?? 0M,
                IsInternal = isInternal
            };
            return Success(response);
        }

        /// <summary>
        /// 获取钱包流水记录
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetMealWalletRecordResponseDto>))]
        public async Task<IActionResult> GetMealWalletRecordAsync([FromBody]GetMealWalletRecordRequestDto requestDto)
        {
            if (string.IsNullOrWhiteSpace(requestDto.UserGuid))
            {
                requestDto.UserGuid = UserID;
            }
            var response = await new MealAccountDetailBiz().GetMealWalletRecordAsync(requestDto);
            return Success(response);
        }

        /// <summary>
        /// 获取订单转让码信息
        /// </summary>
        /// <param name="orderGuid">订单guid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetTransferedWXAcodeResponseDto>))]
        public async Task<IActionResult> GetTransferedWXAcodeAsync(string orderGuid)
        {
            #region 检测待转让订单是否能被转让
            var orderBiz = new MealOrderBiz();
            var orderModelOld = await orderBiz.GetModelAsync(orderGuid);
            if (orderModelOld == null)
            {
                return Failed(ErrorCode.Empty, "无此订单数据");
            }
            if (orderModelOld.OrderStatus != MealOrderStatusEnum.Paided.ToString())
            {
                var enumStatus = (MealOrderStatusEnum)Enum.Parse(typeof(MealOrderStatusEnum), orderModelOld.OrderStatus);
                return Failed(ErrorCode.UserData, $"当前订单状态为[{enumStatus.GetDescription()}]不支持转让");
            }
            else if (orderModelOld.MealEndTime < DateTime.Now)
            {
                return Failed(ErrorCode.UserData, $"当前订单已过用餐时间，无法转让");
            }

            #endregion


            var resToken = WeChartApi.GetAccessToken(PlatformSettings.CDMealClientAppId, PlatformSettings.CDMealClientAppSecret).Result;
            if (resToken.Errcode != 0 || resToken == null)
            {
                return Failed(ErrorCode.SystemException, $"获取token失败：{resToken.Errmsg}");
            }
            var param = new WXACodeParam
            {
                Path = $"pages/OrderTransfer/OrderTransfer?orderGuid={orderGuid}",
                Width = 280
            };
            var res = WeChartApi.GetWXACode(param, resToken.AccessToken).Result;
            if (string.IsNullOrWhiteSpace(res))
            {
                return Failed(ErrorCode.SystemException, $"获取转让码失败");
            }
            var response = new GetTransferedWXAcodeResponseDto
            {
                TransferedCodeImg = res,
                CategoryName = orderModelOld.CategoryName,
                MealStartTime = orderModelOld.MealStartTime,
                MealEndTime = orderModelOld.MealEndTime
            };
            return Success(response);
        }
    }
}
