using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GD.Common;
using GD.Dtos.Mall;
using GD.Dtos.Mall.BackStage;
using GD.Mall;
using GD.Models.CommonEnum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GD.API.Controllers.Mall
{
    /// <summary>
    /// 后台订单相关接口
    /// </summary>
    public class BackStageMallController : MallBaseController
    {
        /// <summary>
        /// 订单类型
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<Dictionary<string, int>>))]
        public IActionResult BackStageGetOrderTypeList()
        {
            var dic = new Dictionary<string, int>();
            var t = typeof(OrderStatusEnum);
            var arr = Enum.GetValues(t);
            foreach (var item in arr)
            {
                dic.Add(item.ToString(), (int)item);
            }
            return Success(dic);
            //"all": 0,
            //"obligation": 1,
            //"shipped": 2,
            //"received": 3,
            //"completed": 4,
            //"canceled": 5
        }
        /// <summary>
        /// 订单列表-分页
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous, Produces(typeof(ResponseDto<BackStageGetOrderListResponseDto>))]
        public async Task<IActionResult> BackStageGetOrderList(BackStageGetOrderListRequestDto requestDto)
        {
            var responseDto = await new OrderBiz().BackStageGetOrderListAsync(requestDto,UserID);
            return Success(responseDto);
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<BackStageGetOrderDetailListResponseDto>))]
        public async Task<IActionResult> BackStageGetOrderDetailList(BackStageGetOrderDetailListRequestDto requestDto)
        {
            var responseDto = await new OrderBiz().BackStageGetOrderDetailListAsync(requestDto);
            return Success(responseDto);
        }

        /// <summary>
        /// 订单发货
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<string>))]
        public IActionResult BackStageGetOrderPostList()
        {
            return Success();
        }


    }
}
