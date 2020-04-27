using GD.Common;
using GD.Dtos.Mall.Groupbuy;
using GD.Mall;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GD.API.Controllers.Mall
{
    /// <inheritdoc />
    /// <summary>
    /// 团购
    /// </summary>
    public class GroupbuyController : MallBaseController
    {
        /// <summary>
        /// 搜索商品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetHomeGroupbuyItemDto>>))]
        public async Task<IActionResult> GetHomeGroupbuyAsync([FromBody]GetHomeGroupbuyRequestDto request)
        {
            var response = await new GroupbuyBiz().GetHomeGroupbuyAsync(request);
            return Success(response);
        }
    }
}
