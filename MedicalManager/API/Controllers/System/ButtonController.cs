using GD.API.Code;
using GD.Common;
using GD.Dtos.Button;
using GD.Dtos.Common;
using GD.Manager.Manager;
using GD.Models.Manager;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.API.Controllers.System
{
    /// <summary>
    /// 按钮
    /// </summary>
    public class ButtonController : SystemBaseController
    {
        /// <summary>
        /// 按钮列表
        /// </summary>
        /// <param name="menuGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<IEnumerable<GetButtonItemDto>>))]
        public async Task<IActionResult> GetButtonListAsync(string menuGuid)
        {
            var buttonBiz = new ButtonBiz();
            var entityList = await buttonBiz.GetListAsync(menuGuid, enable: true);
            var resopnse = entityList.OrderByDescending(a => a.Sort).ThenBy(a => a.CreatedBy).Select(a => a.ToDto<GetButtonItemDto>());
            return Success(resopnse);
        }
        /// <summary>
        /// 添加按钮
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> AddButtonAsync([FromBody]AddButtonRequestDto request)
        {
            var buttonBiz = new ButtonBiz();
            var entity = await buttonBiz.AnyByCodeAsync(request.ButtonCode, request.MenuGuid);
            if (entity)
            {
                return Failed(ErrorCode.UserData, "code重复");
            }
            var result = await buttonBiz.InsertAsync(new ButtonModel
            {
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                Enable = request.Enable,
                ButtonGuid = Guid.NewGuid().ToString("N"),
                ButtonName = request.ButtonName,
                OrgGuid = string.Empty,
                Sort = request.Sort,
                MenuGuid = request.MenuGuid,
                ButtonCode = request.ButtonCode
            });
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 修改按钮
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> UpdateButtonAsync([FromBody]UpdateButtonRequestDto request)
        {
            var buttonBiz = new ButtonBiz();
            var entity = await buttonBiz.GetAsync(request.ButtonGuid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            entity.LastUpdatedBy = UserID;
            entity.LastUpdatedDate = DateTime.Now;
            entity.ButtonName = request.ButtonName;
            entity.ButtonCode = request.ButtonCode;
            entity.Sort = request.Sort;
            entity.Enable = request.Enable;
            entity.MenuGuid = request.MenuGuid;
            var result = await buttonBiz.UpdateAsync(entity);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 禁用启用按钮
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DisableEnableButtonAsync([FromBody]DisableEnableRequestDto request)
        {
            var buttonBiz = new ButtonBiz();
            var entity = await buttonBiz.GetAsync(request.Guid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            entity.LastUpdatedBy = UserID;
            entity.LastUpdatedDate = DateTime.Now;
            entity.Enable = request.Enable;
            var result = await buttonBiz.UpdateAsync(entity);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DeleteButtonAsync([FromBody]DeleteRequestDto request)
        {
            var buttonBiz = new ButtonBiz();
            var result = await buttonBiz.DeleteAsync(request.Guid);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "删除失败");
            }
            return Success();
        }
    }
}
