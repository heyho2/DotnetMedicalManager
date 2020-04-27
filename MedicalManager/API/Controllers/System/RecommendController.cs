using GD.API.Controllers.System;
using GD.Common;
using GD.Dtos.Common;
using GD.Dtos.Recommend;
using GD.Manager.Manager;
using GD.Models.Manager;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.API.Controllers.Manager
{
    /// <summary>
    /// 推荐控制器
    /// </summary>
    public class RecommendController : SystemBaseController
    {
        /// <summary>
        /// 推荐列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetRecommendPageResponseDto>))]
        public async Task<IActionResult> GetRecommendPageAsync([FromBody]GetRecommendPageRequestDto request)
        {
            var response = await new RecommendBiz().GetRecommendPageAsync(request);
            return Success(response);
        }
        /// <summary>
        /// 获取推荐类型
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<IList<EnumItemDto>>))]
        public IActionResult GetRecommendTypes()
        {
            var response = new List<EnumItemDto>();
            foreach (RecommendModel.TypeEnum item in Enum.GetValues(typeof(RecommendModel.TypeEnum)))
            {
                response.Add(new EnumItemDto
                {
                    Code = item.ToString(),
                    Name = item.GetDescription()
                });
            }
            return Success(response);
        }
        /// <summary>
        /// 获取医生推荐列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetRecommendDoctorListResponseDto>))]
        public async Task<IActionResult> GetRecommendDoctorListAsync([FromBody]GetRecommendDoctorListRequestDto request)
        {
            var response = await new RecommendBiz().GetRecommendDoctorListAsync(request);

            return Success(response);
        }
        /// <summary>
        /// 获取医院推荐列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetRecommendHospitalListResponseDto>))]
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
        [HttpPost, Produces(typeof(ResponseDto<GetRecommendOfficeListResponseDto>))]
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
        [HttpPost, Produces(typeof(ResponseDto<GetRecommendArticleListResponseDto>))]
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
        [HttpPost, Produces(typeof(ResponseDto<GetRecommendProductListResponseDto>))]
        public async Task<IActionResult> GetRecommendProductListAsync([FromBody]GetRecommendProductListRequestDto request)
        {
            var response = await new RecommendBiz().GetRecommendProductListAsync(request);
            return Success(response);
        }

        /// <summary>
        /// 新增推荐
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> AddRecommendAsync([FromBody]AddRecommendRequestDto request)
        {
            var response = await new RecommendBiz().InsertAsync(new RecommendModel
            {
                Name = request.Name,
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                Enable = request.Enable,
                LastUpdatedBy = UserID,
                LastUpdatedDate = DateTime.Now,
                OrgGuid = string.Empty,
                PictureGuid = request.PictureGuid,
                RecommendGuid = Guid.NewGuid().ToString("N"),
                Remark = request.Remark,
                Sort = request.Sort,
                Target = request.Target,
                Type = request.Type,
            });
            return Success(response);
        }
        /// <summary>
        /// 删除推荐
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DeleteRecommendAsync([FromBody]DeleteRecommendRequestDto request)
        {
            var response = await new RecommendBiz().DeleteRecommendAsync(request.RecommendGuid);
            if (!response)
            {
                return Failed(ErrorCode.DataBaseError);
            }
            return Success(response);
        }
        /// <summary>
        /// 修改推荐
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<int>))]
        public async Task<IActionResult> UpdateRecommendAsync([FromBody]UpdateRecommendRequestDto request)
        {
            var recommendBiz = new RecommendBiz();
            var entity = await recommendBiz.GetAsync(request.RecommendGuid);
            if (entity == null)
            {
                return Failed(ErrorCode.DataBaseError, "没有找到该推荐");
            }
            entity.Name = request.Name;
            entity.LastUpdatedBy = UserID;
            entity.LastUpdatedDate = DateTime.Now;
            entity.PictureGuid = request.PictureGuid;
            entity.Remark = request.Remark;
            entity.Sort = request.Sort;
            entity.Target = request.Target;
            entity.Enable = request.Enable;
            entity.Type = request.Type;
            var response = await recommendBiz.UpdateAsync(entity);
            if (!response)
            {
                return Failed(ErrorCode.DataBaseError);
            }
            return Success(response);
        }
        /// <summary>
        /// 新增推荐详细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> AddRecommendDetailAsync([FromBody]AddRecommendDetailRequestDto request)
        {
            var recommendBiz = new RecommendDetailBiz();
            List<RecommendDetailModel> recommendDetailModels = new List<RecommendDetailModel>();
            foreach (var item in request.OwnerGuids)
            {
                if ((await recommendBiz.GetListAsync(request.RecommendGuid, item)).Count() == 0)
                {
                    recommendDetailModels.Add(new RecommendDetailModel
                    {
                        CreatedBy = UserID,
                        CreationDate = DateTime.Now,
                        Enable = true,
                        LastUpdatedBy = UserID,
                        LastUpdatedDate = DateTime.Now,
                        OrgGuid = string.Empty,
                        DetailGuid = Guid.NewGuid().ToString("N"),
                        OwnerGuid = item,
                        RecommendGuid = request.RecommendGuid
                    });
                }
            }
            var response = await recommendBiz.InsertListAsync(recommendDetailModels);
            if (!response)
            {
                return Failed(ErrorCode.DataBaseError);
            }
            return Success(response);
        }
        /// <summary>
        /// 删除推荐详细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DeleteRecommendDetailAsync([FromBody]DeleteRecommendDetailRequestDto request)
        {
            var response = await new RecommendDetailBiz().DeleteListByIdsAsync(request.DetailGuids);
            if ((response) == 0)
            {
                return Failed(ErrorCode.DataBaseError);
            }
            return Success(response);
        }
        /// <summary>
        /// 删除推荐详细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DeleteRecommendDetail2Async([FromBody]DeleteRecommendDetail2RequestDto request)
        {
            var response = await new RecommendDetailBiz().DeleteAsync(request.RecommendGuid, request.OwnerGuid);
            if ((response) == 0)
            {
                return Failed(ErrorCode.DataBaseError);
            }
            return Success(response);
        }
        /// <summary>
        /// 启用禁用
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DisableEnableRecommendAsync([FromBody]DisableEnableRecommendRequestDto request)
        {
            var recommendBiz = new RecommendBiz();
            var entity = await recommendBiz.GetAsync(request.Guid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            entity.LastUpdatedBy = UserID;
            entity.LastUpdatedDate = DateTime.Now;
            entity.Enable = request.Enable;
            var result = await recommendBiz.UpdateAsync(entity);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 获取课程推荐
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetRecommendCourseListResponseDto>))]
        public async Task<IActionResult> GetRecommendCourseListAsync([FromBody]GetRecommendCourseListRequestDto request)
        {
            var response = await new RecommendBiz().GetRecommendCourseListAsync(request);
            return Success(response);
        }
        /// <summary>
        /// 获取 推荐集合
        /// </summary>
        /// <param name="guid">guid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<string[]>))]
        public async Task<IActionResult> GetOwnerGuidsAsync(string guid)
        {
            var response = await new RecommendDetailBiz().GetOwnerGuidsAsync(guid);
            return Success(response);
        }
        /// <summary>
        /// 新增推荐详细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> SaveRecommendDetailAllAsync([FromBody]AddRecommendDetailRequestDto request)
        {
            var recommendBiz = new RecommendDetailBiz();
            var serverGuids = await recommendBiz.GetOwnerGuidsAsync(request.RecommendGuid);
            //1，差集 用户传入的鱼服务器 我们添加
            var addGuids = request.OwnerGuids.Except(serverGuids).ToList();//差集
            var addModels = addGuids.Select(item => new RecommendDetailModel
            {
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                Enable = true,
                LastUpdatedBy = UserID,
                LastUpdatedDate = DateTime.Now,
                OrgGuid = string.Empty,
                DetailGuid = Guid.NewGuid().ToString("N"),
                OwnerGuid = item,
                RecommendGuid = request.RecommendGuid
            });
            var deleteGuids = serverGuids.Except(request.OwnerGuids).ToArray();//差集

            var response = await recommendBiz.AddsAsync(addModels.ToList(), request.RecommendGuid, deleteGuids);
            if (!response)
            {
                return Failed(ErrorCode.DataBaseError);
            }
            return Success(response);
        }
    }
}
