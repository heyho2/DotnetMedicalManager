using GD.API.Code;
using GD.Common;
using GD.Dtos.Utility.Accessory;
using GD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GD.API.Controllers.Utility
{
    /// <summary>
    /// 附件控制器
    /// </summary>
    public class AccessoryController : UtilityBaseController
    {
        /// <summary>
        /// 获取附件信息
        /// </summary>
        /// <param name="accessoryGuid">附件guid</param>
        /// <returns></returns>
        [HttpGet,Produces(typeof(ResponseDto<AccessoryDto>)), AllowAnonymous]
        public async Task<IActionResult> GetAcessoryInfoAsync(string accessoryGuid)
        {
            var accessoryModel= await new AccessoryBiz().GetAsync(accessoryGuid);
            if (accessoryModel==null)
            {
                return Failed(Common.ErrorCode.Empty,"未查询到附件数据");
            }
            var response = accessoryModel.ToDto<AccessoryDto>();
            response.FullPath = $"{accessoryModel.BasePath}{accessoryModel.RelativePath}";
            return Success(response);
        }
             

    }
}
