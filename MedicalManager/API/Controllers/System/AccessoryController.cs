using GD.Common;
using GD.Manager.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GD.API.Controllers.System
{
    /// <summary>
    /// 用户
    /// </summary>
    public class AccessoryController : SystemBaseController
    {
        /// <summary>
        /// 获取附件信息
        /// </summary>
        /// <param name="accessoryGuid">附件guid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<string>)), AllowAnonymous]
        public async Task<IActionResult> GetAcessoryUrlAsync(string accessoryGuid)
        {
            var accessoryModel = await new AccessoryBiz().GetAsync(accessoryGuid);
            return Success(data:$"{accessoryModel?.BasePath}{accessoryModel?.RelativePath}");
        }
    }
}
