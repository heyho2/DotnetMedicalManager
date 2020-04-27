using GD.Common;
using GD.Common.EnumDefine;
using GD.Dtos.Help;
using GD.Dtos.Question;
using GD.Manager.Utility;
using GD.Models.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GD.API.Controllers.Utility
{
    /// <summary>
    /// 常见问题
    /// </summary>
    public class HelpController : UtilityBaseController
    {
        /// <summary>
        /// 常见问题
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetHelpPageResponseDto>))]
        public async Task<IActionResult> GetHelpPageAsync([FromBody]GetHelpPageRequestDto request)
        {
            var response = await new HelpBiz().GetHelpPageAsync(request);
            return Success(response);
        }

        /// <summary>
        /// 添加Help
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> AddHelpAsync([FromBody]AddHelpRequestDto request)
        {
            var result = await new HelpBiz().InsertAsync(new HelpModel
            {
                HelpGuid = Guid.NewGuid().ToString("N"),
                Answer = request.Answer,
                Question = request.Question,
                CreatedBy = UserID,
                Sort = request.Sort,
                LastUpdatedBy = UserID,
                Enable = request.Enable,
                OrgGuid = string.Empty,
            });
            if (!result)
            {
                return Failed(ErrorCode.UserData, "添加失败");
            }
            return Success();
        }
        /// <summary>
        /// 修改Help
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> UpdateHelpAsync([FromBody]UpdateHelpRequestDto request)
        {
            HelpBiz helpBiz = new HelpBiz();
            var entity = await helpBiz.GetAsync(request.HelpGuid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            //entity.PlatformType = request.PlatformType.ToString() == PlatformType.CloudDoctor.ToString().ToLower() ? PlatformType.CloudDoctor.ToString() : PlatformType.LifeCosmetology.ToString();
            entity.Question = request.Question;
            entity.Answer = request.Answer;
            entity.Sort = request.Sort;
            entity.Enable = request.Enable;
            entity.LastUpdatedDate = DateTime.Now;
            entity.LastUpdatedBy = UserID;
            var result = await helpBiz.UpdateAsync(entity);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 禁用Help
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DisableEnableHelpAsync([FromBody]DisableEnableHelpRequestDto request)
        {
            HelpBiz helpBiz = new HelpBiz();
            var entity = await helpBiz.GetAsync(request.Guid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            entity.LastUpdatedBy = UserID;
            entity.LastUpdatedDate = DateTime.Now;
            entity.Enable = request.Enable;
            var result = await helpBiz.UpdateAsync(entity);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
    }
}
