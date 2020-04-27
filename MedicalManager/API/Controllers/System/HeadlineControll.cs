using GD.Common;
using GD.Common.EnumDefine;
using GD.Dtos.Headline;
using GD.Manager.Utility;
using GD.Models.Manager;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GD.API.Controllers.System
{
    /// <summary>
    /// 头条
    /// </summary>
    public class HeadlineController : SystemBaseController
    {
        /// <summary>
        /// 头条列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetHeadlinePageResponseDto>>))]
        public async Task<IActionResult> GetHeadlinePageAsync([FromBody]GetHeadlinePageRequestDto request)
        {
            var headlineBiz = new HeadlineBiz();
            var response = await headlineBiz.GetHeadlinePageAsync(request);
            return Success(response);
        }
        /// <summary>
        /// 添加头条
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> AddHeadlineAsync([FromBody]AddHeadlineRequestDto request)
        {
            var headlineBiz = new HeadlineBiz();
            var result = await headlineBiz.InsertAsync(new HeadlineModel
            {
                HeadlineGuid = Guid.NewGuid().ToString("N"),
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                Enable = request.Enable,
                OrgGuid = string.Empty,
                HeadlineAbstract = request.HeadlineAbstract,
                HeadlineName = request.HeadlineName,
                Sort = request.Sort,
                PlatformType = PlatformType.CloudDoctor.ToString(),
                Target = request.Target
            });
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 修改头条
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> UpdateHeadlineAsync([FromBody]UpdateHeadlineRequestDto request)
        {
            var headlineBiz = new HeadlineBiz();
            var entity = await headlineBiz.GetAsync(request.HeadlineGuid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            entity.LastUpdatedBy = UserID;
            entity.Enable = request.Enable;
            entity.LastUpdatedDate = DateTime.Now;
            entity.Sort = request.Sort;
            entity.HeadlineName = request.HeadlineName;
            entity.HeadlineAbstract = request.HeadlineAbstract;
            entity.Target = request.Target;
            var result = await headlineBiz.UpdateAsync(entity);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 禁用头条
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DisableEnableHeadlineAsync([FromBody]DisableEnableHeadlineRequestDto request)
        {
            var headlineBiz = new HeadlineBiz();
            var entity = await headlineBiz.GetAsync(request.Guid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            entity.LastUpdatedBy = UserID;
            entity.LastUpdatedDate = DateTime.Now;
            entity.Enable = request.Enable;
            var result = await headlineBiz.UpdateAsync(entity);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 删除头条
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DeleteHeadlineAsync([FromBody]DeleteHeadlineRequestDto request)
        {
            var headlineBiz = new HeadlineBiz();
            var result = await headlineBiz.DeleteAsync(request.Guid);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "删除失败");
            }
            return Success();
        }
    }
}
