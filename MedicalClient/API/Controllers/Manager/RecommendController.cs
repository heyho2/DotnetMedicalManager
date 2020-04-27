using GD.Common;
using GD.Dtos.Manager.Recommend;
using GD.Manager;
using GD.Models.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GD.API.Controllers.Manager
{
    /// <summary>
    /// 推荐控制器
    /// </summary>
    public class RecommendController : ManagerBaseController
    {
        /// <summary>
        /// 获取首页推荐
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetHomeRecommendItemDto>>)), AllowAnonymous]
        public async Task<IActionResult> GetHomeRecommendAsync([FromBody]GetHomeRecommendRequestDto request)
        {
            var response = await new RecommendBiz().GetHomeRecommendAsync(request);
            return Success(response);
        }
        /// <summary>
        /// 获取推荐类型
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetRecommendTypesItemDto>>)), AllowAnonymous]
        public IActionResult GetRecommendTypes()
        {
            var response = new List<GetRecommendTypesItemDto>();
            foreach (RecommendModel.TypeEnum item in Enum.GetValues(typeof(RecommendModel.TypeEnum)))
            {
                response.Add(new GetRecommendTypesItemDto
                {
                    Code = item.GetDescription(),
                    Name = item.ToString()
                });
            }
            return Success(response);
        }
        /// <summary>
        /// 获取医生推荐列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetRecommendDoctorListResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetRecommendDoctorListAsync([FromBody]GetRecommendDoctorListRequestDto request)
        {
            var response = await new RecommendBiz().GetRecommendDoctorListAsync(request);
            var dictionaryBiz = new DictionaryBiz();
            foreach (var item in response.CurrentPage)
            {
                if (!string.IsNullOrWhiteSpace(item.DocTitleGuid))
                {
                    item.DocTitle = dictionaryBiz.GetNameById(item.DocTitleGuid);
                }
            }
            return Success(response);
        }
        /// <summary>
        /// 获取医院推荐列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetRecommendHospitalListResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetRecommendHospitalListAsync([FromBody]GetRecommendHospitalListRequestDto request)
        {
            var response = await new RecommendBiz().GetRecommendHospitalListAsync(request);
            return Success(response);
        }

        /// <summary>
        /// 获取科室推荐列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetRecommendOfficeListResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetRecommendOfficeListAsync([FromBody]GetRecommendOfficeListAsyncRequestDto request)
        {
            var response = await new RecommendBiz().GetRecommendOfficeListAsync(request);
            return Success(response);
        }
        /// <summary>
        /// 获取文章推荐列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetRecommendArticleListResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetRecommendArticleListAsync([FromBody]GetRecommendArticleListRequestDto request)
        {
            var response = await new RecommendBiz().GetRecommendArticleListAsync(request);
            var dictionaryBiz = new DictionaryBiz();
            foreach (var item in response.CurrentPage)
            {
                item.ArticleType = dictionaryBiz.GetNameById(item.ArticleTypeDic);
            }
            return Success(response);
        }

        /// <summary>
        /// 获取商品推荐列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetRecommendProductListResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetRecommendProductListAsync([FromBody]GetRecommendProductListRequestDto request)
        {
            var response = await new RecommendBiz().GetRecommendProductListAsync(request);
            return Success(response);
        }
        /// <summary>
        /// 获取课程推荐
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetRecommendCourseListResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetRecommendCourseListAsync([FromBody]GetRecommendCourseListRequestDto request)
        {
            var response = await new RecommendBiz().GetRecommendCourseListAsync(request);
            return Success(response);
        }
    }
}
