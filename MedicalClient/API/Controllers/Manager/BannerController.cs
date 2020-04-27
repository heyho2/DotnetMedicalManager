using GD.Common;
using GD.Dtos.Manager.Banner;
using GD.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using GD.Models.Manager;
using System.Linq;
using System;

namespace GD.API.Controllers.Manager
{
    /// <summary>
    /// Banner控制器
    /// </summary>
    public class BannerController : ManagerBaseController
    {
        /// <summary>
        /// 获取首页Banner
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetHomeBannerItemDto>>)), AllowAnonymous]
        public async Task<IActionResult> GetHomeBannerAsync()
        {
            var response = await new BannerBiz().GetHomeBannerAsync(DictionaryType.WechatOfficialAccountHome);
            return Success(response);
        }
        /// <summary>
        /// 获取医院Banner
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetHomeBannerItemDto>>)), AllowAnonymous]
        public async Task<IActionResult> GetHospitalBannerAsync(GetHospitalBannerRequestDto request)
        {
            var response = await new BannerBiz().GetBannerInfoAsync(request.HospitalGuid);
            return Success(response);
        }

        /// <summary>
        /// 修改banner数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost,Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ModifyBannerInfoAsync([FromBody]ModifyBannerInfoRequestDto requestDto)
        {
            var sort = 1;
            var bannerModels= requestDto.Banners.Select(a => new BannerModel
            {
                BannerGuid = Guid.NewGuid().ToString("N"),
                OwnerGuid = requestDto.OwnerGuid,
                Sort = a.Sort ?? (a.Sort = sort++).Value,
                BannerName = string.IsNullOrWhiteSpace(a.BannerName) ? $"banner{a.Sort}" : a.BannerName,
                PictureGuid = a.PictureGuid,
                TargetUrl = a.TargetUrl,
                Description = a.Description,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty
            }).ToList();
            var result= await new BannerBiz().ModifyBannerInfoAsync(requestDto.OwnerGuid, bannerModels);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "修改banner数据出错");
        }
    }
}
