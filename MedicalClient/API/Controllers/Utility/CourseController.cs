using GD.API.Code;
using GD.Common;
using GD.Consumer;
using GD.Doctor;
using GD.Dtos.Manager.Banner;
using GD.Dtos.Utility.Course;
using GD.Manager;
using GD.Models.Manager;
using GD.Module;
using GD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GD.API.Controllers.Utility
{
    /// <summary>
    /// 课程控制器
    /// </summary>
    public class CourseController : UtilityBaseController
    {
        /// <summary>
        /// 获取首页热门课程
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetHotCourseItemDto>>)), AllowAnonymous]
        public async Task<IActionResult> GetHotCourseAsync([FromBody]GetHotCourseRequestDto request)
        {
            var response = await new ConsumerBiz().GetHotCourseAsync(request);
            return Success(response);
        }
        /// <summary>
        /// 获取课程详细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetCourseDetailResponseDto>>)), AllowAnonymous]
        public async Task<IActionResult> GetCourseDetailAsync([FromBody]GetCourseDetailRequestDto request)
        {
            var info = await new ArticleBiz().GetAsync(request.CourseGuid);  //await new CourseBiz().GetAsync(request.CourseGuid);
            var accessoryBiz = new AccessoryBiz();
            var richtextEntity = await new RichtextBiz().GetAsync(info.ContentGuid);
            var logoAccessoryEntity = await accessoryBiz.GetAsync(info.PictureGuid);
            //var videoAccessoryEntity = await accessoryBiz.GetAsync(info.VideoGuid);
            var userEntity =  new AccountBiz().GetUserById(info.AuthorGuid);//await new UserBiz().GetModelAsync(info.AuthorGuid);
            var response = info.ToDto<GetCourseDetailResponseDto>();
            response.Content = richtextEntity?.Content;
            response.LogoUrl = $"{logoAccessoryEntity?.BasePath }{logoAccessoryEntity?.RelativePath }";
            //response.VideoUrl = $"{videoAccessoryEntity?.BasePath }{videoAccessoryEntity?.RelativePath }";
            response.AuthorName = userEntity?.UserName;
            var hotModel = await new HotExBiz().GetModelAsync(request.CourseGuid);
            response.LikeTotal = hotModel?.LikeCount ?? 0;//await new LikeBiz().GetLikeNumByTargetGuidAsync(request.CourseGuid);
            response.VisitTotal = hotModel?.VisitCount ?? 0; //await new BehaviorBiz().GetViewNumByTargetGuidAsync(request.CourseGuid);


            //var doctor = await new DoctorBiz().GetAsync(info.AuthorGuid);
            //if (doctor != null)
            //{
            //    var doctorAccessory = await accessoryBiz.GetAsync(doctor.PortraitGuid);
            //    response.DoctorPortrait = $"{doctorAccessory?.BasePath }{doctorAccessory?.RelativePath }";
            //    response.HospitalName = doctor.HospitalName;
            //}
            return Success(response);
        }

        /// <summary>
        /// 获取健康管理页面banner
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<BannerBaseDto>>)), AllowAnonymous]
        public async Task<IActionResult> GetHealthManagementPageBanner()
        {
            var bannerBiz = new BannerBiz();
            var dtos = await bannerBiz.GetBannerInfoAsync(DictionaryType.HealthManagementPage);
            return Success(dtos);
        }
    }
}
