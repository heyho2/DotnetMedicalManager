using GD.API.Code;
using GD.Common;
using GD.Consumer;
using GD.Dtos.Consumer.Comment;
using GD.Dtos.Consumer.Consumer;
using GD.Mall;
using GD.Models.Consumer;
using GD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.API.Controllers.Consumer
{
    /// <summary>
    /// 评论控制器
    /// </summary>
    public class CommentController : ConsumerBaseController
    {
        /// <summary>
        /// 获取指定目标的评论树(评论商品，文章，医生等)
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetTargetCommentResponseDto>)), AllowAnonymous]
        public IActionResult GetTargetComment([FromBody]GetTargetCommentRequestDto requestDto)
        {
            var commentBiz = new CommentBiz();
            var userBiz = new UserBiz();
            var accessoryBiz = new AccessoryBiz();
            var commentModels = commentBiz.GetModels(requestDto.PageIndex, requestDto.PageSize, "where target_guid=@target_guid and enable=@enable", "last_updated_date desc", new { target_guid = requestDto.TargetGuid, enable = true });
            var theDtos = new List<GetTargetCommentResponseDto>();
            var likeBiz = new LikeBiz();
            foreach (var item in commentModels)
            {
                var dto = item.ToDto<GetTargetCommentResponseDto>();
                var userModel = userBiz.GetUser(item.CreatedBy);
                var accessoryModel = accessoryBiz.GetAccessoryModelByGuid(userModel.PortraitGuid);
                dto.NickName = userModel.NickName;
                dto.Portrait = accessoryModel?.BasePath + accessoryModel?.RelativePath;
                dto.LikeNumber = likeBiz.GetLikeNumByTargetGuid(item.CommentGuid);
                GetSonComment(dto);
                theDtos.Add(dto);
            }
            return Success(theDtos);
        }

        private void GetSonComment(GetTargetCommentResponseDto dto)
        {
            var commentBiz = new CommentBiz();
            var userBiz = new UserBiz();
            var accessoryBiz = new AccessoryBiz();
            var tmpModels = commentBiz.GetModelsByTargetGuid(dto.CommentGuid);
            var lst = new List<GetTargetCommentResponseDto>();
            foreach (var item in tmpModels)
            {
                var tmpDto = item.ToDto<GetTargetCommentResponseDto>();
                var userModel = userBiz.GetUser(item.CreatedBy);
                var accessoryModel = accessoryBiz.GetAccessoryModelByGuid(userModel.PortraitGuid);
                tmpDto.NickName = userModel.NickName;
                tmpDto.Portrait = accessoryModel?.BasePath + accessoryModel?.RelativePath;
                GetSonComment(tmpDto);
                lst.Add(tmpDto);
            }
            dto.SonComments = lst;
        }

        /// <summary>
        /// 新增评论(评论商品，文章，医生等)
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public IActionResult AddComment([FromBody]AddCommentRequestDto requestDto)
        {
            var commentBiz = new CommentBiz();
            var comModel = new CommentModel()
            {
                CommentGuid = Guid.NewGuid().ToString("N"),
                TargetGuid = requestDto.TargetGuid,
                Content = requestDto.Content,
                Score = requestDto.Score,
                Anonymous = requestDto.Anonymous,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                Enable = true
            };
            var isSuccess = commentBiz.Add(comModel);
            return isSuccess ? Success() : Failed(ErrorCode.DataBaseError, "新增数据失败！");
        }

        /// <summary>
        /// 评论医生
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public IActionResult AddCommentForDoctor([FromBody]AddCommentRequestDto requestDto)
        {
            var commentBiz = new CommentBiz();
            var comModel = new CommentModel()
            {
                CommentGuid = Guid.NewGuid().ToString("N"),
                TargetGuid = requestDto.TargetGuid,
                Content = requestDto.Content,
                Score = requestDto.Score,
                Anonymous = requestDto.Anonymous,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                Enable = true
            };
            var isSuccess = commentBiz.Add(comModel);
            if (!isSuccess)
            {
                return Failed(ErrorCode.DataBaseError, "新增数据失败！");
            }
            GetScoreFromAddCommentForDoctor(UserID, requestDto.TargetGuid, requestDto.Score);
            return Success(comModel.CommentGuid, null);
        }

        /// <summary>
        /// 评价医生
        /// </summary>
        /// <param name="userId">用户GUID</param>
        /// <param name="doctorGuid">医生GUID</param>
        /// <param name="score">评价分数</param>
        private void GetScoreFromAddCommentForDoctor(string userId, string doctorGuid, int score)
        {
            try
            {
                var scoreRulesBiz = new ScoreRulesBiz();
                var count = new CommentBiz().CountUserHasCommentTargetOneDayAsync(userId, doctorGuid, DateTime.Now).Result;
                if (count > 1)
                {
                    return;
                }
                switch (score)
                {
                    case 5:

                        scoreRulesBiz.AddScoreByRules(doctorGuid, Common.EnumDefine.ActionEnum.Praise5, Common.EnumDefine.UserType.Doctor);
                        break;

                    case 4:
                        scoreRulesBiz.AddScoreByRules(doctorGuid, Common.EnumDefine.ActionEnum.Praise4, Common.EnumDefine.UserType.Doctor);
                        break;

                    case 3:
                        scoreRulesBiz.AddScoreByRules(doctorGuid, Common.EnumDefine.ActionEnum.Praise3, Common.EnumDefine.UserType.Doctor);
                        break;

                    case 2:
                        scoreRulesBiz.AddScoreByRules(doctorGuid, Common.EnumDefine.ActionEnum.Praise2, Common.EnumDefine.UserType.Doctor);
                        break;

                    case 1:
                        var sd = scoreRulesBiz.AddScoreByRules(doctorGuid, Common.EnumDefine.ActionEnum.Praise1, Common.EnumDefine.UserType.Doctor);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Common.Helper.Logger.Error($"action:{nameof(GetScoreFromAddCommentForDoctor)}  userId:{userId}  doctorGuid:{doctorGuid}  score:{score}  message:{ex.Message}");
            }
        }

        /// <summary>
        /// 批量新增评论
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> AddCommentInBatchesAsync([FromBody]AddCommentInBatchesRequestDto requestDto)
        {
            var commentModels = requestDto.TargetGuids.Select(a => new CommentModel
            {
                CommentGuid = Guid.NewGuid().ToString("N"),
                TargetGuid = a,
                Content = requestDto.Content,
                Score = requestDto.Score,
                Anonymous = requestDto.Anonymous,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                Enable = true
            }).ToList();
            var result = await new CommentBiz().AddRangeAsync(commentModels);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "批量评论失败");
        }

        /// <summary>
        /// 从订单明细评价商品
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> CommentProductFromOrderDetailAsync([FromBody]CommentProductFromOrderDetailRequestDto requestDto)
        {
            var orderDetailBiz = new OrderDetailBiz();
            var detailModel = await orderDetailBiz.GetModelAsync(requestDto.OrderDetailGuid);
            if (detailModel == null)
            {
                return Failed(ErrorCode.Empty, "未查询到此订单明细记录");
            }
            var commentBiz = new CommentBiz();
            var comModel = new CommentModel()
            {
                CommentGuid = Guid.NewGuid().ToString("N"),
                TargetGuid = requestDto.ProductGuid,
                Content = requestDto.Content,
                Score = requestDto.Score,
                Anonymous = requestDto.Anonymous,
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                Enable = true
            };
            detailModel.CommentGuid = comModel.CommentGuid;
            var response = await commentBiz.CommentProductFromOrderDetailAsync(comModel, detailModel);
            if (response)
            {
                return Success(comModel.CommentGuid, null);
            }
            return Failed(ErrorCode.DataBaseError, "评论失败");
        }

        /// <summary>
        /// 获取商铺下商品的所有评论
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetProductCommentsOfMerchantResponseDto>))]
        public async Task<IActionResult> GetProductCommentsOfMerchantAsync([FromBody]GetProductCommentsOfMerchantRequestDto requestDto)
        {
            var commnetBiz = new CommentBiz();
            var response = await commnetBiz.GetProductCommentsOfMerchantAsync(requestDto);
            return Success(response);
        }

        /// <summary>
        /// 根据guid获取评论数据响应Dto
        /// </summary>
        /// <param name="commentGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetCommentByCommentGuidResponseDto>))]
        public async Task<IActionResult> GetCommentByCommentGuidAsync(string commentGuid)
        {
            var model = await new CommentBiz().GetAsync(commentGuid);
            if (model == null)
            {
                return Failed(ErrorCode.Empty, "无数据");
            }
            var response = model.ToDto<GetCommentByCommentGuidResponseDto>();
            return Success(response);
        }

        /// <summary>
        /// 获取目标评论分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetTargetCommentPageListResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetTargetCommentPageListAsync([FromBody]GetTargetCommentPageListRequestDto requestDto)
        {
            if (!string.IsNullOrWhiteSpace(UserID))
            {
                requestDto.UserId = UserID;
            }
            var response = await new CommentBiz().GetTargetCommentPageListAsync(requestDto);
            return Success(response);
        }

        /// <summary>
        /// 获取指定楼层的评论详情
        /// </summary>
        /// <param name="commentGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetTargetCommentResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetCommentDetailAsync(string commentGuid)
        {
            var item = await new CommentBiz().GetAsync(commentGuid);
            var dto = item.ToDto<GetTargetCommentResponseDto>();

            var userModel = new UserBiz().GetUser(item.CreatedBy);
            var accessoryModel = new AccessoryBiz().GetAccessoryModelByGuid(userModel.PortraitGuid);
            dto.NickName = userModel.NickName;
            dto.Portrait = accessoryModel?.BasePath + accessoryModel?.RelativePath;
            dto.LikeNumber = new LikeBiz().GetLikeNumByTargetGuid(item.CommentGuid);
            GetSonComment(dto);

            return Success(dto);
        }

        /// <summary>
        /// 获取已完成的消费记录分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetCompletedConsumptionPageListRequestDto>))]
        public async Task<IActionResult> GetCompletedConsumptionPageListAsync([FromBody]GetCompletedConsumptionPageListRequestDto requestDto)
        {
            if (string.IsNullOrWhiteSpace(requestDto.ConsumerGuid))
            {
                requestDto.ConsumerGuid = UserID;
            }
            var response = await new ConsumptionBiz().GetCompletedConsumptionPageListAsync(requestDto);
            return Success(response);
        }

        /// <summary>
        /// 消费者提交消费的服务评价
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> CommentConsumptionAsync([FromBody]CommentConsumptionRequestDto requestDto)
        {
            var consumptionBiz = new ConsumptionBiz();
            var consumptionModel = await consumptionBiz.GetModelAsync(requestDto.ConsumptionGuid);
            if (consumptionModel == null)
            {
                return Failed(ErrorCode.Empty, "未查询到该消费记录");
            }
            var goodsItemModel = await new GoodsItemBiz().GetModelAsync(consumptionModel.FromItemGuid);
            var projectModel = await new ProjectBiz().GetModelAsync(goodsItemModel?.ProjectGuid);

            var commentModel = new CommentModel
            {
                CommentGuid = Guid.NewGuid().ToString("N"),
                TargetGuid = goodsItemModel?.ProjectGuid,
                Content = requestDto.Content,
                Score = requestDto.Score,
                Anonymous = requestDto.Anonymous,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty
            };
            consumptionModel.CommentGuid = commentModel.CommentGuid;
            consumptionModel.IsComment = true;
            var result = await consumptionBiz.CommentConsumptionAsync(consumptionModel, commentModel);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "评价失败");
        }

       
    }
}