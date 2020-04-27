using GD.API.Code;
using GD.Common;
using GD.Dtos.Member;
using GD.Manager.Consumer;
using GD.Manager.Mall;
using GD.Manager.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GD.API.Controllers.Mall
{
    /// <summary>
    /// 会员控制器
    /// </summary>
    public class MemberController : MallBaseController
    {
        /// <summary>
        /// 会员列表
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetMemberPageResponseDto>))]
        public async Task<IActionResult> GetMemberPageAsync([FromBody]GetMemberPageRequestDto request)
        {
            var response = await new UserBiz().GetMemberPageAsync(request);
            return Success(response);
        }
        /// <summary>
        /// 获取会员信息
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetMemberInfoResponseDto>))]
        public async Task<IActionResult> GetMemberInfoAsync(string userGuid)
        {
            if (string.IsNullOrWhiteSpace(userGuid))
            {
                return Failed(ErrorCode.UserData);
            }
            UserBiz userBiz = new UserBiz();
            var ressult = await userBiz.GetAsync(userGuid);
            if (ressult == null)
            {
                return Failed(ErrorCode.UserData, "userGuid错误");
            }
            var response = ressult.ToDto<GetMemberInfoResponseDto>();
            //获取用户消费信息
            var consumer = await userBiz.GetConsumerAsync(userGuid);
            if (consumer != null)
            {
                response.LastBuyDate = (DateTime?)consumer?.LastBuyDate;
                response.OrderAverage = (decimal)consumer?.OrderAverage;
                response.OrderQty = (int)consumer?.OrderQty;
                response.OrderTotalAmount = (decimal)consumer?.OrderTotalAmount;
            }
            //消费者
            var consumer2 = await new ConsumerBiz().GetAsync(userGuid);
            if (consumer2 != null)
            {
                response.Recommended = (await userBiz.GetAsync(consumer2.RecommendGuid))?.UserName;
            }
            var address = new AddressBiz().GetUserDefaultAddress(userGuid);
            response.Address = $"{address?.Province}{address?.City}{address?.Area}{address?.DetailAddress}";
            return Success(response);
        }
        /// <summary>
        /// 用户订单信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetMemberOrderPageResponseDto>))]
        public async Task<IActionResult> GetMemberOrderPageAsync([FromBody]GetMemberOrderPageRequestDto request)
        {
            var response = await new UserBiz().GetMemberOrderPageAsync(request);
            return Success(response);
        }
        /// <summary>
        /// 订单详细列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetOrderDetailPageResponseDto>))]
        public async Task<IActionResult> GetOrderDetailPageAsync([FromBody]GetOrderDetailPageRequestDto request)
        {
            var response = await new UserBiz().GetOrderDetailPageAsync(request);
            return Success(response);
        }


        /// <summary>
        /// 订单信息（详细信息）
        /// </summary>
        /// <param name="orderGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetOrderInfoResponseDto>))]
        public async Task<IActionResult> GetOrderInfoAsync(string orderGuid)
        {

            var order = await new OrderBiz().GetAsync(orderGuid);
            if (order == null)
            {
                return Failed(ErrorCode.DataBaseError);
            }
            var user = await new UserBiz().GetAsync(order.UserGuid);
            return Success(new GetOrderInfoResponseDto
            {
                OrderReceiver = order.OrderReceiver,
                OrderPhone = order.OrderPhone,
                ProductCount = order.ProductCount,
                OrderNo = order.OrderNo,
                LogisticsNo = null,
                LogisticsName = null,
                PayablesAmount = order.PayablesAmount,
                PointAmount = 0m,
                Remark = order.Remark,
                DiscountAmout = order.DiscountAmout,
                Freight = order.Freight,
                OrderAddress = order.OrderAddress,
                OrderDate = order.CreationDate,
            });
        }
    }
}
