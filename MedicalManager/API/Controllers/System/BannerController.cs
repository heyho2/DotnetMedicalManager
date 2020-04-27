using GD.Common;
using GD.Dtos.Banner;
using GD.Manager.Doctor;
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
    /// banner
    /// </summary>
    public class BannerController : SystemBaseController
    {
        /// <summary>
        /// 获取banner 列表
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetBannerPageResponseDto>))]
        public async Task<IActionResult> GetBannerPageAsync([FromBody]GetBannerPageRequestDto request)
        {
            var response = await new BannerBiz().GetBannerPageAsync(request);
            return Success(response);
        }
        /// <summary>
        /// 获取智慧云医banner
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetBannerTypeItemDto>>))]
        public async Task<IActionResult> GetBannerType1()
        {
            var list = await new DictionaryBiz().GetListAsync(DictionaryType.PageId, true);
            var response = list.Select(a => new GetBannerTypeItemDto
            {
                Guid = a.DicGuid,
                Name = a.ConfigName
            }).ToList();
            var hospitals = await new HospitalBiz().GetAllAsync(true);
            response.AddRange(hospitals.Select(a => new GetBannerTypeItemDto
            {
                Guid = a.HospitalGuid,
                Name = a.HosName
            }));
            return Success(response);
        }

        /// <summary>
        /// 添加banner
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> AddBannerAsync([FromBody]AddBannerRequestDto request)
        {
            var result = await new BannerBiz().InsertAsync(new BannerModel
            {
                BannerGuid = Guid.NewGuid().ToString("N"),
                BannerName = request.BannerName,
                OwnerGuid = request.OwnerGuid,
                TargetUrl = request.TargetUrl,
                PictureGuid = request.PictureGuid,
                Sort = request.Sort,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                Enable = request.Enable,
                OrgGuid = string.Empty,
                Description = request.Description
            });
            if (!result)
            {
                return Failed(ErrorCode.UserData, "添加失败");
            }
            return Success();
        }
        /// <summary>
        /// 修改banner
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> UpdateBannerAsync([FromBody]UpdateBannerRequestDto request)
        {
            BannerBiz bannerBiz = new BannerBiz();
            var entity = await bannerBiz.GetAsync(request.BannerGuid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }

            entity.BannerName = request.BannerName;
            entity.OwnerGuid = request.OwnerGuid;
            entity.TargetUrl = request.TargetUrl;
            entity.PictureGuid = request.PictureGuid;
            entity.Description = request.Description;

            entity.Enable = request.Enable;
            entity.LastUpdatedDate = DateTime.Now;
            entity.Sort = request.Sort;
            var result = await bannerBiz.UpdateAsync(entity);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 禁用banner
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DisableEnableBannerAsync([FromBody]DisableEnableBannerRequestDto request)
        {
            BannerBiz bannerBiz = new BannerBiz();
            var entity = await bannerBiz.GetAsync(request.Guid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            entity.LastUpdatedBy = UserID;
            entity.LastUpdatedDate = DateTime.Now;
            entity.Enable = request.Enable;
            var result = await bannerBiz.UpdateAsync(entity);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }

    }
}
