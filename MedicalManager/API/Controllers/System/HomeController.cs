using GD.Common;
using GD.Manager.Doctor;
using GD.Manager.Mall;
using GD.Manager.Merchant;
using GD.Manager.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GD.API.Controllers.System
{
    /// <summary>
    /// 首页相关接口
    /// </summary>
    public class HomeController : SystemBaseController
    {
        /// <summary>
        /// 获取用户数量
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<int>))]
        public async Task<IActionResult> GetUserQtyAsync()
        {
            var response = await new UserBiz().RecordCountAsync();
            return Success(response);
        }
        /// <summary>
        /// 获取医生数量
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<int>))]
        public async Task<IActionResult> GetDoctorsQtyAsync()
        {
            var response = await new DoctorBiz().RecordCountAsync();
            return Success(response);

        }
        /// <summary>
        /// 获取商户数量
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<int>))]
        public async Task<IActionResult> GetMerchantQtyAsync()
        {
            var response = await new MerchantBiz().RecordCountAsync();
            return Success(response);
        }
        /// <summary>
        /// 获取订单数量
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<int>))]
        public async Task<IActionResult> GetOrderQtyAsync()
        {
            var response = await new OrderBiz().RecordCountAsync();
            return Success(response);
        }
    }
}
