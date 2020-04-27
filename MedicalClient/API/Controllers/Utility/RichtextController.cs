using GD.API.Code;
using GD.Common;
using GD.Dtos.Utility.Richtext;
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
    /// 富文本控制器
    /// </summary>
    public class RichtextController : UtilityBaseController
    {
        /// <summary>
        /// 修改富文本数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ModifyRichtextAsync([FromBody]ModifyRichtextRequestDto requestDto)
        {
            var richtextBiz = new RichtextBiz();
            var richtextModel = await richtextBiz.GetAsync(requestDto.TextGuid);
            if (richtextModel == null)
            {
                return Failed(ErrorCode.Empty, "未查询到此富文本信息");
            }
            richtextModel.Content = requestDto.Content;
            var result = await richtextBiz.UpdateAsync(richtextModel);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "更新失败");
        }

        /// <summary>
        /// 获取富文本
        /// </summary>
        /// <param name="richtextId"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetRichtextResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetRichtextAsync(string richtextId)
        {
            var richtextBiz = new RichtextBiz();
            var richtextModel = await richtextBiz.GetAsync(richtextId);
            if (richtextModel == null)
            {
                return Failed(ErrorCode.Empty, "未查询到此富文本信息");
            }
            var response = richtextModel.ToDto<GetRichtextResponseDto>();
            return Success(response);
        }
    }
}
