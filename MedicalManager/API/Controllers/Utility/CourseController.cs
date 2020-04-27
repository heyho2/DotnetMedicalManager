using GD.Common;
using GD.Dtos.Course;
using GD.Manager.Utility;
using GD.Models.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GD.API.Controllers.Utility
{
    /// <summary>
    /// 课程控制器
    /// </summary>
    public class CourseController : UtilityBaseController
    {
        /// <summary>
        /// 获取课程列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetCourseListResponseDto>))]
        public async Task<IActionResult> GetCourseListAsync([FromBody]GetCourseListRequestDto request)
        {
            var response = await new CourseBiz().GetCourseListAsync(request);
            return Success(response);
        }
        /// <summary>
        /// 添加课程
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> AddCourseAsync([FromBody]AddCourseRequestDto request)
        {
            var CourseGuid = Guid.NewGuid().ToString("N");
            var textGuid = Guid.NewGuid().ToString("N");

            RichtextModel richtextModel = new RichtextModel
            {
                Content = request.Content,
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                Enable = true,
                LastUpdatedBy = UserID,
                LastUpdatedDate = DateTime.Now,
                OrgGuid = string.Empty,
                OwnerGuid = CourseGuid,
                TextGuid = textGuid,
            };

            CourseModel courseModel = new CourseModel
            {
                Summary = request.Abstract,
                CourseGuid = CourseGuid,
                AuthorGuid = UserID,
                ContentGuid = textGuid,
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                Enable = request.Enable,
                LastUpdatedBy = UserID,
                LastUpdatedDate = DateTime.Now,
                OrgGuid = string.Empty,
                Title = request.Title,
                Visible = request.Visible,
                LogoGuid = request.PictureGuid
            };

            var response = await new CourseBiz().AddAsync(richtextModel, courseModel);
            if (response)
            {
                return Success(response);
            }
            else
            {
                return Failed(ErrorCode.DataBaseError, "添加失败");
            }
        }
        /// <summary>
        /// 修改课程
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> UpdateCourseAsync([FromBody]UpdateCourseRequestDto request)
        {
            var CourseBiz = new CourseBiz();
            var contentBiz = new RichtextBiz();
            var CourseModel = await CourseBiz.GetAsync(request.CourseGuid);
            if (CourseModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "数据错误");
            }

            var richtextModel = await contentBiz.GetAsync(CourseModel.ContentGuid);

            richtextModel.Content = request.Content;
            richtextModel.LastUpdatedBy = UserID;
            richtextModel.LastUpdatedDate = DateTime.Now;
            richtextModel.OrgGuid = string.Empty;
            richtextModel.OwnerGuid = request.CourseGuid;

            CourseModel.Summary = request.Abstract;
            CourseModel.LastUpdatedBy = UserID;
            CourseModel.LastUpdatedDate = DateTime.Now;
            CourseModel.Title = request.Title;
            CourseModel.Visible = request.Visible;
            CourseModel.Enable = request.Enable;
            CourseModel.LogoGuid = request.PictureGuid;

            var response = await new CourseBiz().UpdateAsync(richtextModel, CourseModel);
            if (!response)
            {
                return Failed(ErrorCode.DataBaseError, "修改失败");
            }
            return Success(response);
        }


        /// <summary>
        /// 获取课程信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetCourseInfoResponseDto>))]
        public async Task<IActionResult> GetCourseInfoAsync([FromBody]GetCourseInfoRequestDto request)
        {
            var courseBiz = new CourseBiz();
            var contentBiz = new RichtextBiz();
            AccessoryBiz accessoryBiz = new AccessoryBiz();
            var courseModel = await courseBiz.GetAsync(request.CourseGuid);
            if (courseModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "数据错误");
            }
            var richtextModel = await contentBiz.GetAsync(courseModel.ContentGuid);
            var accessory = await accessoryBiz.GetAsync(courseModel.LogoGuid);
            return Success(new GetCourseInfoResponseDto
            {
                Abstract = courseModel.Summary,
                Content = richtextModel.Content,
                PictureGuid = courseModel.LogoGuid,
                Title = courseModel.Title,
                Visible = courseModel.Visible,
                Enable = courseModel.Enable,
                PictureUrl = $"{accessory?.BasePath}{accessory?.RelativePath}",
                CourseGuid = courseModel.CourseGuid
            });
        }
        /// <summary>
        /// 是否可阅读
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<int>))]
        public async Task<IActionResult> SetCourseVisibleAsync([FromBody]SetCourseVisibleRequestDto request)
        {
            var courseBiz = new CourseBiz();
            var CourseModel = await courseBiz.GetAsync(request.CourseGuid);
            if (CourseModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "数据错误");
            }
            CourseModel.Visible = request.Visible;
            var response = await courseBiz.UpdateAsync(CourseModel);
            if (!response)
            {
                return Failed(ErrorCode.DataBaseError, "跟新失败");
            }
            return Success();
        }
        /// <summary>
        /// 搜索课程
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<SearchCourseResponseDto>))]
        public async Task<IActionResult> SearchCourseAsync([FromBody]SearchCourseRequestDto request)
        {
            var response = await new CourseBiz().SearchCourseAsync(request);
            return Success(response);
        }
        /// <summary>
        /// 禁用课程
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DisableEnableCourseAsync([FromBody]DisableEnableCourseRequestDto request)
        {
            var courseBiz = new CourseBiz();
            var entity = await courseBiz.GetAsync(request.Guid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            entity.LastUpdatedBy = UserID;
            entity.LastUpdatedDate = DateTime.Now;
            entity.Enable = request.Enable;
            var result = await courseBiz.UpdateAsync(entity);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 删除课程
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DeleteCourseAsync(string guid)
        {
            var courseBiz = new CourseBiz();
            var result = await courseBiz.DeleteAsync(guid);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "删除失败");
            }
            return Success();
        }
    }
}
