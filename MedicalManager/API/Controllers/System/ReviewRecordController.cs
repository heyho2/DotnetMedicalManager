using GD.Common;
using GD.Dtos.ReviewRecord;
using GD.Manager;
using GD.Manager.Manager;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace GD.API.Controllers.System
{
    /// <summary>
    /// 审核记录制器
    /// </summary>
    public class ReviewRecordController : SystemBaseController
    {
        /// <summary>
        /// 审核记录
        /// </summary>
        /// <returns></returns>

        [HttpPost, Produces(typeof(ResponseDto<GetReviewRecordPageResponseDto>))]
        public async Task<IActionResult> GetReviewRecordPageAsync([FromBody]GetReviewRecordPageRequestDto request)
        {
            var response = await new ReviewRecordBiz().GetReviewRecordPageAsync(request);
            return Success(response);
        }
    }
}
