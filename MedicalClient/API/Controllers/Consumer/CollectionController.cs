using GD.Common;
using GD.Consumer;
using GD.Doctor;
using GD.Dtos.Consumer.Collection;
using GD.FAQs;
using GD.Mall;
using GD.Models.CommonEnum;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GD.API.Controllers.Consumer
{
    /// <summary>
    /// 收藏控制器
    /// </summary>
    public class CollectionController : ConsumerBaseController
    {
        /// <summary>
        /// 收藏医生
        /// </summary>
        /// <param name="doctorGuid">医生guid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public IActionResult CollectDoctor(string doctorGuid)
        {
            var collectionBiz = new CollectionBiz();
            TargetCollectResponseDto result = collectionBiz.CollectTargetResult(doctorGuid, UserID, "doctor");
            if (result.result)
            {
                new ConsumerActionBiz().CollectDoctorToGetScore(UserID, doctorGuid, result.collectionState, result.first);
            }
            return result.result ? Success($"{result.collectionState.GetDescription()}操作成功！") : Failed(ErrorCode.DataBaseError, $"{result.collectionState.GetDescription()}操作失败！");
        }

        /// <summary>
        /// 收藏文章
        /// </summary>
        /// <param name="articleGuid">文章guid</param>
        /// <param name="collectionType">收藏类型</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public IActionResult CollectArticle(string articleGuid, CollectionTypeEnum collectionType= CollectionTypeEnum.Article)
        {
            var collectionBiz = new CollectionBiz();
            var result = collectionBiz.CollectTarget(articleGuid, UserID, collectionType.ToString().ToLower(), out var state);
            return result ? Success($"{state}操作成功！") : Failed(ErrorCode.DataBaseError, $"{state}操作失败！");
        }

        /// <summary>
        /// 收藏产品
        /// </summary>
        /// <param name="productGuid">产品guid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public IActionResult CollectProduct(string productGuid)
        {
            var collectionBiz = new CollectionBiz();
            var productModel = new ProductBiz().GetModelByGuid(productGuid);
            var result = collectionBiz.CollectTarget(productGuid, UserID, "product", out var state, productModel?.PlatformType);
            return result ? Success($"{state}操作成功！") : Failed(ErrorCode.DataBaseError, $"{state}操作失败！");
        }

        /// <summary>
        /// 收藏提问
        /// </summary>
        /// <param name="questionGuid"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CollectQuestion(string questionGuid)
        {
            var collectionBiz = new CollectionBiz();
            var result = collectionBiz.CollectTarget(questionGuid, UserID, "question", out var state, Common.EnumDefine.PlatformType.CloudDoctor.ToString());
            return result ? Success($"{state}操作成功！") : Failed(ErrorCode.DataBaseError, $"{state}操作失败！");
        }

        /// <summary>
        /// 判断目标是否收藏
        /// </summary>
        /// <param name="targetGuid">目标Guid</param>
        /// <returns>是否收藏</returns>
        [HttpGet, Produces(typeof(ResponseDto<bool>))]
        public IActionResult WhetherCollectTarget(string targetGuid)
        {
            var collectionBiz = new CollectionBiz();
            var model = collectionBiz.GetOneModelByUserId(UserID, targetGuid);
            if (model != null && model.Enable)
            {
                return Success(true);
            }
            return Success(false);
        }

        /// <summary>
        /// 获取收藏目标的用户列表
        /// </summary>
        /// <param name="requestDto">参数</param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetTheUserListOfCollectionTargetResponseDto>>))]
        public async Task<IActionResult> GetTheUserListOfCollectionTargetAsync([FromBody]GetTheUserListOfCollectionTargetRequestDto requestDto)
        {
            var response = await new CollectionBiz().GetTheUserListOfCollectionTargetAsync(requestDto.TargetGuid, requestDto.Keyword, requestDto.PageIndex, requestDto.PageSize);
            return Success(response);
        }
    }
}