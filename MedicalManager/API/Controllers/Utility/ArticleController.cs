using GD.Common;
using GD.Common.EnumDefine;
using GD.Dtos.Article;
using GD.Dtos.Common;
using GD.Manager.Manager;
using GD.Manager.Utility;
using GD.Models.Manager;
using GD.Models.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GD.API.Controllers.Utility
{
    ///<summary>
    ///文章表模型
    ///</summary>
    public class ArticleController : UtilityBaseController
    {
        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetArticleListResponseDto>))]
        public async Task<IActionResult> GetArticleListAsync([FromQuery]GetArticleListRequestDto request)
        {
            var response = await new ArticleBiz().GetArticleListAsync(request);
            return Success(response);
        }
        /// <summary>
        /// 获取类型
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetArticleTypeRequestDto>))]
        public async Task<IActionResult> GetArticleTypeAsync()
        {
            var list = await new DictionaryBiz().GetListAsync(DictionaryType.ArticleTypeConfig, true);
            var response = list.Select(a => new GetArticleTypeRequestDto { Name = a.ConfigName, Value = a.DicGuid }).ToList();
            return Success(response);
        }

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> AddArticleAsync([FromBody]AddArticleRequestDto request)
        {
            var articleGuid = Guid.NewGuid().ToString("N");
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
                OwnerGuid = articleGuid,
                TextGuid = textGuid,
            };
            var releaseStatus = Enum.Parse<Models.Utility.ReleaseStatus>(request.ActcleReleaseStatus.ToString());
            var sourceType = Enum.Parse<ArticleModel.ArticleSourceTypeEnum>(request.SourceType.ToString());
            ArticleModel articleModel = new ArticleModel
            {
                Abstract = request.Abstract,
                ArticleGuid = articleGuid,
                ArticleTypeDic = request.ArticleTypeDic,
                AuthorGuid = UserID,
                ContentGuid = textGuid,
                CreatedBy = UserID,
                Enable = request.Enable,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty,
                PlatformType = PlatformType.CloudDoctor.ToString(),
                Sort = 1,
                Title = request.Title,
                Visible = request.Visible,
                PictureGuid = request.PictureGuid,
                ActcleReleaseStatus = releaseStatus,
                SourceType = sourceType.ToString(),
                Keyword = JsonConvert.SerializeObject(request.Keyword),
                ExternalLink = request.ExternalLink ?? string.Empty,
            };
            var response = await new ArticleBiz().AddAsync(richtextModel, articleModel);
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
        /// 修改文章
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> UpdateArticleAsync([FromBody]UpdateArticleRequestDto request)
        {
            var articleBiz = new ArticleBiz();
            var contentBiz = new RichtextBiz();
            var articleModel = await articleBiz.GetAsync(request.ArticleGuid);
            if (articleModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "数据错误");
            }
            var richtextModel = await contentBiz.GetAsync(articleModel.ContentGuid);

            richtextModel.Content = request.Content;
            richtextModel.LastUpdatedBy = UserID;
            richtextModel.LastUpdatedDate = DateTime.Now;
            richtextModel.OrgGuid = string.Empty;
            richtextModel.OwnerGuid = request.ArticleGuid;

            articleModel.Abstract = request.Abstract;
            articleModel.ArticleTypeDic = request.ArticleTypeDic;
            articleModel.LastUpdatedBy = UserID;
            articleModel.LastUpdatedDate = DateTime.Now;
            articleModel.Sort = 1;
            articleModel.Title = request.Title;
            articleModel.Enable = request.Enable;
            articleModel.Visible = request.Visible;
            articleModel.PictureGuid = request.PictureGuid;
            articleModel.ActcleReleaseStatus = Enum.Parse<Models.Utility.ReleaseStatus>(request.ActcleReleaseStatus.ToString());
            articleModel.Keyword = JsonConvert.SerializeObject(request.Keyword);
            articleModel.ExternalLink = request.ExternalLink ?? string.Empty;

            var response = await new ArticleBiz().UpdateAsync(richtextModel, articleModel);
            if (!response)
            {
                return Failed(ErrorCode.DataBaseError, "修改失败");
            }
            return Success(response);
        }


        /// <summary>
        /// 获取文章信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetArticleInfoResponseDto>))]
        public async Task<IActionResult> GetArticleInfoAsync([FromBody]GetArticleInfoRequestDto request)
        {
            var articleBiz = new ArticleBiz();
            var contentBiz = new RichtextBiz();
            AccessoryBiz accessoryBiz = new AccessoryBiz();
            var articleModel = await articleBiz.GetAsync(request.ArticleGuid);
            if (articleModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "数据错误");
            }
            var richtextModel = await contentBiz.GetAsync(articleModel.ContentGuid);
            var accessory = await accessoryBiz.GetAsync(articleModel.PictureGuid);
            return Success(new GetArticleInfoResponseDto
            {
                ArticleTypeDic = articleModel.ArticleTypeDic,
                Abstract = articleModel.Abstract,
                Content = richtextModel.Content,
                PictureGuid = articleModel.PictureGuid,
                Title = articleModel.Title,
                Visible = articleModel.Visible,
                PictureUrl = $"{accessory?.BasePath}{accessory?.RelativePath}",
                ArticleGuid = articleModel.ArticleGuid,
                Enable = articleModel.Enable,
                ActcleReleaseStatus = Enum.Parse<Dtos.Article.ReleaseStatus>(articleModel.ActcleReleaseStatus.ToString()),
                Keyword = JsonConvert.DeserializeObject<string[]>(string.IsNullOrWhiteSpace(articleModel.Keyword) ? "[]" : articleModel.Keyword),
                ExternalLink = articleModel.ExternalLink
            });
        }
        /// <summary>
        /// 是否可阅读
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<int>))]
        public async Task<IActionResult> SetArticleVisibleAsync([FromBody]SetArticleVisibleRequestDto request)
        {
            var articleBiz = new ArticleBiz();
            var articleModel = await articleBiz.GetAsync(request.ArticleGuid);
            if (articleModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "数据错误");
            }
            articleModel.Visible = request.Visible;
            var response = await articleBiz.UpdateAsync(articleModel);
            if (!response)
            {
                return Failed(ErrorCode.DataBaseError, "跟新失败");
            }
            return Success();
        }
        /// <summary>
        /// 搜索文章
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<SearchArticleResponseDto>))]
        public async Task<IActionResult> SearchArticleAsync([FromBody]SearchArticleRequestDto requestDto)
        {
            var response = await new ArticleBiz().SearchArticleAsync(requestDto);
            return Success(response);
        }
        /// <summary>
        /// 删除文章
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DeleteArticleAsync([FromBody]DeleteRequestDto request)
        {
            var articleBiz = new ArticleBiz();
            var result = await articleBiz.DeleteAsync(request.Guid);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "删除失败");
            }
            return Success();
        }
        /// <summary>
        /// 禁用文章
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DisableEnableArticleAsync([FromBody]DisableEnableRequestDto request)
        {
            var articleBiz = new ArticleBiz();
            var result = await articleBiz.DisableEnableAsync(request.Guid, request.Enable, UserID);
            if (!result)
            {
                return Failed(ErrorCode.UserData, "修改失败");
            }
            return Success();
        }
    }
}