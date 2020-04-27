using GD.Common;
using GD.Dtos.Common;
using GD.Dtos.Faqs;
using GD.Manager.Consumer;
using GD.Manager.Faqs;
using GD.Manager.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.API.Controllers.Faqs
{
    /// <summary>
    /// 问答
    /// </summary>
    public class FaqsController : FaqsBaseController
    {
        /// <summary>
        /// 搜索问题
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<SearchFaqsQuestionResponseDto>))]
        public async Task<IActionResult> SearchFaqsQuestionAsync([FromBody]SearchFaqsQuestionRequestDto request)
        {
            var response = await new FaqsQuestionBiz().SearchFaqsQuestionAsync(request);
            var aguids = new List<string> { };
            //var uguids = new List<string> { };
            var guids = new List<string> { };
            foreach (var item in response.CurrentPage)
            {
                item.AttachmentGuidList2 = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(item.AttachmentGuidList ?? "[]");
                aguids.AddRange(item.AttachmentGuidList2);
                //uguids.Add(item.UserGuid);
                guids.Add(item.QuestionGuid);
            }
            var accessorys = await new AccessoryBiz().GetListAsync(aguids);
            //var users = await new UserBiz().GetListAsync(uguids);
            var hots = await new HotBiz().GetListAsync(guids);
            var collectioncs = await new CollectionBiz().GetCollectionCountAsync(guids);

            foreach (var item in response.CurrentPage)
            {
                item.Attachments = accessorys.Where(a => item.AttachmentGuidList2.Contains(a.AccessoryGuid)).Select(s => new SearchFaqsQuestionItemDto.Attachment
                {
                    Guid = s.AccessoryGuid,
                    Url = $"{s.BasePath}{s.RelativePath}"
                }).ToArray();
                //item.UserName = users.FirstOrDefault(a => a.UserGuid == item.UserGuid)?.UserName;

                var hot = hots.FirstOrDefault(a => a.OwnerGuid == item.QuestionGuid);

                //item.LikeCount = hot?.LikeCount ?? 0;
                item.VisitCount = hot?.VisitCount ?? 0;
                item.CollectionCount = hot?.CollectCount ?? 0;
            }
            return Success(response);
        }
        /// <summary>
        /// 禁用问题
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DisableEnableFaqsQuestionAsync([FromBody]DisableEnableRequestDto request)
        {
            var FaqsQuestionBiz = new FaqsQuestionBiz();
            var entity = await FaqsQuestionBiz.GetAsync(request.Guid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            entity.LastUpdatedBy = UserID;
            entity.LastUpdatedDate = DateTime.Now;
            entity.Enable = request.Enable;
            var result = await FaqsQuestionBiz.UpdateAsync(entity);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
        /// <summary>
        /// 删除问题
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DeleteFaqsQuestionAsync([FromBody]DeleteRequestDto request)
        {
            var articleBiz = new FaqsQuestionBiz();
            var result = await articleBiz.DisableEnableAsync(request.Guid, false, UserID);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "删除失败");
            }
            return Success();
        }
        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> SetFaqsSettingStatusAsync([FromBody]SetFaqsSettingStatusAsyncRequestDto request)
        {
            var faqsQuestionBiz = new FaqsQuestionBiz();
            var entity = await faqsQuestionBiz.GetAsync(request.QuestionGuid);
            if (entity == null)
            {
                return Failed(ErrorCode.UserData, "找不到数据");
            }
            entity.Status = request.Status.ToString();
            entity.LastUpdatedDate = DateTime.Now;
            entity.LastUpdatedBy = UserID;
            var response = await faqsQuestionBiz.UpdateAsync(entity);
            return Success(response);
        }
    }
}
