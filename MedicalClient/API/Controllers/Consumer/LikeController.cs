using GD.Common;
using GD.Consumer;
using GD.Models.Consumer;
using GD.Module;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GD.API.Controllers.Consumer
{
    /// <inheritdoc />
    /// <summary>
    /// 点赞控制器
    /// </summary>
    public class LikeController : ConsumerBaseController
    {
        /// <summary>
        /// 点赞/取消点赞目标
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public IActionResult LikeTarget(string targetGuid)
        {
            var likeBiz = new LikeBiz();
            var tmpModel = likeBiz.GetOneLikeModelByUserId(UserID, targetGuid);
            string likeState;
            bool result;
            if (tmpModel != null)
            {

                likeState = tmpModel.Enable ? "取消点赞" : "点赞";
                tmpModel.Enable = !tmpModel.Enable;
                tmpModel.LastUpdatedDate = DateTime.Now;
                tmpModel.LastUpdatedBy = UserID;
                result = likeBiz.UpdateModel(tmpModel);
            }
            else
            {
                likeState = "点赞";
                var model = new LikeModel
                {
                    LikeGuid = Guid.NewGuid().ToString("N"),
                    TargetGuid = targetGuid,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = ""
                };
                result = likeBiz.InsertModel(model);
            }
            if (result)
            {
                var upRes= new HotExBiz().UpdateLikeTotalAsync(targetGuid, likeState == "点赞");
            }
            return result ? Success($"{likeState}操作成功！") : Failed(ErrorCode.DataBaseError, $"{likeState}操作失败！");
        }

        /// <summary>
        /// 检查用户对目标是否点赞
        /// </summary>
        /// <param name="targetGuid">目标guid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public IActionResult GetLikeState(string targetGuid)
        {
            var likeBiz = new LikeBiz();
            var state = likeBiz.GetLikeState(UserID, targetGuid);
            return Success(state);
        }

        /// <summary>
        /// 获取点赞数量
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<int>)), AllowAnonymous]
        public IActionResult GetLikeNumber(string targetGuid)
        {
            var likeBiz = new LikeBiz();
            var num = likeBiz.GetLikeNumByTargetGuid(targetGuid);
            return Success(num);
        }
    }
}
