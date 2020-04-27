using GD.Common;
using GD.Dtos.Advise;
using GD.Manager.Consumer;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GD.API.Controllers.Consumer
{
    /// <summary>
    /// 用户反馈
    /// </summary>
    public class AdviseController : ConsumerBaseController
    {
        /// <summary>
        /// 用户反馈列表
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetAdvisePageResponseDto>))]
        public async Task<IActionResult> GetAdvisePageAsync([FromBody]GetAdvisePageRequestDto request)
        {
            var response = await new AdviseBiz().GetAdvisePageAsync(request);
            return Success(response);
        }
    }
}
