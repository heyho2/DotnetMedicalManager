using GD.Common;
using GD.Dtos.Order;
using GD.Manager.Mall;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace GD.API.Controllers.Mall
{
    /// <summary>
    /// 订单控制器
    /// </summary>
    public class OrderController : MallBaseController
    {
        /// <summary>
        /// 查询商户订单分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetMerchantOrderPageListResponseDto>))]
        public async Task<IActionResult> GetMerchantOrderPageListAsync([FromQuery]GetMerchantOrderPageListRequestDto requestDto)
        {
            var orderBiz = new OrderBiz();
            var orderList = await orderBiz.GetMerchantOrderPageListAsync(requestDto);
            if (!orderList.CurrentPage.Any())
            {
                return Success(orderList);
            }
            var orderIds = orderList.CurrentPage.Select(a => a.OrderGuid).ToList();
            var products = await orderBiz.GetOrderProductListV2Async(orderIds);
            var productIds = products.Select(a => a.ProductGuid).Distinct().ToList();
            var projects = await orderBiz.GetOrderProductProjectsAsync(productIds);
            foreach (var order in orderList.CurrentPage)
            {
                order.Products = products.Where(a => a.OrderGuid == order.OrderGuid).OrderBy(a => a.ProductName).ToList();
                foreach (var product in order.Products)
                {
                    product.Projects = projects.Where(a => a.ProductGuid == product.ProductGuid).OrderBy(a => a.ProjectName).ToList();
                }
            }
            return Success(orderList);
        }
    }
}
